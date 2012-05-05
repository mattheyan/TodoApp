(function() {

	// Generate a random prefix and data keys
	var dataPrefix = ExoWeb.randomText(5);
	var dataCommandsKey = dataPrefix + "-dialogCommands";
	var dataHandlerKey = dataPrefix + "-dialogCommandsHandler";

	function onDialogCommand(sender, args) {
		var commands = this.data(dataCommandsKey);
		var handler = commands[args.get_commandName()];
		if (handler) {
			// allow handler to cancel default action of closing dialog
			if (handler(sender, args) !== false) {
				var commandHandler = this.data(dataHandlerKey);
				this.control().remove_command(commandHandler);
				this.data(dataHandlerKey, null);
				this.data(dataCommandsKey, null);
				this.dialog("destroy");
			}
		}
	}

	$.fn.dialogCommands = function(commands) {
		// Set commands data
		this.data(dataCommandsKey, commands);

		// Subscribe to command if not done already
		if (!this.data(dataHandlerKey)) {
			var handler = onDialogCommand.bind(this);
			this.data(dataHandlerKey, handler);
			this.control().add_command(handler);
		}
	};

}());
