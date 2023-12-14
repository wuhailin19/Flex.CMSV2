using Flex.Core.JsonConvertExtension;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Flex.Domain.Entities
{
    public class SysIndexSet :BaseIntEntity,EntityContext
    {
        /// <summary>
        /// 网站菜单
        /// </summary>
        [Column("Index_System_Menu")]
        public string SystemMenu { set; get; }
        /// <summary>
        /// 系统设置
        /// </summary>
        [Column("Index_Site_Menu")]
        public string SiteMenu { set; get; }
        /// <summary>
        /// 快捷栏目
        /// </summary>
        [Column("Index_Shortcut")]
        public string Shortcut { set; get; }
        /// <summary>
        /// 文件管理菜单
        /// </summary>
        [Column("Index_FileManage")]
        public string FileManage { set; get; }
        [JsonConverter(typeof(IdToStringConverter))]
        public long AdminId { set; get; }
    }
}
