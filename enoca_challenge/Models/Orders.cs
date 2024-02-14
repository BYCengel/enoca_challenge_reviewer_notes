namespace enoca_challenge.Models
{
	public class Orders
	{
		public int OrderId { get; set; }
		public int OrderDesi { get; set; }
		public DateTime OrderDate { get; set; }
		public float OrderCarrierCost { get; set; }

		public Carriers Carriers { get; set; }
	}
}
