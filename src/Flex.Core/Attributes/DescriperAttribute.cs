using System.ComponentModel.DataAnnotations;

namespace Flex.Core.Attributes
{
    public class DescriperAttribute : ValidationAttribute
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { set; get; }

        /// <summary>
        /// 操作权限码0：查看；1：新增；2：修改；3：删除；4：审批；5：导出；6：同步；7：打印
        /// </summary>
        public int Nid { set; get; }

        /// <summary>
        /// 接口操作类型
        /// </summary>
        public int cateEnum { set; get; } = OperationCateEnum.Read;
        /// <summary>
        /// 接口介绍
        /// </summary>
        public string Desc { set; get; }

        /// <summary>
        /// 枚举的时候，是否列在其中
        /// </summary>
        public bool IsFilter { set; get; }
    }

}
