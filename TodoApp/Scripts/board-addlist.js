
// Raise cancel event on escape keypress
$(document.documentElement).on("keydown", ".board-addlist-dialog .dialog-content", function(e) {
	if (e.keyCode === jQuery.ui.keyCode.ESCAPE) {
		Sys.UI.DomElement.raiseBubbleEvent(this, new Sys.CommandEventArgs("cancel", null, this));
	}
});

// Raise ok event on enter keypress
$(document.documentElement).on("keypress", ".board-addlist-dialog .dialog-content", function(e) {
	if (e.keyCode === jQuery.ui.keyCode.ENTER) {
		Sys.UI.DomElement.raiseBubbleEvent(this, new Sys.CommandEventArgs("ok", null, this));
	}
});

$(document.documentElement).on("click", ".board-addlist", function() {
	var $dialog = $(".board-addlist-dialog");

	// Clear out an existing value
	$dialog.find("input[type=text]").val("");

	$dialog.dialog({
		autoOpen: false,
		modal: true,
		title: "Add New List",
		width: $dialog.attr("width") ? parseInt($dialog.attr("width")) : undefined,
		height: $dialog.attr("height") ? parseInt($dialog.attr("height")) : undefined,
		closeOnEscape: false,
		open: function(event, ui) {
			// Disable close button
			$(this).closest(".ui-dialog").find(".ui-dialog-titlebar-close").hide();
		}
	});

	// Set up commands to respond to button clicks
	$dialog.dialogCommands({
		ok: function() {
			var text = $dialog.find("input[type=text]").val();

			// Exit if a name was not entered
			if (!text) {
				return false;
			}

			// Create the new list
			var list = new List({
				Sequence: context.model.user.get_Lists().length,
				User: context.model.user,
				Name: text
			});

			// Add to the list of lists
			context.model.user.get_Lists().add(list);

			// Notify that a list has been added
			$(window).trigger("board:listadded");
		},
		cancel: function() {
			// do nothing
		}
	});

	// Apply data to the dataview
	$dialog.control("data", context.model.user);

	// Open the dialog
	$dialog.dialog("open");
});
