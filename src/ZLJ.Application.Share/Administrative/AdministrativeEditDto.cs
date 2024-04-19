using BXJG.Utils.Application.Share.GeneralTree;
using ZLJ.Core.Share.Enums;

namespace ZLJ.Application.Share.Administrative
{
    public class AdministrativeEditDto : GeneralTreeNodeEditBaseDto
    {
        public AdministrativeLevel Level { get; set; }
    }
}
