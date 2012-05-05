$exoweb({
	contextReady: function (context) {
		// Automatically save changes immediately
		context.server.startAutoSave(context.model.user, 1);

		// Update status when communicating with server
		context.server.addRequestBegin(function () {
			$(".board:not(.sys-template) .board-server-working").show();
		});
		context.server.addRequestEnd(function () {
			$(".board:not(.sys-template) .board-server-working").hide();
		});
		context.server.addRequestSuccess(function () {
			$(".board-status-save").hide();
			$(".board:not(.sys-template) .board-server-failure").hide();
			$(".board:not(.sys-template) .board-server-success").show().fadeOut(2000);
		});
		context.server.addRequestFailed(function () {
			$(".board-status-save").show();
			$(".board:not(.sys-template) .board-server-success").hide();
			$(".board:not(.sys-template) .board-server-failure").show().fadeOut(2000);
		});

		$(document.documentElement).on("click", ".board-status-save", function () {
			$(".board-status-save").hide();
			context.server.save(context.model.user, function () {
				context.server.startAutoSave(context.model.user, 1);
			});
		});
	}
});

$(window).bind("beforeunload", function () {
	// Prompt the user if they will lose unsaved changes by navigating away from the page
	if (context.server.changes(false, context.model.user, true).length > 0) {
		return "Are you sure you want to leave the page? You will lose unsaved changes.";
	}
});
