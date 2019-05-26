/* site.js */
(function () {
	//var ele = $("#username");
	//ele.text("Vla Guzman");

	//var main = $("#main");
	//main.on("mouseenter", function () {
	//main.css('background-color', '#888');
	//});

	//main.on("mouseleave", function () {
	//	main.css('background-color','');
	//});

	//var menuitems = $("ul.menu li a");
	//menuitems.on("click", function () {
	//	var me = $(this);
	//	alert(me.text());
	//});

  var $sidebarAndWrapper = $("#sidebar,#wrapper");
  var $icon = $("#sidebarToggle i.fas");

	$("#sidebarToggle").on("click", function () {
		$sidebarAndWrapper.toggleClass("hide-sidebar");
      if ($sidebarAndWrapper.hasClass("hide-sidebar")) {
				$icon.removeClass("fa-angle-left");
				$icon.addClass("fa-angle-right");
		} else {
        $icon.addClass("fa-angle-left");
        $icon.removeClass("fa-angle-right");
		}
	});
})();