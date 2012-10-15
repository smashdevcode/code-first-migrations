namespace CodeFirstMigrations.Migrations
{
	using System;
	using System.Data.Entity;
	using System.Data.Entity.Migrations;
	using System.Linq;

	internal sealed class Configuration : DbMigrationsConfiguration<CodeFirstMigrations.Context>
	{
		public Configuration()
		{
			AutomaticMigrationsEnabled = false;
		}

		protected override void Seed(CodeFirstMigrations.Context context)
		{
			var order1 = new Order()
			{
				OrderID = 1,
				OrderedOn = new DateTime(2012, 10, 1, 8, 0, 0)
			};
			order1.Items.Add(new OrderItem()
			{
				OrderItemID = 1,
				Order = order1,
				ItemNumber = "Item1",
				Quantity = 1,
				Price = 10M
			});

			var order2 = new Order()
			{
				OrderID = 2,
				OrderedOn = new DateTime(2012, 10, 2, 10, 0, 0)
			};
			order2.Items.Add(new OrderItem()
			{
				OrderItemID = 2,
				Order = order2,
				ItemNumber = "Item1",
				Quantity = 2,
				Price = 10M
			});
			order2.Items.Add(new OrderItem()
			{
				OrderItemID = 3,
				Order = order2,
				ItemNumber = "Item2",
				Quantity = 1,
				Price = 20.5M
			});

			context.Orders.AddOrUpdate(order1);
			context.Orders.AddOrUpdate(order2);
		}
	}
}
