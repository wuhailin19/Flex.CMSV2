using Flex.Domain.Dtos.Field;

namespace Flex.Domain.Basics
{
    public class BaseFieldType
    {
        public virtual string ToHtmlString(sysField sysField, FieldAttritudeModel attritude)
        {
            return string.Empty;
        }
        public virtual string ToHtmlString(sysField sysField, FiledValidateModel validation, FieldAttritudeModel attritude)
        {
            return string.Empty;
        }
    }
}
