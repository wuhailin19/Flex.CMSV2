using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.Dtos.WorkFlow
{
    public class InputWorkFlowAddDto
    {
        [Required(ErrorMessage = "名称不能为空")]
        public string Name { set; get; }
        public string? Introduction { set; get; }
    }
}
