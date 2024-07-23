using Flex.Domain.Dtos.System.ContentModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.Dtos.System.ColumnContent
{
    public class ContentExportExcelDto
    {
        public List<FiledModel> filedModels { set; get; }
        public DataTable result { set; get; }
        public string ExcelName { set; get; }
        public int recount { set; get; }
    }
}
