using Flex.Domain.Enums.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.Dtos.System.Message
{
    public class ApprovalProcessDto
    {
        public int Id { set; get; }
        public string Title { set; get; }
        public int Modeld { set; get; }
        public int FlowId { set; get; }
        public int ContentId { set; get; }
        public int RecieveId { set; get; }
        public string MessageResult { set; get; }
        public int MessageCate { set; get; }
        public bool IsStart { set; get; }
        public bool IsEnd { set; get; }
        public string FromPathId { set; get; }
        public string ToPathId { set; get; }
        public string AddUserName { set; get; }
        public long MsgGroupId { set; get; }
        public ApprovalProcessDto next { set; get; }
    }
}
