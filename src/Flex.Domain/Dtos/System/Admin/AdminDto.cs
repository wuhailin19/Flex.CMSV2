namespace Flex.Domain.Dtos
{
    public class AdminDto: BaseEntity
    {
        public string Account { get; set; }
        public string UserName { get; set; }
        public DateTime LastLoginTime { set; get; }
    }
}
