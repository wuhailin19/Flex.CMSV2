using Flex.Core.Attributes;
using Flex.Core.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Flex.Core.Reflection
{
    public class ReflectionUrl
    {
        /// <summary>
        /// 反射后台所有控制器和下面的公开action
        /// </summary>
        /// <returns></returns>
        public static List<ReflectMenuModel> GetALLMenuByReflection()
        {
            Type T = typeof(HttpMethodAttribute);
            List<ReflectMenuModel> controls = new List<ReflectMenuModel>();
            var assemblies = DAssemblyFinder.Instance.FindAll().Where(m => m.GetName().Name.EndsWith("SingleWeb"));
            List<Type> typeList = new List<Type>();
            assemblies.Foreach(asm =>
            {
                var types = asm.GetTypes();
                foreach (Type type in types)
                {
                    string s = type.FullName.ToLower();
                    if (s.EndsWith("controller"))
                        typeList.Add(type);
                }
                typeList.Sort(delegate (Type type1, Type type2) { return type1.FullName.CompareTo(type2.FullName); });
                foreach (Type type in typeList)
                {
                    ReflectMenuModel rm = new ReflectMenuModel();
                    rm.ActionList = new List<ReflectMenuActionModel>();

                    //Response.Write(type.Name.Replace("Controller","") + "<br/>\n");
                    System.Reflection.MemberInfo[] members = type.FindMembers(System.Reflection.MemberTypes.Method,
                    System.Reflection.BindingFlags.Public |
                    System.Reflection.BindingFlags.Static |
                    System.Reflection.BindingFlags.Instance |
                    System.Reflection.BindingFlags.DeclaredOnly,
                    Type.FilterName, "*");
                    string controller = type.Name.Replace("Controller", "");
                    bool IsAdd = true;
                    //反射Description属性，作为菜单名称
                    object[] descriptionList = type.GetCustomAttributes(typeof(DescriperAttribute), false);
                    if (descriptionList != null && descriptionList.Length > 0)
                    {
                        foreach (var dm in descriptionList)
                        {
                            DescriperAttribute da = dm as DescriperAttribute;
                            if (da.IsFilter)
                            {
                                IsAdd = false;
                                break;
                            }
                            rm.MenuName = da.Name;
                            break;
                        }
                    }
                    else
                    {
                        rm.MenuName = controller;
                    }
                    if (!IsAdd)
                        continue;
                    rm.MenuController = controller;
                    string url = "/api/" + controller + "/";
                    bool isViewAction = false;
                    if (type.GetAttribute<AreaAttribute>() != null)
                    {
                        url = "/system/" + controller + "/";
                        isViewAction = true;
                    }
                    rm.MenuLink = url;
                    foreach (var m in members)
                    {
                        if (m.DeclaringType.Attributes.HasFlag(System.Reflection.TypeAttributes.Public) != true)
                            continue;

                        ReflectMenuActionModel rma = new ReflectMenuActionModel();
                        string action = m.Name;
                        rma.ActionCode = action;

                        if (m.GetCustomAttributes(typeof(IAllowAnonymous), false).Count() > 0)
                        {
                            rma.ActionPermission = false;
                        }

                        foreach (var item in m.CustomAttributes)
                        {
                            if (item.AttributeType.BaseType.Name == T.Name)
                            {
                                rma.ActionType = item.AttributeType.Name.Replace("Attribute", "");
                                if (item.ConstructorArguments.Count <= 0)
                                {
                                    continue;
                                }
                                var argumentname = item.ConstructorArguments.FirstOrDefault().Value.ToString();
                                if (!argumentname.IsNullOrEmpty())
                                {
                                    rma.ActionCode = argumentname;
                                }
                            }
                        }

                        if (action.Contains("<") || action.Contains(">")) continue;
                        rma.Cate = 1;
                        if (isViewAction) {
                            rma.Cate = 2;
                        }
                        bool isNeedAdd = true;
                        //反射系统自己带的其它属性
                        //System.Reflection.TypeAttributes attrs = m.DeclaringType.Attributes;
                        //反射自定义属性DescriperAttribute,过滤不需要的
                        object[] deserlist = m.GetCustomAttributes(typeof(DescriperAttribute), false);
                        if (deserlist != null && deserlist.Length > 0)
                        {
                            foreach (var cm in deserlist)
                            {
                                DescriperAttribute da = cm as DescriperAttribute;
                                if (da.IsFilter)
                                {
                                    isNeedAdd = false;
                                    break;
                                }
                                rma.ActionId = da.cateEnum;
                                rma.ActionDesc = da.Desc;
                                rma.ActionName = da.Name.ToLower();
                                break;
                            }
                        }
                        if (!isNeedAdd)
                        {
                            continue;
                        }

                        if (rma.ActionName.IsNullOrEmpty())
                        {
                            rma.ActionName = action;
                        }
                        if (rma.ActionDesc.IsNullOrEmpty())
                            rma.ActionDesc = rma.ActionName;
                        var om = controls.FirstOrDefault(pm => pm.MenuLink == url);
                        if (om == null)
                        {
                            rm.ActionList.Add(rma);
                            controls.Add(rm);
                        }
                        else
                        {
                            var som = om.ActionList.FirstOrDefault(pm => pm.ActionCode == action);
                            if (som == null)
                            {
                                rm.ActionList.Add(rma);
                            }
                        }
                    }
                }
            });
            return controls;
        }
    }
}
