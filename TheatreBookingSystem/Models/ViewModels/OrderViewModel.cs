using System;
using System.ComponentModel.DataAnnotations;

namespace TheatreBookingSystem.Models
{
	public class OrderViewModel : Category
	{
		[ScaffoldColumn(false)]
		public int OrderID { get; set; }

		[ScaffoldColumn(false)]
		public int DateID { get; set; }

		[Display(Name = "UserName", ResourceType = typeof(Resources.Resource))]
		public string UserName { get; set; }

		[Display(Name = "Play", ResourceType = typeof(Resources.Resource))]
		public string PlayName { get; set; }

		[Display(Name = "Date", ResourceType = typeof(Resources.Resource))]
		[DisplayFormat(DataFormatString ="{0:d}")]
		public DateTime Date { get; set; }

		[Display(Name = "Available", ResourceType = typeof(Resources.Resource))]
		public int Available { get; set; }

		[Display(Name = "Paid", ResourceType = typeof(Resources.Resource))]
		public int Paid { get; set; }

		[Display(Name = "Ordered", ResourceType = typeof(Resources.Resource))]
		public int Ordered { get; set; }

		[RegularExpression("\\d+", ErrorMessageResourceName ="QuantityRegExError", 
			ErrorMessageResourceType = typeof(Resources.Resource))]
		[Display(Name = "Quantity", ResourceType = typeof(Resources.Resource))]
		public int Quantity { get; set; }

		[Display(Name = "IsPaid", ResourceType = typeof(Resources.Resource))]
		public bool IsPaid { get; set; }

		[Display(Name = "Bill", ResourceType = typeof(Resources.Resource))]
		[DisplayFormat(DataFormatString = "{0:N0}")]
		public decimal Bill { get; set; }

		[ScaffoldColumn(false)]
		public Login User { get; set; }

		[ScaffoldColumn(false)]
		public Category Category
		{
			set
			{
				if (value != null)
				{
					CategoryID = value.CategoryID;
					CategoryName = value.CategoryName;
					TicketPrice = value.TicketPrice;
					TotalTickets = value.TotalTickets;
				}
			}
		}

		[ScaffoldColumn(false)]
		public Order Order
		{
			set
			{
				if (value != null)
				{
					DateID = value.Date.DateID;
					OrderID = value.OrderID;
					CategoryID = value.CategoryID;
					UserName = value.Login.Name;
					User = value.Login;
					PlayName = value.Date.Play.Name;
					Date = value.Date.PerformanceDate;
					Quantity = value.Quantity;
					IsPaid = value.IsPaid;
				}
			}
		}

		public OrderViewModel() { }

		public OrderViewModel(Category category)
		{
			CategoryID = category.CategoryID;
			CategoryName = category.CategoryName;
			TotalTickets = category.TotalTickets;
			TicketPrice = category.TicketPrice;
		}

		public OrderViewModel(Order order)
		{
			DateID = order.Date.DateID;
			OrderID = order.OrderID;
			CategoryID = order.CategoryID;
			UserName = order.Login.Name;
			User = order.Login;
			PlayName = order.Date.Play.Name;
			Date = order.Date.PerformanceDate;
			Quantity = order.Quantity;
			IsPaid = order.IsPaid;
		}

	}
}