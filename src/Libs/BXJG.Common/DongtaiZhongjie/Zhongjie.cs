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

    /// <summary>
    /// 有参事件中介
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Zhongjie : ConcurrentDictionary<int, Func<object, ValueTask>>
    {
        //

        // private readonly ConcurrentBag<Func< object, Func<object, ValueTask>>> tongbuChuliqi = new ConcurrentBag<Func< object, Func<object, ValueTask>>>();


        //public void Zhuce(Action act) 
        //{ 

        //}

        //public void Zhuxiao() { 

        //}

        public virtual void Zhuce<T>(Func<T, ValueTask> weituo) where T : class
        {
            TryAdd(weituo.GetHashCode(), oo => weituo(oo as T));
        }

        public virtual void Zhuxiao<T>(Func<T, ValueTask> weituo)
        {
            TryRemove(weituo.GetHashCode(), out _);
        }
        public virtual void Zhuxiao(int hashCode)
        {
            TryRemove(hashCode, out _);
        }
        public virtual async ValueTask Chufa(object canshu)
        {
            await Task.WhenAll(this.Select(c => c.Value(canshu).AsTask()));
        }
    }
    /// <summary>
    /// 无参事件中介
    /// </summary>
    public class ZhongjieWithoutParam : HashSet<Func<string, ValueTask>>
    {
        public virtual async ValueTask Chufa(string shijian)
        {
            await Task.WhenAll(this.Select(c => c(shijian).AsTask()));
        }
    }
}