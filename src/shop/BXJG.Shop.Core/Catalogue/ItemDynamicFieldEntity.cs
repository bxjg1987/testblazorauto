using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;
using BXJG.GeneralTree;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BXJG.Shop.Catalogue
{
    /// <summary>
    /// 商品动态字段字典类
    /// </summary>
    [Table("BXJGShopItemDynamicFields")]
    public class ItemDynamicFieldEntity : GeneralTreeEntity<ItemDynamicFieldEntity>
    {
        public const int IconMaxLength = 500;

        [Column(TypeName="varchar")]
        [StringLength(IconMaxLength)]
        public string Icon { get; set; }
    }
}
