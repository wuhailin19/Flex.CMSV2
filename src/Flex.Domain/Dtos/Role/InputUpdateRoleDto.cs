using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.Dtos.Role
{
    public class InputUpdateRoleDto
    {
        public int Id { get; set; }
        public string RolesName { get; set; }
        public string? RolesDesc { get; set; }
    }
}
