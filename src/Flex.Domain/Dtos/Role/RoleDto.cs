using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Flex.Domain.Dtos.Role
{
    public class RoleDto
    {
        public long Id { get; set; }
        public string UrlPermission { get; set; }
    }
}
