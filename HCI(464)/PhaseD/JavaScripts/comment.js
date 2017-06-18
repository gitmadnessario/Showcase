function addcomment() {
    var text = document.forms["leaveComment"]["comment"].value;
    document.forms["leaveComment"]["comment"].value ="";
    var previous = $('#comments tr:last').css( "background-color" );
    if (previous == "rgb(169, 169, 169)"){
        $('#comments').append("<tr class='commentsfirst'><td class='nickcom'>(2015)"+localStorage.getItem("login")+" said:</td><td class='textcom'>"+text+"</td></tr>");
        return false;
    }
    else{
        $('#comments').append("<tr class='comentssecond'><td class='nickcom'>(2015)"+localStorage.getItem("login")+" said:</td><td class='textcom'>"+text+"</td></tr>");
        return false;
    }
}