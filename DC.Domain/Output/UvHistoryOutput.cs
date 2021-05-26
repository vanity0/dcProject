namespace DC.Domain.Output
{
    public class UvHistoryOutput 
    {
        public string Url { get; set; }
        public string RegId { get; set; }
    }

    /// <summary>
    /// 用户数据,CPS/CPA 数据
    /// </summary>
    public class UvHistorycpaCps
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Ip { get; set; }
        public string SystemType { get; set; }
        public string DCUserDevName { get; set; }
        public string ProductName { get; set; }
        public string Status { get; set; }
        public string CreateTime { get; set; }
    }
}
