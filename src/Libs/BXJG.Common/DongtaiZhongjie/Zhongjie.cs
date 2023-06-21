using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace BXJG.Common.DongtaiZhongjie
{

    /*
     * 触发事件时，触发方并不晓得事件处理方需要同步还是异步，定义事件时也无法确定，因为事件处理程序是动态注册的。
     * 也无法等待返回，因为可能存在多个处理方
     * 通用的办法是假定所有处理方都是异步的，使用ValueTask，触发时使用Task.WhenAll
     */


    public class Zhongjie : ConcurrentDictionary<string, ConcurrentDictionary<Delegate, Func<object, ValueTask>>>
    {
        //

        // private readonly ConcurrentBag<Func< object, Func<object, ValueTask>>> tongbuChuliqi = new ConcurrentBag<Func< object, Func<object, ValueTask>>>();


        //public void Zhuce(Action act) 
        //{ 

        //}

        //public void Zhuxiao() { 

        //}

        protected readonly ILogger logger;

        public Zhongjie(ILoggerFactory logger)
        {

            this.logger = logger.CreateLogger(this.GetType());
        }

        protected void LogDebug()
        {
            int k = 0;
            foreach (var item in this)
            {
                k += item.Value.Count;
            }
            logger.LogDebug($"事件类型数量：{this.Count}，总委托数：{k}");
        }

        public virtual void Zhuce<T>(Func<T, ValueTask> weituo, string eventName = default) where T : class
        {
            if (eventName.IsNullOrWhiteSpaceBXJG())
                eventName = typeof(T).FullName;

            //  var t = typeof(T);

            TryAdd(eventName, new ConcurrentDictionary<Delegate, Func<object, ValueTask>>());

            this[eventName].TryAdd(weituo, oo => weituo(oo as T));

            logger.LogDebug($"注册事件：{eventName}");

            LogDebug();
            // TryAdd(typeof(T), oo => weituo(oo as T));
        }
        public virtual void Zhuce(Func<ValueTask> weituo, string eventName)
        {
            TryAdd(eventName, new ConcurrentDictionary<Delegate, Func<object, ValueTask>>());

            this[eventName].TryAdd(weituo, o => weituo());

            logger.LogDebug($"注册事件：{eventName}");

            LogDebug();
            // TryAdd(typeof(T), oo => weituo(oo as T));
        }
        //不用Delegate做参数类型，因为外面调用时，自动推导调用起来更容易
        public virtual void Zhuxiao<T>(Func<T, ValueTask> weituo)
        {
            var eventName = typeof(T).FullName;
            if (TryGetValue(eventName, out var dic))
            {
                dic.TryRemove(weituo, out _);
            }
            logger.LogDebug($"注销事件：{eventName}");
            LogDebug();
        }

        public virtual void Zhuxiao(Delegate weituo, string eventName = default)
        {
            if (eventName.IsNotNullOrWhiteSpaceBXJG())
            {
                if (TryGetValue(eventName, out var dic))
                {
                    dic.TryRemove(weituo, out _);
                }
            }
            else
            {
                foreach (var dic in this)
                {
                    if(dic.Value.TryRemove(weituo, out _)) 
                        break;
                }
            }
            logger.LogDebug($"注销事件：{eventName}");
            LogDebug();
        }

        public virtual async ValueTask Chufa(object canshu, string eventName = default)
        {
            if (eventName.IsNullOrWhiteSpaceBXJG())
                eventName = canshu.GetType().FullName;

            if (TryGetValue(eventName, out var dic))
            {
                await Task.WhenAll(dic.Select(c => c.Value(canshu).AsTask()));
            }
        }

        public virtual async ValueTask Chufa(string eventName)
        {
            if (TryGetValue(eventName, out var dic))
            {
                await Task.WhenAll(dic.Select(c => c.Value(null).AsTask()));
            }
        }
    }

    ///// <summary>
    ///// 有参事件中介 
    ///// Type事件（参数）类型
    ///// 委托 要执行的委托，key是原始委托，value是转换后的委托
    ///// </summary>
    //public class Zhongjie : ConcurrentDictionary<Type, ConcurrentDictionary<Delegate, Func<object, ValueTask>>>
    //{
    //    //

    //    // private readonly ConcurrentBag<Func< object, Func<object, ValueTask>>> tongbuChuliqi = new ConcurrentBag<Func< object, Func<object, ValueTask>>>();


    //    //public void Zhuce(Action act) 
    //    //{ 

    //    //}

    //    //public void Zhuxiao() { 

    //    //}

    //    public virtual void Zhuce<T>(Func<T, ValueTask> weituo) where T : class
    //    {
    //        var t = typeof(T);

    //        TryAdd(t, new ConcurrentDictionary<Delegate, Func<object, ValueTask>>());

    //        this[t].TryAdd(weituo, oo => weituo(oo as T));

    //        // TryAdd(typeof(T), oo => weituo(oo as T));
    //    }

    //    public virtual void Zhuxiao<T>(Func<T, ValueTask> weituo)
    //    {
    //        var t = typeof(T);
    //        if (TryGetValue(t, out var dic))
    //        {
    //            dic.TryRemove(weituo, out _);
    //        }


    //        // TryRemove(weituo.GetHashCode(), out _);
    //    }

    //    public virtual async ValueTask Chufa(object canshu)
    //    {
    //        var t = canshu.GetType();
    //        if (TryGetValue(t, out var dic))
    //        {
    //            await Task.WhenAll(dic.Select(c => c.Value(canshu).AsTask()));
    //        }
    //        // await Task.WhenAll(this.Select(c => c.Value(canshu).AsTask()));
    //    }
    //}
    ///// <summary>
    ///// 无参事件中介
    ///// </summary>
    //public class ZhongjieWithoutParam : ConcurrentDictionary<string, HashSet<Func<ValueTask>>>
    //{
    //    public virtual void Zhuce(string e, Func<ValueTask> func)
    //    {
    //        this.TryAdd(e, new HashSet<Func<ValueTask>>());
    //        this[e].Add(func);
    //    }

    //    public virtual void Zhuxiao(Func<ValueTask> func)
    //    {
    //        foreach (var item in this)
    //        {
    //            item.Value.Remove(func);
    //        }
    //    }

    //    public virtual async ValueTask Chufa(string shijian)
    //    {
    //        if (TryGetValue(shijian, out var dic))
    //        {
    //            await Task.WhenAll(dic.Select(c => c().AsTask()));
    //        }
    //    }
    //}
}