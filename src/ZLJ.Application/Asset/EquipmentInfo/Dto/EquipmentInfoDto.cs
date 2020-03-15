using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using BXJG.Attachment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using ZLJ.ABPFile;
//using ZLJ.Attachment;

namespace ZLJ.Asset
{
    /// <summary>
    /// 设备档案查询模型
    /// </summary>
    [AutoMapFrom(typeof(EquipmentInfoEntity))]
    public class EquipmentInfoDto : FullAuditedEntityDto<long>
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 分类Id
        /// </summary>
        public long AreaId { get; set; }
        /// <summary>
        /// 设备类别名称
        /// </summary>
        public string AreaDisplayName { get; set; }
        /// <summary>
        /// 规格型号
        /// </summary>
        public string Size { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public double Longitude { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public double Latitude { get; set; }

        public double Distance { get; set; }
    }
}
