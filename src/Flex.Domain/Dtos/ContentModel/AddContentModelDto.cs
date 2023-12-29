using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.Dtos.ContentModel
{
    public class AddContentModelDto
    {
        [Required(ErrorMessage = "名称必须填写")]
        public string Name { set; get; }
        public string? Descriptiton { set; get; }

        [Required(ErrorMessage = "表名必须填写")]
        [RegularExpression("^[a-zA-Z0-9]+$", ErrorMessage = "表名只能包含字母和数字")]
        public string TableName { set; get; }
    }
}
