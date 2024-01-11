using Flex.Domain.Basics;
using Flex.Domain.Dtos.Field;

namespace Flex.Domain.HtmlHelper
{
    public class TextBox: BaseFieldType
    {
        public override string ToHtmlString(sysField sysField, FiledValidateModel validation, FieldAttritudeModel attritude)
        {
            var stylestr = string.Empty;
            if (attritude.Width.IsNotNullOrEmpty())
                stylestr += $"width:{attritude.Width};";
            if (attritude.Height.IsNotNullOrEmpty())
                stylestr += $"height:{attritude.Height};";
            return
            "<div class=\"layui-form-item layui-form-text\">"
            + $"    <label class=\"layui-form-label\">{sysField.Name}</label>"
            + "    <div class=\"layui-input-block\">"
            + $"        <input type=\"text\" name=\"{sysField.FieldName}\" style=\"{stylestr}\" placeholder=\"请输入{sysField.Name}\" autocomplete=\"off\" class=\"layui-input\" {validation.ValidateEmpty} {validation.ValidateNumber}>"
            + "    </div>"
            + "</div>";
        }
    }
}
