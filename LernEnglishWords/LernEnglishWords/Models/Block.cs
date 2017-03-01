/*namespace LernEnglishWords.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table(name:"Blocks")]
    public class Block
    { 

        [Key]
        public int BlockId { get; set; }
        public int NamberBlock { get; set; }

        public string Id { get; set; }
        public AspNetUsers User { get; set; }

        public int WordId { get; set; }
        public EnglishWord EnglishWord { get; set; }

        public Progress Progress { get; set; }
    }
}*/