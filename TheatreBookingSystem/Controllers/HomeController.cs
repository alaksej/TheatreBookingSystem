using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using TheatreBookingSystem.Models;
using TheatreBookingSystem.Filters;

namespace TheatreBookingSystem.Controllers
{
	public class HomeController : Controller
	{
		private TheatreContextService dbService =
			new TheatreContextService(new TheatreContext());
		private readonly List<Play> repertoire = XmlLoader.GetRepertoire(
			HostingEnvironment.ApplicationPhysicalPath +
			"App_Data\\Repertoire.xml");
		private readonly List<Category> categories = XmlLoader.GetCategories(
			HostingEnvironment.ApplicationPhysicalPath +
			"App_Data\\Categories.xml");
		private ILog log = LogManager.GetLogger("HomeController");


		// GET: Home
		public ActionResult Index()
		{
			return View(repertoire);
		}

		public ActionResult Details(int? id)
		{
			if (id == null)
			{
				log.Debug("'id' parameter passed to Details method is null");
				return HttpNotFound();
			}
			var play = repertoire.Find(p => p.PlayID == id);
			return View(play);
		}

		[HttpGet]
		public ActionResult Order(int? id)
		{
			if (id == null)
			{
				log.Debug("'id' parameter passed to Order (Get) method is null");
				return HttpNotFound();
			}
			PlayDate date = GetDate((int)id);
			if (date == null)
			{
				log.Debug("Date not found for id = " + id);
				return HttpNotFound();
			}

			ViewBag.Date = date;
			ViewBag.Play = date.Play;
			string userName = HttpContext.User.Identity.Name;
			var orderViewModels = GetOrderViewModels(categories, userName, date);
			var ovmList = new OrderViewModelList(orderViewModels);
			return View(ovmList);
		}

		private PlayDate GetDate(int dateId)
		{
			PlayDate date = null;
			foreach (var play in repertoire)
			{
				foreach (var d in play.Dates)
				{
					if (d.DateID == dateId)
					{
						date = d;
						break;
					}
				}
				if (date != null)
					break;
			}
			return date;
		}

		private IList<OrderViewModel> GetOrderViewModels
			(IEnumerable<Category> categories, string userName, PlayDate date)
		{
			var list = new List<OrderViewModel>();
			foreach (var c in categories)
			{
				var ovm = new OrderViewModel(c);
				int quantByAllUsers = dbService.GetAllQuantities(c.CategoryID, date.DateID);
				ovm.Available = c.TotalTickets - quantByAllUsers;
				if (!String.IsNullOrEmpty(userName))
				{
					int quantByUser = dbService.GetUserQuantities(c.CategoryID, date.DateID, userName);
					Order order = null;
					order = dbService.GetOrder(c.CategoryID, userName, date);
					if (order != null)
					{
						ovm.Order = order;
						if (!order.IsPaid)
							ovm.Ordered = quantByUser;
						else
							ovm.Paid = quantByUser;
					}
				}
				list.Add(ovm);
			}
			return list;
		}

		private List<OrderViewModel> GetOrderViewModels(IEnumerable<Order> orders)
		{
			var list = new List<OrderViewModel>();
			foreach (var order in orders)
			{
				var ovm = new OrderViewModel(order);
				ovm.Category = GetCategory(order.CategoryID);
				ovm.Bill = ovm.TicketPrice * order.Quantity;
				if (ovm.IsPaid)
					ovm.Paid = order.Quantity;
				else
					ovm.Ordered = order.Quantity;
				list.Add(ovm);
			}
			return list;
		}

		[HttpPost]
		[Authorize]
		public ActionResult Order(OrderViewModelList ovmList, int? id,
			string submitOrder, string cancelOrder)
		{
			if (id == null)
			{
				log.Debug("'id' parameter passed to Order (Post) method is null");
				return HttpNotFound();
			}

			PlayDate date = GetDate((int)id);
			if (date == null)
			{
				log.DebugFormat("Date not found for id = {0}", id);
				return HttpNotFound();
			}
			ViewBag.Date = date;


			string returnUrl = Request.UrlReferrer.AbsolutePath;

			string userName = HttpContext.User.Identity.Name;
			Login user = dbService.GetUser(userName);
			if (user == null)
			{
				log.DebugFormat("User {0} not found in database.", userName);
				return HttpNotFound();
			}

			if (submitOrder != null)
			{
				if (date.PerformanceDate < DateTime.Now)
					return Content(Resources.Resource.UnableToOrderPast);
				return SubmitOrders(ovmList.OrderViewModels, user, date);
			}
			else if (cancelOrder != null)
			{
				return CancelOrders(ovmList.OrderViewModels, user, returnUrl);
			}
			return View();
		}

		private ActionResult SubmitOrders(IEnumerable<OrderViewModel> ovms, Login user, PlayDate date)
		{
			List<OrderViewModel> lineItemsPlaced = new List<OrderViewModel>();
			foreach (var ovm in ovms)
			{
				if (ovm.Quantity > 0 && ovm.Quantity <= ovm.Available)
				{
					var order = dbService.GetOrder(ovm.CategoryID, user, date);
					if (order == null)
					{
						date = BindDateToDb(date);
						order = dbService.CreateOrder(ovm.CategoryID, user, date, ovm.Quantity);
					}
					else
					{
						order.Quantity += ovm.Quantity;
						dbService.UpdateOrder(order);
					}

					ovm.Category = GetCategory(ovm.CategoryID);
					ovm.Date = order.Date.PerformanceDate;
					ovm.PlayName = order.Date.Play.Name;

					lineItemsPlaced.Add(ovm);
				}
			}
			if (lineItemsPlaced.Count() == 0)
				return Content(Resources.Resource.UnableToPlaceOrder);
			return View("OrderNotification", lineItemsPlaced);
		}

