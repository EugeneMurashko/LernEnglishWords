namespace LernEnglishWords.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class COWandPOSwhithFilter : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.WordFilterCategoryOfWords", newName: "CategoryOfWordWFilter");
            RenameTable(name: "dbo.PartOfSpeechWordFilters", newName: "PartOfSpeechWFilter");
            RenameColumn(table: "dbo.CategoryOfWordWFilter", name: "WordFilter_Id", newName: "FilterRefId");
            RenameColumn(table: "dbo.CategoryOfWordWFilter", name: "CategoryOfWord_Id", newName: "CategoryOfWordRefId");
            RenameColumn(table: "dbo.PartOfSpeechWFilter", name: "PartOfSpeech_Id", newName: "PartOfSpeechRefId");
            RenameColumn(table: "dbo.PartOfSpeechWFilter", name: "WordFilter_Id", newName: "FilterRefId");
            RenameIndex(table: "dbo.PartOfSpeechWFilter", name: "IX_PartOfSpeech_Id", newName: "IX_PartOfSpeechRefId");
            RenameIndex(table: "dbo.PartOfSpeechWFilter", name: "IX_WordFilter_Id", newName: "IX_FilterRefId");
            RenameIndex(table: "dbo.CategoryOfWordWFilter", name: "IX_CategoryOfWord_Id", newName: "IX_CategoryOfWordRefId");
            RenameIndex(table: "dbo.CategoryOfWordWFilter", name: "IX_WordFilter_Id", newName: "IX_FilterRefId");
            DropPrimaryKey("dbo.CategoryOfWordWFilter");
            AddPrimaryKey("dbo.CategoryOfWordWFilter", new[] { "CategoryOfWordRefId", "FilterRefId" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.CategoryOfWordWFilter");
            AddPrimaryKey("dbo.CategoryOfWordWFilter", new[] { "WordFilter_Id", "CategoryOfWord_Id" });
            RenameIndex(table: "dbo.CategoryOfWordWFilter", name: "IX_FilterRefId", newName: "IX_WordFilter_Id");
            RenameIndex(table: "dbo.CategoryOfWordWFilter", name: "IX_CategoryOfWordRefId", newName: "IX_CategoryOfWord_Id");
            RenameIndex(table: "dbo.PartOfSpeechWFilter", name: "IX_FilterRefId", newName: "IX_WordFilter_Id");
            RenameIndex(table: "dbo.PartOfSpeechWFilter", name: "IX_PartOfSpeechRefId", newName: "IX_PartOfSpeech_Id");
            RenameColumn(table: "dbo.PartOfSpeechWFilter", name: "FilterRefId", newName: "WordFilter_Id");
            RenameColumn(table: "dbo.PartOfSpeechWFilter", name: "PartOfSpeechRefId", newName: "PartOfSpeech_Id");
            RenameColumn(table: "dbo.CategoryOfWordWFilter", name: "CategoryOfWordRefId", newName: "CategoryOfWord_Id");
            RenameColumn(table: "dbo.CategoryOfWordWFilter", name: "FilterRefId", newName: "WordFilter_Id");
            RenameTable(name: "dbo.PartOfSpeechWFilter", newName: "PartOfSpeechWordFilters");
            RenameTable(name: "dbo.CategoryOfWordWFilter", newName: "WordFilterCategoryOfWords");
        }
    }
}
