using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Application.Contracts.Exceptions
{
    public class ErrorMsgCode : ErrorCodes
    {
        /// <summary> 当前数据已被修改 </summary>
        [Description("当前数据已被修改")]
        public static int DataVersionError = 10005;
        /// <summary> 添加失败 </summary>
        [Description("添加失败")]
        public static int DataInsertError = 10006;
        /// <summary> 修改失败 </summary>
        [Description("修改失败")]
        public static int DataUpdateError = 10007;
        /// <summary> 修改失败 </summary>
        [Description("修改成功")]
        public static int DataUpdateSuccess = 10008;
    }
}
