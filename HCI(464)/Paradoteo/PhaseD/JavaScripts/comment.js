function addcomment() {
    var text = document.forms["leaveComment"]["comment"].value;
    document.forms["leaveComment"]["comment"].value ="";
    var previous = $('#comments tr:last').css( "background-color" );
    if (previous == "rgb(211, 211, 211)"){
        $('#comments').append("<tr class='commentsfirst'><td class='nickcom'>(2015)"+localStorage.getItem("login")+" said:</td><td class='textcom'>"+text+"</td></tr>");
        return false;
    }
    else{
        $('#comments').append("<tr class='comentssecond'><td class='nickcom'>(2015)"+localStorage.getItem("login")+" said:</td><td class='textcom'>"+text+"</td></tr>");
        return false;
    }
}

function addcommentprof(){
    var text = document.forms["proForm"]["commentprof"].value;
    $('#profilecomments').append("<tr><td>"+localStorage.getItem("login")+":</td><td>"+text+"</td></tr>");
    return false;
}

$("#comments th").click(function(){
      var theLink = $(this).text();
      if (theLink == "History"){
          $(this).css('border', 'none'); 
          $(this).prev().css('border', '1px solid black'); 
          $( "#comments tr").show();          
          $( "#comments tr:gt(6)" ).hide();
      }
      else if(theLink == "Comments"){
          $(this).css('border', 'none'); 
          $(this).next().css('border', '1px solid black'); 
          $( "#comments tr").show(); 
          $( "#comments tr:gt(0):lt(6)" ).hide();
      }
});

$(document).ready(function() {
          $( "#commentshead1").css('border', 'none'); 
          $( "#commentshead2").css('border', '1px solid black'); 
          $( "#comments tr").show(); 
          $( "#comments tr:gt(0):lt(6)" ).hide();   
});

function whatever(){
          $( "#commentshead1").css('border', 'none'); 
          $( "#commentshead2").css('border', '1px solid black');
          $( "#comments tr").show(); 
          $( "#comments tr:gt(0):lt(6)" ).hide();   
}

function whatever2(){
          $( "#commentshead2").css('border', 'none'); 
          $( "#commentshead1").css('border', '1px solid black');
          $( "#comments tr").show();          
          $( "#comments tr:gt(6)" ).hide();
}