$(document.documentElement).on("click", ".board-list-deleteitem", function (e) {
	var item = $parentContextData(this, null, null, ListItem);
	item.get_List().get_Items().remove(item);
	e.stopPropagation();
});
