using Flex.Core.Extensions;
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
    /// 211-300 业务异常
    /// </summary>
    public enum ErrorCodes
    {
        /// <summary> 默认错误码 </summary>
        DefaultCode = -1,

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
         NotChooseData = 10013,
        /// <summary> 上传失败 </summary>
        [Description("上传失败")]
        UploadFail = 10014,
        /// <summary>上传文件格式不正确 </summary>
        [Description("上传文件格式不正确")]
        UploadTypeDenied = 10015,
        /// <summary>审批创建成功 </summary>
        [Description("审批流程创建成功")]
        ReviewCreateSuccess = 10016,
        /// <summary>审批创建失败 </summary>
        [Description("审批流程创建失败")]
        ReviewCreateError = 10017,
        /// <summary> 当前栏目信息需审核 </summary>
        [Description("当前栏目信息需审核")]
        DataNeedReview = 10018,
        /// <summary> 审批工作流出错，已重置审批 </summary>
        [Description("审批工作流出错或已取消，已重置审批")]
        ReviewRest = 10019,
        /// <summary> 审批通过 </summary>
        [Description("审批通过")]
        ReviewSuccess = 10020,
        /// <summary> 审批已结束 </summary>
        [Description("审批已结束")]
        ReviewAlreadyComplete = 10021,
        /// <summary> 解析失败，重试 </summary>
        [Description("解析失败，重试")]
        RedirectKeepVerb = 10022,
        /// <summary> 未添加任何站点 </summary>
        [Description("未添加任何站点")]
        NotInsertAnySite = 10023,
        /// <summary> 消息发送失败 </summary>
        [Description("消息发送失败")]
        MsgSendError = 10024,
        /// <summary> 消息发送成功 </summary>
        [Description("消息发送成功")]
        MsgSendSuccess = 10025,
        /// <summary> 数据恢复出错 </summary>
        [Description("数据恢复出错")]
        DataRestError = 10026,
        /// <summary> 数据复制失败 </summary>
        [Description("数据复制失败")]
        DataCopyError = 10027,
        /// <summary> 数据移动失败 </summary>
        [Description("数据移动失败")]
        DataMoveError = 10028,
        /// <summary> 数据引用失败 </summary>
        [Description("数据引用失败")]
        DataLinkError = 10029,
        /// <summary> 数据复制完成 </summary>
        [Description("数据复制完成")]
        DataCopySuccess = 10030,
        /// <summary> 数据移动完成 </summary>
        [Description("数据移动完成")]
        DataMoveSuccess = 10031,
        /// <summary> 数据引用完成 </summary>
        [Description("数据引用完成")]
        DataLinkSuccess = 10032,



        /// <summary> 系统错误 </summary>
        [Description("服务器心情不好，请稍后重试~")]
        SystemError = 211,
        /// <summary> 参数错误 </summary>
        [Description("参数错误")]
        ParamaterError = 212,
        /// <summary> 调用受限 </summary>
        [Description("该请求调用受限")]
        Unauthorized = 213,
        /// <summary> 调用受限 </summary>
        [Description("该请求已超时")]
        ClientTimeoutError = 214,
        /// <summary> 登录已超时 </summary>
        [Description("登录已超时")]
        LoginTimeoutError = 215,
        /// <summary> 未登录 </summary>
        [Description("未登录")]
        NotLogin = 216,
        /// <summary> 没有操作权限 </summary>
        [Description("没有操作权限")]
        NoOperationPermission = 217,
        /// <summary> 数据已存在 </summary>
        [Description("数据已存在")]
        DataExist = 218,
        /// <summary> 账号已锁定 </summary>
        [Description("账号已锁定")]
        AccountLocked = 219,
        /// <summary> 用户名或密码错误 </summary>
        [Description("用户名或密码错误")]
        AccountOrPwdWrong = 220,
        /// <summary> 用户名或密码为空 </summary>
        [Description("用户名或密码为空")]
        AccountOrPwdEmpty = 221,
        /// <summary> 账号已禁用 </summary>
        [Description("账号已禁用")]
        AccountDisabled = 222,
        /// <summary> 账号已过期 </summary>
        [Description("账号已过期")]
        AccountExpried = 223,
        /// <summary> 密码已过期 </summary>
        [Description("密码已过期")]
        PwdExpried = 224,
        /// <summary> 字段名重复 </summary>
        [Description("字段名重复")]
        ApiFieldExist = 225,
        /// <summary> 栏目数据为空 </summary>
        [Description("栏目数据为空")]
        ColumnListEmpty = 226,
    }
}
