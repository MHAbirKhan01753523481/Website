﻿var dataTable = $("#menuDatatable").dataTable({
    "processing": true,
    "serverSide": true,
    "filter": true,
    "pageLength": 50,
    "autoWidth": false,
    "lengthMenu": [[50, 100, 150, 200, 500, -1], [50, 100, 150, 200, 500, "All"]],
    "order": [[0, "desc"]],
    "ajax": {
        "url": "/Menus/LoadMenu/",
        "type": "POST",
        "data": function (data) {
        },
        "complete": function (json) {
        }
    },
    "columns": [
        { "data": "id", "name": "Id", "autowidth": true, "className": "text_center" },
        { "data": "name", "name": "Name", "autowidth": true },
        { "data": "order", "name": "Order", "autowidth": true },
        { "data": "controllerName", "name": "ControllerName ", "autowidth": true },
        { "data": "actionName", "name": "ActionName", "autowidth": true },
        { "data": "dropdownType", "name": "DropdownType", "autowidth": true },
        //{ "data": "isActive", "name": "IsActive", "autowidth": true },
        {
            "render": function (data, type, full, meta) {
                return (full.isActive === true) ? '<span style="color:green;font-size:18px;" class="fa fa fa-check"></span>' : '<span style="color:red;font-size:18px;" class="fa fa-window-close"></span>';
            },
            "targets": 6
        },

        { "data": "parentId", "name": "ParentId", "autowidth": true },

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
    $("#menu-modalBtn").on("click", function (e) {
        $.ajax({
            url: "/Menus/CreateView",
            type: "GET",
            success: function (response) {
                $("#menu-createFormDiv").html(response);
            }
        });
    });

    $("body").on("click", ".editBtn", function (e) {
        e.preventDefault();
        $("#menu-editModal").modal('show');
        let data = {
            id: $(this).val()
        };
        $.ajax({
            url: "/Menus/EditView",
            data: data,
            type:"GET",
            success: function (response) {
                $("#menu-editFormDiv").html(response);

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
            url: "/Menus/Delete",
            type: "GET",
            data: { id: id },
            success: function (response) {
                $("#deleteModal").modal("hide");
                dataTable.fnFilter();
            }
        });
    });

})();