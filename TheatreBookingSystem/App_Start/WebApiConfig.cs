using System.Web.Http;

namespace TheatreBookingSystem.App_Start
{
	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			// other configurations...

			// make all web-api requests to be sent over https
			config.MessageHandlers.Add(new EnforceHttpsHandler());
		}
	}
}