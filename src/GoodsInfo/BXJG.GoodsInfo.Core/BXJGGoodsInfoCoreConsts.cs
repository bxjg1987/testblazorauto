namespace BXJG.GoodsInfo
{
    public class BXJGGoodsInfoCoreConsts
    {
        /// <summary>
        /// 物品模块本地化源名称
        /// </summary>
        public const string LocalizationSourceName = "BXJGGoodsInfo";

        #region 物品分类权限名
        /// <summary>
        /// 物品分类
        /// </summary>
        public const string GoodsInfoCategory = "GoodsInfoCategory";
        /// <summary>
        /// 物品分类管理
        /// </summary>
        public const string GoodsInfoCategoryManager = "GoodsInfoCategoryManager";
        public const string GoodsInfoCategoryCreate = GoodsInfoCategoryManager+ "Create";
        public const string GoodsInfoCategoryUpdate = GoodsInfoCategoryManager + "Update";
        public const string GoodsInfoCategoryDelete = GoodsInfoCategoryManager + "Delete";
        #endregion
        //public const string ConnectionStringName = "Default";

        //public const bool MultiTenancyEnabled = true;
        #region 基础物品信息字段长度的常量
        /// <summary>
        /// 基础物品信息名称长度
        /// </summary>
        public const int GoodsInfoNameMaxLength = 500;
        /// <summary>
        /// 基础物品信息助记码长度
        /// </summary>
        public const int GoodsInfoMnemonicCodeMaxLength = 500;
        /// <summary>
        /// 基础物品信息单位id长度
        /// </summary>
        public const int GoodsInfoUnitIdMaxLength = 80;
        /// <summary>
        /// 基础物品信息关联具体物品类型的名称长度
        /// </summary>
        public const int GoodsInfoExtensionTypeMaxLength = 80;
        #endregion

    }
}
