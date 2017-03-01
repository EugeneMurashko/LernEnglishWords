namespace LernEnglishWords.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TestingMigrations2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.EnglishWords", "Block_BlockId", "dbo.Blocks");
            DropIndex("dbo.EnglishWords", new[] { "Block_BlockId" });
            AddColumn("dbo.Blocks", "WordId", c => c.Int(nullable: false));
            CreateIndex("dbo.Blocks", "WordId");
            AddForeignKey("dbo.Blocks", "WordId", "dbo.EnglishWords", "WordId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            AddColumn("dbo.EnglishWords", "Block_BlockId", c => c.Int());
            DropForeignKey("dbo.Blocks", "WordId", "dbo.EnglishWords");
            DropIndex("dbo.Blocks", new[] { "WordId" });
            DropColumn("dbo.Blocks", "WordId");
            CreateIndex("dbo.EnglishWords", "Block_BlockId");
            AddForeignKey("dbo.EnglishWords", "Block_BlockId", "dbo.Blocks", "BlockId");
        }
    }
}
