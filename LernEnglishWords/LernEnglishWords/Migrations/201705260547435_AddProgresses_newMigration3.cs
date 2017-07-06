namespace LernEnglishWords.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProgresses_newMigration3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Progresses_new", "EnglishWords_WordId", "dbo.EnglishWords");
            DropForeignKey("dbo.Progresses_new", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Progresses_new", new[] { "UserId" });
            DropIndex("dbo.Progresses_new", new[] { "EnglishWords_WordId" });
            DropTable("dbo.Progresses_new");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Progresses_new",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        WordId = c.Int(nullable: false),
                        Repetitions = c.Int(nullable: false),
                        SuccessfulSeriesOfRepetitions = c.Int(nullable: false),
                        Date = c.DateTimeOffset(nullable: false, precision: 7),
                        EnglishWords_WordId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.WordId });
            
            CreateIndex("dbo.Progresses_new", "EnglishWords_WordId");
            CreateIndex("dbo.Progresses_new", "UserId");
            AddForeignKey("dbo.Progresses_new", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Progresses_new", "EnglishWords_WordId", "dbo.EnglishWords", "WordId");
        }
    }
}
