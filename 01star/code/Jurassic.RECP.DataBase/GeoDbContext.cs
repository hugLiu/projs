using Jurassic.RECP.DataBase.Models;

namespace Jurassic.RECP.DataBase
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Data.Entity.ModelConfiguration.Conventions;

    public partial class GeoDbContext : DbContext
    {
        public GeoDbContext()
            : base("name=DefaultConnection")
        {
            System.Diagnostics.Debug.WriteLine("GeoDbContext:" + Guid.NewGuid().ToString());
        }

        public virtual DbSet<RECP_Album> RECP_Album { get; set; }
        public virtual DbSet<RECP_Video> RECP_Video { get; set; }

        public virtual DbSet<webpages_Membership> webpages_Membership { get; set; }
        public virtual DbSet<GT_UserBehavior> GT_UserBehavior { get; set; }
   
        public virtual DbSet<GT_Code> GT_Code { get; set; }
        public virtual DbSet<GT_CodeType> GT_CodeType { get; set; } 

        public virtual DbSet<GT_TopicCard> GT_TopicCard { get; set; }

        public virtual DbSet<UserProfile> UserProfile { get; set; }
        public virtual DbSet<webpages_Roles> webpages_Roles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserProfile>()
                   .Property(e => e.Email)
                   .IsUnicode(false);

            modelBuilder.Entity<UserProfile>()
                .Property(e => e.PhoneNumber)
                .IsUnicode(false);

            modelBuilder.Entity<webpages_Roles>()
                .HasMany(e => e.UserProfile)
                .WithMany(e => e.webpages_Roles)
                .Map(m => m.ToTable("webpages_UsersInRoles").MapLeftKey("RoleId").MapRightKey("UserId")); 
        }
    }
}
