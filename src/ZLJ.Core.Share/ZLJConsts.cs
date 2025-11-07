namespace ZLJ.Core.Share
{
    public class ZLJConsts
    {
        public const string LocalizationSourceName = "ZLJ";
        public const string ConnectionStringName = "Default";
        public const string DefaultPassPhrase = "gsKxGZ112HLL3MI5";
        #region 员工
        public const string StaffId = "staffId";
        public const string StaffRoleName = "staff";
        #endregion
        //public const string ConnectionStringName = "Default";

        public const bool MultiTenancyEnabled = true;
        public const int ExtensionDataMaxLength = 4000;
        public const int RemarkMaxLength = 512;

        public const int CustomerStaffPwdMaxLenght = 20;
        public const int StatusChangedReason = 1000;


        #region 基础资料

        public const string DataDictionaryMigrationValueSettingGroupKey = "DataDictionaryMigrationValueSettingGroup";

        public const string DataDictionaryMigrationValuePrinterBrand = "pingpai";

        public const string DataDictionaryMigrationValueCustomerLevel = "kehuJibie";
        public const string DataDictionaryMigrationValueCustomerCategory = "kehuLeibie";
        public const string DataDictionaryMigrationValuePost = "gangwei";

        public const string SharpHttpClientName = "SharpHttpClient";

        public const string RicohHttpClientName = "RicohHttpClient";

        public const string XeroxHttpClientName = "XeroxHttpClient";

        #region 实体映射常量

        //当实体是泛型时，在其中定义常量不太容易被调用，因此实体中的常量定义在这里

        #region 员工档案

        public const int StaffInfoNameMaxLength = 200;
        public const int StaffInfoNoMaxLength = 64;
        public const int StaffInfoAgeStringMaxLength = 20;
        public const int StaffInfoIdNumberMaxLength = 18;
        public const int StaffInfoCurrentAddressMaxLength = 300;
        #endregion

        #region 来往单位

        public const int AssociatedCompanyNameMaxLength = 256;
        public const int AssociatedCompanyTaxNoMaxLength = 32;
        public const int AssociatedCompanyLinkManMaxLength = 32;
        public const int AssociatedCompanyLinkPhoneMaxLength = 16;
        public const int AssociatedCompanyAddressMaxLength = 256;
        //public const int AssociatedCompanyPinyinMaxLength = 256;
        #endregion


        //--codegenerator==

        #endregion

        //#region 员工
        //public const string StaffId = "staffId";
        //public const string StaffRoleName = "staff";
        //#endregion
        #endregion

        #region 版本特征
        public const string Feature_1 = "Feature_1";
        #endregion


        #region 租户
        //
        // 摘要:
        //     Max length of the Abp.MultiTenancy.AbpTenantBase.TenancyName property.
        public const int MaxTenancyNameLength = 64;

        //
        // 摘要:
        //     Max length of the Abp.MultiTenancy.AbpTenantBase.ConnectionString property.
        public const int MaxConnectionStringLength = 1024;

        //
        // 摘要:
        //     "Default".
        public const string DefaultTenantName = "Default";

        //
        // 摘要:
        //     "^[a-zA-Z][a-zA-Z0-9_-]{1,}$".
        public const string TenancyNameRegex = "^[a-zA-Z][a-zA-Z0-9_-]{1,}$";
        public const int MaxEmailAddressLength = 256;
        //
        // 摘要:
        //     Max length of the Abp.MultiTenancy.AbpTenantBase.Name property.
        public const int MaxNameLength = 128;
        #endregion

        public const string CfgKeyUpload = "Upload:SaveDir";

    }
}
