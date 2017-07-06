namespace LernEnglishWords.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    public class HistoryOfExercises
    {
        public virtual int Id { get; set; }
        public virtual DateTimeOffset Date { get; set; }
        public virtual WordFilter WordFilter { get; set; }
        public virtual AspNetUsers User { get; set; }
    }
}