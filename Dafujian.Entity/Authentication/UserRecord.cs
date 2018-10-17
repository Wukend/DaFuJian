using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dafujian.Entity.Authentication
{
    public class UserRecord
    {
        public System.Guid UserId { get; set; }
        public System.Guid RoleId { get; set; }
        public System.Guid TeamId { get; set; }
        public string Mobile { get; set; }
        public string NickName { get; set; }
        public string RealName { get; set; }
        public string Email { get; set; }
        public Nullable<int> MaxAccess { get; set; }
        public int RoleType { get; set; }
        public string RoleName { get; set; }
        public string TeamName { get; set; }
        public System.DateTime CreateTime { get; set; }
        public System.DateTime UpdateTime { get; set; }
    }
}
