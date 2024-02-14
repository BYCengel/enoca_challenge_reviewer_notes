namespace enoca_challenge.Models
{
	public class Carriers
	{
		public int CarrierId { get; set; }
		public string CarrierName { get; set; }
		public bool CarrierIsActive { get; set; }
		public int CarrierPlusDesiCost { get; set; }

		public ICollection<Orders> Orders { get; set; }
		public ICollection<CarrierConfigurations> CarrierConfigurations { get; set; }
	}
}
