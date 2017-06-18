$(function(){
	// this will get the full URL at the address bar
	var url = window.location.href; 
    var urlcase = url.split("?");
    if (urlcase.length != 2)
        urlcase = url.split("#");
    url = urlcase[0];
	// check every "a" tag 
	$("#Menu_bar li a").each(function() {
		if(url == (this.href)) {
			$(this).closest("li").addClass("active");
		}
	});
});	