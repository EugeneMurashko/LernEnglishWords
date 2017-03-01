namespace LernEnglishWords.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Testing : DbContext
    {
        public Testing()
            : base("name=Testing")
        {
        }

       /* public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<EnglishWords> EnglishWords { get; set; }
        public virtual DbSet<Blocks> Blocks { get; set; }
        public virtual DbSet<Progresses> Progresses { get; set; }*/

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            /*modelBuilder.Entity<AspNetRoles>()
                .HasMany(e => e.AspNetUsers)
                .WithMany(e => e.AspNetRoles)
                .Map(m => m.ToTable("AspNetUserRoles").MapLeftKey("RoleId").MapRightKey("UserId"));

            modelBuilder.Entity<AspNetUsers>()
                .HasMany(e => e.AspNetUserClaims)
                .WithRequired(e => e.AspNetUsers)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<AspNetUsers>()
                .HasMany(e => e.AspNetUserLogins)
                .WithRequired(e => e.AspNetUsers)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<EnglishWords>()
                .Property(e => e.Frequency)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<EnglishWords>()
                .Property(e => e.Word)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<EnglishWords>()
                .Property(e => e.Translate)
                .IsUnicode(false);*/
        }
    }
}
