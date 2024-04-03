using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.Dtos.Field
{
    public class AddFieldDto: FiledInputBaseDto
    {
        public string FieldName { set; get; }
        public string FieldType { set; get; }
        public string FieldDescription { set; get; }
        public string Width { set; get; }
        public string Height { set; get; }
    }
}
