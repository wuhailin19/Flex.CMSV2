using Flex.Domain.Dtos.Column.Basic;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.Dtos.Column
{
    public class AppendColumnDto
    {
        public int ParentId { set; get; }
        [Required(ErrorMessage = "栏目列表为必填")]
        public string ColumnList { set; get; }
        public string ColumnImage { set; get; }
        public int? ModelId { set; get; }
        /// <summary>
        /// 可扩展的栏目模型
        /// </summary>
        public int? ExtensionModelId { get; set; }
        public int? ReviewMode { get; set; }
        public string ColumnUrl { get; set; }
    }
}
