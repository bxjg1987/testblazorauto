namespace ZLJ.Authorization
{
    public static class PermissionNames
    {
        public const string Administrator = "Administrator";

        //{codegenerator}

        #region 资产管理
        public const string AdministratorAsset = "Administrator.Asset";
        //设备档案
        public const string AdministratorAssetEquipmentInfo = "Administrator.Asset.EquipmentInfo";
        public const string AdministratorAssetEquipmentInfoCreate = "Administrator.Asset.EquipmentInfo.Create";
        public const string AdministratorAssetEquipmentInfoUpdate = "Administrator.Asset.EquipmentInfo.Update";
        public const string AdministratorAssetEquipmentInfoDelete = "Administrator.Asset.EquipmentInfo.Delete";
        #endregion

        #region 基础信息
        public const string AdministratorBaseInfo = "Administrator.BaseInfo";

        public const string AdministratorBaseInfoOrganizationUnit = "Administrator.BaseInfo.OrganizationUnit";
        public const string AdministratorBaseInfoOrganizationUnitAdd = "Administrator.BaseInfo.OrganizationUnit.Add";
        public const string AdministratorBaseInfoOrganizationUnitUpdate = "Administrator.BaseInfo.OrganizationUnit.Update";
        public const string AdministratorBaseInfoOrganizationUnitDelete = "Administrator.BaseInfo.OrganizationUnit.Delete";

        public const string AdministratorBaseInfoBtype = "Administrator.BaseInfo.Btype";
        public const string AdministratorBaseInfoBtypeCreate = "Administrator.BaseInfo.Btype.Create";
        public const string AdministratorBaseInfoBtypeUpdate = "Administrator.BaseInfo.Btype.Update";
        public const string AdministratorBaseInfoBtypeDelete = "Administrator.BaseInfo.Btype.Delete";

        public const string AdministratorBaseInfoJob = "Administrator.BaseInfo.Job";
        public const string AdministratorBaseInfoJobCreate = "Administrator.BaseInfo.Job.Create";
        public const string AdministratorBaseInfoJobUpdate = "Administrator.BaseInfo.Job.Update";
        public const string AdministratorBaseInfoJobDelete = "Administrator.BaseInfo.Job.Delete";

        public const string AdministratorBaseInfoEmployee = "Administrator.BaseInfo.Employee";
        public const string AdministratorBaseInfoEmployeeCreate = "Administrator.BaseInfo.Employee.Create";
        public const string AdministratorBaseInfoEmployeeUpdate = "Administrator.BaseInfo.Employee.Update";
        public const string AdministratorBaseInfoEmployeeDelete = "Administrator.BaseInfo.Employee.Delete";

        public const string AdministratorBaseInfoDataDictionary = "Administrator.BaseInfo.DataDictionary";
        public const string AdministratorBaseInfoDataDictionaryCreate = "Administrator.BaseInfo.DataDictionary.Create";
        public const string AdministratorBaseInfoDataDictionaryUpdate = "Administrator.BaseInfo.DataDictionary.Update";
        public const string AdministratorBaseInfoDataDictionaryDelete = "Administrator.BaseInfo.DataDictionary.Delete";

        #endregion

        #region 系统管理
        public const string AdministratorSystem = "Administrator.System";
        public const string AdministratorSystemTenant = "Administrator.System.Tenant";

        public const string AdministratorSystemRole = "Administrator.System.Role";
        public const string AdministratorSystemRoleAdd = "Administrator.System.Role.Add";
        public const string AdministratorSystemRoleUpdate = "Administrator.System.Role.Update";
        public const string AdministratorSystemRoleDelete = "Administrator.System.Role.Delete";

        public const string AdministratorSystemUser = "Administrator.System.User";
        public const string AdministratorSystemUserAdd = "Administrator.System.User.Add";
        public const string AdministratorSystemUserUpdate = "Administrator.System.User.Update";
        public const string AdministratorSystemUserDelete = "Administrator.System.User.Delete";

        public const string AdministratorSystemLog = "Administrator.System.Log";
        public const string AdministratorSystemConfig = "Administrator.System.Config";
        #endregion

        #region 其它
        public const string AdministratorDemo = "Administrator.Demo";
        public const string AdministratorDemoUpload = "Administrator.Demo.Upload";

        //WeChat
        public const string AdministratorWeChat = "Administrator.WeChat";
        public const string AdministratorWeChatIndex = "Administrator.WeChat.Index";
        #endregion

        #region 加盟商
        //管理员对加盟商进行管理的权限
        public const string Franchisee = "Franchisee";
        public const string FranchiseeInfo = "FranchiseeInfo";
        public const string FranchiseeEquipment = "FranchiseeEquipment";


        //加盟商自己的后台权限
        public const string FranchiseeBack = "FranchiseeBack";

        //统计
        public const string FranchiseeBackStatistical = "FranchiseeBackStatistical";
        public const string FranchiseeBackStatisticalIncome = "FranchiseeBackStatisticalIncome";
        public const string FranchiseeBackStatisticalUser = "FranchiseeBackStatisticalUser";
        public const string FranchiseeBackStatisticalOrder = "FranchiseeBackStatisticalOrder";
        public const string FranchiseeBackStatisticalSale = "FranchiseeBackStatisticalSale";

        //设备管理
        public const string FranchiseeBackEquipment = "FranchiseeBackEquipment";
        public const string FranchiseeBackEquipmentOrderStatus = "FranchiseeBackEquipmentOrderStatus";
        public const string FranchiseeBackEquipmentStatus = "FranchiseeBackEquipmentStatus";
        #endregion
    }
}
