using DC.Domain.Global;

namespace DC.Domain.Input
{
    public class ProductQuery : PageQuery
    {
        /// <summary>
        /// 产品名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string Account { get; set; }
    }

    public class SortProductInput
    {
        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool HasUp { get; set; }
    }
}
