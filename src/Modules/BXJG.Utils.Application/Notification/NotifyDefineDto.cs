using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Notification
{
    public class NotifyDefineDto
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string EntityType { get; set; }
        public string Description { get;set;}
        public IDictionary<string, object> Attributes { get;  set; }

        public bool Selected { get; set; }
    }
}