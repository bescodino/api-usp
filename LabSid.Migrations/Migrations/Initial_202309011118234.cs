using FluentMigrator;
using System.Data;

namespace LabSid.Migrations.Migrations
{
    [Migration(202402191354434, "Initial database setup; Create tables")]
    public class Initial_202402191354434 : Migration
    {
        public override void Up()
        {
            Create.Table("user")
                .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn("Name").AsString(100)
                .WithColumn("Email").AsString(50).Unique()
                .WithColumn("Password").AsString(3);


            Create.Table("order")
                .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn("Name").AsString(100)
                .WithColumn("UserId").AsInt32().ForeignKey("FK_Order_Users", "user", "Id").OnDeleteOrUpdate(Rule.Cascade);

            Create.Table("product")
                .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn("Name").AsString(100)
                .WithColumn("Value").AsDecimal(5, 1)
                .WithColumn("OrderId").AsInt32().ForeignKey("FK_Product_Order", "order", "Id").OnDeleteOrUpdate(Rule.Cascade);
        }

        public override void Down()
        {
            Delete.Table("user");
            Delete.Table("order");
            Delete.Table("product");
        }

    }
}
