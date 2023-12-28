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
    /// 20000-99999 业务异常
    /// </summary>
    public abstract class ErrorCodes
    {
        /// <summary> 默认错误码 </summary>
        public const int DefaultCode = -1;

        /// <summary> 系统错误 </summary>
        [Description("服务器心情不好，请稍后重试~")]
        public const int SystemError = 10001;

        /// <summary> 参数错误 </summary>
        [Description("参数错误")]
        public const int ParamaterError = 10002;

        /// <summary> 调用受限 </summary>
        [Description("该请求调用受限")]
        public const int ClientError = 10003;

        /// <summary> 调用受限 </summary>
        [Description("该请求已超时")]
        public const int ClientTimeoutError = 10004;

        /// <summary> 当前数据已被修改 </summary>
        [Description("当前数据已被修改")]
        public const int DataVersionError = 10005;

        /// <summary> 添加失败 </summary>
        [Description("添加失败")]
        public const int DataInsertError = 10006;

        /// <summary> 修改失败 </summary>
        [Description("修改失败")]
        public const int DataUpdateError = 10007;

        /// <summary> 修改成功 </summary>
        [Description("修改成功")]
        public const int DataUpdateSuccess = 10008;

        /// <summary> 数据不存在 </summary>
        [Description("数据不存在")]
        public const int DataNotFound = 10009;

        /// <summary> 添加成功 </summary>
        [Description("添加成功")]
        public const int DataInsertSuccess = 10010;

        /// <summary> 该账号已存在 </summary>
        [Description("该账号已存在")]
        public const int AccountExist = 10011;

        /// <summary> 删除失败 </summary>
        [Description("删除失败")]
        public const int DataDeleteError = 10012;

        /// <summary> 未选择删除数据 </summary>
        [Description("未选择删除数据")]
        public const int NotChooseData = 10013;
    }

    /// <summary> 错误码扩展 </summary>
    public static class ErrorCodesExtension
    {
        private static readonly ConcurrentDictionary<RuntimeTypeHandle, IDictionary<int, string>> ErrorCodesCache =
            new ConcurrentDictionary<RuntimeTypeHandle, IDictionary<int, string>>();

        private static readonly Type ErrorType = typeof(ErrorCodes);

        /// <summary> 获取错误码对应的错误信息 </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string Message<T>(this int code) where T : ErrorCodes
        {
            var codes = Codes<T>();
            return codes.TryGetValue(code, out var message) ? message : ErrorCodes.SystemError.Message<ErrorCodes>();
        }

        /// <summary> 错误编码对应DResult </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="code"></param>
        /// <returns></returns>
        public static DResult CodeResult<T>(this int code) where T : ErrorCodes
        {
            return DResult.Error(code.Message<T>(), code);
        }

        /// <summary> 错误编码对应的Exception </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="code"></param>
        /// <returns></returns>
        public static BusiException CodeException<T>(this int code) where T : ErrorCodes
        {
            return new BusiException(code.Message<T>(), code);
        }

        /// <summary> 获取错误码 </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IDictionary<int, string> Codes<T>() where T : ErrorCodes
        {
            return typeof(T).Codes();
        }

        /// <summary> 获取错误码 </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IDictionary<int, string> Codes(this Type type)
        {
            if (type != ErrorType && !type.IsSubclassOf(ErrorType))
                return new Dictionary<int, string>();
            var key = type.TypeHandle;
            if (ErrorCodesCache.ContainsKey(key) && ErrorCodesCache.TryGetValue(type.TypeHandle, out var codes))
                return codes;
            codes = new Dictionary<int, string>();
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);
            foreach (var field in fields)
            {
                var message = field.GetCustomAttribute<DescriptionAttribute>()?.Description ?? field.Name;
                codes.Add(field.GetRawConstantValue().CastTo<int>(), message);
            }

            while (type != null && type != ErrorType)
            {
                type = type.BaseType;
                type.Codes().Foreach(t => codes.Add(t.Key, t.Value));
            }

            var orderCodes = codes.OrderBy(t => t.Key).ToDictionary(k => k.Key, v => v.Value);
            ErrorCodesCache.TryAdd(key, orderCodes);
            return orderCodes;
        }
    }
}
