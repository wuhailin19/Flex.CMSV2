namespace Flex.Core.Reflection
{
    public class ReflectMenuActionModel
    {
        /// <summary>
        /// 动作名称,读自定义属性的值
        /// </summary>
        public string ActionName { set; get; }

        /// <summary>
        /// 动作对应的权限ID
        /// </summary>
        public int ActionId { set; get; }
        public int Cate { set; get; }

        /// <summary>
        /// 动作描述
        /// </summary>
        public string ActionDesc { set; get; }
        /// <summary>
        /// 请求类型
        /// </summary>
        public string ActionType { set; get; } = "HttpGet";
        /// <summary>
        /// 是否需要权限
        /// </summary>
        public bool ActionPermission { set; get; } = true;

        /// <summary>
        /// Action
        /// </summary>
        public string ActionCode { set; get; }
    }
}
