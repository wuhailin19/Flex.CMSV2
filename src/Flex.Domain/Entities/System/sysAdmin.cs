using System.ComponentModel.DataAnnotations;

namespace Flex.Domain.Entities.System
{
    public class SysAdmin : BaseLongEntity, EntityContext
    {
        public string Account { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Mutiloginccode { get; set; }
        public string? CurrentLoginIP { get; set; }
        public DateTime? CurrentLoginTime { get; set; }
        public DateTime? LockTime { get; set; }
        public DateTime? ExpiredTime { get; set; }
        //密码修改时间
        public DateTime? PwdUpdateTime { get; set; }
        public string? PwdExpiredTime { get; set; }
        public bool? AllowMultiLogin { get; set; }
        public bool Islock { get; set; }
        public int RoleId { get; set; }
        public bool IsSystem { get { return RoleId == 0; } }
        public string? RoleName { get; set; }
        public int LoginCount { get; set; }
        public string? FilterIp { get; set; }
        public string? UserAvatar { get; set; }
        public string? UserSign { get; set; }
        public string SaltValue { set; get; }
        public string? WeiboId { get; set; }
        public string? LoginLogString { set; get; }
        public int ErrorCount { set; get; }
        public int MaxErrorCount { set; get; }
    }
}
