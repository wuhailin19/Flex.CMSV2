using Flex.Core.Framework.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.Entities
{
    public class sysField : BaseIntEntity, EntityContext
    {
        /// <summary>
        /// 字段含义
        /// </summary>
        public string Name { set; get; }
        /// <summary>
        /// 字段名称
        /// </summary>
        public string FieldName { set; get; }
        /// <summary>
        /// 字段描述
        /// </summary>
        public string? FieldDescription { set; get; }
         /// <summary>
         /// 字段类型
         /// </summary>
        public string FieldType { set; get; }

        public int OrderId { set; get; }
        /// <summary>
        /// 字段验证
        /// </summary>
        public string? Validation { set; get;  }
        /// <summary>
        /// 字段属性（宽度高度等）
        /// </summary>
        public string? FieldAttritude { set; get; }
        /// <summary>
        /// 接口字段名
        /// </summary>
        public string? ApiName { set; get; }
        /// <summary>
        /// 是否展示在接口
        /// </summary>
        public bool? IsApiField { set; get; }
        /// <summary>
        /// 是否用于搜索
        /// </summary>
        public bool? IsSearch { set; get; }
        public bool? ShowInTable { set; get; }
        public int ModelId { set; get; }
    }
}
