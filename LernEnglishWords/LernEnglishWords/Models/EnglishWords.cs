namespace LernEnglishWords.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class EnglishWords
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EnglishWords()
        {
            Blocks = new HashSet<Blocks>();
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Blocks> Blocks { get; set; }
    }
}
