/*namespace LernEnglishWords.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EnglishWords")]
    public partial class EnglishWord
    {
        public EnglishWord()
        {
            Blocks = new List<Block>();
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

        public virtual ICollection<Block> Blocks { get; set; }
    }
}*/
