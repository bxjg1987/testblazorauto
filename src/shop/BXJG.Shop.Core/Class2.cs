using Abp.Domain.Services;
using Abp.Events.Bus.Handlers;
using BXJG.GeneralTree;
using BXJG.Shop.Catalogue;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Shop
{
    //public  class Class2<TEntity> : DomainService, IAsyncEventHandler<ItemPublishChangedEventData<TEntity>>
    //{
    //    public Task HandleEventAsync(ItemPublishChangedEventData<TEntity> eventData) 
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
    public class Class2 : DomainService, IAsyncEventHandler<ItemPublishChangedEventData<ItemEntity>>
    {
        public Task HandleEventAsync(ItemPublishChangedEventData<ItemEntity> eventData)
        {
           return Task.CompletedTask;
            //经过测试 事件触发方和事件处理程序在同一个事务中
           // throw new NotImplementedException("gggggggggggggggggggggggggggggggggggggggggggggggg");
        }
    }
    //public class Class2 : Class2
    //{
    //    public override Task HandleEventAsync(ItemPublishChangedEventData eventData)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
