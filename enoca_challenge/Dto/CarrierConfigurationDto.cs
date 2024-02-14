using enoca_challenge.Models;

namespace enoca_challenge.Dto
{
	public class CarrierConfigurationDto
	{
		public int CarrierConfigurationId { get; set; }
		
		public int CarrierMaxDesi { get; set; }
		public int CarrierMinDesi { get; set; }
		public float CarrierCost { get; set; }

	}
}
