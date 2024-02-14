using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace enoca_challenge.Dto
{
	public class CarriersDto
	{
		public int CarrierId { get; set; }
		public string CarrierName { get; set; }
		public bool CarrierIsActive { get; set; }
		public int CarrierPlusDesiCost { get; set; }
	}
}
