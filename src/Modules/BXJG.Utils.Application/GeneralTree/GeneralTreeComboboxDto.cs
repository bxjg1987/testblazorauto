/*
 * 作者：变形精怪 手机/微信17723896676 QQ/邮箱453008453
 * 创建时间：2018-10-13 1:07:12
 *
 * 说明：略...
 */
using Abp.Application.Services.Dto;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.GeneralTree
{
    //abp默认提供的CombboxItemDto只是一个辅助类，我们可以定义一个更符合我们要求的辅助类
    //其实用他的也可以，只是不是很方便

       

    /// <summary>
    /// Easyui的Combobox下拉框控件对应数据的模型，它是一个通用模型
    /// </summary>
    /// <typeparam name="TId"></typeparam>
    public class GeneralTreeComboboxDto: ComboboxItemDto
    {
        public string Code { get; set; }

        private string extensionData;

        //对ExtData的赋值本来可以直接在AutoMapper映射中来做
        //但是模块中使用了泛型，加上实现子类可能有更多泛型，AutoMapper好像支持不太好
        //因此AutoMapper映射原始的ExtensionData，在属性内部设置ExtData
        //ExtensionData本身就不需要序列化到前端了

        [JsonIgnore]//目前默认使用的并非.net 3.x的json序列化
        public string ExtensionData
        {
            get
            {
                return extensionData;
            }
            set
            {
                extensionData = value;
                if (string.IsNullOrWhiteSpace(value))
                    ExtData = null;
                else
                    ExtData = JsonConvert.DeserializeObject<dynamic>(value);
            }
        }
        /// <summary>
        /// 扩展属性
        /// </summary>
       // [Ignore]
        public dynamic ExtData { get; private set; }

        public GeneralTreeComboboxDto()
        {

        }
        public GeneralTreeComboboxDto(long? id, string text, string extData = "")
            :base(id.HasValue?id.ToString():null,text)
        {
            ExtensionData = extData;
        }
    }
}
