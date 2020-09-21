var dataTable = $("#hostingDatatable").dataTable({
    "processing": true,
    "serverSide": true,
    "filter": true,
    "pageLength": 50,
    "autoWidth": false,
    "lengthMenu": [[50, 100, 150, 200, 500, -1], [50, 100, 150, 200, 500, "All"]],
    "order": [[0, "desc"]],
    "ajax": {
        "url": "/HostingPlans/LoadHostingPlan/",
        "type": "POST",
        "data": function (data) {
        },
        "complete": function (json) {
        }
    },
    "columns": [
        { "data": "id", "name": "Id", "autowidth": true, "className": "text_center" },
        { "data": "name", "name": "Name", "autowidth": true },
        { "data": "space", "name": "Space", "autowidth": true },
        { "data": "bandwidth", "name": "Bandwidth ", "autowidth": true },
        { "data": "domain", "name": "Domain", "autowidth": true },
        { "data": "subDomain", "name": "SubDomain", "autowidth": true },
        { "data": "alias", "name": "Alias", "autowidth": true },
        { "data": "email", "name": "Email", "autowidth": true },
        { "data": "cPanel", "name": "CPanel", "autowidth": true },
        { "data": "yearlyPrice", "name": "YearlyPrice", "autowidth": true },
        { "data": "monthlyPrice", "name": "MonthlyPrice", "autowidth": true },
        { "data": "priceUnit", "name": "PriceUnit", "autowidth": true },



        {
            "render": function (data, type, full, meta) {
                return `<button style="font-size: inherit;" class="btn btn-sm btn-rx btn-table detailsBtn" value="${full.id}" data-toggle="tooltip" title="Product details"><i class="fas fa-file-alt"></i></button>
                        <button style="font-size: inherit;" class="btn btn-sm btn-rx btn-table editBtn" value="${full.id}" data-toggle="tooltip" title="Update product info!"><i class="fa fa-pen-fancy"></i></button>
                        <button style="font-size: inherit;" class="btn btn-sm btn-rx btn-table deleteBtn" value="${full.id}" data-toggle="tooltip" title="Delete product!"><i class="fa fa-trash"></i></button>`;
            }
        }
    ]
});

$("#hosting-modalBtn").on("click", function (e) {
    e.preventDefault();
    let data = objectifyForm($(this).serializeArray());
    $.ajax({
        url: "/HostingPlans/CreateView",
        type: "GET",
        data: data,
        success: function (response) {
            $("#hosting-createFormDiv").html(response);
        }
    });
});

$("body").on("click", ".editBtn", function (e) {
    e.preventDefault();
    let data = {
        id: $(this).val()
    };
    $("#hosting-editModal").modal('show');
    $.ajax({
        url: "/HostingPlans/EditView",
        type: "GET",
        data: data,
        success: function (response) {
            $("#hosting-editFormDiv").html(response);
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
        url: "/HostingPlans/Delete",
        type: "GET",
        data: { id: id },
        success: function (response) {
            $("#deleteModal").modal("hide");
            dataTable.fnFilter();
        }
    });
});
