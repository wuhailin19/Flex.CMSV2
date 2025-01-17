﻿using Flex.Core.JsonConvertExtension;
using Newtonsoft.Json;

namespace Flex.Domain.Dtos.Menu
{
    public class MenuDto
    {
        public string title { get; set; }
        public int id { get; set; }
        public int parentid { get; set; }
        public int OrderId { get; set; }
        public string icode { get; set; }
        public bool spread { get; set; } = true;
        public string linkurl { get; set; }
        public bool ismenu { get; set; }
        public bool isaspx { get; set; }
        public bool @checked { get; set; }
        public bool status { get; set; }
        public string className { get; set; }
        public List<MenuDto> children { get; set; }
    }
}
