using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using TheatreBookingSystem;
using TheatreBookingSystem.Models;

namespace TheatreTests
{
	[TestClass]
	public class ContextServiceTests
	{
		[TestMethod]
		public void GetUser_gets_user_by_name()
		{
			var data = new List<Login>
			{
				new Login { Name = "user1" },
				new Login { Name = "user2" },
				new Login { Name = "deliveryman" }
			}.AsQueryable();

			var mockSet = new Mock<DbSet<Login>>();
			mockSet.As<IQueryable<Login>>().Setup(m => m.Provider).Returns(data.Provider);
			mockSet.As<IQueryable<Login>>().Setup(m => m.Expression).Returns(data.Expression);
			mockSet.As<IQueryable<Login>>().Setup(m => m.ElementType).Returns(data.ElementType);
			mockSet.As<IQueryable<Login>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

			var mockContext = new Mock<TheatreContext>();
			mockContext.Setup(c => c.Logins).Returns(mockSet.Object);

			var service = new TheatreContextService(mockContext.Object);
			var user = service.GetUser("user1");

			Assert.AreEqual("user1", user.Name);
		}

		[TestMethod]
		public void GetAllOrders_gets_a_list_of_orders()
		{
			var data = new List<Order>
			{
				new Order { Quantity = 4},
				new Order { Quantity = 5 },
				new Order { Quantity = 7 }
			}.AsQueryable();

			var mockSet = new Mock<DbSet<Order>>();
			mockSet.As<IQueryable<Order>>().Setup(m => m.Provider).Returns(data.Provider);
			mockSet.As<IQueryable<Order>>().Setup(m => m.Expression).Returns(data.Expression);
			mockSet.As<IQueryable<Order>>().Setup(m => m.ElementType).Returns(data.ElementType);
			mockSet.As<IQueryable<Order>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

			var mockContext = new Mock<TheatreContext>();
			mockContext.Setup(c => c.Orders).Returns(mockSet.Object);

			var service = new TheatreContextService(mockContext.Object);
			var orders = service.GetAllOrders();

			Assert.AreEqual(4, orders[0].Quantity);
			Assert.AreEqual(5, orders[1].Quantity);
			Assert.AreEqual(7, orders[2].Quantity);
		}

		[TestMethod]
		public void CreateGenre_saves_a_genre_via_context()
		{
			var mockSet = new Mock<DbSet<Genre>>();

			var mockContext = new Mock<TheatreContext>();
			mockContext.Setup(m => m.Genres).Returns(mockSet.Object);

			var service = new TheatreContextService(mockContext.Object);
			service.CreateGenre(new Genre { Name = "blockbuster" } );

			mockSet.Verify(m => m.Add(It.IsAny<Genre>()), Times.Once());
			mockContext.Verify(m => m.SaveChanges(), Times.Once());
		}
	}
}

