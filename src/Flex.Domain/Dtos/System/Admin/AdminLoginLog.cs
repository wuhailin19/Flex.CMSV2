using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.Dtos.Admin
{
    public class AdminLoginLog
    {
        public DateTime? LastLoginTime { set; get; }
        public DateTime? CurrentLoginTime { set; get; }
        public string? LastLoginIP { set; get; }
        public string? CurrentLoginIP { set; get; }
    }
}
