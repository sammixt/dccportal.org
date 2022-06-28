using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace dccportal.org.Entities
{
    public partial class dccportaldbContext : DbContext
    {
        public dccportaldbContext()
        {
        }

        public dccportaldbContext(DbContextOptions<dccportaldbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AdminLoginDetail> AdminLoginDetails { get; set; }
        public virtual DbSet<AdminUser> AdminUsers { get; set; }
        public virtual DbSet<Attendance> Attendances { get; set; }
        public virtual DbSet<BankDetail> BankDetails { get; set; } 
        public virtual DbSet<Believer> Believers { get; set; }
        public virtual DbSet<Department> Departments { get; set; } 
        public virtual DbSet<Due> Dues { get; set; }
        public virtual DbSet<Mac> Macs { get; set; }
        public virtual DbSet<Member> Members { get; set; } 
        public virtual DbSet<Month> Months { get; set; } 
        public virtual DbSet<Payment> Payments { get; set; } 
        public virtual DbSet<Post> Posts { get; set; } 
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<State> States { get; set; } 
        public virtual DbSet<Unit> Units { get; set; }
        public virtual DbSet<UserInfo> UserInfos { get; set; } 
        public virtual DbSet<UserLogin> UserLogins { get; set; } 
        public virtual DbSet<UsersAccount> UsersAccounts { get; set; } 
        public virtual DbSet<Wallet> Wallets { get; set; }
        public virtual DbSet<WorkerAttendance> WorkerAttendances { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    //             if (!optionsBuilder.IsConfigured)
        //    //             {
        //    // #warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        //    //                 optionsBuilder.UseSqlServer("Server=localhost; Database\n=dccportaldb;Persist Security Info=False;User ID=sa;Password=Sammy1234;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;\n");
        //    //             }
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AdminLoginDetail>(entity =>
            {
                entity.Property(e => e.Email).HasMaxLength(250);

                entity.Property(e => e.Password).HasMaxLength(450);

                entity.Property(e => e.PasswordHash).HasMaxLength(450);

                entity.Property(e => e.UserId).HasMaxLength(450);

                entity.Property(e => e.UserName).HasMaxLength(50);
            });

            modelBuilder.Entity<AdminUser>(entity =>
            {
                entity.Property(e => e.CreatedBy).HasMaxLength(250);

                entity.Property(e => e.Email).HasMaxLength(250);

                entity.Property(e => e.Fullname).HasMaxLength(150);

                entity.Property(e => e.Phone).HasMaxLength(20);

                entity.Property(e => e.Username).HasMaxLength(700);
            });

            modelBuilder.Entity<Attendance>(entity =>
            {
                entity.ToTable("Attendance");

                entity.Property(e => e.DateOfDate).HasColumnType("date");

                entity.Property(e => e.DeptGroup)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.HasOne(d => d.Worker)
                    .WithMany(p => p.Attendances)
                    .HasForeignKey(d => d.WorkerId)
                    .HasConstraintName("FK_Attendance_BelieverLink");
            });

            modelBuilder.Entity<BankDetail>(entity =>
            {
                entity.Property(e => e.AccountNumber).HasMaxLength(20);

                entity.Property(e => e.BankName).HasMaxLength(250);

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.Surname).HasMaxLength(50);

                entity.Property(e => e.UserId).HasMaxLength(450);
            });

            modelBuilder.Entity<Believer>(entity =>
            {
                entity.HasKey(e => e.MemberId);

                entity.Property(e => e.AltPhoneNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.City)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Country)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.DateOfBirth).HasColumnType("date");

                entity.Property(e => e.Email)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.FacebookName)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.HomeAddressOne)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.HomeAddressTwo)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.InstagramHandle)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.MandBid).HasColumnName("MandBId");

                entity.Property(e => e.MaritalStatus)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Sex)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.StateName)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.TwitterHandle)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.WeddingAnniversary).HasColumnType("date");
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasKey(e => e.DeptId);

                entity.ToTable("Department");

                entity.Property(e => e.DeptDesc)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.DeptName)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.ShortCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Vision).IsUnicode(false);
            });

            modelBuilder.Entity<Due>(entity =>
            {
                entity.HasKey(e => e.DuesId);

                entity.ToTable("Due");

                entity.Property(e => e.Amount).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.CreationDate).HasColumnType("date");

                entity.Property(e => e.DuesDesc)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.DuesName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.DuesType)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Mac>(entity =>
            {
                entity.ToTable("mac");

                entity.Property(e => e.MacId).HasColumnName("mac_id");

                entity.Property(e => e.Joined)
                    .HasColumnType("date")
                    .HasColumnName("joined");

                entity.Property(e => e.MacNo)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("mac_no");

                entity.Property(e => e.User)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("user");
            });

            modelBuilder.Entity<Member>(entity =>
            {
                entity.Property(e => e.Groups)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Post)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ProbationStatus)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Believer)
                    .WithMany(p => p.Members)
                    .HasForeignKey(d => d.BelieverId).OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Members_Believers");

                entity.HasOne(d => d.Dept)
                    .WithMany(p => p.Members)
                    .HasForeignKey(d => d.DeptId)
                    .HasConstraintName("FK_Members_Dept");

                entity.HasOne(d => d.PostNavigation)
                    .WithMany(p => p.Members)
                    .HasForeignKey(d => d.Post)
                    .HasConstraintName("FK_Members_Post");

                entity.HasOne(d => d.Unit)
                    .WithMany(p => p.Members)
                    .HasForeignKey(d => d.UnitId)
                    .HasConstraintName("FK_Members_Units");
            });

            modelBuilder.Entity<Month>(entity =>
            {
                entity.Property(e => e.Month1)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Month");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.Property(e => e.Amount).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.AuthorizedBy)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.EntryDate).HasColumnType("date");

                entity.Property(e => e.ExpenseBy)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Narration)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.PaymentDate).HasColumnType("date");

                entity.Property(e => e.PaymentSatus)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TransactionType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Year)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.HasOne(d => d.Dues)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.DuesId)
                    .HasConstraintName("FK_Payments_Due");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.MemberId)
                    .HasConstraintName("FK_Payments_Believers");

                entity.HasOne(d => d.MonthNavigation)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.Month)
                    .HasConstraintName("FK_Payments_Months");
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.Property(e => e.PostId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PostName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.RoleId).ValueGeneratedNever();

                entity.Property(e => e.RoleName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Role_Name");
            });

            modelBuilder.Entity<State>(entity =>
            {
                entity.ToTable("State");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Unit>(entity =>
            {
                entity.ToTable("Unit");

                entity.Property(e => e.DeptId).HasColumnName("DeptID");

                entity.Property(e => e.UnitFunction)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UnitName)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.UnitShortCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Dept)
                    .WithMany(p => p.Units)
                    .HasForeignKey(d => d.DeptId)
                    .HasConstraintName("FK_Unit_Unit");
            });

            modelBuilder.Entity<UserInfo>(entity =>
            {
                entity.Property(e => e.AddressOne).HasMaxLength(250);

                entity.Property(e => e.AddressThree).HasMaxLength(250);

                entity.Property(e => e.AddressTwo).HasMaxLength(250);

                entity.Property(e => e.ContactNumber).HasMaxLength(30);

                entity.Property(e => e.Country).HasMaxLength(150);

                entity.Property(e => e.CountryCode).HasMaxLength(10);

                entity.Property(e => e.DateCreated).HasDefaultValueSql("('0001-01-01T00:00:00.0000000')");

                entity.Property(e => e.DayOfBirth).HasMaxLength(2);

                entity.Property(e => e.EmailAddress).HasMaxLength(150);

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.MonthOfBirth).HasMaxLength(2);

                entity.Property(e => e.PostCode).HasMaxLength(20);

                entity.Property(e => e.State).HasMaxLength(100);

                entity.Property(e => e.Surname).HasMaxLength(50);

                entity.Property(e => e.Title).HasMaxLength(10);

                entity.Property(e => e.Town).HasMaxLength(100);

                entity.Property(e => e.YearOfBirth).HasMaxLength(4);
            });

            modelBuilder.Entity<UserLogin>(entity =>
            {
                entity.Property(e => e.Email).HasMaxLength(250);

                entity.Property(e => e.EmailConfirmLink).HasMaxLength(500);

                entity.Property(e => e.Password).HasMaxLength(450);

                entity.Property(e => e.PasswordHash).HasMaxLength(450);

                entity.Property(e => e.UserId).HasMaxLength(450);

                entity.Property(e => e.UserName).HasMaxLength(50);
            });

            modelBuilder.Entity<UsersAccount>(entity =>
            {
                entity.ToTable("UsersAccount");

                entity.Property(e => e.CreationDate).HasColumnType("date");

                entity.Property(e => e.Password)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UserName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Believer)
                    .WithMany(p => p.UsersAccounts)
                    .HasForeignKey(d => d.BelieverId)
                    .HasConstraintName("FK_UsersAccount_BelieversLink");

                entity.HasOne(d => d.Dept)
                    .WithMany(p => p.UsersAccounts)
                    .HasForeignKey(d => d.DeptId)
                    .HasConstraintName("FK_UsersAccount_DeptLink");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.UsersAccounts)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_UsersAccount_RoleLink");
            });

            modelBuilder.Entity<Wallet>(entity =>
            {
                entity.ToTable("Wallet");

                entity.Property(e => e.Amount).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.TransactionDate).HasColumnType("date");

                entity.Property(e => e.TransactionType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.Wallets)
                    .HasForeignKey(d => d.MemberId)
                    .HasConstraintName("FK_Wallet_Believers");
            });

            modelBuilder.Entity<WorkerAttendance>(entity =>
            {
                entity.ToTable("WorkerAttendance");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.DepartmentGroup)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                // entity.HasOne(d => d.Worker)
                //     .WithMany(p => p.Attendances)
                //     .HasForeignKey(d => d.WorkerId)
                //     .HasConstraintName("FK_Attendance_BelieverLink");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
