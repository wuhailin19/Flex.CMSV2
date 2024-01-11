using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Core.Framework.Enum
{
    public enum FieldType
    {
        [Description("文本框")]
        TextBox = 1,
        [Description("下拉框")]
        SelectBox = 2,
        [Description("图片上传")]
        UploadImage = 3,
        [Description("文件上传")]
        UploadFile = 4,
        [Description("编辑器")]
        Editor = 5
    }
}
