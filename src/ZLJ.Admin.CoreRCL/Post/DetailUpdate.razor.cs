namespace ZLJ.Admin.CoreRCL.Post
{
    public partial class DetailUpdate
    {
        protected override string FuncName => "角色岗位";
        /// <summary>
        /// 
        /// </summary>
        string detailUpdateText => isEdit ? "修改" : "查看";

        string detailUpdateIcon => isEdit ? IconType.Outline.Edit : IconType.Outline.File;
    }
}
