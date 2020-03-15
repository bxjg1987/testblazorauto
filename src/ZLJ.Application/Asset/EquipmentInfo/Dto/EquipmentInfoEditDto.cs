using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.Asset
{
    [AutoMapTo(typeof(EquipmentInfoEntity))]
    public class EquipmentInfoEditDto : EntityDto<long>
    {
        [StringLength(EquipmentInfoEntity.CodeMaxLength)]
        [Required]
        public string Code { get; set; }
       
        /// <summary>
        /// 分类Id
        /// </summary>
        public long AreaId { get; set; }
       
        /// <summary>
        /// 规格型号
        /// </summary>
        [StringLength(EquipmentInfoEntity.SizeMaxLength)]
        public string Size { get; set; }

        /// <summary>
        /// 经度
        /// </summary>
        public double Longitude { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public double Latitude { get; set; }
    }
}
