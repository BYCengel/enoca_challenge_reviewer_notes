using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace enoca_challenge.Dto
{
	public class Carrier_Dto
	{
		[Required]
		public string CarrierName { get; set; }
		[Required]
		public bool CarrierIsActive { get; set; }
		[Required]
		public int CarrierPlusDesiCost { get; set; }
	}
}
