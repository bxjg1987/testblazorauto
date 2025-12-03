namespace BXJG.PSI.MasterData
{
    /// <summary>
    /// PSI主数据模块权限名称定义
    /// </summary>
    public static class PermissionNames
    {
        /// <summary>
        /// PSI主数据模块根权限
        /// </summary>
        public const string PSIMasterData = "PSI.MasterData";

        #region 商品分类
        /// <summary>
        /// 商品分类管理权限
        /// </summary>
        public const string PSIMasterDataProductCategory = "PSI.MasterData.ProductCategory";
        
        /// <summary>
        /// 商品分类查询权限
        /// </summary>
        public const string PSIMasterDataProductCategoryGet = "PSI.MasterData.ProductCategory.Get";
        
        /// <summary>
        /// 商品分类创建权限
        /// </summary>
        public const string PSIMasterDataProductCategoryCreate = "PSI.MasterData.ProductCategory.Create";
        
        /// <summary>
        /// 商品分类更新权限
        /// </summary>
        public const string PSIMasterDataProductCategoryUpdate = "PSI.MasterData.ProductCategory.Update";
        
        /// <summary>
        /// 商品分类删除权限
        /// </summary>
        public const string PSIMasterDataProductCategoryDelete = "PSI.MasterData.ProductCategory.Delete";
        #endregion

        #region 产品档案
        /// <summary>
        /// 产品档案管理权限
        /// </summary>
        public const string PSIMasterDataProduct = "PSI.MasterData.Product";
        
        /// <summary>
        /// 产品档案查询权限
        /// </summary>
        public const string PSIMasterDataProductGet = "PSI.MasterData.Product.Get";
        
        /// <summary>
        /// 产品档案创建权限
        /// </summary>
        public const string PSIMasterDataProductCreate = "PSI.MasterData.Product.Create";
        
        /// <summary>
        /// 产品档案更新权限
        /// </summary>
        public const string PSIMasterDataProductUpdate = "PSI.MasterData.Product.Update";
        
        /// <summary>
        /// 产品档案删除权限
        /// </summary>
        public const string PSIMasterDataProductDelete = "PSI.MasterData.Product.Delete";
        #endregion

        #region 仓库档案
        /// <summary>
        /// 仓库档案管理权限
        /// </summary>
        public const string PSIMasterDataWarehouse = "PSI.MasterData.Warehouse";
        
        /// <summary>
        /// 仓库档案查询权限
        /// </summary>
        public const string PSIMasterDataWarehouseGet = "PSI.MasterData.Warehouse.Get";
        
        /// <summary>
        /// 仓库档案创建权限
        /// </summary>
        public const string PSIMasterDataWarehouseCreate = "PSI.MasterData.Warehouse.Create";
        
        /// <summary>
        /// 仓库档案更新权限
        /// </summary>
        public const string PSIMasterDataWarehouseUpdate = "PSI.MasterData.Warehouse.Update";
        
        /// <summary>
        /// 仓库档案删除权限
        /// </summary>
        public const string PSIMasterDataWarehouseDelete = "PSI.MasterData.Warehouse.Delete";
        #endregion
    }
}