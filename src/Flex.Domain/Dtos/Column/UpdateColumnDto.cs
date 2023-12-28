using Flex.Domain.Dtos.Column.Basic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.Dtos.Column
{
    public class UpdateColumnDto: InputColumnDto
    {
        public int Id { set; get; }
    }
}
