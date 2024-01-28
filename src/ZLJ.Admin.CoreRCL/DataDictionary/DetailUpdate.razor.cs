
namespace ZLJ.Admin.CoreRCL.DataDictionary
{
    public partial class DetailUpdate
    {
        protected override string FuncName => "数据字典";
        /// <summary>
        /// 
        /// </summary>
        string detailUpdateText => isEdit ? "修改" : "查看";

        string detailUpdateIcon => isEdit ? IconType.Outline.Edit : IconType.Outline.File;

        //protected override Task OnInitializedAsync()
        //{
        //    return base.OnInitializedAsync();
        //}
    }
}
