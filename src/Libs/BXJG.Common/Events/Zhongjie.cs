using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BXJG.Common.Events
{

    /* 
     * 一般的事件总线库，都是在事件触发时，从ioc中解析处理器然后执行，在Blazor或客户端开发中，需要随时动态的注册、注销、触发事件。
     * abp文档事件底部已经说明了可以动态注册，但目前（abp8.3.1）中存在线程安全问题，导致偶尔报错，若某个事件正在触发，会遍历事件处理器列表，此时若另一个线程正在注册，就有几率报错。
     * 
     * 一个字符串就是一个事件，一个事件有多个处理程序，每个处理程序都是一个委托，这里规定事件最多允许一个事件参数，若有多个请封装成一个类。 
     *
     * 触发事件时，触发方并不晓得事件处理方需要同步还是异步，定义事件时也无法确定，因为事件处理程序是动态注册的。
     * 也无法等待返回，因为可能存在多个处理方
     * 通用的办法是假定所有处理方都是异步的，使用ValueTask，触发时使用Task.WhenAll 
     *
     * 有两种办法处理线程安全，1、注册和遍历时，都使用ConcurrentDictionary 2、用Dictinary<string,事件处理委托>存储，注册时TryAdd，遍历时先ToImmutableList下。
     * 目前来看，用方式1开发起来简单些，还不确定它的性能是否比第2种方式差多少
     */

    /// <summary>
    /// 轻量级的本地事件总线
    /// 它约等于ConcurrentDictionary<事件名称,事件处理委托>
    /// 主要api：注册、触发、注销
    /// </summary>
    public class Zhongjie //: ConcurrentDictionary<string, ConcurrentDictionary<Delegate, Func<object, ValueTask>>>
    {
        //private readonly ConcurrentBag<Weituo> weituos = new ConcurrentBag<Weituo>();//不好做删除
        //核心存储事件和对应的处理程序（委托）

        internal protected readonly ConcurrentDictionary<string, ConcurrentDictionary<Delegate, Weituo>> weituos = new ConcurrentDictionary<string, ConcurrentDictionary<Delegate, Weituo>>(StringComparer.OrdinalIgnoreCase);

        internal protected readonly ILogger logger;

        public Zhongjie(ILogger<Zhongjie> logger)
        {
            this.logger = logger;//.CreateLogger(GetType());
        }
        public Zhongjie()
        {
            logger = SimpleLogger.Instance;
        }

        //上次记录报警日志的时间
        DateTime bjsj = DateTime.Now.AddYears(-100);

        internal protected void LogDebug()
        {
            int k = weituos.Sum(x => x.Value.Count);
            var msg = $"事件总线{GetHashCode()}，事件类型数量：{weituos.Count}，总委托数：{k}";
            if (k >= 10 * 10000)
            {
                //生产环境中，这里可能产生大量日志k，所以处理下
                if ((DateTime.Now - bjsj).TotalSeconds >= 59)
                {
                    //报警时可以进一步打印 委托的最后时间，观察事件处理委托多久没被调用了
                    logger.LogWarning(msg + string.Join(',', weituos.Keys));
                    bjsj = DateTime.Now;
                }
            }
            else
            {
                logger.LogDebug(msg);
            }
        }

        #region 注册
        /// <summary>
        /// 注册事件处理程序
        /// </summary>
        /// <typeparam name="T">事件参数类型</typeparam>
        /// <param name="weituo">事件发生时要执行的委托</param>
        /// <param name="eventName">事件名，若为空则取typeof(T).FullName</param>
        /// <returns>返回IDispose，释放它将注销此事件</returns>
        public virtual IDisposable Zhuce<T>(Func<T, ValueTask> weituo, string eventName = default)// where T : class
        {
            if (eventName.IsNullOrWhiteSpaceBXJG())
                eventName = typeof(T).FullName;

            //var t = typeof(T);

            //weituos.TryAdd(eventName, new ConcurrentDictionary<Delegate, Weituo>());

            //注册相同委托时，覆盖没意义
            //this[eventName].TryRemove(weituo);

            //weituos[eventName].TryAdd(weituo, new Weituo { Func = oo => weituo((T)oo), AddTime = DateTime.Now });
            var sj = weituos.GetOrAdd(eventName, new ConcurrentDictionary<Delegate, Weituo>());
            //触发时有参的用的object，所以这里的委托要形成包装
            sj.TryAdd(weituo, new Weituo { Func = (object arg)=> weituo((T)arg ), AddTime = DateTime.Now, IsParameterless =false });

            logger.LogDebug($"注册事件：{eventName}");

            LogDebug();

            // TryAdd(typeof(T), oo => weituo(oo as T));
            return new ZhongjieZhuxiaoqi(this, eventName, weituo);
        }
        /// <summary>
        /// 注册事件处理程序
        /// </summary>
        /// <param name="weituo">事件发生时要执行的委托</param>
        /// <param name="eventName">事件名，如：有新的好友请求</param>
        /// <returns>返回IDispose，释放它将注销此事件</returns>
        public virtual IDisposable Zhuce(Func<ValueTask> weituo, string eventName)
        {
            //weituos.TryAdd(eventName, new ConcurrentDictionary<Delegate, Weituo>());
            //weituos[eventName].TryAdd(weituo, new Weituo { Func = o => weituo(), AddTime = DateTime.Now });

            var sj = weituos.GetOrAdd(eventName, new ConcurrentDictionary<Delegate, Weituo>());
            sj.TryAdd(weituo, new Weituo { Func = weituo, AddTime = DateTime.Now, IsParameterless = true });


            logger.LogDebug($"注册事件：{eventName}");

            LogDebug();
            return new ZhongjieZhuxiaoqi(this, eventName, weituo);
            // TryAdd(typeof(T), oo => weituo(oo as T));
        }
        /// <summary>
        /// 注册带有层次的事件
        /// 如：应用、租户、用户、http连接等范围
        /// 事件名_level1_level2
        /// </summary>
        /// <typeparam name="T">事件参数类型</typeparam>
        /// <param name="weituo">事件发生时要执行的委托</param>
        /// <param name="eventName">事件名，如：有新的好友请求</param>
        /// <param name="level">如：应用、租户、用户、http连接等范围 事件名_level1_level2</param>
        /// <returns></returns>
        public virtual IDisposable Zhuce<T>(Func<T, ValueTask> weituo, string eventName = default, params string[] level)
        {
            if (eventName.IsNullOrWhiteSpaceBXJG())
                eventName = typeof(T).FullName;
            eventName = BuildEventName(eventName, level);

            return Zhuce(weituo, eventName);
        }
        /// <summary>
        /// 注册带有层次的事件
        /// 如：应用、租户、用户、http连接等范围
        /// 事件名_level1_level2
        /// </summary>
        /// <param name="weituo">事件发生时要执行的委托</param>
        /// <param name="eventName">事件名，如：有新的好友请求</param>
        /// <param name="level">如：应用、租户、用户、http连接等范围 事件名_level1_level2</param>
        /// <returns></returns>
        public virtual IDisposable Zhuce(Func<ValueTask> weituo, string eventName, params string[] level)
        {
            eventName = BuildEventName(eventName, level);

            return Zhuce(weituo, eventName);
        }
        #endregion

        #region 注销
        /// <summary>
        /// 注销指定事件（typeof(T).FullName）中的指定委托
        /// </summary>
        /// <typeparam name="T">事件参数类型，typeof(T).FullName将作为事件名</typeparam>
        /// <param name="weituo">事件发生时要执行的委托</param>
        public virtual void Zhuxiao<T>(Delegate weituo)
        {
            var eventName = typeof(T).FullName;
            Zhuxiao(weituo, eventName);
        }
        /// <summary>
        /// 注销指定事件中的指定委托
        /// </summary>
        /// <param name="weituo">事件发生时要执行的委托</param>
        /// <param name="eventName">事件名，若为空则遍历整个事件中心尝试注销weituo</param>
        public virtual void Zhuxiao(Delegate weituo, string eventName = default)
        {
            if (eventName.IsNotNullOrWhiteSpaceBXJG())
            {
                if (weituos.TryGetValue(eventName, out var dic))
                {
                    dic.TryRemove(weituo, out _);
                }
            }
            else
            {
                foreach (var dic in weituos)
                {
                    if (dic.Value.TryRemove(weituo, out _))
                        break;
                }
            }
            logger.LogDebug($"注销事件：{eventName}");
            LogDebug();
        }
        /// <summary>
        /// 注销指定事件下的所有处理程序
        /// </summary>
        /// <param name="eventName">若为空则清空整个事件总线，否则清空指定事件下的委托列表</param>
        public virtual void Zhuxiao(string eventName = default)
        {
            if (eventName.IsNullOrWhiteSpaceBXJG())
                weituos.Clear();
            else
                weituos.TryRemove(eventName, out _);

            logger.LogDebug($"注销事件：{eventName}");
            LogDebug();
        }
        /// <summary>
        /// 注销指定事件下的所有处理程序
        /// </summary>
        /// <typeparam name="T">事件参数类型，typeof(T).FullName将作为事件名</typeparam>
        public virtual void Zhuxiao<T>()
        {
            var eventName = typeof(T).FullName;
            Zhuxiao(eventName);
        }
        /// <summary>
        /// 批量注销事件
        /// </summary>
        /// <param name="where">条件表达式</param>
        /// <param name="weituo">委托，若为空则每个事件下所有的委托都将被注销</param>
        public virtual void Zhuxiao(Func<KeyValuePair<string, ConcurrentDictionary<Delegate, Weituo>>, bool> where, Delegate weituo = default)
        {
            var items = weituos.Where(where);
            if (weituo == default)
                foreach (var item in items)
                {
                    Zhuxiao(item.Key);
                }
            else
                foreach (var item in items)
                {
                    Zhuxiao(weituo, item.Key);
                }
        }
        /// <summary>
        /// 批量注销指定事件
        /// </summary>
        /// <param name="weituo"></param>
        /// <param name="eventName"></param>
        /// <param name="level">如：租户id 2，部门id 3，则表示注销这个范围内的所有事件处理程序</param>
        public virtual void Zhuxiao(Delegate weituo, string eventName, params string[] level)
        {
            eventName = BuildEventName(eventName, level);
            Zhuxiao(c => c.Key.StartsWith(eventName), weituo);
        }
        /// <summary>
        /// 注销事件处理程序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="weituo"></param>
        /// <param name="level">如：租户id 2，部门id 3，则表示注销这个范围内的所有事件处理程序</param>
        public virtual void Zhuxiao<T>(Delegate weituo, params string[] level)
        {
            var eventName = typeof(T).FullName;
            Zhuxiao(weituo, eventName, level);
        }
        /// <summary>
        /// 批量注销事件
        /// </summary>
        /// <param name="eventName">事件名称</param>
        /// <param name="level">如：租户id 2，部门id 3，则表示注销这个范围内的所有事件处理程序</param>
        public virtual void Zhuxiao(string eventName, params string[] level)
        {
            eventName = BuildEventName(eventName, level);
            Zhuxiao(c => c.Key.StartsWith(eventName));
        }
        /// <summary>
        /// 批量注销事件
        /// </summary>
        /// <typeparam name="T">事件参数类型，typeof(T).FullName作为事件名</typeparam>
        /// <param name="level">如：租户id 2，部门id 3，则表示注销这个范围内的所有事件处理程序</param>
        public virtual void Zhuxiao<T>(params string[] level)
        {
            var eventName = typeof(T).FullName;
            Zhuxiao(eventName, level);
        }
        #endregion

        #region 触发
        /// <summary>
        /// 触发事件
        /// </summary>
        /// <param name="canshu">事件参数</param>
        /// <param name="eventName">事件名，若为空则取canshu.GetType().FullName</param>
        /// <returns></returns>
        public virtual async Task Chufa(object canshu, string eventName = default)
        {
            if (eventName.IsNullOrWhiteSpaceBXJG())
                eventName = canshu.GetType().FullName;
            logger.LogDebug($"正在触发事件{eventName} ");
            if (weituos.TryGetValue(eventName, out var dic))
            {
                logger.LogDebug($"委托数{dic.Count}");

                //https://github.com/dotnet/runtime/issues/23625
                var tasks = new List<Task>(dic.Count);
                foreach (var kv in dic)
                {
                    tasks.Add(kv.Value.InvokeAsync(canshu).AsTask());
                }
                await Task.WhenAll(tasks);
                //await Task.WhenAll(dic.Select(c => c.Value.InvokeAsync(canshu).AsTask()));
            }
        }
        /// <summary>
        /// 触发事件
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <returns></returns>
        public virtual async Task Chufa(string eventName)
        {
            logger.LogDebug($"正在触发事件{eventName} ");
            if (weituos.TryGetValue(eventName, out var dic))
            {
                logger.LogDebug($"委托数{dic.Count}");

                //https://github.com/dotnet/runtime/issues/23625
                var tasks = new List<Task>(dic.Count);
                foreach (var kv in dic)
                {
                    tasks.Add(kv.Value.InvokeAsync().AsTask());
                }
                await Task.WhenAll(tasks);
                //await Task.WhenAll(dic.Select(c => c.Value.InvokeAsync().AsTask()));
            }
        }
        /// <summary>
        /// 批量触发符合条件的事件
        /// </summary>
        /// <param name="where">条件表达式</param>
        /// <param name="canshu">事件参数</param>
        /// <returns></returns>
        public virtual async Task Chufa(Func<KeyValuePair<string, ConcurrentDictionary<Delegate, Weituo>>, bool> where = default, object canshu = default)
        {
            if (where == default)
                where = c => true;
            var items = weituos.Where(where);
            var ts = new List<Task>();
            foreach (var item in items)
            {
                ts.Add(Chufa(canshu, item.Key));
            }
            await Task.WhenAll(ts);
        }
        /// <summary>
        /// 触发指定层次下的指定事件
        /// </summary>
        /// <param name="canshu">事件参数</param>
        /// <param name="eventName">事件名称，若为空则取canshu.GetType().FullName</param>
        /// <param name="level">如：租户id 2，部门id 3，则表示只在这个范围内触发指定事件</param>
        /// <returns></returns>
        public virtual async Task Chufa(object canshu, string eventName = default, params string[] level)
        {
            if (eventName.IsNullOrWhiteSpaceBXJG())
                eventName = canshu.GetType().FullName;
            eventName = BuildEventName(eventName, level);

            await Chufa(c => c.Key.StartsWith(eventName), canshu);
        }
        /// <summary>
        /// 触发指定层次下的指定事件
        /// </summary>
        /// <param name="eventName">事件名称</param>
        /// <param name="level">如：租户id 2，部门id 3，则表示只在这个范围内触发指定事件</param>
        /// <returns></returns>
        public virtual Task Chufa(string eventName, params string[] level)
        {

            eventName = BuildEventName(eventName, level);
            return Chufa(eventName);
        }
        #endregion

        #region 辅助
        static string BuildEventName(string eventName, params string[] level)
        {
            if (level.Any())
                eventName += "_" + string.Join('_', level);
            return eventName;
        }
        /// <summary>
        /// 封装要执行的委托
        /// </summary>
        public class Weituo
        {
            ///// <summary>
            ///// 原始委托
            ///// </summary>
            //public Delegate Act { get; set; }
            /// <summary>
            /// 转换后的委托
            /// </summary>
            public Delegate Func { get; set; }
            /// <summary>
            /// 注册时间
            /// </summary>
            public DateTime AddTime { get; set; } = DateTime.Now;
            /// <summary>
            /// 最后执行时间
            /// </summary>
            public DateTime LastExecuteTime { get; set; } = DateTime.MinValue;

            // 增加委托类型标记
            public bool IsParameterless { get; set; }

            // 优化后的调用方法
            public ValueTask InvokeAsync(object arg = null)
            {
                LastExecuteTime = DateTime.Now;
                return IsParameterless
                    ? ((Func<ValueTask>)Func)()
                    : ((Func<object, ValueTask>)Func)(arg);
            }
        }

        internal class ZhongjieZhuxiaoqi : IDisposable
        {
            private readonly Zhongjie zhongjie;
            private readonly string eventName;
            private readonly Delegate act;

            public ZhongjieZhuxiaoqi(Zhongjie zj, string eventName, Delegate act)
            {
                zhongjie = zj;
                this.eventName = eventName;
                this.act = act;
            }

            public void Dispose()
            {
                zhongjie.Zhuxiao(act, eventName);
            }
        }

        #endregion

        /// <summary>
        /// 整个应用全局的
        /// </summary>
        public static readonly Zhongjie Instance = new Zhongjie();
        /// <summary>
        /// 当前线程全局的
        /// </summary>
        public static readonly AsyncLocal<Zhongjie> Current = new AsyncLocal<Zhongjie>();
    }
}