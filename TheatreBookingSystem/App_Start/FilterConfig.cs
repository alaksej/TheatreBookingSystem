using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheatreBookingSystem.Filters;

namespace TheatreBookingSystem.App_Start
{
	public class FilterConfig
	{
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new RequreSecureConnectionFilter());
			//filters.Add(new RequireHttpsAttribute());
			filters.Add(new CultureAttribute());
		}
	}
}