using System;
using System.Collections.Generic;
using System.Text;

namespace Flex.Domain.Dtos.Role
{
    public class PermissionDto
    {
        public long RoleId { set; get; }
        public List<string> UrlList { get; set; }
    }
}
