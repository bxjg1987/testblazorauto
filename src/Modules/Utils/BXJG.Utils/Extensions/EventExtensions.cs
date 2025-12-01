using Abp.Events.Bus;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Extensions
{
    public static class EventExtensions
    {
        public static void AddOrReplace(this ICollection<IEventData> collection, IEventData eventData) 
        {
            var sf = collection.SingleOrDefault(c => c.GetType() == eventData.GetType());
            if (sf != null)
                collection.Remove(sf);

            collection.Add(eventData);
        }
        public static void Remove<TEventData>(this ICollection<IEventData> collection) 
        {
            var sf = collection.SingleOrDefault(c => c.GetType() == typeof(TEventData));
            if (sf != null)
                collection.Remove(sf);
        }
    }
}
