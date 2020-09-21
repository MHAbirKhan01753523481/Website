var dataTable = $("#reservationDataTable").dataTable({
    "processing": true,
    "serverSide": true,
    "filter": true,
    "pageLength": 10,
    "autoWidth": false,
    "lengthMenu": [[10, 20, 30, 50, 100, -1], [10, 20, 30, 50, 100, "All"]],
    "order": [[0, "desc"]],
    "ajax": {
        "url": "/Reservations/LoadReservation/",
        "type": "POST",
        "data": function (data) {
        },
        "complete": function (json) {
        }
    },
    "columns": [
     
        { "data": "firstName", "name": "FirstName", "autowidth": true },
        { "data": "adult", "name": "Adult", "autowidth": true },
        { "data": "child", "name": "Child", "autowidth": true },
        { "data": "children", "name": "Children", "autowidth": true },
        { "data": "address", "name": "Address", "autowidth": true },
        { "data": "email", "name": "Email", "autowidth": true },
        { "data": "mobile", "name": "Mobile", "autowidth": true },
        { "data": "city", "name": "City", "autowidth": true },

        { "data": "choseTimeStartTo", "name": "ChoseTimeStartTo", "autowidth": true },
        { "data": "choseTimeEndTo", "name": "ChoseTimeEndTo", "autowidth": true },

        { "data": "name", "name": "Name", "autowidth": true },
        { "data": "quantity", "name": "Quantity", "autowidth": true },
        { "data": "price", "name": "Price", "autowidth": true },
        { "data": "totalPrice", "name": "TotalPrice", "autowidth": true },
        {
            "orderable": false, "render": function (data, type, full, meta) { return calcProfile(full); }
        },

        {
            "render": function (data, type, full, meta) {
                return `<button style="font-size: inherit;" class="btn btn-sm btn-rx btn-table detailsBtn" value="${full.id}" data-toggle="tooltip" title="Product details"><i class="fas fa-file-alt"></i></button>
                        <button style="font-size: inherit;" class="btn btn-sm btn-rx btn-table reservationeditBtn" value="${full.id}" data-toggle="tooltip" title="Update product info!"><i class="fa fa-pen-fancy"></i></button>
                        <button style="font-size: inherit;" class="btn btn-sm btn-rx btn-table ReservationdeleteBtn" value="${full.id}" data-toggle="tooltip" title="Delete product!"><i class="fa fa-trash"></i></button>`;
            }
        }
    ]
});

$(document).ready(function () {

    $("body").on("click", ".ReservationdeleteBtn", function (e) {

        e.preventDefault();
        alert("working");
        let jsonData = {
            id: $(this).val()
        };
        $.ajax({
            url: "/Reservations/Delete",
            type: "GET",
            data: jsonData,
            success: function (response) {
                if (response == response) {
                    toastr.success("Item Delete", "Success");
                    dataTable.fnFilters();
                }
                else {
                    toastr.error("Delete Wrong !!!", "Error");
                }
              
            }

        });
       
    });

});



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