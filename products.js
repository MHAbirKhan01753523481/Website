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

var dataTable = $("#productDatatable").dataTable({
    "processing": true,
    "serverSide": true,
    "filter": true,
    "pageLength": 50,
    "autoWidth": false,
    "lengthMenu": [[50, 100, 150, 200, 500, -1], [50, 100, 150, 200, 500, "All"]],
    "order": [[0, "desc"]],
    "ajax": {
        "url": "/Products/LoadProducts/",
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
        { "data": "categoryName", "name": "ProductCategoryId", "autowidth": true },
        { "data": "purchasePrice", "name": "PurchasePrice", "autowidth": true, "className": "text_center" },
        { "data": "retailPrice", "name": "RetailPrice", "autowidth": true, "className": "text_center" },
        { "data": "wholeSellPrice", "name": "WholeSellPrice", "autowidth": true, "className": "text_center" },
        { "data": "discount", "name": "Discount", "autowidth": true, "className": "text_center" },
        { "data": "shortDescription", "name": "ShortDescription", "autowidth": true },
        { "data": "description", "name": "Description", "autowidth": true },
        { "data": "releaseDate", "name": "ReleaseDate", "autowidth": true },
        {
            "render": function (data, type, full, meta) {
                return (full.isPopular === true) ? '<span style="color:green;font-size:18px;" class="fa fa fa-check"></span>' : '<span style="color:red;font-size:18px;" class="fa fa-window-close"></span>';
            },
            "targets": 6
        },
        //{ "data": "isPopular", "name": "IsPopular", "autowidth": true },
        {
            "orderable": false, "render": function (data, type, full, meta) { return calcProfile(full); }
        },
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

    $("#product-modalBtn").on("click", function (e) {
        $.ajax({
            url: "/Products/CreateView",
            type: "GET",
            success: function (response) {
                $("#product-createFormDiv").html(response);
            }
        });
    });



    $("body").on("click", ".editBtn", function (e) {
        e.preventDefault();

        $("#product-editModal").modal('show');

        let data = {
            id: $(this).val()
        };

        $.ajax({
            url: "/Products/EditView",
            type: "GET",
            data: data,
            success: function (response) {
                $("#product-editFormDiv").html(response);
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
            url: "/Products/Delete",
            type: "GET",
            data: { id: id },
            success: function (response) {
                $("#deleteModal").modal("hide");
                dataTable.fnFilter();
            }
        });
    });



})();

function OnSuccessProductCreate(data) {
    if (data.indexOf("field-validation-error") > -1) {
        $("#alert").html("<div class='alert alert-danger' style='color:black'><strong> Failed!</strong> Validation Failed!</div>");
        $("#alert").fadeIn('slow').delay(2000).fadeOut('slow');
    } else {
        $("#alert").html("<div class='alert alert-success' style='color:black'><strong> Success!</strong> Created Successfully!</div>");
        $("#alert").fadeIn('slow').delay(2000).fadeOut('slow');
    }

    dataTable.fnFilter();
}

function OnFailureProductCreate() {
    $("#alert").html("<div class='alert alert-danger' style='color:black'><strong> Failed!</strong> Couldn't insert the data. Something went wrong!</div>");
    $("#alert").fadeIn('slow').delay(2000).fadeOut('slow');
}





//$(document).ready(function () {

//    var productdataTable = $("#productDatatable").dataTable({
//        "processing": true,
//        "serverSide": true,
//        "filter": true,
//        "pageLength": 10,
//        "autoWidth": false,
//        "lengthMenu": [[10, 20, 30, 50, 100, 150, 200, - 1], [10, 20, 30, 50, 100, 150, 200, "All"]],
//        "order": [[0, "desc"]],
//        "ajax": {
//            "url": "/Products/LoadProducts",
//            "type": "POST",
//            "data": function (data) {
//            },
//            "complete": function (json) {
//            }
//        },
//        "columns": [
//            { "data": "id", "name": "Id", "autowidth": true, "className": "text_center" },
//            { "data": "name", "name": "Name", "autowidth": true },
//            { "data": "order", "name": "Order", "autowidth": true },
//            { "data": "categoryName", "name": "ProductCategoryId", "autowidth": true },
//            { "data": "purchasePrice", "name": "PurchasePrice", "autowidth": true, "className": "text_center" },
//            { "data": "retailPrice", "name": "RetailPrice", "autowidth": true, "className": "text_center" },
//            { "data": "wholeSellPrice", "name": "WholeSellPrice", "autowidth": true, "className": "text_center" },
//            { "data": "discount", "name": "Discount", "autowidth": true, "className": "text_center" },
//            { "data": "shortDescription", "name": "ShortDescription", "autowidth": true },
//            { "data": "description", "name": "Description", "autowidth": true },
//            { "data": "releaseDate", "name": "ReleaseDate", "autowidth": true },
//            { "data": "isPopular", "name": "IsPopular", "autowidth": true },
//            {
//                "render": function (data, type, full, meta) {
//                    return `<button style="font-size: inherit;" class="btn btn-sm btn-rx btn-table detailsBtn" value="${full.id}" data-toggle="tooltip" title="Product details"><i class="fas fa-file-alt"></i></button>
//                        <button style="font-size: inherit;" class="btn btn-sm btn-rx btn-table editBtn" value="${full.id}" data-toggle="tooltip" title="Update product info!"><i class="fa fa-pen-fancy"></i></button>
//                        <button style="font-size: inherit;" class="btn btn-sm btn-rx btn-table deleteBtn" value="${full.id}" data-toggle="tooltip" title="Delete product!"><i class="fa fa-trash"></i></button>`;
//                }
//            }
//        ]
//    });

//    $(document).ready(function () {

//        GetdropdownList();
//    });



//    const GetdropdownList = () => {
//        $.ajax({
//            url: "/Products/GetDropDown",
//            method: "GET",
//            success: function (response) {

//                var dropList = "<option >Select..</option>";

//                for (var i = 0; i < response.length; i++) {
//                    dropList += "<option value = " + response[i].id + ">" + response[i].id + " || " + response[i].name + "</option>";
//                }
             
//                $("#productlist").html(dropList);

//            }
//        });
//    }




//});

