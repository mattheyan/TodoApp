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

		/// <summary>
		/// Ensures that the due date is on or after the day on which the item was created.
		/// </summary>
		static Error ValidateFutureDueDate = new Error<ListItem>(
			"The Due Date must be today or some time in the future",
			item => item.DateCreated != null && item.DueDate != null && item.DueDate.Value < item.DateCreated.Value.Date);

		/// <summary>
		/// Ensure that items in the same list have unique text descriptions.
		/// </summary>
		static Error ValidateUniqueText = new Error<ListItem>(
			"This appears to be a duplicate!",
			item => item.List != null && item.List.Items.Any(i => i != item && i.Description == item.Description),
			"Description")			// Only attach the error to the Description property on the root instance
			.OnInitExisting();		// Ensure validation runs on existing instances since this rule is not enforced
	}
}
