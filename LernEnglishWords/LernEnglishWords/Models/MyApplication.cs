using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LernEnglishWords.Models
{
    public class MyApplication
    {
        private static MyApplication app;

        public EnglishWords CurrentWord{
            get {
                return GetCurrentWord();
            }
            set {
                SetCurrentWord(value);
            }
        }

        public bool wasTheFirstCycle{
            get{
                return (WordListCycle0.Count != 0)? true : false;
            }
        }
        public int Index { get; set; }
        public int Cycle { get; set; }
        public int WordFilter { get; set; }
        public string UserId { get; set; }
        public int Complexity { get; set; }
        public int Success { get; set; }

        public List<EnglishWords> WordListCycle0 { get; set; }
        public List<EnglishWords> WordListCycle1 { get; set; }
        public List<EnglishWords> WordListCycle2 { get; set; }
        public List<EnglishWords> WordListCycle3 { get; set; }
        public List<EnglishWords> WordListCycle4 { get; set; }

        public List<List<EnglishWords>> ListOfWordList { get; set; }

        private MyApplication(string UserId)
        {
            WordListCycle0 = new List<EnglishWords>();
            WordListCycle1 = new List<EnglishWords>();
            WordListCycle2 = new List<EnglishWords>();
            WordListCycle3 = new List<EnglishWords>();
            WordListCycle4 = new List<EnglishWords>();
            ListOfWordList = new List<List<EnglishWords>>();
            ListOfWordList.AddRange( new List<List<EnglishWords>> { WordListCycle0, WordListCycle1, WordListCycle2, WordListCycle3, WordListCycle4 });

            Index = -100; // значение 0 устанавливается когда было добавлено первое слово
            Cycle = 0;
            WordFilter = 0; // Нулевого не существует
            this.UserId = UserId;
            Complexity = 20; // по умолчанию 20
            Success = 3; // по умолчанию 3 - столько успешных раз в серии должно быть результатов
        }
        public static MyApplication GetReference(string UserId)
        {
            if (app == null)
                return app = new MyApplication(UserId);
            else
                return app;
        }
        private EnglishWords GetCurrentWord()
        {
            return ListOfWordList[Cycle][Index];
        }
        private void SetCurrentWord(EnglishWords value)
        {
            ListOfWordList[Cycle][Index] = value;
        }
        public static void Delete()
        {
            app = null;
        }
    }
}