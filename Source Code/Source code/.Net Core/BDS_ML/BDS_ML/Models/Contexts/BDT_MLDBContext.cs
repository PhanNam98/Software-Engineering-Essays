using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BDS_ML.Models.ModelDB
{
    public partial class BDT_MLDBContext : DbContext
    {
        public BDT_MLDBContext()
        {
        }

        public BDT_MLDBContext(DbContextOptions<BDT_MLDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Admin> Admin { get; set; }
        public virtual DbSet<AspNetRoleClaims> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<Block> Block { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<Post> Post { get; set; }
        public virtual DbSet<Post_Comment> Post_Comment { get; set; }
        public virtual DbSet<Post_Detail> Post_Detail { get; set; }
        public virtual DbSet<Post_Favorite> Post_Favorite { get; set; }
        public virtual DbSet<Post_Image> Post_Image { get; set; }
        public virtual DbSet<Post_Location> Post_Location { get; set; }
        public virtual DbSet<Post_Status> Post_Status { get; set; }
        public virtual DbSet<Post_Type> Post_Type { get; set; }
        public virtual DbSet<RealEstate_Type> RealEstate_Type { get; set; }
        public virtual DbSet<Status> Status { get; set; }
        public virtual DbSet<Vip_Status> Vip_Status { get; set; }
        public virtual DbSet<district> district { get; set; }
        public virtual DbSet<project> project { get; set; }
        public virtual DbSet<province> province { get; set; }
        public virtual DbSet<street> street { get; set; }
        public virtual DbSet<ward> ward { get; set; }

        // Unable to generate entity type for table 'dbo.AspNetUserRoles'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.AspNetUserTokens'. Please see the warning messages.

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=(local);Database=BDT_MLDB;Trusted_Connection=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<Admin>(entity =>
            {
                entity.Property(e => e.Avatar_URL).IsUnicode(false);

                entity.Property(e => e.Email).IsUnicode(false);

                entity.Property(e => e.IDNumber).IsUnicode(false);

                entity.Property(e => e.PhoneNumber).IsUnicode(false);

                entity.HasOne(d => d.Account_)
                    .WithMany(p => p.Admin)
                    .HasForeignKey(d => d.Account_ID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Admin_AspNetUsers");
            });

            modelBuilder.Entity<AspNetRoleClaims>(entity =>
            {
                entity.HasIndex(e => e.RoleId);
            });

            modelBuilder.Entity<AspNetRoles>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName)
                    .HasName("RoleNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedName] IS NOT NULL)");

                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<AspNetUserClaims>(entity =>
            {
                entity.HasIndex(e => e.UserId);
            });

            modelBuilder.Entity<AspNetUserLogins>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.HasIndex(e => e.UserId);
            });

            modelBuilder.Entity<AspNetUsers>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail)
                    .HasName("EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName)
                    .HasName("UserNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedUserName] IS NOT NULL)");

                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<Block>(entity =>
            {
                entity.HasKey(e => new { e.ID_Block, e.ID_User });

                entity.Property(e => e.ID_Block).ValueGeneratedOnAdd();

                entity.HasOne(d => d.ID_AdminNavigation)
                    .WithMany(p => p.Block)
                    .HasForeignKey(d => d.ID_Admin)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Block_Admin");

                entity.HasOne(d => d.ID_UserNavigation)
                    .WithMany(p => p.Block)
                    .HasForeignKey(d => d.ID_User)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Block_Customer");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.Property(e => e.Avatar_URL).IsUnicode(false);

                entity.Property(e => e.Email).IsUnicode(false);

                entity.Property(e => e.PhoneNumber).IsUnicode(false);

                entity.HasOne(d => d.Account_)
                    .WithMany(p => p.Customer)
                    .HasForeignKey(d => d.Account_ID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Customer_AspNetUsers");
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.HasOne(d => d.ID_AccountNavigation)
                    .WithMany(p => p.Post)
                    .HasForeignKey(d => d.ID_Account)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Post_AspNetUsers1");

                entity.HasOne(d => d.PostTypeNavigation)
                    .WithMany(p => p.Post)
                    .HasForeignKey(d => d.PostType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Post_Post_Type");

                entity.HasOne(d => d.ProjectNavigation)
                    .WithMany(p => p.Post)
                    .HasForeignKey(d => d.Project)
                    .HasConstraintName("FK_Post_project");

                entity.HasOne(d => d.RealEstateTypeNavigation)
                    .WithMany(p => p.Post)
                    .HasForeignKey(d => d.RealEstateType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Post_RealEstate_Type");
            });

            modelBuilder.Entity<Post_Comment>(entity =>
            {
                entity.HasKey(e => e.Comment_ID)
                    .HasName("PK_Post_Comment_1");

                entity.HasOne(d => d.ID_PostNavigation)
                    .WithMany(p => p.Post_Comment)
                    .HasForeignKey(d => d.ID_Post)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Post_Comment_Post");

                entity.HasOne(d => d.ID_UserNavigation)
                    .WithMany(p => p.Post_Comment)
                    .HasForeignKey(d => d.ID_User)
                    .HasConstraintName("FK_Post_Comment_AspNetUsers");
            });

            modelBuilder.Entity<Post_Detail>(entity =>
            {
                entity.HasKey(e => e.ID_Detail)
                    .HasName("PK_Detail_Post_1");

                entity.HasOne(d => d.ID_PostNavigation)
                    .WithMany(p => p.Post_Detail)
                    .HasForeignKey(d => d.ID_Post)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Detail_Post_Post");
            });

            modelBuilder.Entity<Post_Favorite>(entity =>
            {
                entity.HasKey(e => new { e.ID_User, e.ID_Post })
                    .HasName("PK_Favorite_Post");

                entity.HasOne(d => d.ID_PostNavigation)
                    .WithMany(p => p.Post_Favorite)
                    .HasForeignKey(d => d.ID_Post)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Favorite_Post_Post");

                entity.HasOne(d => d.ID_UserNavigation)
                    .WithMany(p => p.Post_Favorite)
                    .HasForeignKey(d => d.ID_User)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Favorite_Post_AspNetUsers");
            });

            modelBuilder.Entity<Post_Image>(entity =>
            {
                entity.HasOne(d => d.ID_PostNavigation)
                    .WithMany(p => p.Post_Image)
                    .HasForeignKey(d => d.ID_Post)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Post_Image_Post");
            });

            modelBuilder.Entity<Post_Location>(entity =>
            {
                entity.HasKey(e => e.ID_Location)
                    .HasName("PK_Location");

                entity.HasOne(d => d.DuAnNavigation)
                    .WithMany(p => p.Post_Location)
                    .HasForeignKey(d => d.DuAn)
                    .HasConstraintName("FK_Location_project");

                entity.HasOne(d => d.Duong_PhoNavigation)
                    .WithMany(p => p.Post_Location)
                    .HasForeignKey(d => d.Duong_Pho)
                    .HasConstraintName("FK_Location_street");

                entity.HasOne(d => d.ID_PostNavigation)
                    .WithMany(p => p.Post_Location)
                    .HasForeignKey(d => d.ID_Post)
                    .HasConstraintName("FK_Location_Post");

                entity.HasOne(d => d.Phuong_XaNavigation)
                    .WithMany(p => p.Post_Location)
                    .HasForeignKey(d => d.Phuong_Xa)
                    .HasConstraintName("FK_Location_ward");

                entity.HasOne(d => d.Quan_HuyenNavigation)
                    .WithMany(p => p.Post_Location)
                    .HasForeignKey(d => d.Quan_Huyen)
                    .HasConstraintName("FK_Location_district");

                entity.HasOne(d => d.Tinh_TPNavigation)
                    .WithMany(p => p.Post_Location)
                    .HasForeignKey(d => d.Tinh_TP)
                    .HasConstraintName("FK_Location_province");
            });

            modelBuilder.Entity<Post_Status>(entity =>
            {
                entity.HasOne(d => d.ID_AccountNavigation)
                    .WithMany(p => p.Post_Status)
                    .HasForeignKey(d => d.ID_Account)
                    .HasConstraintName("FK_Post_Status_AspNetUsers");

                entity.HasOne(d => d.ID_PostNavigation)
                    .WithMany(p => p.Post_Status)
                    .HasForeignKey(d => d.ID_Post)
                    .HasConstraintName("FK_Post_Status_Post");

                entity.HasOne(d => d.StatusNavigation)
                    .WithMany(p => p.Post_Status)
                    .HasForeignKey(d => d.Status)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Post_Status_Status");
            });

            modelBuilder.Entity<Status>(entity =>
            {
                entity.Property(e => e.ID_Status).ValueGeneratedNever();
            });

            modelBuilder.Entity<Vip_Status>(entity =>
            {
                entity.HasKey(e => new { e.ID_Vip, e.ID_User });

                entity.Property(e => e.ID_Vip).ValueGeneratedOnAdd();

                entity.HasOne(d => d.ID_UserNavigation)
                    .WithMany(p => p.Vip_Status)
                    .HasForeignKey(d => d.ID_User)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Vip_Status_AspNetUsers");
            });

            modelBuilder.Entity<district>(entity =>
            {
                entity.Property(e => e.id).ValueGeneratedNever();

                entity.HasOne(d => d._province_)
                    .WithMany(p => p.district)
                    .HasForeignKey(d => d._province_id)
                    .HasConstraintName("FK_district_province");
            });

            modelBuilder.Entity<project>(entity =>
            {
                entity.Property(e => e.id).ValueGeneratedNever();

                entity.HasOne(d => d._district_)
                    .WithMany(p => p.project)
                    .HasForeignKey(d => d._district_id)
                    .HasConstraintName("FK_project_district");

                entity.HasOne(d => d._province_)
                    .WithMany(p => p.project)
                    .HasForeignKey(d => d._province_id)
                    .HasConstraintName("FK_project_province");
            });

            modelBuilder.Entity<province>(entity =>
            {
                entity.Property(e => e.id).ValueGeneratedNever();
            });

            modelBuilder.Entity<street>(entity =>
            {
                entity.Property(e => e.id).ValueGeneratedNever();

                entity.HasOne(d => d._district_)
                    .WithMany(p => p.street)
                    .HasForeignKey(d => d._district_id)
                    .HasConstraintName("FK_street_district");

                entity.HasOne(d => d._province_)
                    .WithMany(p => p.street)
                    .HasForeignKey(d => d._province_id)
                    .HasConstraintName("FK_street_province");
            });

            modelBuilder.Entity<ward>(entity =>
            {
                entity.Property(e => e.id).ValueGeneratedNever();

                entity.HasOne(d => d._district_)
                    .WithMany(p => p.ward)
                    .HasForeignKey(d => d._district_id)
                    .HasConstraintName("FK_ward_district");

                entity.HasOne(d => d._province_)
                    .WithMany(p => p.ward)
                    .HasForeignKey(d => d._province_id)
                    .HasConstraintName("FK_ward_province");
            });
        }
    }
}
