using System.Web.Mvc;
using System.Web.Security;
using TheatreBookingSystem.Models;
using TheatreBookingSystem.Providers;
using TheatreBookingSystem.Filters;

namespace TheatreBookingSystem.Controllers
{
	public class AccountController : Controller
    {
		public ActionResult Login(string returnUrl)
		{
			ViewBag.ReturnUrl = returnUrl;
			return View();
		}

		[HttpPost]
		public ActionResult Login(LoginViewModel model, string returnUrl)
		{
			if (ModelState.IsValid)
			{
				var provider = new CustomMembershipProvider();
				if (provider.ValidateUser(model.Name, model.Password))
				{
					FormsAuthentication.SetAuthCookie(model.Name, model.RememberMe);
					if (Url.IsLocalUrl(returnUrl))
					{
						return Redirect(returnUrl);
					}
					else
					{
						return RedirectToAction("Index", "Home");
					}
				}
				else
				{
					ModelState.AddModelError("", Resources.Resource.LoginIncorrectMessage);
				}
			}
			return View(model);
		}

		public ActionResult _LoginPartial()
		{
			return PartialView(HttpContext.User);
		}

		public ActionResult LogOut()
		{
			FormsAuthentication.SignOut();

			return RedirectToAction("Index", "Home");
		}

		public ActionResult Register()
		{
			return View();
		}

		[HttpPost]
		public ActionResult Register(RegisterViewModel model)
		{
			if (ModelState.IsValid)
			{
				var provider = new CustomMembershipProvider();
				MembershipUser membershipUser = provider.
					CreateUser(model.Name, model.Password, model.Email, model.Phone);

				if (membershipUser != null)
				{
					FormsAuthentication.SetAuthCookie(model.Name, false);
					return RedirectToAction("Index", "Home");
				}
				else
				{
					ModelState.AddModelError("", Resources.Resource.RegistrationError);
				}
			}
			return View(model);
		}
	}
}