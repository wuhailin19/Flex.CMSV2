using Flex.Core;
using Newtonsoft.Json;
using System;

namespace Flex.Domain.Base
{
    public abstract class IEntity<TKey>
    {
        public virtual TKey Id { set; get; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string? AddUserName { set; get; }
        public long? AddUser { set; get; }
        /// <summary>
        /// 创建时间/注册时间
        /// </summary>
        public DateTime AddTime { set; get; }
        /// <summary>
        /// 最后更新人
        /// </summary>
        public string? LastEditUserName { set; get; }
        public long? LastEditUser { set; get; }
        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime? LastEditDate { set; get; }
        /// <summary>
        /// 数据状态
        /// </summary>
        public StatusCode? StatusCode { set; get; }
        /// <summary>
        /// 数据版本
        /// </summary>
        public int? Version { set; get; } = 0;
    }
}
