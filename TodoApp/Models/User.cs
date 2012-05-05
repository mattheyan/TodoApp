using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using ExoRule;

namespace TodoApp.Models
{
	public class User
	{
		public int Id { get; set; }

		[Required]
		public string Name { get; set; }

		public virtual ICollection<List> Lists { get; set; }
	}
}
