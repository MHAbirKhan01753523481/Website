var dataTable = $("#featuresDatatable").dataTable({
    "processing": true,
    "serverSide": true,
    "filter": true,
    "pageLength": 50,
    "autoWidth": false,
    "lengthMenu": [[50, 100, 150, 200, 500, -1], [50, 100, 150, 200, 500, "All"]],
    "order": [[0, "desc"]],
    "ajax": {
        "url": "/Features/LoadFeatures/",
        "type": "POST",
        "data": function (data) {
        },
        "complete": function (json) {
        }
    },
    "columns": [
   
        { "data": "name", "name": "Name", "autowidth": true },
        { "data": "icon", "name": "Icon", "autowidth": true },
        { "data": "iconImage", "name": "IconImage", "autowidth": true },
        { "data": "shortDescription", "name": "ShortDescription", "autowidth": true },
     
        {
            "render": function (data, type, full, meta) {
                return `<button style="font-size: inherit;" class="btn btn-sm btn-rx btn-table detailsBtn" value="${full.id}" data-toggle="tooltip" title="Product details"><i class="fas fa-file-alt"></i></button>
                        <button style="font-size: inherit;" class="btn btn-sm btn-rx btn-table editBtn" value="${full.id}" data-toggle="tooltip" title="Update product info!"><i class="fa fa-pen-fancy"></i></button>
                        <button style="font-size: inherit;" class="btn btn-sm btn-rx btn-table featuresdeleteBtn" value="${full.id}" data-toggle="tooltip" title="Delete product!"><i class="fa fa-trash"></i></button>`;
            }
        }
    ]
});


$(document).ready(function () {

     $("#features-createmodalBtn").on("click", function (e) {
         e.preventDefault();
         $.ajax({
             url: "/Features/CreateView",
             type: "GET",
             success: function (response) {
                 console.log(response);
                 $("#features-createFormDiv").html(response);
             }
         });
     });

    $("body").on("click",".editBtn", function (e) {
        e.preventDefault();
        let jsonData = {
            id: $(this).val()
        };
        $("#features-editModal").modal("show");
        $.ajax({
            url: "/Features/EditView",
            type: "GET",
            data: jsonData,
            success: function (response) {
                console.log(response);
                $("#features-editFormDiv").html(response);
              
            }
        });
    });


    $("body").on("click",".featuresdeleteBtn", function (e) {
        e.preventDefault();
        let id = $(this).val();
        $(".feturesDeleteBtn").attr("data-id", id);
        $("#feturesModal").modal("show");
    });
  
    $("body").on("click",".feturesDeleteBtn", function (e) {
        e.preventDefault();
        let id = $(this).attr("data-id");
        $.ajax({
            url: "/Features/Delete",
            type: "GET",
            data: {id:id},
            success: function (response) {
                $("#feturesModal").modal("hide");
                dataTable.fnFilter();
            }
        });
    });

   


      
   
 });