using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dafujian.Entity.Models.Mapping
{
    public class D_UserMap : EntityTypeConfiguration<D_User>
    {
        public D_UserMap()
        {
            // Primary Key
            this.HasKey(t => t.UserId);

            // Properties
            this.Property(t => t.Mobile)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Pwd)
                .IsRequired()
                .HasMaxLength(80);

            this.Property(t => t.Salt)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Nickname)
                .HasMaxLength(20);

            this.Property(t => t.Real)
                .HasMaxLength(20);

            this.Property(t => t.Email)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("D_User");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.RoleId).HasColumnName("RoleId");
            this.Property(t => t.Mobile).HasColumnName("Mobile");
            this.Property(t => t.Pwd).HasColumnName("Pwd");
            this.Property(t => t.Salt).HasColumnName("Salt");
            this.Property(t => t.Nickname).HasColumnName("Nickname");
            this.Property(t => t.Real).HasColumnName("Real");
            this.Property(t => t.Email).HasColumnName("Email");
            this.Property(t => t.MaxAccess).HasColumnName("MaxAccess");
            this.Property(t => t.IsDelete).HasColumnName("IsDelete");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            this.Property(t => t.UpdateTime).HasColumnName("UpdateTime");
        }
    }
}
