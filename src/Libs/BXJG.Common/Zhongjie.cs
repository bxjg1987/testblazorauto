using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace BXJG.Common
{

    /*
     * 注意：abp文档事件底部已经说明了可以动态注册，所以在abp项目中不需要使用Zhongjie类，直接用abp的就好
     *
     * 触发事件时，触发方并不晓得事件处理方需要同步还是异步，定义事件时也无法确定，因为事件处理程序是动态注册的。
     * 也无法等待返回，因为可能存在多个处理方
     * 通用的办法是假定所有处理方都是异步的，使用ValueTask，触发时使用Task.WhenAll
     */


    public class Zhongjie //: ConcurrentDictionary<string, ConcurrentDictionary<Delegate, Func<object, ValueTask>>>
    {
        //private readonly ConcurrentBag<Weituo> weituos = new ConcurrentBag<Weituo>();//不好做删除

        internal protected readonly ConcurrentDictionary<string, ConcurrentDictionary<Delegate, Weituo>> weituos = new ConcurrentDictionary<string, ConcurrentDictionary<Delegate, Weituo>>();

        //

        // private readonly ConcurrentBag<Func< object, Func<object, ValueTask>>> tongbuChuliqi = new ConcurrentBag<Func< object, Func<object, ValueTask>>>();


        //public void Zhuce(Action act) 
        //{ 

        //}

        //public void Zhuxiao() { 

        //}

        internal protected readonly ILogger logger;

        public Zhongjie(ILoggerFactory logger)
        {
            this.logger = logger.CreateLogger(GetType());
        }

        internal protected void LogDebug()
        {
            int k = 0;
            foreach (var item in weituos)
            {
                k += item.Value.Count;
            }
            logger.LogDebug($"事件类型数量：{weituos.Count}，总委托数：{k}");
        }
        /// <summary>
        /// 注册事件处理程序
        /// </summary>
        /// <typeparam name="T">事件参数类型</typeparam>
        /// <param name="weituo">事件发生时要执行的委托</param>
        /// <param name="eventName">事件名，如：有新的好友请求</param>
        /// <returns></returns>
        public virtual IDisposable Zhuce<T>(Func<T, ValueTask> weituo, string eventName = default)// where T : class
        {
            if (eventName.IsNullOrWhiteSpaceBXJG())
                eventName = typeof(T).FullName;

            //  var t = typeof(T);

            weituos.TryAdd(eventName, new ConcurrentDictionary<Delegate, Weituo>());

            //注册相同委托时，覆盖没意义
            //this[eventName].TryRemove(weituo);

            weituos[eventName].TryAdd(weituo, new Weituo { Func = oo => weituo((T)oo), AddTime = DateTime.Now });

            logger.LogDebug($"注册事件：{eventName}");

            LogDebug();
            // TryAdd(typeof(T), oo => weituo(oo as T));
            return new ZhongjieZhuxiaoqi(this, eventName, weituo);
        }
        /// <summary>
        /// 注册事件处理程序
        /// </summary>
        /// <typeparam name="T">事件参数类型</typeparam>
        /// <param name="weituo">事件发生时要执行的委托</param>
        /// <param name="eventName">事件名，如：有新的好友请求</param>
        /// <returns></returns>
        public virtual IDisposable Zhuce(Func<ValueTask> weituo, string eventName)
        {
            weituos.TryAdd(eventName, new ConcurrentDictionary<Delegate, Weituo>());

            weituos[eventName].TryAdd(weituo, new Weituo { Func = o => weituo(), AddTime = DateTime.Now });

            logger.LogDebug($"注册事件：{eventName}");

            LogDebug();
            return new ZhongjieZhuxiaoqi(this, eventName, weituo);
            // TryAdd(typeof(T), oo => weituo(oo as T));
        }
        //不用Delegate做参数类型，因为外面调用时，自动推导调用起来更容易
        public virtual void Zhuxiao<T>(Delegate weituo)
        {
            var eventName = typeof(T).FullName;
            Zhuxiao(weituo, eventName);
        }
        /// <summary>
        /// 注销指定事件下的指定处理程序
        /// </summary>
        /// <param name="weituo"></param>
        /// <param name="eventName"></param>
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
                foreach (var dic in this.weituos)
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
        /// <param name="eventName"></param>
        public virtual void Zhuxiao(string eventName = default)
        {
            if (eventName.IsNullOrWhiteSpaceBXJG())
                this.weituos.Clear();


            this.weituos.TryRemove(eventName, out _);
            logger.LogDebug($"注销事件：{eventName}");

            LogDebug();
        }
        /// <summary>
        /// 注销指定事件下的所有处理程序
        /// </summary>
        /// <typeparam name="T">事件参数类型</typeparam>
        public virtual void Zhuxiao<T>()
        {
            var eventName = typeof(T).FullName;
            Zhuxiao(eventName);
        }


        public virtual async Task Chufa(object canshu, string eventName = default)
        {
            if (eventName.IsNullOrWhiteSpaceBXJG())
                eventName = canshu.GetType().FullName;

            if (this.weituos.TryGetValue(eventName, out var dic))
            {
                await Task.WhenAll(dic.Select(c =>
                {
                    c.Value.LastExecuteTime = DateTime.Now; //有线程冲突也无所谓
                    return c.Value.Func(canshu).AsTask();
                }));
            }
        }

        public virtual async Task Chufa(string eventName)
        {
            await Chufa(null, eventName);
            //if (TryGetValue(eventName, out var dic))
            //{
            //    //var func = dic.Select(c=>c.Value).ToImmutableHashSet();
            //    await Task.WhenAll(dic.Select(c => c.Value(null).AsTask()));
            //}
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
    }
    public static class ZhongjieExt
    {
        /// <summary>
        /// 注册带有层次的事件
        /// 如：应用、租户、用户、http连接等范围
        /// 事件名_level1_level2
        /// </summary>
        /// <typeparam name="T">事件参数类型</typeparam>
        /// <param name="zhongjie"></param>
        /// <param name="weituo">事件发生时要执行的委托</param>
        /// <param name="eventName">事件名，如：有新的好友请求</param>
        /// <param name="level">如：应用、租户、用户、http连接等范围 事件名_level1_level2</param>
        /// <returns></returns>
        public static IDisposable Zhuce<T>(this Zhongjie zhongjie, Func<T, ValueTask> weituo, string eventName = default, params string[] level)
        {
            if (eventName.IsNullOrWhiteSpaceBXJG())
                eventName = typeof(T).FullName;

            eventName = eventName.BuildEventName(level);

            return zhongjie.Zhuce(weituo, eventName);
        }
        /// <summary>
        /// 注册带有层次的事件
        /// 如：应用、租户、用户、http连接等范围
        /// 事件名_level1_level2
        /// </summary>
        /// <typeparam name="T">事件参数类型</typeparam>
        /// <param name="zhongjie"></param>
        /// <param name="weituo">事件发生时要执行的委托</param>
        /// <param name="eventName">事件名，如：有新的好友请求</param>
        /// <param name="level">如：应用、租户、用户、http连接等范围 事件名_level1_level2</param>
        /// <returns></returns>
        public static IDisposable Zhuce(this Zhongjie zhongjie, Func<ValueTask> weituo, string eventName, params string[] level)
        {
            eventName = eventName.BuildEventName(level);

            return zhongjie.Zhuce(weituo, eventName);
        }

        /// <summary>
        /// 批量注销指定事件
        /// </summary>
        /// <param name="zhongjie"></param>
        /// <param name="weituo"></param>
        /// <param name="eventName"></param>
        ///  <param name="level">如：应用、租户、用户、http连接等范围 事件名_level1_level2</param>
        public static void Zhuxiao(this Zhongjie zhongjie, Delegate weituo, string eventName, params string[] level)
        {
            eventName = eventName.BuildEventName(level);

            var items = zhongjie.weituos.Where(c => c.Key.StartsWith(eventName));
            foreach (var item in items)
            {
                zhongjie.Zhuxiao(weituo, item.Key);
            }
        }
        /// <summary>
        /// 注销事件处理程序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="zhongjie"></param>
        /// <param name="weituo"></param>
        /// <param name="level">如：应用、租户、用户、http连接等范围 事件名_level1_level2</param>
        public static void Zhuxiao<T>(this Zhongjie zhongjie, Delegate weituo, params string[] level)
        {
            var eventName = typeof(T).FullName;
            zhongjie.Zhuxiao(weituo, eventName, level);
        }
        /// <summary>
        /// 批量注销事件
        /// </summary>
        /// <param name="zhongjie"></param>
        /// <param name="eventName"></param>
        ///  <param name="level">如：应用、租户、用户、http连接等范围 事件名_level1_level2</param>
        public static void Zhuxiao(this Zhongjie zhongjie, string eventName, params string[] level)
        {
            eventName = eventName.BuildEventName(level);

            var items = zhongjie.weituos.Where(c => c.Key.StartsWith(eventName));

            foreach (var item in items)
            {
                zhongjie.Zhuxiao(item.Key);
            }
        }

        private static string BuildEventName(this string eventName, params string[] level)
        {
            if (level.Any())
                eventName += "_" + string.Join('_', level);
            return eventName;
        }
        /// <summary>
        /// 批量注销事件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="zhongjie"></param>
        ///  <param name="level">如：应用、租户、用户、http连接等范围 事件名_level1_level2</param>
        public static void Zhuxiao<T>(this Zhongjie zhongjie, params string[] level)
        {
            var eventName = typeof(T).FullName;
            zhongjie.Zhuxiao(eventName, level);
        }

        public static async Task Chufa(this Zhongjie zhongjie, object canshu, string eventName = default, params string[] level)
        {
            if (eventName.IsNullOrWhiteSpaceBXJG())
                eventName = canshu.GetType().FullName;

            eventName = eventName.BuildEventName(level);

            var items = zhongjie.weituos.Where(c => c.Key.StartsWith(eventName));
            var ts = new List<Task>();
            foreach (var item in items)
            {
                ts.Add(zhongjie.Chufa(canshu, item.Key));
            }
            await Task.WhenAll(ts);
        }
        /// <summary>
        /// 触发指定层次下的指定事件
        /// </summary>
        /// <param name="zhongjie"></param>
        /// <param name="eventName"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public static async Task Chufa(this Zhongjie zhongjie, string eventName, params string[] level)
        {
            //eventName = eventName.BuildEventName(level);
            //var items = zhongjie.Where(c => c.Key.StartsWith(eventName));
            //var ts = new List<Task>();
            //foreach (var item in items)
            //{
            //    ts.Add(zhongjie.Chufa(item.Key));
            //}
            //await Task.WhenAll(ts);
            await zhongjie.Chufa(null, eventName, level);
        }
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
        public Func<object, ValueTask> Func { get; set; }
        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime AddTime { get; set; } = DateTime.Now;
        /// <summary>
        /// 最后执行时间
        /// </summary>
        public DateTime LastExecuteTime { get; set; } = DateTime.MinValue;
    }
}