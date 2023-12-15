using Flex.Core.JsonConvertExtension;
using System.Text.Json.Serialization;

namespace Flex.Domain.Dtos.Admin
{
    public class SimpleEditAdminDto
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string UserAvatar { get; set; }
        public string UserSign { get; set; }
        public string FilterIp { get; set; }
        public bool AllowMultiLogin { get; set; } = true;
        public bool Islock { get; set; } = false;
        public int Version { set; get; }
    }
}
