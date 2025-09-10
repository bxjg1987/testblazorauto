using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ZLJ.Application.BaseInfo.AssociatedCompany.Dto
{
    /// <summary>
    /// 
    /// </summary>
    public class AssociatedCompanyEditDto : EntityDto<long>, IPassivable
    {
        public bool IsActive { get; set; }
        public string Pinyin => TinyPinyin.PinyinHelper.GetPinyinInitials(Name);//.GetPinYinFirstLetter();
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 税号
        /// </summary>
        public string TaxNo { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        public string LinkMan { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string LinkPhone { get; set; }

        /// <summary>
        /// 详细地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 经度
        /// </summary>
        public decimal? Lng { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public decimal? Lat { get; set; }
        /// <summary>
        /// 所属区域Id
        /// </summary>
        public long? AreaId { get; set; }

        /// <summary>
        /// 客户等级Id
        /// </summary>
        public long? LevelId { get; set; }
        /// <summary>
        /// 客户类别Id
        /// </summary>
        public long? CategoryId { get; set; }
        /// <summary>
        /// 管理员密码
        /// </summary>
        [Required]
        [MinLength(6)]
        public string AdminEquipmentPwd { get; set; }
    }
}