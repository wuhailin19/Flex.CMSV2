using System;
using System.Collections.Generic;
using System.Text;

namespace Flex.Domain.Dtos.Admin
{
    public class UserData
    {
        public long Id { get; set; }
        public string Account { get; set; }
        public string UserName { get; set; }
        public string RoleId { get; set; }
    }
}
