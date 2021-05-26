namespace DC.Domain.Output
{
    /// <summary>
    /// 下拉框树选择
    /// </summary>
    public class TreeSelectOutPut
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 父级ID
        /// </summary>
        public string PId { get; set; }
        /// <summary>
        /// 绑定值
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// 显示值
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 是否是叶子节点
        /// </summary>
        public bool IsLeaf { get; set; }
    }
}
