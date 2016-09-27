using System.ComponentModel.DataAnnotations;

namespace TheatreBookingSystem.Models
{
	public class Category
	{
		[ScaffoldColumn(false)]
		public int CategoryID { get; set; }

		[Display(Name = "Category", ResourceType = typeof(Resources.Resource))]
		public string CategoryName { get; set; }

		[Display(Name = "TotalTickets", ResourceType = typeof(Resources.Resource))]
		public int TotalTickets { get; set; }

		[Display(Name = "TicketPrice", ResourceType = typeof(Resources.Resource))]
		[DisplayFormat(DataFormatString = "{0:N0}")]
		public decimal TicketPrice { get; set; }
	}
}