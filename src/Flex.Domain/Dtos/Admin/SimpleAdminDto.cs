using Flex.Core.JsonConvertExtension;
using System.Text.Json.Serialization;

namespace Flex.Domain.Dtos.Admin
{
    public class SimpleAdminDto
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string UserAvatar { get; set; }
        public string UserSign { get; set; }
        public string FilterIp { get; set; }
        public string? LastLoginIP { get; set; }
        public bool AllowMultiLogin { get; set; }
        public bool Islock { get; set; }
        public DateTime? LastLoginTime { set; get; }
        public DateTime? LastLogoutTime { set; get; }
        public AdminLoginLog adminLoginLog { set; get; }
        public int Version { set; get; }
    }
}
