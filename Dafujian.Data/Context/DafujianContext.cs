using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Dafujian.Data.Context
{
    public partial class DafujianContext : DbContext
    {
        static DafujianContext()
        {
            Database.SetInitializer<DafujianContext>(null);
        }

        public DafujianContext()
            : base("Name=DafujianContext")
        {
        }

        //public DbSet<EM_Base> EM_Base { get; set; }
        //public DbSet<EM_Client> EM_Client { get; set; }
        //public DbSet<EM_Dialog> EM_Dialog { get; set; }
        //public DbSet<EM_Evaluate> EM_Evaluate { get; set; }
        //public DbSet<EM_Menu> EM_Menu { get; set; }
        //public DbSet<EM_QnaMaker> EM_QnaMaker { get; set; }
        //public DbSet<EM_QuickReply> EM_QuickReply { get; set; }
        //public DbSet<EM_QuickReplyTeam> EM_QuickReplyTeam { get; set; }
        //public DbSet<EM_Role> EM_Role { get; set; }
        //public DbSet<EM_Setting> EM_Setting { get; set; }
        //public DbSet<EM_Team> EM_Team { get; set; }
        //public DbSet<EM_Team_User> EM_Team_User { get; set; }
        //public DbSet<EM_User> EM_User { get; set; }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Configurations.Add(new EM_BaseMap());
        //    modelBuilder.Configurations.Add(new EM_ClientMap());
        //    modelBuilder.Configurations.Add(new EM_DialogMap());
        //    modelBuilder.Configurations.Add(new EM_EvaluateMap());
        //    modelBuilder.Configurations.Add(new EM_MenuMap());
        //    modelBuilder.Configurations.Add(new EM_QnaMakerMap());
        //    modelBuilder.Configurations.Add(new EM_QuickReplyMap());
        //    modelBuilder.Configurations.Add(new EM_QuickReplyTeamMap());
        //    modelBuilder.Configurations.Add(new EM_RoleMap());
        //    modelBuilder.Configurations.Add(new EM_SettingMap());
        //    modelBuilder.Configurations.Add(new EM_TeamMap());
        //    modelBuilder.Configurations.Add(new EM_Team_UserMap());
        //    modelBuilder.Configurations.Add(new EM_UserMap());
        //}
    }
}
