using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using BXJG.Common.Contracts;

using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ZLJ.Application.Share.AssociatedCompany
{
    /// <summary>
    /// 
    /// </summary>
    public class AssociatedCompanyEditDto : EntityDto<long>, IPassivable//, IReset
    {
        //若需要保留大量数据时，使用DeepCloner
        //public void Reset()
        //{
        //    Name = default;
        //    TaxNo = default;
        //    LinkMan = default;
        //    LinkPhone = default;
        //    Address = default;
        //    Lng = default;
        //    Lat = default;
        //}

        public AssociatedCompanyEditDto() {
           //Reset();
        }

        [DisplayName("是否启用")]
        public bool IsActive { get; set; } = true;
        //public string? Pinyin => TinyPinyin.PinyinHelper.GetPinyinInitials(Name);//.GetPinYinFirstLetter();
        /// <summary>
        /// 名称
        /// </summary>
        [DisplayName("客户名称")]
        [Display(Name="客户名称")]
        [Required]
        [StringLength(ZLJ.Core.Share.ZLJConsts.AssociatedCompanyNameMaxLength)]
        public string Name { get; set; }
        /// <summary>
        /// 税号
        /// </summary>
        [DisplayName("税号")]
        [StringLength(ZLJ.Core.Share.ZLJConsts.AssociatedCompanyTaxNoMaxLength)]
        public string? TaxNo { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        [DisplayName("联系人")]
        [StringLength(ZLJ.Core.Share.ZLJConsts.AssociatedCompanyLinkManMaxLength)]
        public string? LinkMan { get; set; }
        //public string? LinkManPinyin => LinkMan.IsNotNullOrWhiteSpaceBXJG()? TinyPinyin.PinyinHelper.GetPinyinInitials(LinkMan):default;
        /// <summary>
        /// 联系电话
        /// </summary>
        [DisplayName("联系人电话")]
        [StringLength(ZLJ.Core.Share.ZLJConsts.AssociatedCompanyLinkPhoneMaxLength)]
        public string? LinkPhone { get; set; }
        /// <summary>
        /// 详细地址
        /// </summary>
        [DisplayName("地址")]
        [StringLength(ZLJ.Core.Share.ZLJConsts.AssociatedCompanyAddressMaxLength)]
        public string? Address { get; set; }
        //public string? AddressPinyin => Address.IsNotNullOrWhiteSpaceBXJG()? TinyPinyin.PinyinHelper.GetPinyinInitials(Address):default;
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
        [Display(Name = "所属地区")]
        public long? AreaId { get; set; }
        //[DisplayName("所属地区")]
        //public string? AreaIdString
        //{
        //    get
        //    {
        //        if (!AreaId.HasValue)
        //        {
        //            return null;
        //        }

        //        return AreaId.ToString();
        //    }
        //    set
        //    {
        //        AreaId = (value.IsNotNullOrWhiteSpaceBXJG()&& value!="0" ? long.Parse(value) : null);
        //    }
        //}
        /// <summary>
        /// 客户等级Id
        /// </summary>
        [Display(Name = "级别")]
        public long? LevelId { get; set; }
        //[DisplayName("级别")]
        //public string? LevelIdString
        //{
        //    get
        //    {
        //        if (!LevelId.HasValue)
        //        {
        //            return null;
        //        }

        //        return LevelId.ToString();
        //    }
        //    set
        //    {
        //        LevelId = (value.IsNotNullOrWhiteSpaceBXJG() && value != "0" ? long.Parse(value) : null);
        //    }
        //}

        ///// <summary>
        ///// 客户类别Id
        ///// </summary>
        //public long? CategoryId { get; set; }
        ///// <summary>
        ///// 管理员密码
        ///// </summary>
        //[Required]
        //[MinLength(6)]
        //public string AdminEquipmentPwd { get; set; }
    }
}