using System.ComponentModel.DataAnnotations;

namespace Flex.Domain.Dtos.Column.Basic
{
    public class InputColumnDto
    {
        [Required(ErrorMessage = "栏目名称为必填")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "栏目名称的长度必须在1到50个字符之间")]
        public string Name { set; get; }
        public string? ColumnImage { set; get; }
        public int? ModelId { set; get; }
        /// <summary>
        /// 可扩展的栏目模型
        /// </summary>
        public int? ExtensionModelId { get; set; }
        public int ParentId { set; get; }
        public bool IsShow { set; get; }
        public string? ColumnUrl { set; get; }
        public string? SeoTitle { set; get; }
        public string? SeoKeyWord { set; get; }
        public string? SeoDescription { set; get; }
        public int OrderId { set; get; }
    }
}
