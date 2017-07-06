namespace LernEnglishWords.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    public class CategoryOfWord
    {
        public CategoryOfWord()
        {
            EnglishWords = new HashSet<EnglishWords>();
            WordFilter = new HashSet<WordFilter>();
        }
        public virtual int Id { get; set; }
        [Required]
        public virtual string Name { get; set; }
        public virtual ICollection<EnglishWords> EnglishWords { get; set; }
        public virtual ICollection<WordFilter> WordFilter { get; set; }
    }
}