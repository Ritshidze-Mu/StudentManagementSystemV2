namespace StudentManagementSystemV2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifyStudentClass : DbMigration
    {
        public override void Up()
        {
            // Drop the existing primary key on StudentId
            DropPrimaryKey("dbo.Students");

            // Rename StudentId to UserId
            RenameColumn(table: "dbo.Students", name: "StudentId", newName: "UserId");

            // Modify the columns as needed
            AlterColumn("dbo.Students", "Email", c => c.String());
            AlterColumn("dbo.Students", "PhoneNumber", c => c.String());
            AlterColumn("dbo.Students", "UserId", c => c.String(nullable: false, maxLength: 128));

            // Add primary key on UserId
            AddPrimaryKey("dbo.Students", "UserId");

            // Create an index on UserId
            CreateIndex("dbo.Students", "UserId");
        }

        public override void Down()
        {
            // Revert back the changes by dropping the index and primary key
            DropIndex("dbo.Students", new[] { "UserId" });
            DropPrimaryKey("dbo.Students");

            AlterColumn("dbo.Students", "UserId", c => c.String(nullable: false));
            AlterColumn("dbo.Students", "PhoneNumber", c => c.String(nullable: false, maxLength: 10));
            AlterColumn("dbo.Students", "Email", c => c.String(nullable: false, maxLength: 256));

            // Rename UserId back to StudentId
            RenameColumn(table: "dbo.Students", name: "UserId", newName: "StudentId");

            // Add primary key on StudentId
            AddPrimaryKey("dbo.Students", "StudentId");

            // Create an index on StudentId
            CreateIndex("dbo.Students", "StudentId");
        }
    }
}
