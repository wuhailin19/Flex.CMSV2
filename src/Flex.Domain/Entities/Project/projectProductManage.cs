namespace Flex.Domain.Entities
{
    /// <summary>
    /// 项目管理
    /// </summary>
    public class projectProductManage : BaseIntEntity, EntityContext
    {
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProductName { set; get; }
        /// <summary>
        /// 参与者
        /// </summary>
        public string Participants { set; get; }
        /// <summary>
        /// 服务器信息
        /// </summary>
        public string  ServerInfo{ set; get; }
    }
}
