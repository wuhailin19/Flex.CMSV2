using System;
using System.Collections.Generic;
using System.Text;

namespace Flex.Domain.Dtos.Admin
{
    public class AdminValidateDto
    {
        public long Id { get; set; }
        public string Account { get; set; }
        public string UserName { get; set; }
        public bool Islock { get; set; }
        public string RoleId { get; set; }
        public bool IsSystem { get { return RoleId == "0"; } }
    }
}
