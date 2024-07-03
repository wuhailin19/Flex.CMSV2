using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.Dtos.System.TableRelation
{
    public class AddTableRelationDto
    {
        [Required(ErrorMessage = "此项不能为空")]
        public int ParentModelId { get; set; }
        [Required(ErrorMessage = "此项不能为空")]
        public int ChildModelId { get; set; }
        public string LinkName { get; set; } = "相关列表";
    }
}
