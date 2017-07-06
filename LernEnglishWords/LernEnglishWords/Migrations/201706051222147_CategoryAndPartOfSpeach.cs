namespace LernEnglishWords.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CategoryAndPartOfSpeach : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CategoryOfWords", "Name", c => c.String(nullable: false));
            AddColumn("dbo.PartOfSpeeches", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PartOfSpeeches", "Name");
            DropColumn("dbo.CategoryOfWords", "Name");
        }
    }
}
