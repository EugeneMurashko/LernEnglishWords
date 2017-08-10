namespace LernEnglishWords.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class POSandCOW : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.CategoryOfWordEnglishWords", newName: "CategoryOfWordEnWord");
            RenameTable(name: "dbo.PartOfSpeechEnglishWords", newName: "PartOfSpeechEnWord");
            RenameColumn(table: "dbo.CategoryOfWordEnWord", name: "CategoryOfWord_Id", newName: "CategoryOfWordRefId");
            RenameColumn(table: "dbo.CategoryOfWordEnWord", name: "EnglishWords_WordId", newName: "EnWordRefId");
            RenameColumn(table: "dbo.PartOfSpeechEnWord", name: "PartOfSpeech_Id", newName: "PartOfSpeechRefId");
            RenameColumn(table: "dbo.PartOfSpeechEnWord", name: "EnglishWords_WordId", newName: "EnWordRefId");
            RenameIndex(table: "dbo.CategoryOfWordEnWord", name: "IX_CategoryOfWord_Id", newName: "IX_CategoryOfWordRefId");
            RenameIndex(table: "dbo.CategoryOfWordEnWord", name: "IX_EnglishWords_WordId", newName: "IX_EnWordRefId");
            RenameIndex(table: "dbo.PartOfSpeechEnWord", name: "IX_PartOfSpeech_Id", newName: "IX_PartOfSpeechRefId");
            RenameIndex(table: "dbo.PartOfSpeechEnWord", name: "IX_EnglishWords_WordId", newName: "IX_EnWordRefId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.PartOfSpeechEnWord", name: "IX_EnWordRefId", newName: "IX_EnglishWords_WordId");
            RenameIndex(table: "dbo.PartOfSpeechEnWord", name: "IX_PartOfSpeechRefId", newName: "IX_PartOfSpeech_Id");
            RenameIndex(table: "dbo.CategoryOfWordEnWord", name: "IX_EnWordRefId", newName: "IX_EnglishWords_WordId");
            RenameIndex(table: "dbo.CategoryOfWordEnWord", name: "IX_CategoryOfWordRefId", newName: "IX_CategoryOfWord_Id");
            RenameColumn(table: "dbo.PartOfSpeechEnWord", name: "EnWordRefId", newName: "EnglishWords_WordId");
            RenameColumn(table: "dbo.PartOfSpeechEnWord", name: "PartOfSpeechRefId", newName: "PartOfSpeech_Id");
            RenameColumn(table: "dbo.CategoryOfWordEnWord", name: "EnWordRefId", newName: "EnglishWords_WordId");
            RenameColumn(table: "dbo.CategoryOfWordEnWord", name: "CategoryOfWordRefId", newName: "CategoryOfWord_Id");
            RenameTable(name: "dbo.PartOfSpeechEnWord", newName: "PartOfSpeechEnglishWords");
            RenameTable(name: "dbo.CategoryOfWordEnWord", newName: "CategoryOfWordEnglishWords");
        }
    }
}
