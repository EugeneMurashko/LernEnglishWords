namespace LernEnglishWords.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Progresses_new
    {
        public int WordId { get; set; }

        [StringLength(128)]
        public string UserId { get; set; }

        public int Repetitions { get; set; }

        public int SuccessfulSeriesOfRepetitions { get; set; }

        public DateTimeOffset Date { get; set; }

        public virtual AspNetUsers AspNetUsers { get; set; }
        public virtual EnglishWords EnglishWords { get; set; }
    }
}
