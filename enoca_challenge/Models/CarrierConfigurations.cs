namespace enoca_challenge.Models
{
	public class CarrierConfigurations
	{
		public int CarrierConfigurationId { get; set; }
		public int CarrierMaxDesi { get; set; }
		public int CarrierMinDesi { get; set; }
		public float CarrierCost { get; set; }

		public Carriers Carriers { get; set; }
	}
}
