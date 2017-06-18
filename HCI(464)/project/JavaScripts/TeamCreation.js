function create464(a,b){
    if (a=="case"){
        $("#breadcrumb_path li:last").remove();
        $("#breadcrumb_path").append("<li><a href='HY-464_project.html'>Θέμα Α HCI Web Design.html</a></li>");
        $("#breadcrumb_path").append("<li><b> > </b></li>");
        $("#breadcrumb_path").append("<li><b> Team Creation </b></li>");
        $("#content").load("TeamCreate464.html");
    }
    else{
        $("#breadcrumb_path li:last").remove();
        $("#breadcrumb_path").append("<li><a href='"+b+".html'>"+a+"</a></li>");
        $("#breadcrumb_path").append("<li><b> > </b></li>");
        $("#breadcrumb_path").append("<li><b> Team Creation </b></li>");
        $("#content").load("TeamCreate464.html");
    }
}

function create463(){
    $("#path").load("team_path.html");
    $("#content").load("TeamCreate463.html");
}

function create340(){
    $("#path").load("team_path.html");
    $("#content").load("TeamCreate340.html");
}

function create454(){
    $("#path").load("team_path.html");
    $("#content").load("TeamCreate454.html");
}

function create335b(){
    $("#path").load("team_path.html");
    $("#content").load("TeamCreate335b.html");
}

function create(){
    //$("#path").load("project_path.html");
    $("#content").load("TeamCreate.html");
}

function AddMembers(){
    //$("#content").load("add_members.html");
    //document.load("add_members.html").style.display='block';
    //"document.getElementById('light').style.display='block';document.getElementById('fade').style.display='block'"
}