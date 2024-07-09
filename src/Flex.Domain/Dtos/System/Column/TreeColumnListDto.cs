using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.Dtos.Column
{
    public class TreeColumnListDto
    {
        public int id { set; get; }
        public int ParentId { set; get; }
        public string title { set; get; }
        public bool IsShow { set; get; }
        public string ColumnUrl { set; get; }
        public string href { set; get; }
        public int OrderId { set; get; }
        public bool CanCopy { set; get; }
        public bool disabled { set; get; }
        public bool IsSelect { set; get; }
        public bool IsAdd { set; get; }
        public bool IsEdit { set; get; }
        public bool IsDelete { set; get; }
        public bool spread { set; get; } = true;

        public List<TreeColumnListDto> children { set; get; }
    }
}
