using System.ComponentModel.DataAnnotations;

namespace enoca_challenge.Dto
{
	public class CarrierConfiguration_Dto
	{
		[Required]
		public int CarrierMaxDesi { get; set; }
		[Required]
		public int CarrierMinDesi { get; set; }
		[Required]
		public float CarrierCost { get; set; }
	}
}
