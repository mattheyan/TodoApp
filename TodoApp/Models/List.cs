using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ExoRule.DataAnnotations;

namespace TodoApp.Models
{
	public class List
	{
		public int Id { get; set; }

		[Required]
		public string Name { get; set; }

		[Required]
		public int Sequence { get; set; }

		public virtual User User { get; set; }

		public virtual ICollection<ListItem> Items { get; set; }
	}
}
