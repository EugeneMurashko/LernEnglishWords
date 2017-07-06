namespace LernEnglishWords.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProgresses_newMigration4 : DbMigration
    {
        public override void Up()
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
                .PrimaryKey(t => new { t.UserId, t.WordId })
                .ForeignKey("dbo.EnglishWords", t => t.EnglishWords_WordId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.EnglishWords_WordId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Progresses_new", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Progresses_new", "EnglishWords_WordId", "dbo.EnglishWords");
            DropIndex("dbo.Progresses_new", new[] { "EnglishWords_WordId" });
            DropIndex("dbo.Progresses_new", new[] { "UserId" });
            DropTable("dbo.Progresses_new");
        }
    }
}
