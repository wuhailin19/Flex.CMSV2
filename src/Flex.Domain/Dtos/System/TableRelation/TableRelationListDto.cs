using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.Dtos.System.TableRelation
{
    public class TableRelationListDto
    {
        public int ParentModelId { get; set; }
        public int ChildModelId { get; set; }
        public string LinkName { get; set; } = "相关列表";
    }
}
