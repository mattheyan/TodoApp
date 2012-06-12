namespace TodoApp.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "List",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Sequence = c.Int(nullable: false),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("User", t => t.User_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "User",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        OpenId = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "ListItem",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false),
                        Sequence = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DueDate = c.DateTime(),
                        List_Id = c.Int(),
                        Priority_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("List", t => t.List_Id)
                .ForeignKey("Priority", t => t.Priority_Id)
                .Index(t => t.List_Id)
                .Index(t => t.Priority_Id);
            
            CreateTable(
                "Priority",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Sequence = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropIndex("ListItem", new[] { "Priority_Id" });
            DropIndex("ListItem", new[] { "List_Id" });
            DropIndex("List", new[] { "User_Id" });
            DropForeignKey("ListItem", "Priority_Id", "Priority");
            DropForeignKey("ListItem", "List_Id", "List");
            DropForeignKey("List", "User_Id", "User");
            DropTable("Priority");
            DropTable("ListItem");
            DropTable("User");
            DropTable("List");
        }
    }
}
