using System.ComponentModel.DataAnnotations;

namespace TheatreBookingSystem.Models
{
	public class LoginViewModel
	{
		[Required(ErrorMessageResourceType = typeof(Resources.Resource), 
			ErrorMessageResourceName ="NameRequired")]
		[Display(Name = "UserName", ResourceType = typeof(Resources.Resource))]
		public string Name { get; set; }

		[Required(ErrorMessageResourceType = typeof(Resources.Resource),
			ErrorMessageResourceName = "PasswordRequired")]
		[DataType(DataType.Password)]
		[Display(Name = "Password", ResourceType = typeof(Resources.Resource))]
		public string Password { get; set; }

		[Display(Name = "RememberMe", ResourceType = typeof(Resources.Resource))]
		public bool RememberMe { get; set; }

	}

	public class RegisterViewModel
	{
		[Required(ErrorMessageResourceType = typeof(Resources.Resource),
			ErrorMessageResourceName = "NameRequired")]
		[Display(Name = "UserName", ResourceType = typeof(Resources.Resource))]
		public string Name { get; set; }

		[Required(ErrorMessageResourceType = typeof(Resources.Resource),
			ErrorMessageResourceName = "EmailRequired")]
		[Display(Name = "Email", ResourceType = typeof(Resources.Resource))]
		public string Email { get; set; }

		[Display(Name = "Phone", ResourceType = typeof(Resources.Resource))]
		[DataType(DataType.PhoneNumber)]
		public string Phone { get; set; }

		[Required(ErrorMessageResourceType = typeof(Resources.Resource),
			ErrorMessageResourceName = "PasswordRequired")]
		[StringLength(100, MinimumLength = 6, ErrorMessageResourceName ="PasswordError", 
			ErrorMessageResourceType = typeof(Resources.Resource))]
		[DataType(DataType.Password)]
		[Display(Name = "Password", ResourceType = typeof(Resources.Resource))]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "ConfirmPassword", 
			ResourceType = typeof(Resources.Resource))]
		[Compare("Password", ErrorMessageResourceType = typeof(Resources.Resource), 
			ErrorMessageResourceName = "PasswordsDoNotMatch")]
		public string ConfirmPassword { get; set; }
	}
}