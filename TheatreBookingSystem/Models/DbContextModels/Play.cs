using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace TheatreBookingSystem.Models
{
	public class Play
	{
		public Play()
		{
			Dates = new List<PlayDate>();
		}
		[HiddenInput(DisplayValue =false)]
		public int PlayID { get; set; }

		[Display(Name = "PlayName", ResourceType = typeof(Resources.Resource))]
		public string Name { get; set; }
		public virtual Author Author { get; set; }
		public virtual Genre Genre { get; set; }
		public virtual List<PlayDate> Dates { get; set; }
	}
}