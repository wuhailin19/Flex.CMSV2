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
            $"                <textarea name=\"{sysField.FieldName}\" id=\"{sysField.FieldName}\" style=\"width: 0px\" ></textarea>" +
            "                <script type=\"text/javascript\">" +
            $"                    {{{{# var editorOption = {{ initialFrameWidth: {(attritude.Width.IsNullOrEmpty()?"700":attritude.Width)}, initialFrameHeight: {(attritude.Height.IsNullOrEmpty() ? "420" : attritude.Height)} }}; " +
            "                    var ue = new UE.ui.Editor(editorOption); " +
            $"                    ue.render(\"{sysField.FieldName}\"); }}}}" +
            "               </script>" +
            "            </div>" +
            "        </div>";
            return htmltemplete;
        }
    }
}
