using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace LernEnglishWords.Models
{
    public class Repository
    {
        public static IQueryable<TEntity> Select<TEntity>()
            where TEntity : class
        {
            LernEnglishContext context = new LernEnglishContext();

            // Здесь мы можем указывать различные настройки контекста,
            // например выводить в отладчик сгенерированный SQL-код
            context.Database.Log =
                (s => System.Diagnostics.Debug.WriteLine(s));

            // Загрузка данных с помощью универсального метода Set
            return context.Set<TEntity>();
        }

        public static void Insert<TEntity>(TEntity entity) where TEntity : class
        {
            // Настройки контекста
            LernEnglishContext context = new LernEnglishContext();
            context.Database.Log = (s => System.Diagnostics.Debug.WriteLine(s));

            context.Entry(entity).State = EntityState.Added;
            context.SaveChanges();
        }

        public static void Inserts<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            // Настройки контекста
            LernEnglishContext context = new LernEnglishContext();

            // Отключаем отслеживание и проверку изменений для оптимизации вставки множества полей
            context.Configuration.AutoDetectChangesEnabled = false;
            context.Configuration.ValidateOnSaveEnabled = false;

            context.Database.Log = (s => System.Diagnostics.Debug.WriteLine(s));


            foreach (TEntity entity in entities)
                context.Entry(entity).State = EntityState.Added;
            context.SaveChanges();

            context.Configuration.AutoDetectChangesEnabled = true;
            context.Configuration.ValidateOnSaveEnabled = true;
        }
 
        public static void Update<TEntity>(TEntity entity)
    where TEntity : class
        {
            // Настройки контекста
            LernEnglishContext context = new LernEnglishContext();
            context.Database.Log = (s => System.Diagnostics.Debug.WriteLine(s));

            context.Entry<TEntity>(entity).State = EntityState.Modified;
            context.SaveChanges();
        }

        public static void Delete<TEntity>(TEntity entity)
    where TEntity : class
        {
            // Настройки контекста
            LernEnglishContext context = new LernEnglishContext();
            context.Database.Log = (s => System.Diagnostics.Debug.WriteLine(s));

            context.Entry<TEntity>(entity).State = EntityState.Deleted;
            context.SaveChanges();
        }
    }
}
