namespace LernEnglishWords.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TheFirstBigCleaning : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Blocks", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Blocks", "WordId", "dbo.EnglishWords");
            DropForeignKey("dbo.Progresses", "BlockId", "dbo.Blocks");
            DropIndex("dbo.Blocks", new[] { "Id" });
            DropIndex("dbo.Blocks", new[] { "WordId" });
            DropIndex("dbo.Progresses", new[] { "BlockId" });
            DropTable("dbo.Blocks");
            DropTable("dbo.Progresses");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Progresses",
                c => new
                    {
                        BlockId = c.Int(nullable: false),
                        Done = c.Boolean(nullable: false),
                        Repetitions = c.Int(nullable: false),
                        SuccessfulSeriesOfRepetitions = c.Int(nullable: false),
                        Date = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.BlockId);
            
            CreateTable(
                "dbo.Blocks",
                c => new
                    {
                        BlockId = c.Int(nullable: false, identity: true),
                        NamberBlock = c.Int(nullable: false),
                        Id = c.String(maxLength: 128),
                        WordId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BlockId);
            
            CreateIndex("dbo.Progresses", "BlockId");
            CreateIndex("dbo.Blocks", "WordId");
            CreateIndex("dbo.Blocks", "Id");
            AddForeignKey("dbo.Progresses", "BlockId", "dbo.Blocks", "BlockId");
            AddForeignKey("dbo.Blocks", "WordId", "dbo.EnglishWords", "WordId", cascadeDelete: true);
            AddForeignKey("dbo.Blocks", "Id", "dbo.AspNetUsers", "Id");
        }
    }
}
