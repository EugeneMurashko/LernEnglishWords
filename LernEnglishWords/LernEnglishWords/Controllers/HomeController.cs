using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LernEnglishWords.Models;
using System.Data.Entity;
using System.Text.RegularExpressions;
using System.Xml;
using System.IO;
using System.Data.Entity.Infrastructure;
using System.IO.Compression;
using System.Data.SqlClient;


namespace LernEnglishWords.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Blocks = Repository.Select<Blocks>().ToList();
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

        public ActionResult AddBlock()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddNewBlock(string UserId)
        {
            int thisnamberblock = Blocks.GetNumberBlock(UserId);
            List<Blocks> blockList = Blocks.GetWordList(UserId, thisnamberblock, 3);

            Repository.Inserts<Blocks>(blockList);
            ViewBag.Info = "Имя пользователя: " + blockList[0].Id + "; Номер блока: " + blockList[0].NamberBlock.ToString() + "; Количество новых слов: " + blockList.Count().ToString() + ".";
            return PartialView();
        }

        [HttpPost]
        public ActionResult BlockSearch(string name)
        {
            var allblocks = Repository.Select<Blocks>().Where(a => a.AspNetUsers.UserName.Contains(name)).ToList();
            if (allblocks.Count <= 0)
            {
                return HttpNotFound();
            }
            return PartialView(allblocks);
        }

        [HttpPost]
        public ActionResult DeleteBlock(string name, string id)
        {
            int idid = Convert.ToInt32(id);
            var allblocks = Repository.Select<Blocks>()
                .Where(a => a.AspNetUsers.UserName.Contains(name) && a.NamberBlock == idid)
                .AsEnumerable()
                .Select(a => new Blocks
                {
                    BlockId = a.BlockId
                })
                .ToList();
            if (allblocks.Count <= 0)
            {
                return HttpNotFound();
            }
            Repository.Deletes<Blocks>(allblocks);


            ViewBag.DeleteInfo = "Удалены слова пользователя: " + name + " из блока номер " + id + ".";
            return PartialView();
        }

        
        /*удалить*/
        public ActionResult BlockList()
        {
            var AllEW = Repository.Select<EnglishWords>()
                .AsEnumerable()
                .ToList();

            var AllUser = Repository.Select<AspNetUsers>()
                .AsEnumerable()
                .ToList();

            var EWList = new List<SelectListItem>();
            SelectList ewlist = new SelectList(AllEW);
            EWList.AddRange(ewlist);
            /*int col = 0;
            foreach (EnglishWords englishword in AllEW)
            {
                EWList.Add(new SelectListItem
                {
                    Text = englishword.Word.ToString(),
                    Value = englishword.Word.ToString()
                });
                col++;
            }*/

            
            
            var UserList = new List<SelectListItem>();
            int col = 0;
            foreach (AspNetUsers user in AllUser)
            {
                UserList.Add(new SelectListItem
                {
                    Text = user.UserName.ToString(),
                    Value = user.UserName.ToString()
                });
                col++;
            }

            ViewBag.EWList = EWList;
            ViewBag.UserList = UserList;

            return View();
        }



    }
    // когда бд не хочет мигрировать
    // использовать осторожно!!!
   /* public static class HelpClass
    {
        public static void Temp()
        {
            Testing context = new Testing();
            MemoryStream stream = new MemoryStream();
            using (var xmlWriter = XmlWriter.Create(stream, new XmlWriterSettings { Indent = true }))
            {
                EdmxWriter.WriteEdmx(context, xmlWriter);
            }

            using (var gzip = new GZipStream(stream, CompressionMode.Compress))
            using (var xmlWriter = XmlWriter.Create(gzip, new XmlWriterSettings { Indent = true }))
            {
                EdmxWriter.WriteEdmx(context, xmlWriter);
            }
        }

        public static void ForceUpdate(this Testing context)
        {
            using (var connection = ((SqlConnection)context.Database.Connection))
            {
                var migrationId = string.Empty;
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"select top 1 MigrationId
                                      from __MigrationHistory
                                     order by MigrationId desc";
                    connection.Open();
                    migrationId = command.ExecuteScalar().ToString();
                    connection.Close();
                }

                using (var command = connection.CreateCommand())
                {
                    command.CommandText =
                        @"
                 update __MigrationHistory
                    set Model = @Model
                  where MigrationId = @MigrationId;
                ";
                    command.Parameters.AddWithValue("@Model", GetModel(context));
                    command.Parameters.AddWithValue("@MigrationId", migrationId);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        private static byte[] GetModel(Testing context)
        {
            using (var stream = new MemoryStream())
            {
                using (var gzip = new GZipStream(stream, CompressionMode.Compress))
                using (var xml = XmlWriter.Create(gzip, new XmlWriterSettings { Indent = true }))
                {
                    EdmxWriter.WriteEdmx(context, xml);
                }

                return stream.ToArray();
            }
        }
    }*/
}