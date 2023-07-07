using BXJG.Utils.GeneralTree;
using ZLJ.App.Admin.BaseInfo.Administrative;
using ZLJ.BaseInfo.Administrative;

namespace ZLJ.App.Admin.BaseInfo.Administrative.Dto
{
    public class AdministrativeEditDto : GeneralTreeNodeEditBaseDto
    {
        public AdministrativeLevel Level { get; set; }
    }
}
