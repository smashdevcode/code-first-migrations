namespace CodeFirstMigrations.Migrations
{
	using System;
	using System.Data.Entity.Migrations;

	public partial class AddItemTable : DbMigration
	{
		public override void Up()
		{
			CreateTable(
				"dbo.Item",
				c => new
					{
						ItemID = c.Int(nullable: false, identity: true),
						ItemNumber = c.String(nullable: false, maxLength: 100),
					})
				.PrimaryKey(t => t.ItemID);
			CreateIndex("dbo.Item", "ItemNumber", true);

			// populate the Item table
			Sql(@"
				insert Item (ItemNumber)
				select distinct ItemNumber
				from OrderItem
				order by ItemNumber
			");

			AddColumn("dbo.OrderItem", "ItemID", c => c.Int(nullable: false));

			// update the OrderItem.ItemID column
			Sql(@"
				update oi
				set oi.ItemID = i.ItemID
				from OrderItem oi
				join Item i on i.ItemNumber = oi.ItemNumber
			");
	
			AddForeignKey("dbo.OrderItem", "ItemID", "dbo.Item", "ItemID", cascadeDelete: true);
			CreateIndex("dbo.OrderItem", "ItemID");
			DropColumn("dbo.OrderItem", "ItemNumber");
		}

		public override void Down()
		{
			AddColumn("dbo.OrderItem", "ItemNumber", c => c.String(nullable: false, maxLength: 100));

			// update the OrderItem.ItemNumber column
			Sql(@"
				update oi
				set oi.ItemNumber = i.ItemNumber
				from OrderItem oi
				join Item i on i.ItemID = oi.ItemID
			");

			DropIndex("dbo.OrderItem", new[] { "ItemID" });
			DropForeignKey("dbo.OrderItem", "ItemID", "dbo.Item");
			DropColumn("dbo.OrderItem", "ItemID");
			DropIndex("dbo.Item", new[] { "ItemNumber" });
			DropTable("dbo.Item");
		}
	}
}
