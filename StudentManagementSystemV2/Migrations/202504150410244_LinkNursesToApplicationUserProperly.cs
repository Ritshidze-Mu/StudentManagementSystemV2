namespace StudentManagementSystemV2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LinkNursesToApplicationUserProperly : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Nurses", "ApplicationUserId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Nurses", "ApplicationUserId");
            AddForeignKey("dbo.Nurses", "ApplicationUserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            DropColumn("dbo.Nurses", "Password");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Nurses", "Password", c => c.String(nullable: false));
            DropForeignKey("dbo.Nurses", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.Nurses", new[] { "ApplicationUserId" });
            DropColumn("dbo.Nurses", "ApplicationUserId");
        }
    }
}
