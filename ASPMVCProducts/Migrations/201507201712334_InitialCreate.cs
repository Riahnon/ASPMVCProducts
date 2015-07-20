namespace ASPMVCProducts.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductLists",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 256),
                        Owner_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserProfile", t => t.Owner_UserId)
                .Index(t => t.Owner_UserId);
            
            CreateTable(
                "dbo.UserProfile",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.ProductEntries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Amount = c.Int(nullable: false),
                        Comments = c.String(),
                        List_Id = c.Int(),
                        Product_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProductLists", t => t.List_Id)
                .ForeignKey("dbo.Products", t => t.Product_Id)
                .Index(t => t.List_Id)
                .Index(t => t.Product_Id);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductEntries", "Product_Id", "dbo.Products");
            DropForeignKey("dbo.ProductEntries", "List_Id", "dbo.ProductLists");
            DropForeignKey("dbo.ProductLists", "Owner_UserId", "dbo.UserProfile");
            DropIndex("dbo.Products", new[] { "Name" });
            DropIndex("dbo.ProductEntries", new[] { "Product_Id" });
            DropIndex("dbo.ProductEntries", new[] { "List_Id" });
            DropIndex("dbo.ProductLists", new[] { "Owner_UserId" });
            DropTable("dbo.Products");
            DropTable("dbo.ProductEntries");
            DropTable("dbo.UserProfile");
            DropTable("dbo.ProductLists");
        }
    }
}
