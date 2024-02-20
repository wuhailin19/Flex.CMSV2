using Flex.Domain.Enums.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.Dtos.Message
{
    public class MessageOutputDto
    {
        public int Id { set; get; }
        public DateTime AddTime { set; get; }
        public string Title { set; get; }
        public string AddUserName { set; get; }
        public string? MsgContent { set; get; }
        public MessageCate MessageCate { set; get; } = MessageCate.Nomal;
    }
}
