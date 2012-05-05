(function() {

	function adjustListWidth() {
		var totalWidth = $(".board-background").width() - 25;
		var numLists = $(".board-list").length;
		var listWidth = Math.max(Math.min(totalWidth / numLists, 320), 200);
		$(".board-list").css("width", listWidth + "px");
	}

	var boardPadding = 40;
	var listPadding = 25;

	function adjustBoardHeight() {
		var winHeight = $(window).height();
		var $board = $(".board:not(.sys-template)");
		var titleHeight = $board.find(".board-title").height();

		var boardHeight = winHeight - titleHeight - boardPadding;
		$board.find(".board-background").css("height", boardHeight);

		var listMaxHeight = boardHeight - listPadding;
		$board.find(".board-list").css("max-height", listMaxHeight);
	}

	// Initial state
	$(function () {
		adjustListWidth();
		adjustBoardHeight();
	});

	// Update when the window resizes
	$(window).resize(function() {
		adjustListWidth();
		adjustBoardHeight();
	});

}());
