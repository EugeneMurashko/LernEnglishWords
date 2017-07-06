namespace LernEnglishWords.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFilterMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CategoryOfWords",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.WordFilters",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.HistoryOfExercises",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTimeOffset(nullable: false, precision: 7),
                        WordFilter_Id = c.Int(nullable: false),
                        User_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.WordFilters", t => t.WordFilter_Id, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.WordFilter_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.PartOfSpeeches",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CategoryOfWordEnglishWords",
                c => new
                    {
                        CategoryOfWord_Id = c.Int(nullable: false),
                        EnglishWords_WordId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CategoryOfWord_Id, t.EnglishWords_WordId })
                .ForeignKey("dbo.CategoryOfWords", t => t.CategoryOfWord_Id, cascadeDelete: true)
                .ForeignKey("dbo.EnglishWords", t => t.EnglishWords_WordId, cascadeDelete: true)
                .Index(t => t.CategoryOfWord_Id)
                .Index(t => t.EnglishWords_WordId);
            
            CreateTable(
                "dbo.WordFilterAspNetUsers",
                c => new
                    {
                        WordFilter_Id = c.Int(nullable: false),
                        AspNetUsers_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.WordFilter_Id, t.AspNetUsers_Id })
                .ForeignKey("dbo.WordFilters", t => t.WordFilter_Id, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.AspNetUsers_Id, cascadeDelete: true)
                .Index(t => t.WordFilter_Id)
                .Index(t => t.AspNetUsers_Id);
            
            CreateTable(
                "dbo.WordFilterCategoryOfWords",
                c => new
                    {
                        WordFilter_Id = c.Int(nullable: false),
                        CategoryOfWord_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.WordFilter_Id, t.CategoryOfWord_Id })
                .ForeignKey("dbo.WordFilters", t => t.WordFilter_Id, cascadeDelete: true)
                .ForeignKey("dbo.CategoryOfWords", t => t.CategoryOfWord_Id, cascadeDelete: true)
                .Index(t => t.WordFilter_Id)
                .Index(t => t.CategoryOfWord_Id);
            
            CreateTable(
                "dbo.PartOfSpeechEnglishWords",
                c => new
                    {
                        PartOfSpeech_Id = c.Int(nullable: false),
                        EnglishWords_WordId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.PartOfSpeech_Id, t.EnglishWords_WordId })
                .ForeignKey("dbo.PartOfSpeeches", t => t.PartOfSpeech_Id, cascadeDelete: true)
                .ForeignKey("dbo.EnglishWords", t => t.EnglishWords_WordId, cascadeDelete: true)
                .Index(t => t.PartOfSpeech_Id)
                .Index(t => t.EnglishWords_WordId);
            
            CreateTable(
                "dbo.PartOfSpeechWordFilters",
                c => new
                    {
                        PartOfSpeech_Id = c.Int(nullable: false),
                        WordFilter_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.PartOfSpeech_Id, t.WordFilter_Id })
                .ForeignKey("dbo.PartOfSpeeches", t => t.PartOfSpeech_Id, cascadeDelete: true)
                .ForeignKey("dbo.WordFilters", t => t.WordFilter_Id, cascadeDelete: true)
                .Index(t => t.PartOfSpeech_Id)
                .Index(t => t.WordFilter_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.HistoryOfExercises", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.PartOfSpeechWordFilters", "WordFilter_Id", "dbo.WordFilters");
            DropForeignKey("dbo.PartOfSpeechWordFilters", "PartOfSpeech_Id", "dbo.PartOfSpeeches");
            DropForeignKey("dbo.PartOfSpeechEnglishWords", "EnglishWords_WordId", "dbo.EnglishWords");
            DropForeignKey("dbo.PartOfSpeechEnglishWords", "PartOfSpeech_Id", "dbo.PartOfSpeeches");
            DropForeignKey("dbo.HistoryOfExercises", "WordFilter_Id", "dbo.WordFilters");
            DropForeignKey("dbo.WordFilterCategoryOfWords", "CategoryOfWord_Id", "dbo.CategoryOfWords");
            DropForeignKey("dbo.WordFilterCategoryOfWords", "WordFilter_Id", "dbo.WordFilters");
            DropForeignKey("dbo.WordFilterAspNetUsers", "AspNetUsers_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.WordFilterAspNetUsers", "WordFilter_Id", "dbo.WordFilters");
            DropForeignKey("dbo.CategoryOfWordEnglishWords", "EnglishWords_WordId", "dbo.EnglishWords");
            DropForeignKey("dbo.CategoryOfWordEnglishWords", "CategoryOfWord_Id", "dbo.CategoryOfWords");
            DropIndex("dbo.PartOfSpeechWordFilters", new[] { "WordFilter_Id" });
            DropIndex("dbo.PartOfSpeechWordFilters", new[] { "PartOfSpeech_Id" });
            DropIndex("dbo.PartOfSpeechEnglishWords", new[] { "EnglishWords_WordId" });
            DropIndex("dbo.PartOfSpeechEnglishWords", new[] { "PartOfSpeech_Id" });
            DropIndex("dbo.WordFilterCategoryOfWords", new[] { "CategoryOfWord_Id" });
            DropIndex("dbo.WordFilterCategoryOfWords", new[] { "WordFilter_Id" });
            DropIndex("dbo.WordFilterAspNetUsers", new[] { "AspNetUsers_Id" });
            DropIndex("dbo.WordFilterAspNetUsers", new[] { "WordFilter_Id" });
            DropIndex("dbo.CategoryOfWordEnglishWords", new[] { "EnglishWords_WordId" });
            DropIndex("dbo.CategoryOfWordEnglishWords", new[] { "CategoryOfWord_Id" });
            DropIndex("dbo.HistoryOfExercises", new[] { "User_Id" });
            DropIndex("dbo.HistoryOfExercises", new[] { "WordFilter_Id" });
            DropTable("dbo.PartOfSpeechWordFilters");
            DropTable("dbo.PartOfSpeechEnglishWords");
            DropTable("dbo.WordFilterCategoryOfWords");
            DropTable("dbo.WordFilterAspNetUsers");
            DropTable("dbo.CategoryOfWordEnglishWords");
            DropTable("dbo.PartOfSpeeches");
            DropTable("dbo.HistoryOfExercises");
            DropTable("dbo.WordFilters");
            DropTable("dbo.CategoryOfWords");
        }
    }
}
