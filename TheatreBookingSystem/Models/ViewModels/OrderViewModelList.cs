using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheatreBookingSystem.Models
{
	public class OrderViewModelList
	{
		private decimal totalOrdered;
		private decimal totalPaid;

		public IList<OrderViewModel> OrderViewModels { get; set; }

		public decimal TotalOrdered
		{
			get
			{
				totalOrdered = 0M;
				foreach (var item in OrderViewModels)
				{
					totalOrdered += item.TicketPrice * item.Ordered;
				}
				return totalOrdered;
			}
			set
			{
				totalOrdered = value;
			}
		}

		public decimal TotalPaid
		{
			get
			{
				totalPaid = 0M;
				foreach (var item in OrderViewModels)
				{
					totalPaid += item.TicketPrice * item.Paid;
				}
				return totalPaid;
			}
			set
			{
				totalPaid = value;
			}
		}

		public OrderViewModelList()
		{
			OrderViewModels = new List<OrderViewModel>();
		}

		public OrderViewModelList(IList<OrderViewModel> ovms)
		{
			OrderViewModels = ovms;
		}
	}
}