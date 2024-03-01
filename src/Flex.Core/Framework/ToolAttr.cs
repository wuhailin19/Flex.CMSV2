using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// 表格字段属性
/// </summary>
public class ToolAttr : Attribute
{
    /// <summary>
    /// 列名
    /// </summary>
    public string NameAttr { get; set; }
    /// <summary>
    /// 文字对齐方向
    /// </summary>
    public string AlignAttr { get; set; }
    /// <summary>
    /// 是否可排序
    /// </summary>
    public bool SortAttr { get; set; }
    public bool IsEdit { get; set; }
    /// <summary>
    /// 列描述
    /// </summary>
    public string Description { get; set; }
    /// <summary>
    /// 工具模板
    /// </summary>
    public string Toolbar { get; set; }
    /// <summary>
    /// 浮动方向
    /// </summary>
    public string Fixed { get; set; }
    /// <summary>
    /// 列宽度
    /// </summary>
    public string Width { get; set; }
    /// <summary>
    /// 列最小宽度
    /// </summary>
    public string minWidth { get; set; }
    /// <summary>
    /// 列最大宽度
    /// </summary>
    public string maxWidth { get; set; }
    /// <summary>
    /// 控件类型
    /// </summary>
    public string Types { get; set; }
    public string Style { get; set; }
    /// <summary>
    /// 是否隐藏
    /// </summary>
    public bool HideFiled { get; set; }

    public ToolAttr() { }
    /// <summary>
    /// 字段特性
    /// </summary>
    /// <param name="nameattr">字段名</param>
    /// <param name="description">字段描述</param>
    /// <param name="alignattr">文字水平对齐方向</param>
    /// <param name="sortattr">字段排序</param>
    /// <param name="toolbar">模板标识</param>
    /// <param name="_fixeds">浮动方向</param>
    /// <param name="width">字段展示宽度</param>
    /// <param name="_type">控件</param>
    public ToolAttr(
        string nameattr, bool isShow = false, bool sortattr = false,
        string width = "0",string minWidth="80",string maxWidth = "80", string alignattr = AlignEnum.Center, string description = null,
        string toolbar = null, string _fixeds = null, string _type = null, string _style = null, bool _edit = false)
    {
        this.NameAttr = nameattr;
        this.AlignAttr = alignattr;
        this.SortAttr = sortattr;
        this.Description = description;
        this.Toolbar = toolbar;
        this.Fixed = _fixeds;
        this.Width = width;
        this.Types = _type;
        this.HideFiled = isShow;
        this.minWidth = minWidth;
        this.maxWidth = maxWidth;
        this.Style = _style;
        this.IsEdit = _edit;
    }
}