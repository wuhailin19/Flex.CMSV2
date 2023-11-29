using System.ComponentModel.DataAnnotations;

namespace Flex.Domain.Entities.System
{
    public class SysAdmin : BaseLongEntity, EntityContext
    {
        [Display(Name ="账号")]
        public string Account { get; set; }
        [Display(Name = "别名")]
        public string UserName { get; set; }
        [Display(Name = "密码")]
        public string Password { get; set; }
        public string Mutiloginccode { get; set; }
        public string LastLoginIP { get; set; }
        public DateTime? LastLoginTime { get; set; }
        public DateTime? LockTime { get; set; }
        public bool AllowMultiLogin { get; set; }
        public bool Islock { get; set; }
        public string RoleId { get; set; }
        public bool IsSystem { get { return RoleId == "0"; } }
        public string? RoleName { get; set; }
        public int LoginCount { get; set; }
        public string? FilterIp { get; set; }
        public string? UserAvatar { get; set; }
        public string? UserSign { get; set; }
        public string SaltValue { set; get; }
        public int ErrorCount { set; get; }
        public int MaxErrorCount { set; get; }
    }
}
