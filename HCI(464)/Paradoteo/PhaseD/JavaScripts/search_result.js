$('#myForm input').on('change', function() {
    var selected = $('input[name=radioName]:checked', '#myForm').val();
    if (selected=="Both"){
        $("#tablesearchresult tr").show();
        $("#tablesearchhad tr:first").find("td:eq(1)").html('6/6');
    }
    else if (selected=="Student"){
        $("#tablesearchhad tr:first").find("td:eq(1)").html('3/6');
        $("#tablesearchresult tr").each(function() { //loop over each row
            var cellhas = $(this).find("td:eq(1)").text();
            if (cellhas == "Student"){
                $(this).show(); //show the row 
            }
            else{
                $(this).hide(); //hide everything else
            }
        });
    }
    else{
       $("#tablesearchhad tr:first").find("td:eq(1)").html('3/6');
       $("#tablesearchresult tr").each(function() { //loop over each row
            var cellhas = $(this).find("td:eq(1)").text();
            if (cellhas == "Project"){
                $(this).show(); //show the row 
            }
            else{
                $(this).hide(); //hide everything else
            }
        });
    }
});