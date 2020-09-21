

$("body").on("click", "#updateComment", function (e) {
    e.preventDefault();
    let jsonData = {
        id: $(this).attr("data-id")
    };
    console.log(jsonData);
    $("#reply-editModal").modal('show');
    $.ajax({
        url: "/ClientView/UpdateCommentDetails",
        type: "GET",
        data: jsonData,
        success: function (response) {
            $("#Reply-editFormDiv").html(response);
        }
    });

});