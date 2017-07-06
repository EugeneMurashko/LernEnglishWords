namespace LernEnglishWords.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;

    public partial class Blocks
    {
        [Key]
        public int BlockId { get; set; }

        public int NamberBlock { get; set; } // переименовать в BlockNamber

        [StringLength(128)]
        public string Id { get; set; }

        public int WordId { get; set; }

        public virtual AspNetUsers AspNetUsers { get; set; }

        public virtual EnglishWords EnglishWords { get; set; }

        public virtual Progresses Progresses { get; set; }

        public static int GetNumberBlock(string UserId) // нет проверки на авторизацию!!!!!!!!!!
        {
            var b = Repository.Select<Blocks>()
               .Where(c => c.Id == UserId)
               .Select(c => new
               {
                   NB = c.NamberBlock
               })
               .ToList();

            return b.Count == 0 ? 1 : b.Max(p => p.NB) + 1; 
        }
        public static List<Blocks> GetWordList(string UserId, int namblock, int col)
        {
            List<Blocks> blockList = new List<Blocks>();
            var wordList = Repository.Select<EnglishWords>()
                .ToList();
            var wordidList = new List<int>();
            foreach (var word in wordList)
            {
                wordidList.Add(word.WordId);
            }

            for (int i = 0; i < col; i++)
            {
                blockList.Add(new Blocks
                {
                    NamberBlock = namblock,
                    Id = UserId,
                    WordId = Blocks.GetWord(UserId, namblock, wordidList) // нужна проверка на то, что слова закончились и больше нечего добавлять
                });
                // нужно удалить те слова, которые уже не нужно проверять. // походу костыль уже написан
            }
            return blockList;
        }

        private static int GetWord(string userId, int namblock, List<int> wordidList)
        {
            bool finded = false;
            var blist = Repository.Select<Blocks>()
                .Where(c => c.Id == userId)
                .ToList();
            foreach (var w in wordidList)
            {
                finded = false;
                foreach (Blocks b in blist)
                {
                    if (b.WordId == w)
                    {
                        finded = true;
                        break;
                    }  
                }
                if (!finded)
                {
                    for (int i = 0; i < wordidList.Count; i++) // что же выгоднее? 1) через перебор удалить элемент из списка wordidList. Или утяжелить список blist, добавив туда этот id?
                    {
                        if(wordidList[i] == w)
                        {
                            wordidList.RemoveAt(i);
                            break;
                        }

                    }
                    return w;
                }
            }
            return 0;
        }
    }
}
