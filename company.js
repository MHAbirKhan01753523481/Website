



var dataTable = $("#companyDatatable").dataTable({
    "processing": true,
    "serverSide": true,
    "filter": true,
    "pageLength": 50,
    "autoWidth": false,
    "lengthMenu": [[50, 100, 150, 200, 500, -1], [50, 100, 150, 200, 500, "All"]],
    "order": [[0, "desc"]],
    "ajax": {
        "url": "/Companies/LoadCompany/",
        "type": "POST",
        "data": function (data) {
        },
        "complete": function (json) {
        }
    },
    "columns": [
        { "data": "id", "name": "Id", "autowidth": true, "className": "text_center" },
        { "data": "name", "name": "Name", "autowidth": true },
        { "data": "logo", "name": "Logo", "autowidth": true },
        { "data": "slogan", "name": "Slogan", "autowidth": true },
        { "data": "address", "name": "Address", "autowidth": true },
        { "data": "phone", "name": "Phone", "autowidth": true },
        { "data": "email", "name": "Email", "autowidth": true },
        { "data": "facebook", "name": "Facebook", "autowidth": true },
        { "data": "linkedIn", "name": "LinkedIn", "autowidth": true },
        { "data": "twitter", "name": "Twitter", "autowidth": true },
        { "data": "youtube", "name": "Youtube", "autowidth": true },
        { "data": "favicon", "name": "Favicon", "autowidth": true },
        { "data": "shortDescription", "name": "ShortDescription", "autowidth": true },
        { "data": "description", "name": "Description", "autowidth": true },
        { "data": "googleMapLocation", "name": "GoogleMapLocation", "autowidth": true },
       
        {
            "render": function (data, type, full, meta) {
                return `<button style="font-size: inherit;" class="btn btn-sm btn-rx btn-table detailsBtn" value="${full.id}" data-toggle="tooltip" title="Product details"><i class="fas fa-file-alt"></i></button>
                        <button style="font-size: inherit;" class="btn btn-sm btn-rx btn-table editBtn" value="${full.id}" data-toggle="tooltip" title="Update product info!"><i class="fa fa-pen-fancy"></i></button>
                        <button style="font-size: inherit;" class="btn btn-sm btn-rx btn-table deleteBtn" value="${full.id}" data-toggle="tooltip" title="Delete product!"><i class="fa fa-trash"></i></button>`;
            }
        }
    ]
});

$("#company-createmodalBtn").on("click", function (e) {
    e.preventDefault();
    let data = objectifyForm($(this).serializeArray());
    $.ajax({
        url: "/Companies/CreateView",
        type: "GET",
        data: data,
        success: function (response) {
            $("#company-createFormDiv").html(response);
        }


    });

});


$("body").on("click", ".editBtn", function (e) {
    e.preventDefault();
    $("#companies-editModal").modal('show');
    let data = {
        id: $(this).val()
    };
    $.ajax({
        url: "/Companies/EditView",
        type: "GET",
        data: data,
        success: function (response) {
            $("#company-editFormDiv").html(response);
          
        }
    });
});

$("body").on("click", ".deleteBtn", function (e) {
    e.preventDefault();
    let id = $(this).val();
    $(".modaldeleteBtn").attr("data-id", id);
    $("#deleteModal").modal("show");
});

$("body").on("click", ".modaldeleteBtn", function (e) {
    e.preventDefault();
    let id = $(this).attr("data-id");
    $.ajax({
        url: "/Companies/Delete",
        type: "GET",
        data: { id: id },
        success: function (response) {
            $("#deleteModal").modal("hide");
            dataTable.fnFilter();
        }
    });
});
