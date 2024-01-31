using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.Dtos.Field
{
    public class FiledQuickEditDto
    {
        [Required]
        public string Id { set; get; }
        public string IsApiField { set; get; }
        public string IsSearch { get; set; }
    }
}
