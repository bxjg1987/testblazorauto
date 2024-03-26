using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BXJG.Common
{

    /*
     * 暂时决定不实现了，需要的地方直接用缓存临时解决下吧
     * 
     * 有时候希望给某个实例附加一些属性，但这个实例的类我们无法修改源码。
     * 可以定义一个字典，用此实例作为key，value是另一个用来存放额外状态的对象
     * 这里是必须要静态属性存储的，导致实例会被静态属性引用，实例就无法像原来一样自动释放
     * 为了兜底，需要做个过期策略，若某个对象超时了，自动从静态属性引用中剔除它
     * 
     * 由于整个系统中使用这个单例，或者静态对象，所以需要缩小范围，我们就用实例类型来缩小范围吧
     * 
     * 保险起见，强制过期，所以这些状态只能是临时性的，过期后对象实例不再被引用，从而能回收
     * 不能为单个状态设置过期，因为我们要回收的是对象
     * 对象过期可以是滑动的，也可以是固定的，
     */
    ///// <summary>
    ///// 对象挎斗，为对象添加额外状态，
    ///// 实例会在
    ///// </summary>
    //public class ObjectSideCar : IDisposable
    //{
    //    /// <summary>
    //    /// 核心存储，
    //    /// string实例类型名称或其它唯一名称，用来缩小范围
    //    /// object实例对象
    //    /// Item额外状态以及时间戳
    //    /// </summary>
    //    readonly ConcurrentDictionary<string, ConcurrentDictionary<object, Item>> items = new ConcurrentDictionary<string, ConcurrentDictionary<object, Item>>();

    //    readonly CancellationTokenSource tokenSource = new CancellationTokenSource();
    //    public ObjectSideCar()
    //    {
    //        //开启线程，清理过期项
    //        Task.Factory.StartNew(async () =>
    //        {
    //            while (!tokenSource.IsCancellationRequested)
    //            {
    //                await Task.Delay(1);
    //                foreach (var item in items)
    //                {
    //                    var sdf = item.Value.Where(c => c.Value.CreatedTime.AddMinutes(30) < DateTime.Now).ToArray();
                        
    //                }
    //            }
    //        }, TaskCreationOptions.LongRunning);
    //    }

    //    public void Dispose()
    //    {
    //        //结束清理过期项的线程
    //    }
    //    /// <summary>
    //    /// 向对象添加额外状态
    //    /// </summary>
    //    /// <param name="obj"></param>
    //    /// <param name="propertyName"></param>
    //    /// <param name="state"></param>
    //    /// <param name="outcls">最外层的key，若不设置则使用obj的类型全名</param>
    //    public void Set(object obj, string propertyName, object state, string outcls = default)
    //    {
    //        var item1 = items.GetOrAdd(outcls ?? obj.GetType().FullName, s => new ConcurrentDictionary<object, Item>());
    //        var item2 = item1.GetOrAdd(obj, s => new Item { CreatedTime = DateTime.Now, Instance = obj, States = new Dictionary<string, object>() });
    //        item2.States[propertyName] = state;
    //    }
    //    /// <summary>
    //    /// 从对象获取额外状态
    //    /// </summary>
    //    /// <param name="obj"></param>
    //    /// <param name="propertyName"></param>
    //    /// <param name="outcls">最外层的key，若不设置则使用obj的类型全名</param>
    //    public object Get(object obj, string propertyName, string outcls = default)
    //    {
    //        if (items.TryGetValue(outcls ?? obj.GetType().FullName, out var item1))
    //        {
    //            if (item1.TryGetValue(obj, out var item2))
    //            {
    //                if (item2.States.TryGetValue(propertyName, out var item3))
    //                    return item3;
    //            }
    //        }
    //        return default;
    //    }

    //    class Item
    //    {
    //        public object Instance { get; set; }
    //        /// <summary>
    //        /// 过期时间点，同一个类型的对象的不同实例，过期时间可以不同
    //        /// </summary>
    //        public DateTime CreatedTime { get; set; }
    //        public Dictionary<string, object> States { get; set; } //= new Dictionary<string, object>();
    //    }
    //}
}
