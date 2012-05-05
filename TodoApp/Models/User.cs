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
	}
}
