using System;
using System.Collections.Generic;
using System.Text;

namespace Flex.Core.Reflection
{
    /// <summary>
    /// 反射控制器实体
    /// </summary>
    public class ReflectMenuModel
    {
        /// <summary>
        /// 菜单名称,通过反射Desction属性获得
        /// </summary>
        public string MenuName { set; get; }

        /// <summary>
        /// 控制器名称
        /// </summary>
        public string MenuController { set; get; }


        /// <summary>
        /// 菜单链接
        /// </summary>
        public string MenuLink { set; get; }

        /// <summary>
        /// 菜单权限明细
        /// </summary>
        public List<ReflectMenuActionModel> ActionList { set; get; }
    }
}
