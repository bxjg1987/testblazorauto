using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using BXJG.GeneralTree;
using BXJG.Shop.Catalogue;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Shop.Common.Dto
{
    /// <summary>
    /// 通用树形数据更新模型
    /// </summary>
    [AutoMapTo(typeof(BXJGShopDictionaryEntity))]
    public class DictionaryEditDto : GeneralTreeNodeEditBaseDto
    {
        public string Icon { get; set; }
        //public bool IsSysDefine { get; set; }
        public bool IsTree { get; set; }
    }

}
