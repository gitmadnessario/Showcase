function hide_button(a,b){
                //var tenp =document.getElementById(b).textContent.replace("Member","No Member");
                $("#"+b).html('<br><br><br><br>Left ');
                $("#"+a).hide(); //hide everything else
                //b=tenp;
                //alert($("#"+b).textContent.replace("Member","No Member"));
    
}
function hide_buttons_accept(a,b,c){
                //var tenp =document.getElementById(b).textContent.replace("Member","No Member");
                $("#"+c).html('<br><br><br><br>Member ');
                $("#"+a).hide(); //hide everything else
                $("#"+b).hide();
                //alert($("#"+b).textContent.replace("Member","No Member"));
    
}
function hide_buttons_decline(a,b,c){
                //var tenp =document.getElementById(b).textContent.replace("Member","No Member");
                $("#"+c).html('<br><br><br><br> ');
                $("#"+a).hide(); //hide everything else
                $("#"+b).hide();
                //alert($("#"+b).textContent.replace("Member","No Member"));
    
}
function hide_button_cancel(a,b){
                //var tenp =document.getElementById(b).textContent.replace("Member","No Member");
                $("#"+b).html('<br><br><br><br>Canceled');
                $("#"+a).hide(); //hide everything else
                //b=tenp;
                //alert($("#"+b).textContent.replace("Member","No Member"));
    
}
