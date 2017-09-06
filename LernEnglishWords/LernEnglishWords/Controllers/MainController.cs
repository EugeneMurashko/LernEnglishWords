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
                    COWList = context.CategoryOfWord.ToList();
                    POSList = context.PartOfSpeech.ToList();
                }

                List<string> pos = new List<string>();
                pos.AddRange(POSs);

                List<string> cow = new List<string>();
                cow.AddRange(COWs);

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
                    AspNetUsers _user = new AspNetUsers();
                    List<WordFilter> _filterList = new List<WordFilter>();
                    string user_id = User.Identity.GetUserId();

                    using (var context = new LernEnglishContext())
                    {
                        _user = context.AspNetUsers
                            .Where(u => u.Id == user_id)
                            .FirstOrDefault();
                    }

                    _filterList.Add(Filter);
                    _user.WordFilter = _filterList;

                    using (var newContext = new LernEnglishContext())
                    {
                        newContext.Entry(_user).State = Filter.AspNetUsers
                            .Where(u => u.Id == user_id)
                            .First()
                            .Equals(null)
                            ? EntityState.Modified: EntityState.Unchanged;

                        newContext.SaveChanges();
                    }
                }
            }
            return View("Index");
        }

        // Выводить недавние фильтры, либо один - последний
        // Скопировать логику с WordFiltres
        [Authorize]
        public ActionResult Continue() // ТЗ Поработать над названием
        {
            if(Session["Application"] as MyApplication == null)
                Session["Application"] = MyApplication.GetReference(User.Identity.GetUserId());

            string userId = User.Identity.GetUserId();

            // ТЗ
            // Получаем список фильтров, которые можем продолжить изучать
            // Если таких нет то возвращаем другое частичное представление,
            // в котором есть возможность добавить новый фильтр
            return PartialView(/*new List<WordFiltres>()*/);
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

        [Authorize]
        public ActionResult Application()
        {
            if (Session["Application"] as MyApplication == null)
                Session["Application"] = MyApplication.GetReference(User.Identity.GetUserId());

            MyApplication app = MyApplication.GetReference(User.Identity.GetUserId());

            // Отправляем выбрать упражнение
            if (app.WordFilter == 0)
                return RedirectToAction("Index");

            WordFilter wFilter = Repository.Select<WordFilter>() // ТЗ Создать отдельный метод в репозитории RepositoryWordFilter.SelectWithPartOfSpeechAndCategoryOfWord
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
            // 1. Спросить: Начать новое? 
            // 2. Или продолжить старое?
            // 3. Предоставить информацию.
            if (app.WordFilter == 0 || app.WordFilter != WordFilterId)
            {
                app.WordFilter = WordFilterId; // Заглушка
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


        [HttpPost]
        public ActionResult App_TempName(int result)
        {
            if (Session["Application"] as MyApplication == null)
                Session["Application"] = MyApplication.GetReference(User.Identity.GetUserId());

            MyApplication app = MyApplication.GetReference(User.Identity.GetUserId());

            // возвращает состояние сохранения данных
            var check = app.DemoSet(result); 

            app.DemoGetNextWord();

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

            app.DemoGetNextWord();

            return PartialView("App_TempName", app);
        }
    }
}