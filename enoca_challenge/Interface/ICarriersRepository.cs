using enoca_challenge.Models;
using System.Numerics;

namespace enoca_challenge.Interface
{
	public interface ICarriersRepository
	{
		ICollection<Carriers> GetCarriers();
		Carriers GetCarriers(int id);
		Carriers GetCarriers(string name);
		bool CarrierExists(int carrierId);
		Carriers GetCarrierOfAnOrder(int orderId); //id'si girilen order'ın carrier'ını döner
		Carriers GetCarrierOfAConfiguration(int configurationId); //id'si girilen configuration'ın carrier'ını döner.
		ICollection<Orders> GetOrdersFromACarrier(int carrierId); //id'si girilen carrier'daki orderları döner.
		ICollection<CarrierConfigurations> GetConfigsFromACarrier(int carrierId); //id'si girilen carrier'daki configurationları döner.

		bool AddCarrier(Carriers carriers);
		bool UpdateCarrier(Carriers carriers);	
		bool DeleteCarrier(Carriers carriers);
		bool Save();
	}
}
