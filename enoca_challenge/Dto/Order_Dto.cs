using System.ComponentModel.DataAnnotations;

namespace enoca_challenge.Dto
{
	public class Order_Dto
	{
		[Required]
		public int OrderDesi { get; set; }
		[Required]
		public DateTime OrderDate { get; set; }
	}
}
