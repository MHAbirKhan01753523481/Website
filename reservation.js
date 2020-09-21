//var dataTable = $("#datatable-buttons").dataTable({
//    "processing": true,
//    "serverSide": true,
//    "filter": true,
//    "pageLength": 50,
//    "autoWidth": false,
//    "lengthMenu": [[50, 100, 150, 200, 500, -1], [50, 100, 150, 200, 500, "All"]],
//    "order": [[0, "desc"]],
//    "ajax": {
//        "url": "/OurTeams/LoadOurTeam/",
//        "type": "POST",
//        "data": function (data) {
//        },
//        "complete": function (json) {
//        }
//    },
//    "columns": [
     
//        {
//            "render": function (data, type, full, meta) {
//                return `<button style="font-size: inherit;" class="btn btn-sm btn-rx btn-table detailsBtn" value="${full.id}" data-toggle="tooltip" title="Product details"><i class="fas fa-file-alt"></i></button>
//                        <button style="font-size: inherit;" class="btn btn-sm btn-rx btn-table editBtn" value="${full.id}" data-toggle="tooltip" title="Update product info!"><i class="fa fa-pen-fancy"></i></button>
//                        <button style="font-size: inherit;" class="btn btn-sm btn-rx btn-table deleteBtn" value="${full.id}" data-toggle="tooltip" title="Delete product!"><i class="fa fa-trash"></i></button>`;
//            }
//        }
//    ]
//});







