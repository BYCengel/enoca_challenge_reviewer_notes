using enoca_challenge.Data;
using enoca_challenge.Interface;
using enoca_challenge.Models;

namespace enoca_challenge.Repository
{
	public class OrdersRepository : IOrdersRepository
	{
		private readonly DataBContext _context;
		public OrdersRepository(DataBContext context)
		{
			_context = context;
		}


		public ICollection<Orders> GetOrders()
		{
			return _context.Orders.ToList();
		}

		public Orders GetOrders(int id)
		{
			return _context.Orders.Where(o => o.OrderId == id).FirstOrDefault();
		}

		public bool OrderExists(int id)
		{
			return _context.Orders.Any(o => o.OrderId == id);
		}

		public bool AddOrder(Orders order)
		{
			_context.Add(order);
			return Save();
		}
		public bool Save()
		{
			var saved = _context.SaveChanges();
			if (saved > 0)
				return true;
			else
				return false;
		}

		public bool DeleteOrder(Orders order)
		{
			_context.Remove(order);
			return Save();
		}
	}
}
