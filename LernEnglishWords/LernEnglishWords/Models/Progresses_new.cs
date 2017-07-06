namespace LernEnglishWords.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Progresses_new: ICloneable
    {
        //[Key, Column(Order = 1)]
        //[ForeignKey("EnglishWords")]
        public int WordId { get; set; }

        [StringLength(128)]
        //[Key, Column(Order = 2)]
        //[ForeignKey("AspNetUsers")]
        public string UserId { get; set; }

        public int Repetitions { get; set; }

        public int SuccessfulSeriesOfRepetitions { get; set; }

        public DateTimeOffset Date { get; set; }

        public virtual AspNetUsers AspNetUsers { get; set; }
        public virtual EnglishWords EnglishWords { get; set; }

        public Object Clone()
        {
            return new Progresses_new
            {
                WordId = this.WordId,
                UserId = this.UserId,
                Repetitions = this.Repetitions,
                SuccessfulSeriesOfRepetitions = this.SuccessfulSeriesOfRepetitions,
                Date = this.Date,
                AspNetUsers = this.AspNetUsers,
                EnglishWords = this.EnglishWords
            };
        }
    }
}
