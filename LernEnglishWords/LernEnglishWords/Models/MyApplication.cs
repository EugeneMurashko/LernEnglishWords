using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

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

        public string DemoSet(int result)
        {
            var progress = Repository.Select<Progresses_new>()
                .Where(c => c.WordId == this.CurrentWord.WordId && c.UserId == this.UserId)
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
                .ToList()
                .FirstOrDefault();

            // Первый круг упражнения
            if (this.Cycle == 0 || (!this.wasTheFirstCycle && this.Cycle == 1))
            {
                // Был данн положительный ответ
                if (result == 1)
                {
                    // Прогресс еще не сохранялся
                    if (progress == null)
                    {
                        using (LernEnglishContext context = new LernEnglishContext())
                        {
                            EnglishWords TempWord = context.Set<EnglishWords>()
                                .Where(c => c.WordId == this.CurrentWord.WordId)
                                .FirstOrDefault();

                            this.CurrentWord = TempWord;

                            Progresses_new Temp = new Progresses_new
                            {
                                WordId = this.CurrentWord.WordId,
                                UserId = this.UserId,
                                Date = DateTime.UtcNow,
                                Repetitions = 1,
                                SuccessfulSeriesOfRepetitions = 100,
                                EnglishWords = this.CurrentWord
                            };

                            context.Entry(Temp).State = EntityState.Added;
                            context.SaveChanges();
                        }

                        //
                        // Этому тут не место
                        //
                        this.WordListCycle0.RemoveAt(this.Index);
                        this.Index--;
                        //
                        //
                        //

                        return "skipWord";
                    }
                    // Строка была создана ранее, но без прогресса
                    else if (progress.Repetitions == 0)
                    {
                        using (LernEnglishContext context = new LernEnglishContext())
                        {
                            progress.Date = DateTime.UtcNow;
                            progress.Repetitions = 1;
                            progress.SuccessfulSeriesOfRepetitions = 100;

                            context.Entry(progress).State = EntityState.Modified;
                            context.SaveChanges();
                        }

                        //
                        // Этому тут не место
                        //
                        this.WordListCycle0.RemoveAt(this.Index);
                        this.Index--;
                        //
                        //
                        //

                        return "skipWord";
                    }
                    // Слово уже изучалось ранее
                    else if (progress.Repetitions >= 1)
                    {
                        using (LernEnglishContext context = new LernEnglishContext())
                        {
                            progress.Date = DateTime.UtcNow;
                            progress.Repetitions = progress.Repetitions + 1;
                            progress.SuccessfulSeriesOfRepetitions = progress.SuccessfulSeriesOfRepetitions + 1;

                            context.Entry(progress).State = EntityState.Modified;
                            context.SaveChanges();
                        }
                    }
                    else // отлавливаем остальные нетипичные варианты
                    {
                        return "Error";
                    }
                }
                // Был данн неверный ответ
                else if (result <= 0)
                {
                    // Прогресс еще не сохранялся
                    if (progress == null)
                    {
                        using (LernEnglishContext context = new LernEnglishContext())
                        {
                            EnglishWords TempWord = context.Set<EnglishWords>()
                                .Where(c => c.WordId == this.CurrentWord.WordId)
                                .FirstOrDefault();

                            this.CurrentWord = TempWord;

                            Progresses_new Temp = new Progresses_new
                            {
                                WordId = this.CurrentWord.WordId,
                                UserId = this.UserId,
                                Date = DateTime.UtcNow,
                                Repetitions = 1,
                                SuccessfulSeriesOfRepetitions = 0,
                                EnglishWords = this.CurrentWord
                            };

                            context.Entry(Temp).State = EntityState.Added;
                            context.SaveChanges();
                        }

                        this.ListOfWordList[this.Cycle + 1].Add(this.CurrentWord);
                    }
                    else if (progress.Repetitions >= 0)
                    {
                        using (LernEnglishContext context = new LernEnglishContext())
                        {
                            progress.Date = DateTime.UtcNow;
                            progress.Repetitions = progress.Repetitions + 1;
                            progress.SuccessfulSeriesOfRepetitions = 0;

                            context.Entry(progress).State = EntityState.Modified;
                            context.SaveChanges();
                        }

                        this.ListOfWordList[this.Cycle + 1].Add(this.CurrentWord);
                    }
                    else
                    {
                        return "Error";
                    }
                }
            }
            // Это слова второго круга которых не было на первом круге
            else if (this.Cycle == 1 && result == 1 && this.WordListCycle0.Where(x => x == this.CurrentWord).FirstOrDefault() == null)
            {
                if (progress == null)
                {
                    using (LernEnglishContext context = new LernEnglishContext())
                    {
                        EnglishWords TempWord = context.Set<EnglishWords>()
                            .Where(c => c.WordId == this.CurrentWord.WordId)
                            .FirstOrDefault();

                        this.CurrentWord = TempWord;

                        Progresses_new Temp = new Progresses_new
                        {
                            WordId = this.CurrentWord.WordId,
                            UserId = this.UserId,
                            Date = DateTime.UtcNow,
                            Repetitions = 1,
                            SuccessfulSeriesOfRepetitions = 100,
                            EnglishWords = this.CurrentWord
                        };

                        context.Entry(Temp).State = EntityState.Added;
                        context.SaveChanges();
                    }
                }
                else if (progress.Repetitions == 0)
                {
                    using (LernEnglishContext context = new LernEnglishContext())
                    {
                        progress.Date = DateTime.UtcNow;
                        progress.Repetitions = 1;
                        progress.SuccessfulSeriesOfRepetitions = 100;

                        context.Entry(progress).State = EntityState.Modified;
                        context.SaveChanges();
                    }
                }
                else
                {
                    using (LernEnglishContext context = new LernEnglishContext())
                    {
                        progress.Date = DateTime.UtcNow;
                        progress.Repetitions++;
                        progress.SuccessfulSeriesOfRepetitions++;

                        context.Entry(progress).State = EntityState.Modified;
                        context.SaveChanges();
                    }
                }

            }
            else if (this.Cycle >= 1 && this.Cycle <= 4)
            {
                using (LernEnglishContext context = new LernEnglishContext())
                {
                    progress.Date = DateTime.UtcNow;

                    context.Entry(progress).State = EntityState.Modified;
                    context.SaveChanges();
                }
                // Добавляем слово на следующий круг, когда результат повторения отрицательный и при этом это не полследний круг
                if (result <= 0 && this.Cycle <= 3)
                    this.ListOfWordList[this.Cycle + 1].Add(this.CurrentWord);
            }
            else
            {
                return "Error";
            }

            return "Ok";
        }

        // Возращает слово, которое еще не изучалось
        private EnglishWords GetNewWord()
        {
            // Находим все слова, которые еще на изучении.
            List<Progresses_new> TempProgressList = Repository.Select<Progresses_new>()
                    .Where(c => c.UserId == this.UserId)
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
                    .Where(c => c.Repetitions > 0) // Выбрать те, которые на изучении, но еще не выучены
                    .ToList();

            // Берем все существующие слова.
            List<EnglishWords> allwords = Repository.Select<EnglishWords>().ToList();

            bool flag = true;
            //int count = 0;
            // Выбираем все слова, которые в процессе изучения.
            foreach (EnglishWords word in allwords)
            {
                flag = true;
                foreach (Progresses_new progress in TempProgressList)
                {
                    if (word.WordId == progress.WordId)
                        flag = false;
                }
                // Добавляем слово, если не было найдено ни одного совпадения
                if (flag)
                {
                    /*this.WordListCycle0.Add(word);
                    count++;
                    if (count + this.WordListCycle1.Count == this.Complexity) // Завершаем перебор, когда было найдено достаточное количество слов
                        break;*/
                    //добавляем лишь одно
                    return word;
                }
            }
            return null;
        }
        private List<EnglishWords> GetAllInProgressWords()
        {
            // Находим все слова, которые еще на изучении.
            List<Progresses_new> progressList = Repository.Select<Progresses_new>()
                        .Where(c => c.UserId == this.UserId)
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
                        .Where(c => c.Repetitions > 0 && c.SuccessfulSeriesOfRepetitions < this.Success) // Выбрать те, которые на изучении, но еще не выучены
                        .ToList();

            // Берем все существующие слова.
            List<EnglishWords> allwords = Repository.Select<EnglishWords>().ToList();
            List<EnglishWords> wordList = new List<EnglishWords>();

            // Выбираем все слова, которые в процессе изучения.
            foreach (EnglishWords word in allwords)
            {
                foreach (Progresses_new progress in progressList)
                {
                    if (word.WordId == progress.WordId)
                        wordList.Add(word);
                }
            }

            return wordList;
        }
        public void DemoGetNextWord()
        {
            // первый круг - круг отбора слов для тренировки
            if (this.Cycle == 0)
            {
                if (this.Index == -100)
                { // самое начало тренировки

                    // Получаем все слова, которые находятся на изучении
                    List<EnglishWords> wordList = GetAllInProgressWords();

                    // Если уже есть достаточное количество слов, чтобы перейти на второй этап
                    if (wordList.Count >= this.Complexity)
                    {
                        this.WordListCycle1
                            .AddRange(wordList.Take(this.Complexity));

                        this.Cycle = 1;
                        this.Index = 0;

                        return;
                    }
                    else
                    {
                        this.WordListCycle1.AddRange(wordList);
                        this.WordListCycle0.Add(GetNewWord());
                        this.Index = 0;

                        return;
                    }
                }
                // Первый круг. Последнее слово. (при завершении первого круга, второй полностью заполняется)
                else if (this.Complexity == this.WordListCycle1.Count)
                {
                    this.Cycle++;
                    this.Index = 0;

                    return;
                }
                // Первый круг. Не первое слово и не последнее слово.
                else
                {
                    this.WordListCycle0.Add(GetNewWord());
                    this.Index++;
                }
            }
            else
            {
                if (this.Index + 1 < this.ListOfWordList[this.Cycle].Count)
                    this.Index++;
                else
                {
                    this.Cycle++;
                    this.Index = 0;
                }
            }
        }
    }
}