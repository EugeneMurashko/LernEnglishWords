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

//вариант из учебника. Здесь почемуто соединение передается как параметр
// хотя в этом есть смысл, если у нашего сайта было бы несколько соенидений с разными бд
/*public static void Update<TEntity>(TEntity entity, DbContext context)
where TEntity : class
{
    // Настройки контекста
    context.Database.Log = (s => System.Diagnostics.Debug.WriteLine(s));

    context.Entry<TEntity>(entity).State = EntityState.Modified;
    context.SaveChanges();
}*/

// перегруженный вариант Update, по аналогии с остальными

/* обработчик позваляющий отслеживать изменения (удаления и добавления) локальных данных
public static void LocalLinqQueryies()
{
    SampleContext context = new SampleContext();

    context.Customers.Local
        .CollectionChanged += (sender, args) => // лямбда-выражение. Можно добавить и отдельный метод
        {
            if (args.NewItems != null)
            {
                foreach (Customer c in args.NewItems)
                {
                    Console.WriteLine("Добавлен: " + c.FirstName);
                }
            }
            if (args.OldItems != null)
            {
                foreach (Customer c in args.OldItems)
                {
                    Console.WriteLine("Удален: " + c.FirstName);
                }
            }
        };

    context.Customers.Load();
}*/
