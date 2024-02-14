using enoca_challenge.Models;

namespace enoca_challenge.Interface
{
	public interface IOrdersRepository
	{
		ICollection<Orders> GetOrders();
		Orders GetOrders(int id);
		bool OrderExists(int id);

		bool AddOrder(Orders order);
		bool DeleteOrder(Orders order);
		bool Save();
	}
}
