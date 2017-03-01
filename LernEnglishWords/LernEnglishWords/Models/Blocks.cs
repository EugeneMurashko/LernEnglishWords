namespace LernEnglishWords.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Blocks
    {
        [Key]
        public int BlockId { get; set; }

        public int NamberBlock { get; set; }

        [StringLength(128)]
        public string Id { get; set; }

        public int WordId { get; set; }

        public virtual AspNetUsers AspNetUsers { get; set; }

        public virtual EnglishWords EnglishWords { get; set; }

        public virtual Progresses Progresses { get; set; }
    }
}
