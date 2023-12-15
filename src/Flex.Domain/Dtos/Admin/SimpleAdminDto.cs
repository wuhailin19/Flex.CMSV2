using Flex.Core.JsonConvertExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Flex.Domain.Dtos.Admin
{
    public class SimpleAdminDto
    {
        [JsonConverter(typeof(IdToStringConverter))]
        public long Id { get; set; }
        public string UserName { get; set; }
        public string UserAvatar { get; set; }
        public string UserSign { get; set; }
        public string FilterIp { get; set; }
        public string? LastLoginIP { get; set; }
        public string? AllowMultiLogin { get; set; }
        public string? Islock { get; set; }
        public DateTime? LastLoginTime { set; get; }
        public DateTime? LastLogoutTime { set; get; }
        public int Version { set; get; }
    }
}
