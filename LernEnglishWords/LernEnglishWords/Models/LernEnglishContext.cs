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

        public virtual DbSet<EnglishWord> EnglishWord { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EnglishWord>()
                .Property(e => e.Frequency)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<EnglishWord>()
                .Property(e => e.Word)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<EnglishWord>()
                .Property(e => e.Translate)
                .IsUnicode(false);
        }
    }
}
