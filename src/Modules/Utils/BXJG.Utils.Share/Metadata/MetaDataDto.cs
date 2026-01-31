namespace BXJG.Utils.Share.Metadata
{
    /// <summary>
    /// 元数据传输对象
    /// </summary>
    public class MetaDataDto
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 节点标识
        /// 不同租户下同类型的节点，此字段一样
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        public string? DisplayName { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 父节点ID
        /// </summary>
        public long? ParentId { get; set; }


        /// <summary>
        /// 子节点数量
        /// </summary>
        public int ChildrenCount { get; set; }


        /// <summary>
        /// 指定的实体类型
        /// </summary>
        public string? EntityTypeFullName { get; set; }
    }
}
