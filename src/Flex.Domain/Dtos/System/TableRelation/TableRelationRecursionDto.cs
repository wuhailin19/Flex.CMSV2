using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.Dtos.System.TableRelation
{
    public class TableRelationRecursionDto
    {
        public int Id { get; set; }
        public string ParentTableName { get; set; }
        public string ChildTableName { get; set; }
        public int ParentModelId { set; get; }
        public int ChildModelId { set; get; }
        public List<TableRelationRecursionDto> children { get; set; }
    }
}
