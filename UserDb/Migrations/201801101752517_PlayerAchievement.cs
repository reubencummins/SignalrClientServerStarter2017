namespace UserDb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PlayerAchievement : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Achievement", "Game_GameID", "dbo.Game");
            DropIndex("dbo.Achievement", new[] { "Game_GameID" });
            RenameColumn(table: "dbo.Achievement", name: "Game_GameID", newName: "GameID");
            AlterColumn("dbo.Achievement", "GameID", c => c.Int(nullable: false));
            CreateIndex("dbo.Achievement", "GameID");
            AddForeignKey("dbo.Achievement", "GameID", "dbo.Game", "GameID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Achievement", "GameID", "dbo.Game");
            DropIndex("dbo.Achievement", new[] { "GameID" });
            AlterColumn("dbo.Achievement", "GameID", c => c.Int());
            RenameColumn(table: "dbo.Achievement", name: "GameID", newName: "Game_GameID");
            CreateIndex("dbo.Achievement", "Game_GameID");
            AddForeignKey("dbo.Achievement", "Game_GameID", "dbo.Game", "GameID");
        }
    }
}
