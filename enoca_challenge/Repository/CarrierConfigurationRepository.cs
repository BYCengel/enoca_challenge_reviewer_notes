using enoca_challenge.Data;
using enoca_challenge.Interface;
using enoca_challenge.Models;
using Microsoft.Extensions.FileSystemGlobbing.Internal.PatternContexts;
using System.Threading;

namespace enoca_challenge.Repository
{
	public class CarrierConfigurationRepository : ICConfigRepository
	{
		private DataBContext _context;
        public CarrierConfigurationRepository(DataBContext context)
        {
			_context = context;
        }

		

		public bool CarrierConfigurationExists(int id)
		{
			return _context.CarrierConfigurations.Any(cc => cc.CarrierConfigurationId == id);
		}

		public ICollection<CarrierConfigurations> GetCarrierConfigurations()
		{
			return _context.CarrierConfigurations.ToList();
		}

		public CarrierConfigurations GetCarrierConfigurations(int id)
		{
			return _context.CarrierConfigurations.Where(cc => cc.CarrierConfigurationId == id).FirstOrDefault();
		}

		public bool AddCarrierConfiguration(CarrierConfigurations carrierConfiguration)
		{
			_context.Add(carrierConfiguration);
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

		public bool UpdateCarrierConfiguration(CarrierConfigurations carrierConfiguration)
		{
			_context.Update(carrierConfiguration);
			return Save();
		}

		public bool DeleteCarrierConfiguration(CarrierConfigurations carrierConfigurations)
		{
			_context.Remove(carrierConfigurations);
			return Save();
		}
	}
}
