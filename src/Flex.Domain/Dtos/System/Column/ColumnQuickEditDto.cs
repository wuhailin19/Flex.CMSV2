using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.Dtos.System.Column
{
    public class ColumnQuickEditDto
    {
        [Required]
        public int Id { set; get; }
        public string IsShow { set; get; }
    }
}
