using Jurassic.RECP.DataBase.Models;
using Jurassic.RECP.DataBase.Models.View;

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
         
        public virtual DbSet<webpages_Membership> webpages_Membership { get; set; }
        public virtual DbSet<GT_UserBehavior> GT_UserBehavior { get; set; } 
        public virtual DbSet<RECP_User_Behavior> RECP_User_Behavior { get; set; }
        public virtual DbSet<RECP_User_FavoriteCatalog> RECP_User_FavoriteCatalog { get; set; }
        public virtual DbSet<RECP_BO_ProMetaAttribute> RECP_BO_ProMetaAttribute { get; set; }
        public virtual DbSet<RECP_BO_ProMetaDefine> RECP_BO_ProMetaDefine { get; set; }

        public virtual DbSet<BO_BaseInfo> BO_BaseInfo { get; set; }
        public virtual DbSet<BO_BOAlias> BO_BOAlias { get; set; } 
  
        public virtual DbSet<GT_Code> GT_Code { get; set; }
        public virtual DbSet<GT_CodeType> GT_CodeType { get; set; }

        public virtual DbSet<Temp_ImportProData> Temp_ImportProData { get; set; }

        public virtual DbSet<v_BO_ThreeLevReserve> v_BO_ThreeLevReserve { get; set; }
        public virtual DbSet<v_BO_WellPoint> v_BO_WellPoint { get; set; }
        public virtual DbSet<v_BO_WellTest> v_BO_WellTest { get; set; }
        public virtual DbSet<V_Temp_ImportBO> V_Temp_ImportBO { get; set; }
        public virtual DbSet<V_Temp_ImportProData> V_Temp_ImportProData { get; set; }

        public virtual DbSet<GT_TopicCard> GT_TopicCard { get; set; }

        public virtual DbSet<UserProfile> UserProfile { get; set; }
        public virtual DbSet<webpages_Roles> webpages_Roles { get; set; }

        public virtual DbSet<RECP_ScopeBaseInfo> RECP_ScopeBaseInfo { get; set; }

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

            modelBuilder.Entity<RECP_BO_ProMetaDefine>()
                .HasMany(e => e.RECP_BO_ProMetaAttribute)
                .WithRequired(e => e.RECP_BO_ProMetaDefine)
                .HasForeignKey(e => new { e.FieldNo, e.State })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<v_BO_ThreeLevReserve>()
                 .Property(e => e.Year)
                 .IsFixedLength()
                 .IsUnicode(false);

            modelBuilder.Entity<v_BO_ThreeLevReserve>()
                .Property(e => e.Category)
                .IsUnicode(false);

            modelBuilder.Entity<v_BO_ThreeLevReserve>()
                .Property(e => e.Series)
                .IsUnicode(false);

            modelBuilder.Entity<v_BO_ThreeLevReserve>()
                .Property(e => e.Formation)
                .IsUnicode(false);

            modelBuilder.Entity<v_BO_ThreeLevReserve>()
                .Property(e => e.Layer)
                .IsUnicode(false);

            modelBuilder.Entity<v_BO_ThreeLevReserve>()
                .Property(e => e.BlockGuan)
                .IsUnicode(false);

            modelBuilder.Entity<v_BO_WellPoint>()
                .Property(e => e.Elevation)
                .IsUnicode(false);

            modelBuilder.Entity<v_BO_WellPoint>()
                .Property(e => e.XCoordinate)
                .IsUnicode(false);

            modelBuilder.Entity<v_BO_WellPoint>()
                .Property(e => e.YCoordinate)
                .IsUnicode(false);

            modelBuilder.Entity<v_BO_WellPoint>()
                .Property(e => e.Parents)
                .IsUnicode(false);

            modelBuilder.Entity<v_BO_WellPoint>()
                .Property(e => e.BlockGuan)
                .IsUnicode(false);

            modelBuilder.Entity<v_BO_WellPoint>()
                .Property(e => e.BlockType)
                .IsUnicode(false);

            modelBuilder.Entity<v_BO_WellTest>()
                .Property(e => e.Parents)
                .IsUnicode(false);

            modelBuilder.Entity<v_BO_WellTest>()
                .Property(e => e.BlockGuan)
                .IsUnicode(false);

            modelBuilder.Entity<v_BO_WellTest>()
                .Property(e => e.BlockType)
                .IsUnicode(false);

            modelBuilder.Entity<GT_CodeType>()
                .HasMany(e => e.GT_Code)
                .WithRequired(e => e.GT_CodeType)
                .HasForeignKey(e => e.CodeTypeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Temp_ImportProData>()
                .Property(e => e.MetaEntity)
                .IsUnicode(false);

            modelBuilder.Entity<Temp_ImportProData>()
                .Property(e => e.A)
                .IsUnicode(false);

            modelBuilder.Entity<Temp_ImportProData>()
                .Property(e => e.B)
                .IsUnicode(false);

            modelBuilder.Entity<Temp_ImportProData>()
                .Property(e => e.C)
                .IsUnicode(false);

            modelBuilder.Entity<Temp_ImportProData>()
                .Property(e => e.D)
                .IsUnicode(false);

            modelBuilder.Entity<Temp_ImportProData>()
                .Property(e => e.E)
                .IsUnicode(false);

            modelBuilder.Entity<Temp_ImportProData>()
                .Property(e => e.F)
                .IsUnicode(false);

            modelBuilder.Entity<Temp_ImportProData>()
                .Property(e => e.G)
                .IsUnicode(false);

            modelBuilder.Entity<Temp_ImportProData>()
                .Property(e => e.H)
                .IsUnicode(false);

            modelBuilder.Entity<Temp_ImportProData>()
                .Property(e => e.I)
                .IsUnicode(false);

            modelBuilder.Entity<Temp_ImportProData>()
                .Property(e => e.J)
                .IsUnicode(false);

            modelBuilder.Entity<Temp_ImportProData>()
                .Property(e => e.K)
                .IsUnicode(false);

            modelBuilder.Entity<Temp_ImportProData>()
                .Property(e => e.L)
                .IsUnicode(false);

            modelBuilder.Entity<Temp_ImportProData>()
                .Property(e => e.M)
                .IsUnicode(false);

            modelBuilder.Entity<Temp_ImportProData>()
                .Property(e => e.N)
                .IsUnicode(false);

            modelBuilder.Entity<Temp_ImportProData>()
                .Property(e => e.O)
                .IsUnicode(false);

            modelBuilder.Entity<Temp_ImportProData>()
                .Property(e => e.P)
                .IsUnicode(false);

            modelBuilder.Entity<Temp_ImportProData>()
                .Property(e => e.Q)
                .IsUnicode(false);

            modelBuilder.Entity<Temp_ImportProData>()
                .Property(e => e.R)
                .IsUnicode(false);

            modelBuilder.Entity<Temp_ImportProData>()
                .Property(e => e.S)
                .IsUnicode(false);

            modelBuilder.Entity<Temp_ImportProData>()
                .Property(e => e.T)
                .IsUnicode(false);

            modelBuilder.Entity<Temp_ImportProData>()
                .Property(e => e.U)
                .IsUnicode(false);

            modelBuilder.Entity<Temp_ImportProData>()
                .Property(e => e.V)
                .IsUnicode(false);

            modelBuilder.Entity<Temp_ImportProData>()
                .Property(e => e.W)
                .IsUnicode(false);

            modelBuilder.Entity<Temp_ImportProData>()
                .Property(e => e.X)
                .IsUnicode(false);

            modelBuilder.Entity<Temp_ImportProData>()
                .Property(e => e.Y)
                .IsUnicode(false);

            modelBuilder.Entity<Temp_ImportProData>()
                .Property(e => e.Z)
                .IsUnicode(false);

            modelBuilder.Entity<Temp_ImportProData>()
                .Property(e => e.AA)
                .IsUnicode(false);

            modelBuilder.Entity<Temp_ImportProData>()
                .Property(e => e.AB)
                .IsUnicode(false);

            modelBuilder.Entity<Temp_ImportProData>()
                .Property(e => e.AC)
                .IsUnicode(false);

            modelBuilder.Entity<Temp_ImportProData>()
                .Property(e => e.AD)
                .IsUnicode(false);

            modelBuilder.Entity<Temp_ImportProData>()
                .Property(e => e.AE)
                .IsUnicode(false);

            modelBuilder.Entity<Temp_ImportProData>()
                .Property(e => e.AF)
                .IsUnicode(false);

            modelBuilder.Entity<Temp_ImportProData>()
                .Property(e => e.AG)
                .IsUnicode(false);

            modelBuilder.Entity<Temp_ImportProData>()
                .Property(e => e.AH)
                .IsUnicode(false);

            modelBuilder.Entity<Temp_ImportProData>()
                .Property(e => e.AI)
                .IsUnicode(false);

            modelBuilder.Entity<Temp_ImportProData>()
                .Property(e => e.AJ)
                .IsUnicode(false);

            modelBuilder.Entity<Temp_ImportProData>()
                .Property(e => e.AK)
                .IsUnicode(false);

            modelBuilder.Entity<Temp_ImportProData>()
                .Property(e => e.AL)
                .IsUnicode(false);

            modelBuilder.Entity<Temp_ImportProData>()
                .Property(e => e.AM)
                .IsUnicode(false);

            modelBuilder.Entity<Temp_ImportProData>()
                .Property(e => e.AN)
                .IsUnicode(false);

            modelBuilder.Entity<Temp_ImportProData>()
                .Property(e => e.AO)
                .IsUnicode(false);

            modelBuilder.Entity<V_Temp_ImportBO>()
                .Property(e => e.MetaEntity)
                .IsUnicode(false);

            modelBuilder.Entity<V_Temp_ImportBO>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<V_Temp_ImportProData>()
                .Property(e => e.MetaEntity)
                .IsUnicode(false);

            modelBuilder.Entity<V_Temp_ImportProData>()
                .Property(e => e.A)
                .IsUnicode(false);

            modelBuilder.Entity<V_Temp_ImportProData>()
                .Property(e => e.B)
                .IsUnicode(false);

            modelBuilder.Entity<V_Temp_ImportProData>()
                .Property(e => e.C)
                .IsUnicode(false);

            modelBuilder.Entity<V_Temp_ImportProData>()
                .Property(e => e.D)
                .IsUnicode(false);

            modelBuilder.Entity<V_Temp_ImportProData>()
                .Property(e => e.E)
                .IsUnicode(false);

            modelBuilder.Entity<V_Temp_ImportProData>()
                .Property(e => e.F)
                .IsUnicode(false);

            modelBuilder.Entity<V_Temp_ImportProData>()
                .Property(e => e.G)
                .IsUnicode(false);

            modelBuilder.Entity<V_Temp_ImportProData>()
                .Property(e => e.H)
                .IsUnicode(false);

            modelBuilder.Entity<V_Temp_ImportProData>()
                .Property(e => e.I)
                .IsUnicode(false);

            modelBuilder.Entity<V_Temp_ImportProData>()
                .Property(e => e.J)
                .IsUnicode(false);

            modelBuilder.Entity<V_Temp_ImportProData>()
                .Property(e => e.K)
                .IsUnicode(false);

            modelBuilder.Entity<V_Temp_ImportProData>()
                .Property(e => e.L)
                .IsUnicode(false);

            modelBuilder.Entity<V_Temp_ImportProData>()
                .Property(e => e.M)
                .IsUnicode(false);

            modelBuilder.Entity<V_Temp_ImportProData>()
                .Property(e => e.N)
                .IsUnicode(false);

            modelBuilder.Entity<V_Temp_ImportProData>()
                .Property(e => e.O)
                .IsUnicode(false);

            modelBuilder.Entity<V_Temp_ImportProData>()
                .Property(e => e.P)
                .IsUnicode(false);

            modelBuilder.Entity<V_Temp_ImportProData>()
                .Property(e => e.Q)
                .IsUnicode(false);

            modelBuilder.Entity<V_Temp_ImportProData>()
                .Property(e => e.R)
                .IsUnicode(false);

            modelBuilder.Entity<V_Temp_ImportProData>()
                .Property(e => e.S)
                .IsUnicode(false);

            modelBuilder.Entity<V_Temp_ImportProData>()
                .Property(e => e.T)
                .IsUnicode(false);

            modelBuilder.Entity<V_Temp_ImportProData>()
                .Property(e => e.U)
                .IsUnicode(false);

            modelBuilder.Entity<V_Temp_ImportProData>()
                .Property(e => e.V)
                .IsUnicode(false);

            modelBuilder.Entity<V_Temp_ImportProData>()
                .Property(e => e.W)
                .IsUnicode(false);

            modelBuilder.Entity<V_Temp_ImportProData>()
                .Property(e => e.X)
                .IsUnicode(false);

            modelBuilder.Entity<V_Temp_ImportProData>()
                .Property(e => e.Y)
                .IsUnicode(false);

            modelBuilder.Entity<V_Temp_ImportProData>()
                .Property(e => e.Z)
                .IsUnicode(false);

            modelBuilder.Entity<V_Temp_ImportProData>()
                .Property(e => e.AA)
                .IsUnicode(false);

            modelBuilder.Entity<V_Temp_ImportProData>()
                .Property(e => e.AB)
                .IsUnicode(false);

            modelBuilder.Entity<V_Temp_ImportProData>()
                .Property(e => e.AC)
                .IsUnicode(false);

            modelBuilder.Entity<V_Temp_ImportProData>()
                .Property(e => e.AD)
                .IsUnicode(false);

            modelBuilder.Entity<V_Temp_ImportProData>()
                .Property(e => e.AE)
                .IsUnicode(false);

            modelBuilder.Entity<V_Temp_ImportProData>()
                .Property(e => e.AF)
                .IsUnicode(false);

            modelBuilder.Entity<V_Temp_ImportProData>()
                .Property(e => e.AG)
                .IsUnicode(false);

            modelBuilder.Entity<V_Temp_ImportProData>()
                .Property(e => e.AH)
                .IsUnicode(false);

            modelBuilder.Entity<V_Temp_ImportProData>()
                .Property(e => e.AI)
                .IsUnicode(false);

            modelBuilder.Entity<V_Temp_ImportProData>()
                .Property(e => e.AJ)
                .IsUnicode(false);

            modelBuilder.Entity<V_Temp_ImportProData>()
                .Property(e => e.AK)
                .IsUnicode(false);

            modelBuilder.Entity<V_Temp_ImportProData>()
                .Property(e => e.AL)
                .IsUnicode(false);

            modelBuilder.Entity<V_Temp_ImportProData>()
                .Property(e => e.AM)
                .IsUnicode(false);

            modelBuilder.Entity<V_Temp_ImportProData>()
                .Property(e => e.AN)
                .IsUnicode(false);

            modelBuilder.Entity<V_Temp_ImportProData>()
                .Property(e => e.AO)
                .IsUnicode(false);

            modelBuilder.Entity<V_Temp_ImportProData>()
                .Property(e => e.BOAction)
                .IsUnicode(false);

            modelBuilder.Entity<V_Temp_ImportProData>()
                .Property(e => e.ExecStateTxt)
                .IsUnicode(false);
        }
    }
}
