using enoca_challenge.Data;
using enoca_challenge.Interface;
using enoca_challenge.Models;

namespace enoca_challenge.Repository
{
	public class CarriersRepository : ICarriersRepository
	{
		private readonly DataBContext _context;
		public CarriersRepository(DataBContext context)
		{ 
			_context = context;
		}

		public bool CarrierExists(int carrierId)
		{
			return _context.Carriers.Any(c=> c.CarrierId == carrierId);
		}

		public ICollection<Carriers> GetCarriers() 
		{
			return _context.Carriers.OrderBy(c=>c.CarrierId).ToList();
		}

		public Carriers GetCarriers(int id)
		{
			return _context.Carriers.Where(c => c.CarrierId == id).FirstOrDefault();
		}

		public Carriers GetCarriers(string name)
		{
			return _context.Carriers.Where(c => c.CarrierName == name).FirstOrDefault();
		}
		public Carriers GetCarrierOfAnOrder(int orderId)
		{
			return _context.Orders.Where(o => o.OrderId == orderId).Select(c => c.Carriers).FirstOrDefault();
		}
		public Carriers GetCarrierOfAConfiguration(int configurationId)
		{
			return _context.CarrierConfigurations.Where(o => o.CarrierConfigurationId == configurationId).Select(c => c.Carriers).FirstOrDefault();
		}

		public ICollection<CarrierConfigurations> GetConfigsFromACarrier(int carrierId)
		{
			return _context.CarrierConfigurations.Where(c => c.Carriers.CarrierId == carrierId).ToList();
		}

		public ICollection<Orders> GetOrdersFromACarrier(int carrierId)
		{
			return _context.Orders.Where(c => c.Carriers.CarrierId == carrierId).ToList();
		}

		public bool AddCarrier(Carriers carriers)
		{
			_context.Add(carriers);

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

		public bool UpdateCarrier(Carriers carriers)
		{
			_context.Update(carriers);
			return Save();
		}

		public bool DeleteCarrier(Carriers carriers)
		{
			_context.Remove(carriers);
			return Save();
		}
	}
}
