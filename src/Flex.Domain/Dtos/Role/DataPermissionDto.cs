using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.Dtos.Role
{
    public class DataPermissionDto
    {
        public string select { set; get; }
        public string add { set; get; }
        public string edit { set; get; }
        public string delete { set; get; }
    }
}
