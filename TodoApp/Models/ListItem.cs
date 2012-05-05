using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using ExoModel;
using ExoRule;
using ExoRule.DataAnnotations;

namespace TodoApp.Models
{
	public class ListItem
	{
		public int Id { get; set; }

		[Required]
		public string Description { get; set; }

		[Required]
		public int Sequence { get; set; }

		public virtual List List { get; set; }

		[Required]
		[DisplayFormat(DataFormatString = "d")]
		public DateTime? DateCreated { get; set; }

		[DisplayFormat(DataFormatString = "d")]
		public DateTime? DueDate { get; set; }

		[AllowedValues("Priority.All")]
		public Priority Priority { get; set; }
	}
}
