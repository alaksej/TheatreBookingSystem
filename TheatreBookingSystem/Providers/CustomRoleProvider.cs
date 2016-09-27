using System;
using System.Linq;
using System.Web.Security;
using TheatreBookingSystem.Models;

namespace TheatreBookingSystem.Providers
{
	public class CustomRoleProvider : RoleProvider
	{
		public override string[] GetRolesForUser(string name)
		{
			string[] role = new string[] { };
			using (TheatreContext db = new TheatreContext())
			{
				try
				{
					Login user = (from u in db.Logins
								  where u.Name == name
								  select u).FirstOrDefault();
					if (user != null)
					{
						role = new string[] { user.Role.ToString() };
					}
				}
				catch
				{
					role = new string[] { };
				}
			}
			return role;
		}

		public override bool IsUserInRole(string username, string roleName)
		{
			bool outputResult = false;

			using (TheatreContext db = new TheatreContext())
			{
				try
				{
					Login user = (from u in db.Logins
								  where u.Name == username
								  select u).FirstOrDefault();
					if (user != null &&	user.Role.ToString() == roleName)
					{
						outputResult = true;
					}
				}
				catch
				{
					outputResult = false;
				}
			}
			return outputResult;
		}

		#region NotImplementedMembers
		public override void CreateRole(string roleName)
		{

		}

		public override void AddUsersToRoles(string[] usernames, string[] roleNames)
		{
			throw new NotImplementedException();
		}

		public override string ApplicationName
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
		{
			throw new NotImplementedException();
		}

		public override string[] FindUsersInRole(string roleName, string usernameToMatch)
		{
			throw new NotImplementedException();
		}

		public override string[] GetAllRoles()
		{
			throw new NotImplementedException();
		}

		public override string[] GetUsersInRole(string roleName)
		{
			throw new NotImplementedException();
		}

		public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
		{
			throw new NotImplementedException();
		}

		public override bool RoleExists(string roleName)
		{
			throw new NotImplementedException();
		}
		#endregion
	}
}