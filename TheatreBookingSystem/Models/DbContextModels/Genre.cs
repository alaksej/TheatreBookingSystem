using System.ComponentModel.DataAnnotations;

namespace TheatreBookingSystem.Models
{
	public class Genre
	{
		public int GenreID { get; set; }
		[Display(Name = "GenreName", ResourceType = typeof(Resources.Resource))]
		public string Name { get; set; }
	}
}