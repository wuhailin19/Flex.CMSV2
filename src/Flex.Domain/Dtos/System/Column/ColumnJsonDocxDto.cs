using Flex.Domain.Dtos.System.ContentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.Dtos.System.Column
{
    public class ColumnJsonDocxDto
    {
        public Dictionary<object, List<FiledModel>> full_fileds { set; get; }
        public string JsonStr { set; get; }
        public string urlext { set; get; }
    }
}
