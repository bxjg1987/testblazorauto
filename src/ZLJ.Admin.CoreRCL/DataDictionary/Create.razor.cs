
namespace ZLJ.Admin.CoreRCL.DataDictionary
{
    public partial class Create
    {
        public override string FuncName =>"数据字典";

        string? pid;

        protected override Task SaveCore()
        {
            if(pid.IsNotNullOrWhiteSpaceBXJG())
                createDto.ParentId= int.Parse(pid);
            return base.SaveCore();
        }
    }
}
