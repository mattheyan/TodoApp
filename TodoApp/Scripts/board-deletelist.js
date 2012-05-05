// delete list
$(document.documentElement).on("click", ".board-deletelist", function (e) {
	var list = $parentContextData(this, null, null, List);
	list.get_User().get_Lists().remove(list);
	e.stopPropagation();
});
