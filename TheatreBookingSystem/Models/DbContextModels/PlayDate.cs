using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TheatreBookingSystem.Models
{
	[Table("Dates")]
	public class PlayDate
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int DateID { get; set; }
		public DateTime PerformanceDate { get; set; }
		public virtual Play Play { get; set; }
		public virtual List<Order> Orders { get; set; }
	}
}