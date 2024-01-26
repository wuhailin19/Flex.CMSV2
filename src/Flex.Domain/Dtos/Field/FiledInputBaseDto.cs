using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.Dtos.Field
{
    public class FiledInputBaseDto
    {
        public string Name { set; get; } 
        public string ApiName { set; get; }
        public bool IsApiField { set; get; }
        public bool ShowInTable { set; get; }
        public int ModelId { set; get; }
        public bool IsSearch { set; get; }
        public string? ValidateNumber { set; get; }
        public string? ValidateEmpty { set; get; }
    }
}
