using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dafujian.Entity.Models
{
    public partial class D_User
    {
        public System.Guid UserId { get; set; }
        public System.Guid RoleId { get; set; }
        public string Mobile { get; set; }
        public string Pwd { get; set; }
        public string Salt { get; set; }
        public string Nickname { get; set; }
        public string Real { get; set; }
        public string Email { get; set; }
        public Nullable<int> MaxAccess { get; set; }
        public int IsDelete { get; set; }
        public System.DateTime CreateTime { get; set; }
        public System.DateTime UpdateTime { get; set; }

        /// <summary>
        /// This field is used to extend the value, does not exist in the database
        /// </summary>
        [NotMapped]
        public string ExtenKey { get; set; }


        /// <summary>
        /// This field is used to extend the value, does not exist in the database
        /// </summary>
        [NotMapped]
        public string ExtenKey2 { get; set; }


        /// <summary>
        /// This field is used to extend the value, does not exist in the database
        /// </summary>
        [NotMapped]
        public string ExtenKey3 { get; set; }


        /// <summary>
        /// This field is used to extend the value, does not exist in the database
        /// </summary>
        [NotMapped]
        public List<string> ExtenKeyList { get; set; }
    }
}
