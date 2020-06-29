using Abp.Dependency;
using Abp.Domain.Services;
using Abp.Events.Bus;
using Abp.Events.Bus.Entities;
using BXJG.GeneralTree;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Shop.Catalogue
{
    /// <summary>
    /// 上架信息领域服务
    /// 内部会触发相应领域事件
    /// </summary>
    /// <typeparam name="TDataDictionary"></typeparam>
    public class ItemManager<TDataDictionary> : BXJGShopDomainServiceBase
    //where TDataDictionary: GeneralTreeEntity<TDataDictionary>
    {
        /// <summary>
        /// 发布此商品
        /// </summary>
        /// <param name="yxq">开始发布时间，默认当前时间</param>
        /// <param name="js">结束时间</param>
        public Task PublishAsync(ItemEntity<TDataDictionary> item, DateTimeOffset? yxq = default, DateTimeOffset? js = default)
        {
            item.Publish(yxq, js);
            return EventBus.TriggerAsync(new ItemPublishChangedEventData<TDataDictionary>(item));
        }
        /// <summary>
        /// 发布此商品
        /// </summary>
        /// <param name="yxq">开始发布时间，默认当前时间</param>
        /// <param name="js">有效期，单位秒</param>
        public Task PublishDurationAsync(ItemEntity<TDataDictionary> item, DateTimeOffset? yxq = default, long js = 60 * 60 * 24 * 365 * 10)
        {
            yxq = yxq ?? DateTimeOffset.Now;
            return PublishAsync(item, yxq, yxq.Value.AddSeconds(js));
        }
        /// <summary>
        /// 取消发布指定商品
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Task UnPublishAsync(ItemEntity<TDataDictionary> item) {
            item.Published = false;
            return EventBus.TriggerAsync(new ItemPublishChangedEventData<TDataDictionary>(item));
            //item.AvailableStart = null;
        }
    }
}
