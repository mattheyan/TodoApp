$(document.documentElement).on("click", ".board-list-item", function() {
	// Disable save on the item being edited
	var item = $parentContextData(this, null, null, ListItem);
	context.server.disableSave(item);

	var checkpoint = context.server._changeLog.checkpoint();

	var $dialog = $(".board-list-edititem-dialog");

	$dialog.dialog({
		autoOpen: false,
		modal: true,
		title: "Edit Item",
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
			if (item.meta.conditions().length === 0) {
				context.server.enableSave(item);
			}
			else {
				return false;
			}
		},
		cancel: function() {
			context.server.rollback(checkpoint, function() {
				context.server.enableSave(item);
			});
		}
	});

	// Apply data to the dataview
	$dialog.control("data", item);

	// Open the dialog
	$dialog.dialog("open");
});
