using System;
using System.Collections.Generic;
using System.Text;

namespace Flex.Domain.Dtos.AuthCode
{
    public class AuthCodeOutputDto
    {
        public string CodeId { get; set; }
        public string ImageCode { get; set; }
        public string Publickey { get; set; }
    }
}
