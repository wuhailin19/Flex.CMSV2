using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

/// 生成数据表格字段属性
/// </summary>
/// <typeparam name="T"></typeparam>
public class ModelTools<T>
{
    public string field { get; set; }
    public string title { get; set; }
    public string align { get; set; }
    public string expandedMode { get; set; }
    public string expandedWidth { get; set; }
    public bool sort { get; set; }
    public string type { get; set; }
    public string toolbar { get; set; }
    public string templet { get; set; }
    public string @fixed { get; set; }
    public string width { get; set; }
    public string minWidth { get; set; }
    public string maxWidth { get; set; }
    public string style { get; set; }
    public string edit { get; set; }
    // 获取字段的属性
    private static ToolAttr getDescription(PropertyInfo field)
    {
        var toolAttr = (ToolAttr)Attribute.GetCustomAttribute(field, typeof(ToolAttr));
        return toolAttr;
    }
    /// <summary>
    /// 生成对应表内容
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public static List<ModelTools<T>> getColumnDescList()
    {
        List<ModelTools<T>> modelTools = new List<ModelTools<T>>();

        Type type = typeof(T);
        ToolAttr ToolAttrs = null;
        foreach (PropertyInfo pro in type.GetProperties())
        {
            ModelTools<T> modelTool = new ModelTools<T>();
            ToolAttrs = getDescription(pro);
            if (ToolAttrs.HideFiled)
                continue;
            if (pro.PropertyType != typeof(controlType))
            {
                modelTool.field = pro.Name;
            }
            modelTool.sort = ToolAttrs.SortAttr;
            modelTool.align = ToolAttrs.AlignAttr;
            modelTool.title = ToolAttrs.NameAttr;
            modelTool.templet = ToolAttrs.Toolbar;
            modelTool.expandedWidth = ToolAttrs.expandedWidth;
            modelTool.expandedMode = ToolAttrs.expandedMode;
            modelTool.type = ToolAttrs.Types;
            modelTool.@fixed = ToolAttrs.Fixed;
            modelTool.width = ToolAttrs.Width;
            modelTool.minWidth = ToolAttrs.minWidth;
            modelTool.maxWidth = ToolAttrs.maxWidth;
            modelTool.style = ToolAttrs.Style;
            modelTool.edit = ToolAttrs.IsEdit ? "editable" : "";

            modelTools.Add(modelTool);
        }
        return modelTools;
    }
}







