using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.Dtos.ContentModel
{
    public class InputContentBaseDto
    {
        [Required(ErrorMessage = "名称必须填写")]
        public string Name { set; get; }
        public string? Description { set; get; }

        [Required(ErrorMessage = "表名必须填写")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "表名的长度必须在3到50个字符之间")]
        [RegularExpression("^[a-zA-Z0-9_]+$", ErrorMessage = "表名只能包含字母、数字和下划线")]
        public string TableName { set; get; }
    }
}
