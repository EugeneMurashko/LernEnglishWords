using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LernEnglishWords.Models;
using System.Data.Entity;
using System.Text.RegularExpressions;

namespace LernEnglishWords.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        // Метод со всеми видами манипуляций с данными
        private void Test()
        {
            var EnglishWords = Repository.Select<EnglishWord>().ToList();

            EnglishWord EngWord = new EnglishWord { Word = "TEST_WORD" };
            Repository.Insert<EnglishWord>(EngWord);
            var EnglishWords2 = Repository.Select<EnglishWord>().ToList();

            var EnglishWordTest = EnglishWords2
                .Where(c => c.Word.StartsWith("TEST_WORD")).ToList();
            foreach (EnglishWord b in EnglishWordTest)
            {
                b.Word = "DELETE";
                Repository.Update<EnglishWord>(b);
            }

            var EnglishWords3 = Repository.Select<EnglishWord>().ToList();
            var EnglishWordDelete = EnglishWords2
                .Where(c => c.Word.StartsWith("DELETE")).ToList();
            foreach (EnglishWord b in EnglishWordDelete)
            {
                Repository.Delete<EnglishWord>(b);
            }

            var EnglishWords4 = Repository.Select<EnglishWord>().ToList();
            var EnglishWordTest3 = EnglishWords4
               .Where(c => c.Word.StartsWith("TEST_WORD")).ToList();
            var EnglishWordDelete3 = EnglishWords4
                .Where(c => c.Word.StartsWith("DELETE")).ToList();
        }    
    }
}