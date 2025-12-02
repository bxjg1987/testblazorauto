using BXJG.Utils.Application.Share.GeneralTree;

namespace BXJG.PSI.MasterData.Application.Share.Product
{
    /// <summary>
    /// 产品分类选择DTO
    /// </summary>
    public class ProductCategorySelectDto : GeneralTreeNodeForSelectDto<ProductCategorySelectDto>
    {
        // 基类中已包含以下字段：
        // - Code
        // - DisplayName (通过Text属性实现)
        // - Name
        // - ExtensionData
        // 因此不需要在子类中重复定义
    }
}
