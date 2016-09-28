using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TheatreBookingSystem.Models;

namespace TheatreBookingSystem
{
	public class TheatreContextService
	{
		private TheatreContext db;

		public TheatreContextService(TheatreContext context)
		{
			db = context;
		}

		public int GetAllQuantities(int categoryId, int dateId)
		{
			return (from o in db.Orders
					where o.CategoryID == categoryId &&
					o.Date.DateID == dateId
					select o.Quantity).ToList().Sum();
		}

		public int GetUserQuantities(int categoryId, int dateId, string userName)
		{
			return (from o in db.Orders
					where o.CategoryID == categoryId &&
					o.Date.DateID == dateId &&
					o.Login.Name == userName
					select o.Quantity).ToList().Sum();
		}

		//public bool GetOrderIsPaid(int categoryId, int dateId, string userName)
		//{
		//	return (from o in db.Orders
		//			where o.CategoryID == categoryId &&
		//			o.Date.DateID == dateId &&
		//			o.Login.Name == userName
		//			select o.IsPaid).FirstOrDefault();
		//}

		public Login GetUser(string userName)
		{
			return db.Logins.FirstOrDefault(u => u.Name == userName);
		}

		public Order GetOrder(int categoryId, Login user, PlayDate date)
		{
			return (from o in db.Orders
					where o.CategoryID == categoryId &&
				   o.Login.LoginID == user.LoginID &&
				   o.Date.DateID == date.DateID
					select o).FirstOrDefault();
		}

		public Order GetOrder(int categoryId, string userName, PlayDate date)
		{
			return (from o in db.Orders
					where o.CategoryID == categoryId &&
				    o.Login.Name == userName &&
				    o.Date.DateID == date.DateID
					select o).FirstOrDefault();
		}

		public Order CreateOrder(int categoryId, Login user, PlayDate date, int quantity)
		{
			var order = new Order
			{
				CategoryID = categoryId,
				Login = user,
				Date = date,
				Quantity = quantity
			};
			db.Orders.Add(order);
			db.SaveChanges();
			return order;
		}

		public void UpdateOrder(Order order)
		{
			db.Entry(order).State = EntityState.Modified;
			db.SaveChanges();
		}

		public PlayDate GetPlayDate(int dateId)
		{
			return db.Dates.FirstOrDefault(d => d.DateID == dateId);
		}

		public Author GetAuthor(string name)
		{
			return (from a in db.Authors
					where a.Name == name
					select a).FirstOrDefault();
		}

		public void CreateAuthor(Author author)
		{
			db.Authors.Add(author);
			db.SaveChanges();
		}

		public Genre GetGenre(string name)
		{
			return (from g in db.Genres
					where g.Name == name
					select g).FirstOrDefault();
		}

		public void CreateGenre(Genre Genre)
		{
			db.Genres.Add(Genre);
			db.SaveChanges();
		}

		public Play GetPlay(string playName, string authorName)
		{
			return (from p in db.Plays
					where p.Name == playName &&
					p.Author.Name == authorName
					select p).FirstOrDefault();
		}

		public void CreatePlay(Play Play)
		{
			db.Plays.Add(Play);
			db.SaveChanges();
		}

		public void CreateDateWithIdExplicit(PlayDate date)
		{
			using (var transaction = db.Database.BeginTransaction())
			{
				//db.Database.ExecuteSqlCommand(@"SET IDENTITY_INSERT [dbo].[Dates] ON");
				db.Dates.Add(date);
				db.SaveChanges();
				//db.Database.ExecuteSqlCommand(@"SET IDENTITY_INSERT [dbo].[Dates] OFF");
				transaction.Commit();
			}
		}

		public void DeleteOrder(Order order)
		{
			db.Orders.Remove(order);
			db.SaveChanges();
		}

		public Order GetUnpaidOrder(int categoryId, Login user, PlayDate date)
		{
			return (from o in db.Orders
					where o.CategoryID == categoryId &&
				   o.Login.LoginID == user.LoginID &&
				   o.Date.DateID == date.DateID &&
				   o.IsPaid == false
					select o).FirstOrDefault();
		}

		public Order GetUnpaidOrder(int orderId)
		{
			return (from o in db.Orders
					where o.OrderID == orderId &&
				   o.IsPaid == false
					select o).FirstOrDefault();
		}

		public IList<Order> GetAllOrders()
		{
			return db.Orders.ToList();
		}

		public IList<Order> GetUnpaidOrders()
		{
			return db.Orders.Where(o => !(o.IsPaid)).ToList();
		}


		public Order GetOrder(int orderId)
		{
			return db.Orders.FirstOrDefault(o => o.OrderID == orderId);
		}

		public IList<Order> GetUserOrders(string userName)
		{
			return (from o in db.Orders
					where o.Login.Name == userName
					select o).ToList();
		}

		public void Dispose()
		{
			db.Dispose();
		}
	}
}