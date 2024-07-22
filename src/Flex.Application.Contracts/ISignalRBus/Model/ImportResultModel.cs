using Flex.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Application.Contracts.ISignalRBus.Model
{
    public class ImportResultModel
    {
        public DataTable dt { set; get; }
        public int statuscode { set; get; }
        public int OrderId { set; get; }
        public string TableName { set; get; }
    }
}
