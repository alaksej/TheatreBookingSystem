using System.ComponentModel.DataAnnotations;

namespace TheatreBookingSystem.Models
{
	public class Author
	{
		public int AuthorID { get; set; }
		[Display(Name = "AuthorName", ResourceType = typeof(Resources.Resource))]
		public string Name { get; set; }
	}
}