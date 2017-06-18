function showExtra(td){
	var screen = $( window ).width();
	if (screen<480){ //mobile
		if (td>-1 && td<32){
			document.getElementById("calhidden").innerHTML = td +" " + "<img src='plus_sign.png' align='right' style='margin-right:10px;'>";
			$("#calspecial").addClass('hidden'); //hide
			$("#calsum").addClass('hidden'); //hide
			$("#calhidden").removeClass('hidden'); //show
			if (td==19){
				$("#calspecial").removeClass('hidden'); //show
			}
			if (td==0){
				$("#calsum").removeClass('hidden'); //show
				document.getElementById("calhidden").innerHTML = "Sum of events this month" +" " + "<img src='plus_sign.png' align='right'>";
			}
		}
	}	
}

function enterDetails(){
		document.location.href = "hy564_meeting_details.html";
}

function irefresh(num){
	if (num == 0)
		window.location.reload();
	else{
		document.getElementById("calframe"+num).contentDocument.location.reload(true);
	}
	
}

function goback(){
	alert("dsada");
	
}