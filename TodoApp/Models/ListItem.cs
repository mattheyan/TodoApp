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

		// The age of the item relative to the curent time of the user.
		[NotMapped]
		public TimeSpan Age { get; private set; }

		// Calculates the age of the item relative to the current time.
		static Rule CalculateAge = Rule<ListItem>.Calculate(
			item => item.Age,
			item => item.DateCreated == null ? TimeSpan.Zero : item.List.User.CurrentTime.Subtract(item.DateCreated.Value));

		// A friendly description of the relative age of the item.
		[NotMapped]
		public string AgeDescription { get; private set; }

		// Calculates the description of the age of the item.
		static Rule CalculateAgeDescription = Rule<ListItem>.Calculate(
			item => item.AgeDescription,
			item => "created " + (
				item.Age.Ticks < 0 ? "in the future" :
				item.Age.TotalMinutes < 1 ? "just now" :
				item.Age.TotalMinutes < 2 ? "1 minute ago" :
				item.Age.TotalHours < 1 ? item.Age.Minutes + " minutes ago" :
				item.Age.TotalHours < 2 ? "1 hour ago" :
				item.Age.TotalDays < 1 ? item.Age.Hours + " hours ago" :
				item.Age.TotalDays < 2 ? "yesterday" :
				item.Age.TotalDays < 7 ? item.Age.Days + " days ago" :
				item.Age.TotalDays < 14 ? "last week" :
				item.Age.TotalDays < 35 ? Math.Round(item.Age.Days / 7.0) + " weeks ago" :
				"a long time ago"));

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
