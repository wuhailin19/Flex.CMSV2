﻿using Flex.Core.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Application.Contracts.Exceptions
{
    /// <summary>
    /// 10001-20000 系统错误码
    /// 20000-99999 业务异常
    /// </summary>
    public enum ErrorCodes
    {
        /// <summary> 默认错误码 </summary>
        DefaultCode = -1,

        /// <summary> 系统错误 </summary>
        [Description("服务器心情不好，请稍后重试~")]
         SystemError = 10001,

        /// <summary> 参数错误 </summary>
        [Description("参数错误")]
         ParamaterError = 10002,

        /// <summary> 调用受限 </summary>
        [Description("该请求调用受限")]
         ClientError = 10003,

        /// <summary> 调用受限 </summary>
        [Description("该请求已超时")]
         ClientTimeoutError = 10004,

        /// <summary> 当前数据已被修改 </summary>
        [Description("当前数据已被修改")]
         DataVersionError = 10005,

        /// <summary> 添加失败 </summary>
        [Description("添加失败")]
         DataInsertError = 10006,

        /// <summary> 修改失败 </summary>
        [Description("修改失败")]
         DataUpdateError = 10007,

        /// <summary> 修改成功 </summary>
        [Description("修改成功")]
         DataUpdateSuccess = 10008,

        /// <summary> 数据不存在 </summary>
        [Description("数据不存在")]
         DataNotFound = 10009,

        /// <summary> 添加成功 </summary>
        [Description("添加成功")]
         DataInsertSuccess = 10010,

        /// <summary> 该账号已存在 </summary>
        [Description("该账号已存在")]
         AccountExist = 10011,

        /// <summary> 删除失败 </summary>
        [Description("删除失败")]
         DataDeleteError = 10012,

        /// <summary> 未选择删除数据 </summary>
        [Description("未选择删除数据")]
         NotChooseData = 10013
    }
}