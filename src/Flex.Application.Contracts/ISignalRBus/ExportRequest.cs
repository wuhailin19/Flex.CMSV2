using Flex.Domain.Dtos.System.ContentModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Application.Contracts.ISignalRBus
{
    public class ExportRequest
    {
        public DataTable table { set; get; }
        public List<FiledModel> fileModes { set; get; }
        public string FileName { set; get; }
        public long UserId { set; get; }
    }
}
