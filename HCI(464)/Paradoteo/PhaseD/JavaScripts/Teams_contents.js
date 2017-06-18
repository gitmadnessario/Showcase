$(function(){
if (localStorage.getItem("login")==="kaloyannis"){
            $("#content").load("Teams_ka.html"); 
            
    }
    else if (localStorage.getItem("login")==="dagalakis"){
            $("#content").load("Teams_dag.html"); 
    }
    else{
         
    }
});


function TeamMember(){
     $("#content").load("TeamMember_ka.html"); 
    
}