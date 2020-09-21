
$("body").on("click", "#tableCreateBtn", function (e) {
    e.preventDefault();
    tableInsert();

});


const tableInsert = () => {

    let tableData = {
        Name: $("#Name").val()
    };
    console.log(tableData);
    $.ajax({
        url: "/Tables/Create",
        type: "POST",
        data: tableData,
        success: function (response) {
            $("#tableDataId").html(response);
        }
    });
}