using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace TodoApp.Models
{
	public class Priority
	{
		public int Id { get; set; }

		[Required]
		public string Name { get; set; }

		[Required]
		public int Sequence { get; set; }

		public static Priority[] All
		{
			get
			{
				return TodoContext.Current.Priorities.ToArray();
			}
		}
	}
}
