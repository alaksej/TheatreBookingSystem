using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace TheatreBookingSystem.Models
{
	public class Login
	{
		public int LoginID { get; set; }
		public string Name { get; set; }
		public string Password { get; set; }
		public Role Role { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
	}

}