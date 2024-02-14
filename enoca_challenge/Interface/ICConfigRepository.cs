using enoca_challenge.Models;
using System.ComponentModel;

namespace enoca_challenge.Interface
{
	public interface ICConfigRepository
	{
		ICollection<CarrierConfigurations> GetCarrierConfigurations();
		CarrierConfigurations GetCarrierConfigurations(int id);
		bool CarrierConfigurationExists(int id);

		bool AddCarrierConfiguration(CarrierConfigurations carrierConfiguration);
		bool UpdateCarrierConfiguration(CarrierConfigurations carrierConfiguration);
		bool DeleteCarrierConfiguration(CarrierConfigurations carrierConfigurations);
		bool Save();
	}
}
