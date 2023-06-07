using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Common.DongtaiZhongjie
{

    /*
     * 触发事件时，触发方并不晓得事件处理方需要同步还是异步，定义事件时也无法确定，因为事件处理程序是动态注册的。
     * 也无法等待返回，因为可能存在多个处理方
     * 通用的办法是假定所有处理方都是异步的，使用ValueTask，触发时使用Task.WhenAll
     * 
     * 每个事件一个中介，这样事情会变得简单，一个中介只处理一个事件
     * 
     * 触发时，调用开始注册的委托，给你一个事件参数，你返回一个事件处理的委托。
     */

    /// <summary>
    /// 有参事件中介
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Zhongjie<T> : HashSet<Func<T, ValueTask>>
    {
        //

        // private readonly ConcurrentBag<Func< object, Func<object, ValueTask>>> tongbuChuliqi = new ConcurrentBag<Func< object, Func<object, ValueTask>>>();


        //public void Zhuce(Action act) 
        //{ 

        //}

        //public void Zhuxiao() { 

        //}

        public virtual async ValueTask Chufa(T canshu)
        {
           // var wts = this.Select(c => c(canshu)).Where(c => c != default);
            await Task.WhenAll(this.Select(c => c(canshu).AsTask()));
        }
    }
    /// <summary>
    /// 无参事件中介
    /// 这个就不是每个事件一个子类了，而是一个子类处理所有事件
    /// </summary>
    public class Zhongjie : HashSet<Func<string, ValueTask>>
    {
        //

        // private readonly ConcurrentBag<Func< object, Func<object, ValueTask>>> tongbuChuliqi = new ConcurrentBag<Func< object, Func<object, ValueTask>>>();


        //public void Zhuce(Action act) 
        //{ 

        //}

        //public void Zhuxiao() { 

        //}

        public virtual async ValueTask Chufa(string shijian)
        {
           // var wts = this.Select(c => c(shijian)).Where(c => c != default);
            await Task.WhenAll(this.Select(c => c(shijian).AsTask()));
        }
    }
}