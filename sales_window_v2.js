(function () {
    $('input[name="billDate"]').daterangepicker({
        showDropdowns: true,
        singleDatePicker: true
    });

    $("#productModel").prop('disabled', true);
    $("#productColor").prop('disabled', true);



})();