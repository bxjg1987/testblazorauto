using BXJG.Utils.Application.Share.GeneralTree;
using BXJG.Utils.GeneralTree;
using ZLJ.Application.Admin.BaseInfo.Administrative;
using ZLJ.Core.BaseInfo.Administrative;

namespace ZLJ.Application.Admin.BaseInfo.Administrative.Dto
{
    public class AdministrativeEditDto : GeneralTreeNodeEditBaseDto
    {
        public AdministrativeLevel Level { get; set; }
    }
}
