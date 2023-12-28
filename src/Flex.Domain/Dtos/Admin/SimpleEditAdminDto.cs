using Flex.Core.JsonConvertExtension;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Flex.Domain.Dtos.Admin
{
    public class SimpleEditAdminDto
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "昵称不能为空")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "昵称的长度必须在3到50个字符之间")]
        public string UserName { get; set; }
        public string? UserAvatar { get; set; }
        public string? UserSign { get; set; }
        public string? FilterIp { get; set; }
        public bool? AllowMultiLogin { get; set; } = true;
        public bool? Islock { get; set; } = false;
        public int Version { set; get; }
    }
}
