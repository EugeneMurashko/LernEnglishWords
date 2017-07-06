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
            Blocks = new HashSet<Blocks>();
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Blocks> Blocks { get; set; }

        public virtual Progresses_new Progresses_new { get; set; }
        public virtual ICollection<CategoryOfWord> CategoryOfWord { get; set; }
        public virtual ICollection<PartOfSpeech> PartOfSpeech { get; set; }

        internal static EnglishWords GetNext(string UserId, IEnumerable<EnglishWords> usedWord)
        {
            List<EnglishWords> allWords = Repository.Select<EnglishWords>()
                .Select(c=>new
                {
                    _wordId = c.WordId,
                    _word = c.Word,
                    _translate = c.Translate
                })
                .AsEnumerable()
                .Select(an => new EnglishWords
                {
                    WordId = an._wordId,
                    Word = an._word,
                    Translate = an._translate
                })
                .ToList();
            contextDetuches<EnglishWords>(allWords);
            return GetWord(UserId, allWords, usedWord, 1)[0];
        }

        internal static EnglishWords GetNextNew(string UserId, List<EnglishWords> usedWord)
        {
            List<EnglishWords> allWords = Repository.Select<EnglishWords>()
                .Select(c => new
                {
                    _wordId = c.WordId,
                    _word = c.Word,
                    _translate = c.Translate
                })
                .AsEnumerable()
                .Select(an => new EnglishWords
                {
                    WordId = an._wordId,
                    Word = an._word,
                    Translate = an._translate
                })
                .ToList();
            contextDetuches<EnglishWords>(allWords);
            return GetWordNew(UserId, allWords, usedWord, 1)[0];
        }

        // Ищем слова, которые уже начинали изучаться, но еще не изучены
        internal static List<EnglishWords> GetWordList_TempName(string UserId, List<EnglishWords> usedWord)
        {
            List<EnglishWords> allWords = Repository.Select<EnglishWords>().Select(c => new
            {
                _wordId = c.WordId,
                _word = c.Word,
                _translate = c.Translate
            })
                .AsEnumerable()
                .Select(an => new EnglishWords
                {
                    WordId = an._wordId,
                    Word = an._word,
                    Translate = an._translate
                })
                .ToList();

            return GetWordList_TempName(UserId, allWords, usedWord, 20);
        }
        //  ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
        private static List<EnglishWords> GetWordList_TempName(string userId, List<EnglishWords> allWords, IEnumerable<EnglishWords> usedWord, int col)
        {
            List<EnglishWords> words = new List<EnglishWords>();
            int count = 0;
            bool flagCheck = false;

            var plist = Repository.Select<Progresses_new>()
                .Where(c => c.UserId == userId &&
                c.Repetitions >= 1 && 
                c. SuccessfulSeriesOfRepetitions <= 2)
                .Select(c=> new
                {
                    _UserId = c.UserId,
                    _WordId = c.WordId,
                    _Date = c.Date,
                    _Repetitions = c.Repetitions,
                    _SuccessfulSeriesOfRepetitions = c.SuccessfulSeriesOfRepetitions
                })
                .AsEnumerable()
                .Select(an=> new Progresses_new
                {
                    UserId = an._UserId,
                    WordId = an._WordId,
                    Date = an._Date,
                    Repetitions = an._Repetitions,
                    SuccessfulSeriesOfRepetitions = an._SuccessfulSeriesOfRepetitions
                })
                .ToList();
            contextDetuches<Progresses_new>(plist);
            foreach (var word in allWords)
            {
                foreach (Progresses_new p in plist)
                {
                    if ((p.WordId == word.WordId)) // если слово не существует, то значит никогда не использовалось
                        flagCheck = true;
                }

                foreach (EnglishWords w in usedWord)
                {
                    if (w.WordId == word.WordId)
                        flagCheck = false;
                }

                if (flagCheck)
                {
                    words.Add(word);
                    count++;
                    if (count + 1 > col)
                    {
                        return words;
                    }
                }
                flagCheck = true;
            }
            return words;
        }

        private static List<EnglishWords> GetWord(string userId, List<EnglishWords> allWords, IEnumerable<EnglishWords> usedWord, int col)
        {
            List<EnglishWords> words = new List<EnglishWords>(); 
            int count = 0;
            bool flagCheck = true;

            var plist = Repository.Select<Progresses_new>()
                .Where(c => c.UserId == userId)
                .Select(c => new
                {
                    _UserId = c.UserId,
                    _WordId = c.WordId,
                    _Date = c.Date,
                    _Repetitions = c.Repetitions,
                    _SuccessfulSeriesOfRepetitions = c.SuccessfulSeriesOfRepetitions
                })
                .AsEnumerable()
                .Select(an => new Progresses_new
                {
                    UserId = an._UserId,
                    WordId = an._WordId,
                    Date = an._Date,
                    Repetitions = an._Repetitions,
                    SuccessfulSeriesOfRepetitions = an._SuccessfulSeriesOfRepetitions
                })
                .ToList();
            contextDetuches<Progresses_new>(plist);
            foreach (var word in allWords)
            {
                foreach (Progresses_new p in plist)
                {
                    if (p.WordId == word.WordId)
                    {
                        if (p.SuccessfulSeriesOfRepetitions > 2)
                            flagCheck = false;
                    }
                }

                foreach(EnglishWords w in usedWord)
                {
                    if (w.WordId == word.WordId)
                        flagCheck = false;
                }

                if (flagCheck)
                {
                    words.Add(word);
                    count++;
                    if (count + 1 > col)
                    {
                        return words;
                    }
                }
                flagCheck = true;
            }
            return words;
        }


        private static List<EnglishWords> GetWordNew(string userId, List<EnglishWords> allWords, IEnumerable<EnglishWords> usedWord, int col)
        {
            List<EnglishWords> words = new List<EnglishWords>();
            int count = 0;
            bool flagCheck = true;

            var plist = Repository.Select<Progresses_new>()
                .Where(c => c.UserId == userId)
                .Select(c => new
                {
                    _UserId = c.UserId,
                    _WordId = c.WordId,
                    _Date = c.Date,
                    _Repetitions = c.Repetitions,
                    _SuccessfulSeriesOfRepetitions = c.SuccessfulSeriesOfRepetitions
                })
                .AsEnumerable()
                .Select(an => new Progresses_new
                {
                    UserId = an._UserId,
                    WordId = an._WordId,
                    Date = an._Date,
                    Repetitions = an._Repetitions,
                    SuccessfulSeriesOfRepetitions = an._SuccessfulSeriesOfRepetitions
                })
                .ToList();
            contextDetuches<Progresses_new>(plist);
            foreach (var word in allWords)
            {
                foreach (Progresses_new p in plist)
                {
                    if (p.WordId == word.WordId)
                    {
                        if (p.Repetitions > 0)
                            flagCheck = false;
                    }
                }

                foreach (EnglishWords w in usedWord)
                {
                    if (w.WordId == word.WordId)
                        flagCheck = false;
                }

                if (flagCheck)
                {
                    words.Add(word);
                    count++;
                    if (count + 1 > col)
                    {
                        return words;
                    }
                }
                flagCheck = true;
            }
            return words;
        }

        private static void contextDetuch<TEntity>(TEntity entity) // Это временная мера :(
            where TEntity : class
        {
            using (LernEnglishContext context = new LernEnglishContext())
            {
                context.Entry(entity).State = System.Data.Entity.EntityState.Detached;
                context.SaveChanges();
            }
        }

        private static void contextDetuches<TEntity>(IEnumerable<TEntity> entities) // Это временная мера :(
            where TEntity : class
        {
            LernEnglishContext context = new LernEnglishContext();

            context.Configuration.AutoDetectChangesEnabled = false;
            context.Configuration.ValidateOnSaveEnabled = false;

            context.Database.Log = (s => System.Diagnostics.Debug.WriteLine(s));

            foreach (var entity in entities)
            {
                context.Entry(entity).State = System.Data.Entity.EntityState.Detached;
            }
            context.SaveChanges();

            context.Configuration.AutoDetectChangesEnabled = true;
            context.Configuration.ValidateOnSaveEnabled = true;

        }
    }
}
