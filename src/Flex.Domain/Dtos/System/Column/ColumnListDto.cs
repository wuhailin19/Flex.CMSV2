﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.Dtos.Column
{
    public class ColumnListDto
    {
        public int Id { set; get; }
        public int ParentId { set; get; }
        public string Name { set; get; }
        public bool IsShow { set; get; }
        public string ColumnUrl { set; get; }
        public int OrderId { set; get; }
        public bool IsSelect { set; get; }
        public bool IsAdd { set; get; }
        public bool IsEdit { set; get; }
        public bool IsDelete { set; get; }
    }
}
