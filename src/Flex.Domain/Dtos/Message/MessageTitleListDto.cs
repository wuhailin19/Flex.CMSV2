using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.Dtos.Message
{
    public class MessageTitleListDto
    {
        public int Id { set; get; }
        public DateTime AddTime { set; get; }
        public string Title { set; get; }
        public bool IsRead { set; get; }
    }
}
