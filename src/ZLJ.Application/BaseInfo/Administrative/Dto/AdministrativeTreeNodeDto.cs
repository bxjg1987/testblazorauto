using BXJG.GeneralTree;
using ZLJ.BaseInfo.Administrative;

namespace ZLJ.BaseInfo.Administrative.Dto
{
    public class AdministrativeTreeNodeDto : GeneralTreeNodeDto<AdministrativeTreeNodeDto>
    {
        public AdministrativeLevel Level { get; set; }
    }
}
