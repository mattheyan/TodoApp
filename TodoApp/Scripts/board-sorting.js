$(function() {

	function registerSorting() {
		$(".sortable").sortable({
			placeholder: "ui-state-highlight",
			start: function(event, ui) {
				// Retrieve the card being moved
				var cardEl = ui.item[0];
				var $cardEl = $(cardEl);

				// Find the item
				var item = $parentContextData(cardEl, null, null, ListItem);

				// Cache off this information for later use
				$cardEl.data("item", item);
			},
			beforeStop: function(event, ui) {
				var $cardEl = $(event.originalEvent.target).closest(".board-list-item").parent();
				var cardEl = $cardEl.get(0);

				// Determine item, from and to list
				var item = $cardEl.data("item");
				var fromIndex = item.get_Sequence();
				var toIndex = $cardEl.index();

				if (fromIndex != toIndex) {
					// Move the item to its new place in the list
					item.set_Sequence(toIndex);

					// Moved up in the list
					if (fromIndex > toIndex) {
						// Increment every item between the start and end
						item.get_List().get_Items().forEach(function(otherItem) {
							if (otherItem !== item && otherItem.get_Sequence() >= toIndex && otherItem.get_Sequence() < fromIndex) {
								otherItem.set_Sequence(otherItem.get_Sequence() + 1);
							}
						});
					}
					// Moved down in the list
					else if (toIndex > fromIndex) {
						// Decrement every item between the start and end
						item.get_List().get_Items().forEach(function(otherItem) {
							if (otherItem !== item && otherItem.get_Sequence() > fromIndex && otherItem.get_Sequence() <= toIndex) {
								otherItem.set_Sequence(otherItem.get_Sequence() - 1);
							}
						});
					}
				}

				// Clear out cached data
				$cardEl.data("item", null);
			}
		});
	}

	registerSorting();

	$(window).bind("board:listadded", registerSorting);

});
