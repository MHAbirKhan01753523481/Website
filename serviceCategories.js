function calcProfile(obj) {

    var totalLength = 380;
    var value = obj.id * 15;
    var borderLen = (value / 100) * totalLength;
    var backgroundPos = '';
    if (borderLen <= 90) {
        backgroundPos = 'background-position: ' + (-90 + borderLen) + 'px 0px, 85px -100px, 90px 95px, 0px 100px';
        $('.progress').attr('style', backgroundPos);
    } else if (borderLen <= 190) {
        backgroundPos = 'background-position: 0px 0px, 85px ' + (-100 + (borderLen - 90)) + 'px, 90px 95px, 0px 100px';
        $('.progress').attr('style', backgroundPos);
    } else if (borderLen <= 280) {
        backgroundPos = 'background-position: 0px 0px, 85px 0px, ' + (90 - (borderLen - 90 - 100)) + 'px 95px, 0px 100px';
        $('.progress').attr('style', backgroundPos);
    } else {
        backgroundPos = 'background-position: 0px 0px, 85px 0px, 0px 95px, 0px ' + (100 - (borderLen - (90 * 2) - 100)) + 'px';
        $('.progress').attr('style', backgroundPos);
    }


    return '<div class="profileProgress" style="' + backgroundPos + '" ><img style="margin:5px;" height="90" width="80" src="' + obj.image + '" /></div>';


}


var dataTable = $("#serviceCategoryDatatable").dataTable({
    "processing": true,
    "serverSide": true,
    "filter": true,
    "pageLength": 50,
    "autoWidth": false,
    "lengthMenu": [[50, 100, 150, 200, 500, -1], [50, 100, 150, 200, 500, "All"]],
    "order": [[0, "desc"]],
    "ajax": {
        "url": "/ServiceCategories/LoadServiceCategories/",
        "type": "POST",
        "data": function (data) {
        },
        "complete": function (json) {
        }
    },
    "columns": [
        { "data": "id", "name": "Id", "autowidth": true, "className": "text_center" },
        { "data": "name", "name": "Name", "autowidth": true },
        //{ "data": "order", "name": "Order", "autowidth": true },
        { "data": "description", "Description": "Name", "autowidth": true },
        {
            "orderable": false, "render": function (data, type, full, meta) { return calcProfile(full); }
        },
        //{ "data": "image", "name": "Image", "autowidth": true },
        {
            "render": function (data, type, full, meta) {
                return `<button style="font-size: inherit;" class="btn btn-sm btn-rx btn-table detailsBtn" value="${full.id}" data-toggle="tooltip" title="Product details"><i class="fas fa-file-alt"></i></button>
                        <button style="font-size: inherit;" class="btn btn-sm btn-rx btn-table editBtn" value="${full.id}" data-toggle="tooltip" title="Update product info!"><i class="fa fa-pen-fancy"></i></button>
                        <button style="font-size: inherit;" class="btn btn-sm btn-rx btn-table deleteBtnCategory" value="${full.id}" data-toggle="tooltip" title="Delete product!"><i class="fa fa-trash"></i></button>`;
            }
        }
    ]
});

(function () {

    $("#serviceCategory-modalBtn").on("click", function (e) {
        $.ajax({
            url: "/ServiceCategories/CreateView",
            type: "GET",
            success: function (response) {
                $("#serviceCategory-createFormDiv").html(response);
            }
        });
    });

    //$("body").on("click", ".deleteBtn", function (e) {
    //    e.preventDefault();

    //    let data = {
    //        id: $(this).val()
    //    };

    //    $.ajax({
    //        url: "/ServiceCategories/Delete",
    //        type: "GET",
    //        data: data,
    //        success: function (response) {
    //            dataTable.fnFilter();
    //        }
    //    });
    //}); 

    $("body").on("click", ".editBtn", function (e) {
        e.preventDefault();

        $("#serviceCategory-editModal").modal('show');

        let data = {
            id: $(this).val()
        };

        $.ajax({
            url: "/ServiceCategories/EditView",
            type: "GET",
            data: data,
            success: function (response) {
                $("#serviceCategory-editFormDiv").html(response);
            }
        });
    });

    $("body").on("click", ".deleteBtnCategory", function (e) {
        e.preventDefault();
        let id = $(this).val();
        $(".modaldeleteBtn").attr("data-id", id);
        $("#deleteModal").modal("show");

    });

    $("body").on("click", ".modaldeleteBtn", function (e) {
        e.preventDefault();
        let id = $(this).attr("data-id");
        $.ajax({
            url: "/ServiceCategories/Delete",
            type: "GET",
            data: { id: id },
            success: function (response) {
                $("#deleteModal").modal("hide");
                dataTable.fnFilter();
            }
        });
    });

})();