using System;
using System.Linq;
using System.Web.Security;
using System.Web.Helpers;
using TheatreBookingSystem.Models;

namespace TheatreBookingSystem.Providers
{
	public class CustomMembershipProvider : MembershipProvider
	{
		public override bool ValidateUser(string username, string password)
		{
			bool isValid = false;

			using (TheatreContext db = new TheatreContext())
			{
				try
				{
					Login user = (from u in db.Logins
								 where u.Name == username
								 select u).FirstOrDefault();

					if (user != null && Crypto.VerifyHashedPassword(user.Password, password))
					{
						isValid = true;
					}
				}
				catch
				{
					isValid = false;
				}
			}
			return isValid;
		}

		public MembershipUser CreateUser(string name, string password, string email, string phone)
		{
			MembershipUser membershipUser = GetUser(name, false);

			if (membershipUser == null)
			{
				try
				{
					using (TheatreContext db = new TheatreContext())
					{
						Login user = new Login();
						user.Name = name;
						user.Password = Crypto.HashPassword(password);
						user.Email = email;
						user.Phone = phone;
						user.Role = Role.User;

						db.Logins.Add(user);
						db.SaveChanges();
						membershipUser = GetUser(name, false);
						return membershipUser;
					}
				}
				catch
				{
					return null;
				}
			}
			return null;
		}

		public override MembershipUser GetUser(string name, bool userIsOnline)
		{
			try
			{
				using (TheatreContext db = new TheatreContext())
				{
					var users = from u in db.Logins
								where u.Name == name
								select u;
					if (users.Count() > 0)
					{
						Login user = users.First();
						MembershipUser memberUser = new MembershipUser("MyMembershipProvider",
							user.Name, null, user.Email, null, null, false, false, DateTime.MinValue, 
							DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue);
						return memberUser;
					}
				}
			}
			catch
			{
				return null;
			}
			return null;
		}

		#region NotImplementedMembers

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

		public override bool ChangePassword(string username, string oldPassword, string newPassword)
		{
			throw new NotImplementedException();
		}

		public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
		{
			throw new NotImplementedException();
		}

		public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
		{
			throw new NotImplementedException();
		}

		public override bool DeleteUser(string username, bool deleteAllRelatedData)
		{
			throw new NotImplementedException();
		}

		public override bool EnablePasswordReset
		{
			get { throw new NotImplementedException(); }
		}
		public override bool EnablePasswordRetrieval
		{
			get { throw new NotImplementedException(); }
		}
		public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
		{
			throw new NotImplementedException();
		}
		public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
		{
			throw new NotImplementedException();
		}
		public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
		{
			throw new NotImplementedException();
		}
		public override int GetNumberOfUsersOnline()
		{
			throw new NotImplementedException();
		}
		public override string GetPassword(string username, string answer)
		{
			throw new NotImplementedException();
		}
		public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
		{
			throw new NotImplementedException();
		}
		public override string GetUserNameByEmail(string email)
		{
			throw new NotImplementedException();
		}
		public override int MaxInvalidPasswordAttempts
		{
			get { throw new NotImplementedException(); }
		}
		public override int MinRequiredNonAlphanumericCharacters
		{
			get { throw new NotImplementedException(); }
		}
		public override int MinRequiredPasswordLength
		{
			get { throw new NotImplementedException(); }
		}
		public override int PasswordAttemptWindow
		{
			get { throw new NotImplementedException(); }
		}
		public override MembershipPasswordFormat PasswordFormat
		{
			get { throw new NotImplementedException(); }
		}
		public override string PasswordStrengthRegularExpression
		{
			get { throw new NotImplementedException(); }
		}
		public override bool RequiresQuestionAndAnswer
		{
			get { throw new NotImplementedException(); }
		}
		public override bool RequiresUniqueEmail
		{
			get { throw new NotImplementedException(); }
		}
		public override string ResetPassword(string username, string answer)
		{
			throw new NotImplementedException();
		}
		public override bool UnlockUser(string userName)
		{
			throw new NotImplementedException();
		}
		public override void UpdateUser(MembershipUser user)
		{
			throw new NotImplementedException();
		}
		#endregion
	}
}