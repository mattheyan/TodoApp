(function() {

	function addNewItem(textEntered) {
		// Find the parent list
		var parentList = $parentContextData(this, null, null, List);

		// Get the default priority (medium)
		var mediumPriority = Priority.meta.get("2");

		// Use the current date and time as the date created
		var now = new Date();

		// Create the new list item
		var newItem = new ListItem({
			List: parentList,
			Priority: mediumPriority,
			Sequence: parentList.get_Items().length,
			Description: textEntered,
			DateCreated: now
		});

		// Add the item to the list
		parentList.get_Items().add(newItem);
	}

	$(document.documentElement)

		// Create a new item when the add link is clicked
		.on("click", ".board-list-additem a", function() {
			// Hide link and show input
			$(this).hide().prev().show().focus();
		})

		// Create the new item and continue when the user presses the tab key
		.on("keydown", ".board-list-additem input[type=text]", function(e) {

			// Check for TAB to signify completion
			if (e.keyCode == jQuery.ui.keyCode.TAB) {
				var $this = $(this);
				var textEntered = $this.val();
				if (!textEntered) {
					alert("Enter description");
				}
				else {
					// Add the new item
					addNewItem.call(this, textEntered);
				}

				// Clear the input
				$this.val("");

				// Stop default tab behavior
				e.stopPropagation();
				return false;
			}

		})

		// Create the new item when the user presses the enter key
		.on("keypress", ".board-list-additem input[type=text]", function(e) {

			// Check for ENTER to signify completion
			if (e.keyCode == jQuery.ui.keyCode.ENTER) {
				var $this = $(this);
				var textEntered = $this.val();
				if (!textEntered) {
					alert("Enter description");
				}
				else {
					// Add the new item
					addNewItem.call(this, textEntered);

					// Clear the input
					$this.val("");

					// Hide input and show link
					$this.hide().next().show();
					return false;
				}
			}

		})

		// Hide the add field when the user leaves
		.on("blur", ".board-list-additem input[type=text]", function(e) {
			// Hide input and show link
			$(this).hide().next().show();
			return false;
		});

}());
