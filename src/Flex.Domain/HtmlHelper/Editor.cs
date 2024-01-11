using Flex.Domain.Basics;
using Flex.Domain.Dtos.Field;

namespace Flex.Domain.HtmlHelper
{
    public class Editor:BaseFieldType
    {
        public override string ToHtmlString(sysField sysField, FiledValidateModel validation, FieldAttritudeModel attritude)
        {
            string htmltemplete = "<div class=\"layui-form-item layui-form-text\">" +
            $"            <label class=\"layui-form-label\">{sysField.Name}</label>" +
            "            <div class=\"layui-input-block\">" +
            $"                <script id=\"{sysField.FieldName}\" type=\"text/plain\" style=\"\"></script>" +
            "                <script type=\"text/javascript\">" +
            $"                    {{{{#         editorarray.push('{sysField.FieldName}'); var {sysField.FieldName}_editorOption = {{ initialFrameWidth: {(attritude.Width.IsNullOrEmpty()?"700":attritude.Width)}, initialFrameHeight: {(attritude.Height.IsNullOrEmpty() ? "420" : attritude.Height)} }}; " +
            $"                    var {sysField.FieldName}_ue = new UE.ui.Editor({sysField.FieldName}_editorOption); " +
            $"                    {sysField.FieldName}_ue.render(\"{sysField.FieldName}\"); }}}}" +
            "               </script>" +
            "            </div>" +
            "        </div>";
            return htmltemplete;
        }
    }
}
