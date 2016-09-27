using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;
using TheatreBookingSystem.Controllers;

namespace TheatreTests
{
	[TestClass]
	public class HomeControllerTests
	{
		[TestMethod]
		public void Index()
		{
			// Arrange
			HomeController controller = new HomeController();

			// Act
			ViewResult view = controller.Index() as ViewResult;

			// Assert
			Assert.IsNotNull(view);
		}

		[TestMethod]
		public void Details_WhenIdIsNull_ReturnsHttpNotFoundResult()
		{
			// Arrange
			HomeController controller = new HomeController();
			int? id = null;

			// Act
			HttpNotFoundResult result = controller.Details(id) as HttpNotFoundResult;

			// Assert
			Assert.IsInstanceOfType(result, typeof(HttpNotFoundResult));
		}

	}
}
