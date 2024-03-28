using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.Dtos.RoleUrl
{
    public class ApiPermissionDto
    {
        public string Id { set; get; }
        public string menuapi { set; get; }
        public string dataapi { set; get; }
        public string pageapi { set; get; }
    }
}
