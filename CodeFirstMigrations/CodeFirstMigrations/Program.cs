using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstMigrations
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var context = new Context();
			var orders = context.Orders
				.Include(o => o.Items.Select(oi => oi.Item))
				.ToList();

			foreach (var order in orders)
				Console.WriteLine(order.ToString());

			Console.WriteLine("Press Enter to continue");
			Console.Read();
		}
	}

	public class Item
	{
		public int ItemID { get; set; }
		public string ItemNumber { get; set; }
	}
	public class Order
	{
		public int OrderID { get; set; }
		public DateTime OrderedOn { get; set; }
		public List<OrderItem> Items { get; set; }

		public Order()
		{
			this.Items = new List<OrderItem>();
		}

		public override string ToString()
		{
			var sb = new StringBuilder();

			sb.AppendFormat("OrderID: {0}\n", this.OrderID);
			sb.AppendFormat("OrderedOn: {0}\n", this.OrderedOn);

			foreach (var orderItem in Items)
				sb.AppendFormat("OrderItemID: {0}, ItemNumber: {1}, Quantity: {2}, Price: {3:c2}\n",
					orderItem.OrderItemID, orderItem.Item.ItemNumber, orderItem.Quantity, orderItem.Price);

			sb.AppendLine();

			return sb.ToString();
		}
	}
	public class OrderItem
	{
		public int OrderItemID { get; set; }
		public int OrderID { get; set; }
		public Order Order { get; set; }
		public int ItemID { get; set; }
		public Item Item { get; set; }
		public int Quantity { get; set; }
		public decimal Price { get; set; }
	}

	public class Context : DbContext
	{
		public DbSet<Order> Orders { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
		}
	}
}
