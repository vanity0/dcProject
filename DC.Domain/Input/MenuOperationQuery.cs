namespace DC.Domain.Input
{
    /// <summary>
    /// 操作按钮查询列表参数
    /// </summary>
    public class MenuOperationQuery
    {  
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 所属菜单
        /// </summary>
        public string MenuId { get; set; }
    }
}
