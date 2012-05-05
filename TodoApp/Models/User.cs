using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using ExoRule;

namespace TodoApp.Models
{
	public class User : System.Security.Principal.IIdentity
	{
		public int Id { get; set; }

		[Required]
		public string Name { get; set; }

		public virtual ICollection<List> Lists { get; set; }

		[Required]
		public string OpenId { get; set; }

		[NotMapped]
		public string AuthenticationType
		{
			get { return "TodoApp"; }
		}

		[NotMapped]
		public bool IsAuthenticated
		{
			get { return true; }
		}

		// Represents the current time for the user, which is used to determine the relative age of list items for the user.
		[NotMapped]
		public DateTime CurrentTime { get; private set; }

		// Sets the current time of the user to the current system date and time.
		static Rule CalculateCurrentTime = Rule<User>.Calculate(
			user => user.CurrentTime,
			item => DateTime.Now);
	}
}