		private Category GetCategory(int categoryId)
		{
			return (from c in categories
					where c.CategoryID == categoryId
					select c).FirstOrDefault();
		}


		private PlayDate BindDateToDb(PlayDate date)
		{
			var dateEntry = dbService.GetPlayDate(date.DateID);
			if (dateEntry != null)
				return dateEntry;

			BindAuthorToDb(date);
			BindGenreToDb(date);
			BindPlayToDb(date);

			dbService.CreateDateWithIdExplicit(date);

			return date;
		}

		private void BindAuthorToDb(PlayDate date)
		{
			var authorEntry = dbService.GetAuthor(date.Play.Author.Name);
			if (authorEntry == null)
			{
				dbService.CreateAuthor(date.Play.Author);
			}
			else
			{
				date.Play.Author = authorEntry;
			}
		}

		private void BindGenreToDb(PlayDate date)
		{
			var genreEntry = dbService.GetGenre(date.Play.Genre.Name);
			if (genreEntry == null)
			{
				dbService.CreateGenre(date.Play.Genre);
			}
			else
			{
				date.Play.Genre = genreEntry;
			}
		}

		private void BindPlayToDb(PlayDate date)
		{
			var playEntry = dbService.GetPlay(date.Play.Name, date.Play.Author.Name);
			if (playEntry == null)
				dbService.CreatePlay(date.Play);
			else
				date.Play = playEntry;
		}

		private ActionResult CancelOrders(IEnumerable<OrderViewModel> ovms, Login user, string returnUrl)
		{
			List<OrderViewModel> itemsCancelled = new List<OrderViewModel>();
			foreach (var ovm in ovms)
			{
				if (ovm.Quantity > 0)
				{
					var order = dbService.GetUnpaidOrder(ovm.OrderID);
					if (order == null || ovm.Quantity > order.Quantity || order.Login != user)
						continue;
					ovm.Category = GetCategory(order.CategoryID);
					ovm.Date = order.Date.PerformanceDate;
					ovm.PlayName = order.Date.Play.Name;
					if (ovm.Quantity == order.Quantity)
					{
						dbService.DeleteOrder(order);
					}
					else
					{
						order.Quantity -= ovm.Quantity;
						dbService.UpdateOrder(order);
					}
					itemsCancelled.Add(ovm);
				}
			}
			if (itemsCancelled.Count() == 0)
				return Content(Resources.Resource.UnableToCancelOrder);
			ViewBag.ReturnUrl = returnUrl;
			return View("CancelOrderNotification", itemsCancelled);
		}

		[HttpGet]
		[Authorize(Roles = "DeliveryMan, Admin")]
		public ActionResult ManagePayments()
		{
			var orders = dbService.GetAllOrders();
			var ovms = GetOrderViewModels(orders);
			return View(ovms);
		}


		[HttpPost]
		[Authorize(Roles = "DeliveryMan, Admin")]
		public ActionResult ManagePayments(IList<OrderViewModel> ovms)
		{
			foreach (var ovm in ovms)
			{
				var order = dbService.GetOrder(ovm.OrderID);
				if (order == null)
				{
					var message = String.Format(
						"Could not get Order from database. OrderId: {0}", ovm.OrderID);
					log.Warn(message);
					return Content(message);
				}
				if (order.IsPaid ^ ovm.IsPaid)
				{
					order.IsPaid = ovm.IsPaid;
					dbService.UpdateOrder(order);
				}
			}
			return RedirectToAction("ManagePayments");
		}

		[Authorize(Roles = "DeliveryMan, Admin")]
		public ActionResult ExportOrdersToXml()
		{
			var unpaidOrders = dbService.GetUnpaidOrders();
			var ovms = GetOrderViewModels(unpaidOrders);
			var filename = Server.MapPath("~/App_Data/Export/Unpaid_orders.xml");
			XmlExporter.ExportOrders(filename, ovms);
			return File(filename, "application/xml");
		}

		public ActionResult ChangeCulture(string lang)
		{
			string returnUrl = Request.UrlReferrer.AbsolutePath;
			List<string> cultures = new List<string>() { "be", "en", "ru" };
			if (!cultures.Contains(lang))
			{
				lang = "en";
			}

			HttpCookie cookie = Request.Cookies["lang"];
			if (cookie != null)
				cookie.Value = lang;
			else
			{
				cookie = new HttpCookie("lang");
				cookie.HttpOnly = false;
				cookie.Value = lang;
				cookie.Expires = DateTime.Now.AddYears(1);
			}
			Response.Cookies.Add(cookie);
			return Redirect(returnUrl);
		}

		[Authorize]
		[HttpGet]
		public ActionResult MyOrders()
		{
			string userName = HttpContext.User.Identity.Name;
			ViewBag.UserName = userName;
			IList<Order> userOrders = dbService.GetUserOrders(userName);
			var ovms = GetOrderViewModels(userOrders);
			var ovmList = new OrderViewModelList(ovms);
			return View(ovmList);
		}

		[Authorize]
		[HttpPost]
		public ActionResult MyOrders(OrderViewModelList ovmList)
		{
			string returnUrl = Request.UrlReferrer.AbsolutePath;
			Login user = dbService.GetUser(HttpContext.User.Identity.Name);
			return CancelOrders(ovmList.OrderViewModels, user, returnUrl);
		}

		protected override void Dispose(bool disposing)
		{
			dbService.Dispose();
			base.Dispose(disposing);
		}

	}


}