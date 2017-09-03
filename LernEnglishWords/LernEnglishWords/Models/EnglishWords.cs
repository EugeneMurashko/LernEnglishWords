namespace LernEnglishWords.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;

    public partial class EnglishWords
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EnglishWords()
        {
            CategoryOfWord = new HashSet<CategoryOfWord>();
            PartOfSpeech = new HashSet<PartOfSpeech>();
        }

        [Key]
        public int WordId { get; set; }

        [StringLength(8)]
        public string Frequency { get; set; }

        [Required]
        [StringLength(64)]
        public string Word { get; set; }

        [Column(TypeName = "text")]
        public string Translate { get; set; }

        public virtual Progresses_new Progresses_new { get; set; }
        public virtual ICollection<CategoryOfWord> CategoryOfWord { get; set; }
        public virtual ICollection<PartOfSpeech> PartOfSpeech { get; set; }
    }
}
