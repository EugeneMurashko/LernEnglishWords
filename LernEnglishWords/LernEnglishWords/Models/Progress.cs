/*namespace LernEnglishWords.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public class Progress
    {
        [Key]
        [ForeignKey("Block")]
        public int BlockId { get; set; }
        public bool Done { get; set; }
        public int Repetitions { get; set; }
        public int SuccessfulSeriesOfRepetitions { get; set; }
        public DateTimeOffset Date { get; set; }

        public Block Block { get; set; }
    }
}*/