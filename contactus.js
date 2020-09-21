var dataTable = $("#contactusDatatable").dataTable({
    "processing": true,
    "serverSide": true,
    "filter": true,
    "pageLength": 50,
    "autoWidth": false,
    "lengthMenu": [[50, 100, 150, 200, 500, -1], [50, 100, 150, 200, 500, "All"]],
    "order": [[0, "desc"]],
    "ajax": {
        "url": "/Contactus/LoadContract/",
        "type": "POST",
        "data": function (data) {
        },
        "complete": function (json) {
        }
    },
    "columns": [
       // { "data": "id", "name": "Id", "autowidth": true, "className": "text_center" },
        { "data": "name", "name": "Name", "autowidth": true },
        { "data": "email", "name": "Email ", "autowidth": true },
        { "data": "phone", "name": "Phone ", "autowidth": true },
        { "data": "message", "name": "Message", "autowidth": true },
        {
            "render": function (data, type, full, meta) {
                return `<button style="font-size: inherit;" class="btn btn-sm btn-rx btn-table detailsBtn" value="${full.id}" data-toggle="tooltip" title="Product details"><i class="fas fa-file-alt"></i></button>
                        <button style="font-size: inherit;" class="btn btn-sm btn-rx btn-table editBtn" value="${full.id}" data-toggle="tooltip" title="Update product info!"><i class="fa fa-pen-fancy"></i></button>
                        <button style="font-size: inherit;" class="btn btn-sm btn-rx btn-table deleteBtn" value="${full.id}" data-toggle="tooltip" title="Delete product!"><i class="fa fa-trash"></i></button>`;
            }
        }
    ]
});

(function () {
    $("#contractus-modalBtn").on("click", function (e) {

        e.preventDefault();
        let data = objectifyForm($(this).serializeArray());
        $.ajax({
            url: "/Contactus/CreateView",
            type: "GET",
            data: data,
            success: function (response) {
                $("#contactus-createFormDiv").html(response);
            }
        })


    });

    $("body").on("click", ".editBtn", function (e) {
        e.preventDefault();
        $("#contact-editModal").modal('show');
        let data = {
            id: $(this).val()
        };
        $.ajax({
            url: "/Contactus/EditView",
            data: data,
            type: "GET",
            success: function (response) {
                $("#contact-editFormDiv").html(response);
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
            url: "/Contactus/Delete",
            type: "GET",
            data: { id: id },
            success: function (response) {
                $("#deleteModal").modal("hide");
                dataTable.fnFilter();
            }
        });
    });


})();