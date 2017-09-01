using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LernEnglishWords.Models;
using System.Data.Entity;
using Microsoft.AspNet.Identity;

namespace LernEnglishWords.Controllers
{
    public class MainController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            if (Session["Application"] as MyApplication == null)
                Session["Application"] = MyApplication.GetReference(User.Identity.GetUserId());
            return View();
        }

        // 1.   При перезагрузке вызывается именно этот метод.
        // 1.1. Описать проверку, обходящую повторный запрос к БД.
        [HttpPost]
        public ActionResult Index(string[] POSs, string[] COWs)
        {
            if (Session["Application"] as MyApplication == null)
                Session["Application"] = MyApplication.GetReference(User.Identity.GetUserId());

            if (POSs != null && COWs != null)
            {
                List<CategoryOfWord> COWList;
                List<PartOfSpeech> POSList;
                using (var context = new LernEnglishContext())
                {
                    COWList = context.CategoryOfWord
                        .ToList();
                    POSList = context.PartOfSpeech
                        .ToList();
                }

                List<string> pos = new List<string>();
                foreach (var s in POSs)
                {
                    pos.Add(s);
                }

                List<string> cow = new List<string>();
                foreach (var s in COWs)
                {
                    cow.Add(s);
                }

                List<WordFilter> filterList = new List<WordFilter>();
                using (var context = new LernEnglishContext())
                {
                    context.Configuration.ProxyCreationEnabled = false;

                    filterList = context.WordFilter
                        .Include(e => e.CategoryOfWord)
                        .Include(p => p.PartOfSpeech)
                        .ToList();
                }

                bool found = false;
                WordFilter Filter = new WordFilter();
                foreach (var filter in filterList)
                {
                    List<string> s = new List<string>();
                    foreach (var p in filter.PartOfSpeech)
                    {
                        s.Add(p.Name);
                    }
                    if (!s.SequenceEqual(pos))
                    {
                        continue;
                    }

                    s.Clear();
                    foreach (var p in filter.CategoryOfWord)
                    {
                        s.Add(p.Name);
                    }
                    if (s.SequenceEqual(cow))
                    {
                        Filter = filter;
                        found = true;
                        break;
                    }
                }

                if(!found)
                {

                    using (var preNewContext = new LernEnglishContext())
                    {
                        preNewContext.WordFilter.Add(Filter);
                        preNewContext.SaveChanges();
                    }

                    string user_id = User.Identity.GetUserId();
                    using (var context = new LernEnglishContext())
                    {
                        context.Configuration.ProxyCreationEnabled = false;

                        AspNetUsers _User = context.AspNetUsers
                            .Where(u => u.Id == user_id)
                            .FirstOrDefault();
                        _User.WordFilter.Add(Filter);
                        context.Entry<AspNetUsers>(_User).State = EntityState.Modified;
                        context.SaveChanges();


                        foreach (var _cow in COWs)
                        {
                            CategoryOfWord _CategoryOfWord = context.CategoryOfWord
                                .Where(c => c.Name == _cow)
                                .FirstOrDefault();
                            _CategoryOfWord.WordFilter.Add(Filter);
                            context.Entry<CategoryOfWord>(_CategoryOfWord).State = EntityState.Modified;
                            context.SaveChanges();
                        }

                        foreach (var _pos in POSs)
                        {
                            PartOfSpeech _PartOfSpeech = context.PartOfSpeech
                                .Where(c => c.Name == _pos)
                                .FirstOrDefault();
                            _PartOfSpeech.WordFilter.Add(Filter);
                            context.Entry<PartOfSpeech>(_PartOfSpeech).State = EntityState.Modified;
                            context.SaveChanges();
                        }
                    }
                }
                else
                {
                    List<AspNetUsers> _user = new List<AspNetUsers>();
                    string user_id = User.Identity.GetUserId();
                    using (var context = new LernEnglishContext())
                    {
                        _user.Add(
                            context.AspNetUsers
                            .Where(u => u.Id == user_id)
                            .FirstOrDefault()
                            );
                    }
                    Filter.AspNetUsers = _user;

                    using (var newContext = new LernEnglishContext())
                    {
                        newContext.Entry(Filter).State = EntityState.Modified;
                        newContext.SaveChanges();
                    }
                }

                // Отправляем в БД
            }
            return View("Index");
        }



        // Выводить недавние фильтры, либо один - последний
        // Скопировать логику с WordFiltres
        [Authorize]
        public ActionResult Continue()// поработать над названием
        {
            if(Session["Application"] as MyApplication == null)
                Session["Application"] = MyApplication.GetReference(User.Identity.GetUserId());
            string userId = User.Identity.GetUserId();

            // Получаем список фильтров, которые можем продолжить изучать
            // Если таких нет то возвращаем другое частичное представление,
            // в котором есть возможность добавить новый фильтр
            return PartialView(/*new List<WordFiltres>()*/);
        }

        private bool checkData(int id, List<PartOfSpeech> POS, string[] poss)
        {
            throw new NotImplementedException();
        }
        private bool checkData(int id, List<CategoryOfWord> COW, string[] cows)
        {
            throw new NotImplementedException();
        }

        public ActionResult AddNewFilter()
        {
            using(var context = new LernEnglishContext())
            {
                ViewBag.PartOfSpeech = context.PartOfSpeech.ToList();
                ViewBag.CategoryOfWord = context.CategoryOfWord.ToList();
            }
            return PartialView();
        }

        // Выводит все шаблоны добавленные пользователем
        [Authorize]
        public ActionResult WordFiltres()
        {
            if (Session["Application"] as MyApplication == null)
                Session["Application"] = MyApplication.GetReference(User.Identity.GetUserId());

            MyApplication app = MyApplication.GetReference(User.Identity.GetUserId());

            AspNetUsers _User = Repository.Select<AspNetUsers>()
                .Select(c => new
                {
                    _id = c.Id,
                    _wf = c.WordFilter.Select(o => new
                    {
                        _id = o.Id,
                        _ps = o.PartOfSpeech.Select(k=> new
                        {
                            _name = k.Name
                        }),
                        _cow = o.CategoryOfWord.Select(l=> new
                        {
                            _name = l.Name
                        })

                    })
                })
                .AsEnumerable()
                .Select(c => new AspNetUsers
                {
                    Id = c._id,
                    WordFilter = c._wf.Select(o => new WordFilter
                    {
                        Id = o._id,
                        PartOfSpeech = o._ps.Select(k => new PartOfSpeech
                        {
                            Name = k._name
                        })
                        .ToList(),
                         CategoryOfWord = o._cow.Select(l=> new CategoryOfWord
                         {
                             Name = l._name
                         })
                         .ToList()
                        
                    })
                    .ToList()
                })
                .Where(c => c.Id == app.UserId)
                .ToList()
                .FirstOrDefault();

            List<WordFilter> wordFList = _User.WordFilter.ToList();
                

            return PartialView(wordFList);
        }

        // soon
        public ActionResult Statistics()
        {
            if (Session["Application"] as MyApplication == null)
                Session["Application"] = MyApplication.GetReference(User.Identity.GetUserId());
            return PartialView();
        }

        // сверить функционал с братомблизнецом
        [Authorize]
        public ActionResult Application()
        {
            if (Session["Application"] as MyApplication == null)
                Session["Application"] = MyApplication.GetReference(User.Identity.GetUserId());

            MyApplication app = MyApplication.GetReference(User.Identity.GetUserId());

            // Отправляем выбрать упражнение
            if (app.WordFilter == 0)
                return RedirectToAction("Index");

            WordFilter wFilter = Repository.Select<WordFilter>() // Создать отдельный метод в репозитории RepositoryWordFilter.SelectWithPartOfSpeechAndCategoryOfWord
                    .Where(c => c.Id == app.WordFilter)
                    .Select(c => new
                    {
                        _id = c.Id,
                        _ps = c.PartOfSpeech.Select(o => new
                        {
                            _name = o.Name
                        })

                    })
                    .AsEnumerable()
                    .Select(an => new WordFilter
                    {
                        Id = an._id,
                        PartOfSpeech = an._ps.Select(o => new PartOfSpeech
                        {
                            Name = o._name
                        }).ToList()
                    })
                    .ToList()
                    .FirstOrDefault();

            return View(wFilter);
        }

        // Тренировка продолжается, но с ошибкой в счетчике
        [Authorize]
        [HttpPost]
        public ActionResult Application(int WordFilterId) // FilterId
        {
            if (Session["Application"] as MyApplication == null)
                Session["Application"] = MyApplication.GetReference(User.Identity.GetUserId());

            MyApplication app = MyApplication.GetReference(User.Identity.GetUserId());

            // Задача.
            // 1. Спросить: Начать новое. 
            // 2. Или продолжить старое.
            // 3. Предоставить информацию.
            if (app.WordFilter == 0 || app.WordFilter != WordFilterId)
            {
                // Заглушка
                app.WordFilter = WordFilterId;
            }

            
            WordFilter wFilter = Repository.Select<WordFilter>()
                    .Where(c => c.Id == WordFilterId)
                    .Select(c => new
                    {
                        _id = c.Id,
                        _ps = c.PartOfSpeech.Select(o => new
                        {
                            _name = o.Name
                        })

                    })
                    .AsEnumerable()
                    .Select(an => new WordFilter
                    {
                        Id = an._id,
                        PartOfSpeech = an._ps.Select(o => new PartOfSpeech
                        {
                            Name = o._name
                        }).ToList()
                    })
                    .ToList()
                    .FirstOrDefault();

            return View(wFilter);
        }

        private string DemoSet(int result)
        {
            MyApplication app = MyApplication.GetReference(User.Identity.GetUserId());

            var progress = Repository.Select<Progresses_new>()
                .Where(c => c.WordId == app.CurrentWord.WordId && c.UserId == app.UserId)
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

            if (app.Cycle == 0 || (!app.wasTheFirstCycle && app.Cycle == 1)) // Первый круг упражнения
            {
                if (result == 1) // Был данн положительный ответ
                {
                    if (progress == null) // Прогресс еще не сохранялся
                    {
                        using (LernEnglishContext context = new LernEnglishContext()) // переместить в Repository
                        {
                            EnglishWords TempWord = context.Set<EnglishWords>().Where(c => c.WordId == app.CurrentWord.WordId).FirstOrDefault();
                            app.CurrentWord = TempWord;
                            Progresses_new Temp = new Progresses_new { WordId = app.CurrentWord.WordId, UserId = app.UserId, Date = DateTime.UtcNow, Repetitions = 1, SuccessfulSeriesOfRepetitions = 100, EnglishWords = app.CurrentWord };
                            context.Entry(Temp).State = EntityState.Added;
                            context.SaveChanges();
                        }

                        //
                        // Этому тут не место
                        //
                        app.WordListCycle0.RemoveAt(app.Index);
                        app.Index--;
                        //
                        //
                        //

                        return "skipWord";
                    }
                    else if (progress.Repetitions == 0) // Строка была создана ранее, но без прогресса
                    {
                        using (LernEnglishContext context = new LernEnglishContext()) // переместить в Repository
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
                        app.WordListCycle0.RemoveAt(app.Index);
                        app.Index--;
                        //
                        //
                        //

                        return "skipWord";
                    }
                    else if (progress.Repetitions >= 1) // Слово уже изучалось ранее
                    {
                        using (LernEnglishContext context = new LernEnglishContext()) // переместить в Repository
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
                else if (result <= 0) // Был данн неверный ответ
                {
                    if (progress == null) // // Прогресс еще не сохранялся
                    {
                        using (LernEnglishContext context = new LernEnglishContext()) // переместить в Repository
                        {
                            EnglishWords TempWord = context.Set<EnglishWords>().Where(c => c.WordId == app.CurrentWord.WordId).FirstOrDefault();
                            app.CurrentWord = TempWord;
                            Progresses_new Temp = new Progresses_new { WordId = app.CurrentWord.WordId, UserId = app.UserId, Date = DateTime.UtcNow, Repetitions = 1, SuccessfulSeriesOfRepetitions = 0, EnglishWords = app.CurrentWord };
                            context.Entry(Temp).State = EntityState.Added;
                            context.SaveChanges();
                        }
                        app.ListOfWordList[app.Cycle + 1].Add(app.CurrentWord);
                    }
                    else if (progress.Repetitions >= 0)
                    {
                        using (LernEnglishContext context = new LernEnglishContext()) // переместить в Repository
                        {
                            progress.Date = DateTime.UtcNow;
                            progress.Repetitions = progress.Repetitions + 1;
                            progress.SuccessfulSeriesOfRepetitions = 0;
                            context.Entry(progress).State = EntityState.Modified;
                            context.SaveChanges();
                        }
                        app.ListOfWordList[app.Cycle + 1].Add(app.CurrentWord); // убрал
                    }
                    else
                    {
                        return "Error";
                    }
                }
            }
            else if (app.Cycle == 1 && result == 1 && app.WordListCycle0.Where(x => x == app.CurrentWord).FirstOrDefault() == null) // Это слова второго круга которых не было на первом круге
            {
                if (progress == null)
                {
                    using (LernEnglishContext context = new LernEnglishContext()) // переместить в Repository
                    {
                        EnglishWords TempWord = context.Set<EnglishWords>().Where(c => c.WordId == app.CurrentWord.WordId).FirstOrDefault();
                        app.CurrentWord = TempWord;
                        Progresses_new Temp = new Progresses_new { WordId = app.CurrentWord.WordId, UserId = app.UserId, Date = DateTime.UtcNow, Repetitions = 1, SuccessfulSeriesOfRepetitions = 100, EnglishWords = app.CurrentWord };
                        context.Entry(Temp).State = EntityState.Added;
                        context.SaveChanges();
                    }
                }
                else if (progress.Repetitions == 0)
                {
                    using (LernEnglishContext context = new LernEnglishContext()) // переместить в Repository
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
                    using (LernEnglishContext context = new LernEnglishContext()) // переместить в Repository
                    {
                        progress.Date = DateTime.UtcNow;
                        progress.Repetitions++;
                        progress.SuccessfulSeriesOfRepetitions++;
                        context.Entry(progress).State = EntityState.Modified;
                        context.SaveChanges();
                    }
                }

            }
            else if (app.Cycle >= 1 && app.Cycle <= 4) 
            {
                using (LernEnglishContext context = new LernEnglishContext()) // переместить в Repository
                {
                    progress.Date = DateTime.UtcNow;
                    context.Entry(progress).State = EntityState.Modified;
                    context.SaveChanges();
                }
                if(result <= 0 && app.Cycle <= 3) // Добавляем слово на следующий круг, если результат повторения отрицательный и если это не полследний круг
                    app.ListOfWordList[app.Cycle + 1].Add(app.CurrentWord);
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
            MyApplication app = MyApplication.GetReference(User.Identity.GetUserId());
            // Находим все слова, которые еще на изучении.
            List<Progresses_new> TempProgressList = Repository.Select<Progresses_new>()
                    .Where(c => c.UserId == app.UserId)
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
                if (flag) // Добавляем слово, если не было найдено ни одного совпадения
                {
                    /*app.WordListCycle0.Add(word);
                    count++;
                    if (count + app.WordListCycle1.Count == app.Complexity) // Завершаем перебор, когда было найдено достаточное количество слов
                        break;*/
                    //добавляем лишь одно
                    return word;
                }
            }
            return null;
        }
        private List<EnglishWords> GetAllInProgressWords()
        {
            MyApplication app = MyApplication.GetReference(User.Identity.GetUserId());

            // Находим все слова, которые еще на изучении.
            List<Progresses_new> progressList = Repository.Select<Progresses_new>()
                        .Where(c => c.UserId == app.UserId)
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
                        .Where(c => c.Repetitions > 0 && c.SuccessfulSeriesOfRepetitions < app.Success) // Выбрать те, которые на изучении, но еще не выучены
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
        private void DemoGetNextWord()
        {
            MyApplication app = MyApplication.GetReference(User.Identity.GetUserId());

            if (app.Cycle == 0) // первый круг - круг отбора слов для тренировки
            {
                if (app.Index == -100)
                { // самое начало тренировки

                    List<EnglishWords> wordList = GetAllInProgressWords(); // Получаем все слова, которые находятся на изучении

                    if (wordList.Count >= app.Complexity) // Если уже есть достаточное количество слов, чтобы перейти на второй этап
                    {
                        app.WordListCycle1
                            .AddRange(wordList.Take(app.Complexity));
                        app.Cycle = 1;
                        app.Index = 0;
                        return;
                    }
                    else
                    {
                        app.WordListCycle1.AddRange(wordList);
                        app.WordListCycle0.Add(GetNewWord());
                        app.Index = 0;

                        return;
                    }
                }
                else if (app.Complexity == app.WordListCycle1.Count) // Первый круг. Последнее слово. (при завершении первого круга, второй полностью заполняется)
                {
                    app.Cycle++;
                    app.Index = 0;
                    return;
                }
                else // Первый круг. Не первое слово и не последнее слово.
                {
                    app.WordListCycle0.Add(GetNewWord());
                    app.Index++;
                }
            }
            else
            {
                if (app.Index + 1 < app.ListOfWordList[app.Cycle].Count)
                     app.Index++;
                else
                {
                    app.Cycle++;
                    app.Index = 0;
                }
            }
        }

        [HttpPost]
        public ActionResult App_TempName(int result)
        {
            if (Session["Application"] as MyApplication == null)
                Session["Application"] = MyApplication.GetReference(User.Identity.GetUserId());
            MyApplication app = MyApplication.GetReference(User.Identity.GetUserId());

            var check = DemoSet(result); // возвращает состояние сохранения данных

            DemoGetNextWord();

            if (app.Cycle > 4 || app.ListOfWordList[app.Cycle].Count == 0)
            {
                MyApplication.Delete();
                return View("Index");
            }
            return PartialView(app);
        }

        [Authorize]
        public ActionResult App_TempName_First()
        {
            if (Session["Application"] as MyApplication == null)
                Session["Application"] = MyApplication.GetReference(User.Identity.GetUserId());

            MyApplication app = MyApplication.GetReference(User.Identity.GetUserId());

            DemoGetNextWord(); 

            return PartialView("App_TempName", app);
        }
    }
}