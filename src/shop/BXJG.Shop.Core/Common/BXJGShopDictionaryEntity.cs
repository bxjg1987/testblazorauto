using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;
using BXJG.GeneralTree;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BXJG.Shop.Common
{
    /// <summary>
    /// 商城模块自己的通用字典
    /// </summary>
    [Table("BXJGShopDictionaries")]
    public class BXJGShopDictionaryEntity : GeneralTreeEntity<BXJGShopDictionaryEntity>
    {
        public const int IconMaxLength = 500;

        [Column(TypeName= "varchar(500)")]
        [MaxLength(IconMaxLength)]
        public string Icon { get; set; }
    }
}
