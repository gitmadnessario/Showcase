$(function(){ 
    if (localStorage.getItem("login")==="kaloyannis"){
            //$("#right_side").load("right_side_kaloyannis.html")
			window.location.href = "hy564_home.html";
    }
    /*else{
        window.location.href = "login.html" 
    }*/
});

function validateForm() { //server-side only,for obvious reasons,in a real site
    var user = document.forms["myForm"]["user"].value;
    var pass = document.forms["myForm"]["pass"].value;
    var test = document.getElementById("remember_me").checked;
    localStorage.setItem("remember",test);
    if (user=="kaloyannis" && pass=="12345"){
        localStorage.setItem("login","kaloyannis");
    }
    else{
        alert("Invalid username or password.Please try again.");
    }
}

function logout(){
    localStorage.setItem("login","none");
    location.reload(); 
}

function showkaloyannis(){
    if (localStorage.getItem("login")==="kaloyannis"){
            location.href='hy564_home.html'; 
    }
    else{
            location.href='login.html';   
    }
      
}
