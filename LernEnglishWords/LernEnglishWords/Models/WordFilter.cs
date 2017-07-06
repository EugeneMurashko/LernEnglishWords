namespace LernEnglishWords.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    public class WordFilter
    {
        public WordFilter()
        {
            PartOfSpeech = new HashSet<PartOfSpeech>();
            CategoryOfWord = new HashSet<CategoryOfWord>();
            AspNetUsers = new HashSet<AspNetUsers>();
            HistoryOfExercises = new HashSet<HistoryOfExercises>();
        }
        public virtual int Id { get; set; }
        public virtual ICollection<PartOfSpeech> PartOfSpeech { get; set; }
        public virtual ICollection<CategoryOfWord> CategoryOfWord { get; set; }
        public virtual ICollection<AspNetUsers> AspNetUsers { get; set; }
        public virtual ICollection<HistoryOfExercises> HistoryOfExercises { get; set; }
    }
}