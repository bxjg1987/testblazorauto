namespace ZLJ.Admin.CoreRCL.Administrative
{
    public partial class DetailUpdate
    {
        protected override string FuncName => "省市区";
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