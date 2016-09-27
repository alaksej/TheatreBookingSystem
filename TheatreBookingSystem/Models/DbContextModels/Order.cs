using System.ComponentModel.DataAnnotations;

namespace TheatreBookingSystem.Models
{
	public class Order
	{
		[ScaffoldColumn(false)]
		public int OrderID { get; set; }

		[Display(Name = "Date", ResourceType = typeof(Resources.Resource))]
		public virtual PlayDate Date { get; set; }

		[Display(Name = "UserName", ResourceType = typeof(Resources.Resource))]
		public virtual Login Login { get; set; }

		public int CategoryID { get; set; }

		[Display(Name = "Quantity", ResourceType = typeof(Resources.Resource))]
		public int Quantity { get; set; }

		[Display(Name = "IsPaid", ResourceType = typeof(Resources.Resource))]
		public bool IsPaid { get; set; }
	}
}