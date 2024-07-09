using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.Dtos.System.ColumnContent
{
    public class ContentToolsDto
    {
        public string checkcolumnId { set; get; }
        public string checkcontentId { set; get; }
        public int modelId { set; get; }
        [Required(ErrorMessage = "栏目不能为空")]
        public int parentId { set; get; }
        [Required(ErrorMessage = "操作为必选")]
        public DataOpreate operation { set; get; }
    }

    public enum DataOpreate
    {
        Copy = 1,
        Move = 2,
        Link = 3
    }
}
