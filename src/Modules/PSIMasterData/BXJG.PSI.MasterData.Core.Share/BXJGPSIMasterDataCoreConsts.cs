using System;

namespace BXJG.PSI.MasterData
{
    public class BXJGPSIMasterDataCoreConsts
    {
        public const string LocalizationSourceName = "BXJGPSIMasterData";

        public const int WarehouseNameMaxLength = 256;

        public const int ProductNameMaxLength = 256;
        public const int ProductSpecMaxLength = 256;
        public const int ProductModelMaxLength = 128;
        public const int ProductCategoryNameMaxLength = 128;
        public const int ProductUnitMaxLength = 32;

        #region AssociatedCompany
        public const int AssociatedCompanyNameMaxLength = 256;
        public const int AssociatedCompanyPinyinMaxLength = 256;
        public const int AssociatedCompanyTaxNoMaxLength = 128;
        #endregion
    }
}