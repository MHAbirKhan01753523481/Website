


(function () {
    $(document).ready(function () {



        var hiddenIndex;

        var categorydataTable = $("#categoryDataTable").dataTable({
            "processing": true,
            "serverSide": true,
            "filter": true,
            "pageLength": 16660,
            "autoWidth": false,
            "lengthMenu": [[10, 20, 30, 50, 100, -1], [10, 20, 30, 50, 100, "All"]],
            "order": [[0, "desc"]],
            "ajax": {
                "url": "/ProductCategories/LoadproductCategories/",
                "type": "POST",
                "data": function (data) {
                },
                "complete": function (json) {
                }
            },
            "columns": [
                { "data": "id", "name": "Id", "autowidth": true, "className": "text_center" },
                { "data": "name", "name": "Name", "autowidth": true },

                {
                    "render": function (data, type, full, meta) {
                        return `<button style="font-size: inherit;" class="btn btn-sm btn-rx btn-table detailsBtn" value="${full.id}" data-toggle="tooltip" title="Product details"><i class="fas fa-file-alt"></i></button>
                        <button style="font-size: inherit;" class="btn btn-sm btn-rx btn-table catgoryeditBtn" categoryItem="${full.name}" value="${full.id}"  data-toggle="tooltip" title="Update product info!"><i class="fa fa-pen-fancy"></i></button>
                        <button style="font-size: inherit;" class="btn btn-sm btn-rx btn-table deleteBtn" value="${full.id}" data-toggle="tooltip" title="Delete product!"><i class="fa fa-trash"></i></button>`;
                    }
                }
            ]
        });


        $("body").on("click", ".catgoryeditBtn", function (e) {

            e.preventDefault();

            $("#Name").val($(this).attr("categoryItem")).change();

          
            hiddenIndex = $(this).attr("value");

            $("#categoryBtnSubmit").text("Update");
            categorydataTable.fnFilter();
        });


        $("body").on("click", "#categoryBtnSubmit", function (e) {
            e.preventDefault();

            if ($(this).text() == "Update") {
                categoryUpdate();
                $(this).text("Save");
                categorydataTable.fnFilter();
            }
            else {
                categorySaveBtn();
                categorydataTable.fnFilter();
            }

        });


        const categorySaveBtn = () => {

            let data = {

                Name: $("#Name").val()

            };

            $.ajax({
                url: "/ProductCategories/Create",
                type: "POST",
                data: data,
                success: function (response) {

                    if (response == true) {
                        toastr.success("Category Item Save", "Success !!!");

                        categorydataTable.fnFilter();
                    }
                    else {
                        toastr.error("Error Something Error", "Error!!!");
                    }
                }

            });


        }

        const categoryUpdate = () => {

            let data = {
                id: hiddenIndex,
                Name: $("#Name").val()

            };

            $.ajax({
                url: "/ProductCategories/Edit",
                type: "POST",
                data: data,
                success: function (response) {

                    if (response == true) {
                        toastr.success("Category Item Edit", "Success !!!");

                        categorydataTable.fnFilter();
                    }
                    else {
                        toastr.error("Error Something Error", "Error!!!");
                    }
                }

            });


        }

    });
})();



