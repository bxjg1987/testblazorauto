using BXJG.Utils.GeneralTree;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Metadata
{
    /// <summary>
    /// 元数据
    /// </summary>
    [Table("BXJGUtilsMetadata")]
    public class MetadataEntity : GeneralTreeNoTenantEntity<MetadataEntity>
    {
        //默认都是系统预设的
    }
}