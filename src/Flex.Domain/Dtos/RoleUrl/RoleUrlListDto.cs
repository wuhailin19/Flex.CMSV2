using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.Dtos.RoleUrl
{
    public class RoleUrlListDto
    {
        public string Id { set; get; }
        public string Name { set; get; }
        public string Url { set; get; }
        public string ParentId { set; get; }
        public bool IsSelect { set; get; }
    }
}
