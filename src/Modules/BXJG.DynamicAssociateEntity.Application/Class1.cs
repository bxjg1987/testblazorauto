using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Dynamic;
using Abp.UI;

namespace BXJG.DynamicAssociateEntity
{
    public class Class1
    {
        //protected readonly DynamicAssociateEntityDefineManager dynamicAssociateEntityDefineManager;

        //public Class1(DynamicAssociateEntityDefineManager dynamicAssociateEntityDefineManager)
        //{
        //    this.dynamicAssociateEntityDefineManager = dynamicAssociateEntityDefineManager;
        //}

        public static string DtoMapToEntity(IReadOnlyList<DefineMapItem> defines, IDictionary<string, object> dto)
        {
            #region 数据格式
            /*
             * dto
             * [
             *      { "a" : 3 },
             *      { "b" : 6 }
             * ]
             * 
             * entity
             * [
             *      { "a" : 3 },
             *      { "b" : 6 ,"child":{ "p":734 }}
             * ]
             */
            #endregion

            var targets = new List<object>();
            foreach (var item in defines)
            {
                var define = item.Define;
                dynamic parent = null;
                while (define != null)
                {
                    if (!dto.ContainsKey(define.Name))
                  
                        if (item.Required)
                            throw new UserFriendlyException($"{define.Name}为必填");


                  
                        var entityItem = new ExpandoObject();
                        (entityItem as IDictionary<string, object>)[define.Name] = dto[define.Name];
                        
                   
                }
            }
            return JsonSerializer.Serialize(targets);
        }
    }
}
