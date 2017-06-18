function details454(){
    $("#path").load("project_details454_path.html"); 
    $("#content").load("HY-454_project.html");   
}

function details340(){
    
}

function details335b(){
    
}

function details464(a){
    $("#breadcrumb_path li:last").remove();
    var b = getName(a);
    $("#breadcrumb_path").append("<li><a href='"+a+"'>"+b+"</a></li>");
    $("#breadcrumb_path").append("<li><b> > </b></li>");
    $("#breadcrumb_path").append("<li><b> Θέμα Α HCI Web Design</b></li>");
    $("#content").load("TeamCreate464.html");
     $("#content").load("HY-464_project.html");   
}

function details463(){
    
}

function getName(a){
    var semi = a.split("/");
    var almost = semi[semi.length-1];
    almost = almost.split(".");
    found = almost[0];
    return found;   
}