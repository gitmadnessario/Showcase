$(function(){ 
    if (localStorage.getItem("login")==="kaloyannis"){
            $("#right_side").load("right_side_kaloyannis.html")
    }
    else if (localStorage.getItem("login")==="dagalakis"){
            $("#right_side").load("right_side_dagalakis.html")
    }
    else{
        $("#right_side").load("form.html"); 
    }
});

function validateForm() { //server-side only,for obvious reasons,in a real site
    var user = document.forms["myForm"]["user"].value;
    var pass = document.forms["myForm"]["pass"].value;
    var test = document.getElementById("remember_me").checked;
    localStorage.setItem("remember",test);
    if (user=="csd2706@csd.uoc.gr" && pass=="12345"){
        localStorage.setItem("login","kaloyannis");
    }
    else if (user=="csd2790@csd.uoc.gr" && pass=="12345"){
        localStorage.setItem("login","dagalakis");
    }
    else{
        alert("Invalid username or password.Please try again.");
    }
}

function logout(){
    localStorage.setItem("login","none");
    location.href='Home.html';
}

function showkaloyannis(){
    if (localStorage.getItem("login")==="kaloyannis"){
            location.href='kaloyannis.html'; 
    }
    else{
            location.href='kaloyannis-view.html';   
    }
      
}
