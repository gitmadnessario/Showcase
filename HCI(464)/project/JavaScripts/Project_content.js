$(function(){
if (localStorage.getItem("login")==="kaloyannis"){
            $("#content").load("Projects-kal.html",function() {
            var newTable = document.getElementById('project_table');
            sorttable.makeSortable(newTable);
        }); 
    }
    else if (localStorage.getItem("login")==="dagalakis"){
            $("#content").load("Projects-dag.html",function() {
            var newTable = document.getElementById('project_table');
            sorttable.makeSortable(newTable);
        }); 
    }
    else{
        $("#content").load("Projects-content.html",function() {
            var newTable = document.getElementById('project_table');
            sorttable.makeSortable(newTable);
        }); 
    }
});
