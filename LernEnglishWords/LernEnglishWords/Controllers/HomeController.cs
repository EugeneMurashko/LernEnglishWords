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