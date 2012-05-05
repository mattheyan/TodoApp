$(function () {

	// Apply rounded corners
	$(".board-background, .board-list, .board-list-item").corner("round 5px");

	// Keep track of last focused item
	$(".board-list-item").live("mouseover", function () {
		$(".board-list-item-focus").removeClass("board-list-item-focus");
		$(this).addClass("board-list-item-focus");
	});

	// Hide delete buttons since they will be shown on hover
	$(".board-deletelist, .board-list-deleteitem").hide();

	// show list delete button
	$(".board-list").live('mouseenter', function () {
		$(this).find(".board-deletelist").stop(true, true).fadeIn(500);
	});

	// hide list delete button
	$(".board-list").live('mouseleave', function () {
		$(this).find(".board-deletelist").stop(true, true).fadeOut(500);
	});

	// enable item delete button
	$(".board-deletelist").live('mouseenter', function () {
		$(this).addClass("board-deletelist-focus");
	});

	// disable item delete button
	$(".board-deletelist").live('mouseleave', function () {
		$(this).removeClass("board-deletelist-focus")
	});

	// show item delete button
	$(".board-list-item").live('mouseenter', function () {
		$(this).find(".board-list-deleteitem").stop(true, true).fadeIn(500);
	});

	// hide item delete button
	$(".board-list-item").live('mouseleave', function () {
		$(this).find(".board-list-deleteitem").stop(true, true).fadeOut(500);
	});

	// enable item delete button
	$(".board-list-deleteitem").live('mouseenter', function () {
		$(this).addClass("board-list-deleteitem-focus");
	});

	// disable item delete button
	$(".board-list-deleteitem").live('mouseleave', function () {
		$(this).removeClass("board-list-deleteitem-focus")
	});

});
