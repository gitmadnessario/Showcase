function search_result(){
    var searchinput = document.getElementsByClassName('search_field')[0].value
    $("#content").load("search_result.html");
}

function pSearch(){
    var searchinput = document.getElementsByClassName('p_search_field')[0].value;
    var dropdown = $("#selection_list option:selected").text();
    if (dropdown == "HY-XXX"){
        $("#project_table tbody tr").each(function() { //loop over each row
            var cellhas = $(this).find("td:eq(1)").text();
            if (cellhas.indexOf(searchinput) >= 0){
                $(this).show(); //show the row 
            }
            else{
                $(this).hide(); //hide everything else
            }
        });
    }
    else if (dropdown == "Professor"){
        $("#project_table tbody tr").each(function() { //loop over each row
             var cellhas = $(this).find("td:eq(2)").text();
             if (cellhas.indexOf(searchinput) >= 0){
                $(this).show(); //show the row 
             }
             else{
                $(this).hide(); //hide everything else
             }
        });
    }
}

function selchanged(){
    var selectedValue = document.getElementById('selection_list').value;
    if (selectedValue == "all"){
        $("#project_table tr").show();
        document.getElementsByClassName('p_search_field')[0].value = "";
    }
    
}

function addmemberssearch(){
    var searchinput = document.getElementsByClassName('p_search_field')[0].value;
    if (searchinput=="")
        searchinput = "a";
    $("#add_members_inner tr").each(function() {
        var cellhas = $(this).find("td:eq(0)").text();
        if (cellhas.contains(searchinput)){
              $(this).show(); 
        }
        else{
            $(this).hide();
        }
    });
}


function addmember(){
    alert("dsad");
   $(this).hide();     
}