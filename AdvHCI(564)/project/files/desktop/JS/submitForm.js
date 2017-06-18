function FormSubmit(){
	var name = $("input#name-input").val();
	var desc = $("textarea#desc-input").val();
	var datetime = $("input#datetimepicker").val();
	if (name == "" || desc == "" || datetime == ""){
		alert("You need to fill all the required fields before you can continue");
	}else{
		var img = $("input#img-input").val();
		var place = $("input#place-input").val();
		alert("to kana auto");
	}
}