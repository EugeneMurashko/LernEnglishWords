namespace LernEnglishWords.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class LernEnglishContext : DbContext
    {
        public LernEnglishContext()
            : base("name=LernEnglishContext")
        {
        }
       
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<EnglishWords> EnglishWords { get; set; }
        public virtual DbSet<Progresses_new> Progresses_new { get; set; }
        public virtual DbSet<HistoryOfExercises> HistoryOfExercices { get; set; }
        public virtual DbSet<WordFilter> WordFilter { get; set; }
        public virtual DbSet<PartOfSpeech> PartOfSpeech { get; set; }
        public virtual DbSet<CategoryOfWord> CategoryOfWord { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {


            modelBuilder.Entity<AspNetRoles>()
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

            modelBuilder.Entity<AspNetUsers>()
                .HasMany(c => c.Progresses_new)
                .WithRequired(e => e.AspNetUsers)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<AspNetUsers>()
                .HasMany(e => e.HistoryOfExercises)
                .WithRequired(e => e.User);

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
                .IsUnicode(false);

            modelBuilder.Entity<Progresses_new>()
                .HasKey(p => new { p.UserId, p.WordId });

            modelBuilder.Entity<Progresses_new>()
                .HasRequired(c => c.EnglishWords)
                .WithOptional(c => c.Progresses_new);

            modelBuilder.Entity<WordFilter>()
                .HasMany(e => e.HistoryOfExercises)
                .WithRequired(e => e.WordFilter);

            modelBuilder.Entity<PartOfSpeech>()
                .HasMany(s => s.EnglishWords)
                .WithMany(c => c.PartOfSpeech)
                .Map(cs =>
                {
                    cs.MapLeftKey("PartOfSpeechRefId");
                    cs.MapRightKey("EnWordRefId");
                    cs.ToTable("PartOfSpeechEnWord");
                });

            modelBuilder.Entity<CategoryOfWord>()
                .HasMany(s => s.EnglishWords)
                .WithMany(c => c.CategoryOfWord)
                .Map(cs =>
                {
                    cs.MapLeftKey("CategoryOfWordRefId");
                    cs.MapRightKey("EnWordRefId");
                    cs.ToTable("CategoryOfWordEnWord");
                });

            modelBuilder.Entity<PartOfSpeech>()
                .HasMany(s => s.WordFilter)
                .WithMany(c => c.PartOfSpeech)
                .Map(cs =>
                {
                    cs.MapLeftKey("PartOfSpeechRefId");
                    cs.MapRightKey("FilterRefId");
                    cs.ToTable("PartOfSpeechWFilter");
                });

            modelBuilder.Entity<CategoryOfWord>()
                .HasMany(s => s.WordFilter)
                .WithMany(c => c.CategoryOfWord)
                .Map(cs =>
                {
                    cs.MapLeftKey("CategoryOfWordRefId");
                    cs.MapRightKey("FilterRefId");
                    cs.ToTable("CategoryOfWordWFilter");
                });
        }
    }
}
