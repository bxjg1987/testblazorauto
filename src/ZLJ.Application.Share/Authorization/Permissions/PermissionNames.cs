namespace ZLJ.Application.Share.Authorization.Permissions
{
    public partial class PermissionNames
    {

        public const string Administrator = "Administrator";

        //--codegenerator==


        #region 基础信息
        public const string BaseInfo = "BaseInfo";



        public const string AdministratorBaseInfo = "Administrator.BaseInfo";
        #region 公司和部门

        public const string AdministratorBaseInfoOrganizationUnit = "Administrator.BaseInfo.OrganizationUnit";
        public const string AdministratorBaseInfoOrganizationUnitAdd = "Administrator.BaseInfo.OrganizationUnit.Add";
        public const string AdministratorBaseInfoOrganizationUnitUpdate = "Administrator.BaseInfo.OrganizationUnit.Update";
        public const string AdministratorBaseInfoOrganizationUnitDelete = "Administrator.BaseInfo.OrganizationUnit.Delete";

        #endregion
        //public const string AdministratorBaseInfoBtype = "Administrator.BaseInfo.Btype";
        //public const string AdministratorBaseInfoBtypeCreate = "Administrator.BaseInfo.Btype.Create";
        //public const string AdministratorBaseInfoBtypeUpdate = "Administrator.BaseInfo.Btype.Update";
        //public const string AdministratorBaseInfoBtypeDelete = "Administrator.BaseInfo.Btype.Delete";

        #region 岗位
        public const string AdministratorBaseInfoPost = "Administrator.BaseInfo.Post";
        public const string AdministratorBaseInfoPostCreate = "Administrator.BaseInfo.Post.Create";
        public const string AdministratorBaseInfoPostUpdate = "Administrator.BaseInfo.Post.Update";
        public const string AdministratorBaseInfoPostDelete = "Administrator.BaseInfo.Post.Delete";
        #endregion

        #region 员工档案

        public const string BXJGBaseInfoStaffInfo = "BXJGBaseInfoStaffInfo";
        public const string BXJGBaseInfoStaffInfoCreate = "BXJGBaseInfoStaffInfoCreate";
        public const string BXJGBaseInfoStaffInfoUpdate = "BXJGBaseInfoStaffInfoUpdate";
        public const string BXJGBaseInfoStaffInfoDelete = "BXJGBaseInfoStaffInfoDelete";

        #endregion
        //public const string AdministratorBaseInfoEmployee = "Administrator.BaseInfo.Employee";
        //public const string AdministratorBaseInfoEmployeeCreate = "Administrator.BaseInfo.Employee.Create";
        //public const string AdministratorBaseInfoEmployeeUpdate = "Administrator.BaseInfo.Employee.Update";
        //public const string AdministratorBaseInfoEmployeeDelete = "Administrator.BaseInfo.Employee.Delete";

        //public const string AdministratorBaseInfoDataDictionary = "Administrator.BaseInfo.DataDictionary";
        //public const string AdministratorBaseInfoDataDictionaryCreate = "Administrator.BaseInfo.DataDictionary.Create";
        //public const string AdministratorBaseInfoDataDictionaryUpdate = "Administrator.BaseInfo.DataDictionary.Update";
        //public const string AdministratorBaseInfoDataDictionaryDelete = "Administrator.BaseInfo.DataDictionary.Delete";

        #region 行政区

        public const string BXJGBaseInfoAdministrative = "BXJGBaseInfoAdministrative";
        public const string BXJGBaseInfoAdministrativeCreate = "BXJGBaseInfoAdministrativeCreate";
        public const string BXJGBaseInfoAdministrativeUpdate = "BXJGBaseInfoAdministrativeUpdate";
        public const string BXJGBaseInfoAdministrativeDelete = "BXJGBaseInfoAdministrativeDelete";

        #endregion



        #region 来往单位

        public const string BXJGBaseInfoAssociatedCompany = "BXJGBaseInfoAssociatedCompany";
        public const string BXJGBaseInfoAssociatedCompanyCreate = "BXJGBaseInfoAssociatedCompanyCreate";
        public const string BXJGBaseInfoAssociatedCompanyUpdate = "BXJGBaseInfoAssociatedCompanyUpdate";
        public const string BXJGBaseInfoAssociatedCompanyDelete = "BXJGBaseInfoAssociatedCompanyDelete";

        #endregion

        #region 多租户
        public const string AdminMultiTenancy = "AdminMultiTenancy";

        #region 租户
        public const string AdminTenant = "AdminTenant";
        public const string AdminTenantCreate = "AdminTenantCreate";
        public const string AdminTenantUpdate = "AdminTenantUpdate";
        public const string AdminTenantDelete= "AdminTenantDelete";
        #endregion

        //版本

        //特征

        #endregion

        public const string AdministratorSystem = "Administrator.System";
  

        public const string AdministratorSystemRole = "Administrator.System.Role";
        public const string AdministratorSystemRoleGet = "Administrator.System.Role.Get";
        public const string AdministratorSystemRoleAdd = "Administrator.System.Role.Add";
        public const string AdministratorSystemRoleUpdate = "Administrator.System.Role.Update";
        public const string AdministratorSystemRoleDelete = "Administrator.System.Role.Delete";

        public const string AdministratorSystemUser = "Administrator.System.User";
        public const string AdministratorSystemUserGet = "Administrator.System.User.Get";
        public const string AdministratorSystemUserAdd = "Administrator.System.User.Add";
        public const string AdministratorSystemUserUpdate = "Administrator.System.User.Update";
        public const string AdministratorSystemUserDelete = "Administrator.System.User.Delete";

        public const string AdministratorSystemLog = "Administrator.System.Log";
        public const string AdministratorSystemConfig = "Administrator.System.Config";


        public const string AdministratorDemo = "Administrator.Demo";
        public const string AdministratorDemoUpload = "Administrator.Demo.Upload";

        //WeChat
        public const string AdministratorWeChat = "Administrator.WeChat";
        public const string AdministratorWeChatIndex = "Administrator.WeChat.Index";


        /// <summary>
        /// 整个框架使用了hangfire，它对应的权限名称
        /// </summary>
        public const string HangFireDashboard = "HangFireDashboard";

        //#region 维修人员App端权限
        //public const string EmployeeApp = "EmployeeApp";

        //public const string EmployeeAppGd = EmployeeApp + "gd";
        #endregion
        //public const string EmployeeAppGdKhsb = EmployeeAppGd + "khsb";
        //public const string EmployeeAppGdKhsblq = EmployeeAppGdKhsb + "lq";
        //public const string EmployeeAppGdKhsbzx = EmployeeAppGdKhsb + "zx";
        //public const string EmployeeAppGdKhsbwc = EmployeeAppGdKhsb + "wc";
        //public const string EmployeeAppGdKhsbjj = EmployeeAppGdKhsb + "jj";
        //#endregion

        #region 客户
        /// <summary>
        /// 客户服务平台统一权限名称
        /// 由于种子数据需要，所以把它定义在Core里，本身应该定义在各app的应用层的
        /// </summary>
        public const string Customer = "CustomerApp";
        #endregion
    }
}
