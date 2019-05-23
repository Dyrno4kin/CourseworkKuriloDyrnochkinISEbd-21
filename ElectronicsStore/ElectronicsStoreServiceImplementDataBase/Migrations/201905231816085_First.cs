namespace ElectronicsStoreServiceImplementDataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class First : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerFIO = c.String(nullable: false),
                        Login = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        DateRegistration = c.DateTime(nullable: false),
                        Bonus = c.Int(nullable: false),
                        CustomerStatus = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Indents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(nullable: false),
                        Sum = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Status = c.Int(nullable: false),
                        DateCreate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.CustomerId, cascadeDelete: true)
                .Index(t => t.CustomerId);
            
            CreateTable(
                "dbo.IndentPayments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IndentId = c.Int(nullable: false),
                        DatePayment = c.DateTime(nullable: false),
                        SumPayment = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Indents", t => t.IndentId, cascadeDelete: true)
                .Index(t => t.IndentId);
            
            CreateTable(
                "dbo.IndentProducts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IndentId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        Count = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Indents", t => t.IndentId, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.IndentId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductName = c.String(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.IndentProducts", "ProductId", "dbo.Products");
            DropForeignKey("dbo.IndentProducts", "IndentId", "dbo.Indents");
            DropForeignKey("dbo.IndentPayments", "IndentId", "dbo.Indents");
            DropForeignKey("dbo.Indents", "CustomerId", "dbo.Customers");
            DropIndex("dbo.IndentProducts", new[] { "ProductId" });
            DropIndex("dbo.IndentProducts", new[] { "IndentId" });
            DropIndex("dbo.IndentPayments", new[] { "IndentId" });
            DropIndex("dbo.Indents", new[] { "CustomerId" });
            DropTable("dbo.Products");
            DropTable("dbo.IndentProducts");
            DropTable("dbo.IndentPayments");
            DropTable("dbo.Indents");
            DropTable("dbo.Customers");
        }
    }
}
