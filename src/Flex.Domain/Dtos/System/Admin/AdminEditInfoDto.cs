using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.Dtos.Admin
{
    public class AdminEditInfoDto
    {
        public long Id { get; set; }
		public string Account { get; set; }
		public string UserName { get; set; }
        public string UserAvatar { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string UserSign { get; set; }
        public string CurrentLoginIP { get; set; }
        public int ErrorCount { get; set; }
        public int MaxErrorCount { get; set; }
        public int LoginCount { get; set; }
        public bool AllowMultiLogin { get; set; }
        public AdminLoginLog adminLoginLog { set; get; }
        public bool Islock { get; set; }
        public string FilterIp { set; get; }
        public string AddUserName { set; get; }
        public string LastEditUserName { set; get; }
        public DateTime LastEditDate { set; get; }
        public DateTime AddTime { set; get; }
        public DateTime CurrentLoginTime { set; get; }
        public int Version { set; get; }
    }
}
