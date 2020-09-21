(function () {
    //let selector = {
    //    setting_item: $(".setting_item")
    //};

    //selector.setting_item.on("click", function (e) {
    //    e.preventDefault();

    //    let id = $(this).attr("id");

    //    $(".setting_sub_items").css("display", "none");

    //    $(`.setting_sub_items[data-menu="${id}"]`).slideDown(300, "linear");
    //});

    $("body").on("click", "#reservationDivOpen", function () {
        $("#reservation-Modal").modal('show');
        reservationOpenModal();
    });

        //Category List View//



        $("body").on("click", "#productCategoryList", function (e) {
            e.preventDefault();

        $("#productCategory-Modal").modal('show');

            $.ajax({
            url: "/ProductCategories/CategoryList",
        type: "GET",
                success: function (response) {

            $("#productCategory-FormDiv").html(response);
    }

});

});


        $("body").on("click", "#productOpenDiv", function (e) {
            e.preventDefault();

        $("#product-Modal").modal('show');

            $.ajax({
            url: "/Products/ProductList",
        type: "GET",
                success: function (response) {

            $("#product-FormDiv").html(response);
    }

});

});

    

})();

const reservationOpenModal = () => {



    $.ajax({
        url: "/Reservation/ReservationList",
        type: "GET",
        success: function (response) {

            $("#reservation-FormDiv").html(response);
        }

    });
}

//function companySetup() {
//    $.ajax({
//        type: "GET",
//        url: "/Settings/CompanySetup",
//        success: function (response) {
//            $("#setting_sub_item_detail").html(response);
//        }
//    });
//}

//function branchSetup() {
//    $.ajax({
//        type: "GET",
//        url: "/Settings/BranchSetup",
//        success: function (response) {
//            $("#setting_sub_item_detail").html(response);
//        }
//    });
//}

//function companyLicenseSetup() {
//    $.ajax({
//        type: "GET",
//        url: "/Settings/CompanyLicense",
//        success: function (response) {
//            $("#setting_sub_item_detail").html(response);
//        }
//    });
//}

//function productCategorySetup() {
//    $.ajax({
//        type: "GET",
//        url: "/ProductCategories/CategoryList",
//        success: function (response) {
//            $("#setting_sub_item_detail").html(response);
//        }
//    });
//}

//function employeeSetup() {
//    $.ajax({
//        type: "GET",
//        url: "/Employee/EmployeeList",
//        success: function (response) {
//            $("#setting_sub_item_detail").html(response);
//        }
//    });
//}

//function projectSetup() {
//    $.ajax({
//        type: "GET",
//        url: "/Settings/ProjectSetup",
//        success: function (response) {
//            $("#setting_sub_item_detail").html(response);
//        }
//    });
//}

//function purchaseExpenseSetup() {
//    $.ajax({
//        type: "GET",
//        url: "/PurchaseExpense/PurchaseExpenseList",
//        success: function (response) {
//            $("#setting_sub_item_detail").html(response);
//        }
//    });
//}

//function administrativeExpenseSetup() {
//    $.ajax({
//        type: "GET",
//        url: "/AdministrativeExpense/AdministrativeExpenseList",
//        success: function (response) {
//            $("#setting_sub_item_detail").html(response);
//        }
//    });
//}

//function marketingExpenseSetup() {
//    $.ajax({
//        type: "GET",
//        url: "/MarketingExpenses/MarketingExpenseList",
//        success: function (response) {
//            $("#setting_sub_item_detail").html(response);
//        }
//    });
//}

//function productionExpenseSetup() {
//    $.ajax({
//        type: "GET",
//        url: "/ProductionExpenses/ProductionExpenseList",
//        success: function (response) {
//            $("#setting_sub_item_detail").html(response);
//        }
//    });
//}

//function operatingExpenseSetup() {
//    $.ajax({
//        type: "GET",
//        url: "/OperatingExpenses/OperatingExpenseList",
//        success: function (response) {
//            $("#setting_sub_item_detail").html(response);
//        }
//    });
//}

//function customerSetup() {
//    $.ajax({
//        type: "GET",
//        url: "/Customers/CustomerList",
//        success: function (response) {
//            $("#setting_sub_item_detail").html(response);
//        }
//    });
//}

//function doctorSetup() {
//    $.ajax({
//        type: "GET",
//        url: "/Doctors/DoctorList",
//        success: function (response) {
//            $("#setting_sub_item_detail").html(response);
//        }
//    });
//}

//function OwnersEquitiesSetup() {
//    $.ajax({
//        type: "GET",
//        url: "/OwnersEquities/OwnersEquitiesList",
//        success: function (response) {
//            $("#setting_sub_item_detail").html(response);
//        }
//    });
//}

//function bankAccountSetup() {
//    $.ajax({
//        type: "GET",
//        url: "/BankAccounts/BankAccountList",
//        success: function (response) {
//            $("#setting_sub_item_detail").html(response);
//        }
//    });
//}

//function HospitalsSetUp() {
//    $.ajax({
//        type: "GET",
//        url: "/Hospitals/HospitalList",

//        success: function (response) {
//            $("#setting_sub_item_detail").html(response);
//        }
//    });
//}

//function supplierSetUp() {
//    $.ajax({
//        type: "GET",
//        url: "/Suppliers/SupplierList",
//        success: function (response) {
//            $("#setting_sub_item_detail").html(response);
//        }
//    });
//}

//function investorSetUp() {
//    $.ajax({
//        type: "GET",
//        url: "/Investors/InvestorList",
//        success: function (response) {
//            $("#setting_sub_item_detail").html(response);
//        }
//    });
//}

//function agentSetUp() {
//    $.ajax({
//        type: "GET",
//        url: "/Agents/AgentsList",
//        success: function (response) {
//            $("#setting_sub_item_detail").html(response);
//        }
//    });
//}

//function vendorSetUp() {
//    $.ajax({
//        type: "GET",
//        url: "/Vendors/VendorList",
//        success: function (response) {
//            $("#setting_sub_item_detail").html(response);
//        }
//    });
//}

//function cashInHandSetup() {
//    $.ajax({
//        type: "GET",
//        url: "/CashInHand/CashInHandList",
//        success: function (response) {
//            $("#setting_sub_item_detail").html(response);
//        }
//    });
//}

//function loanReceivableSetup() {
//    $.ajax({
//        type: "GET",
//        url: "/LoanReceivable/LoanReceivableList",
//        success: function (response) {
//            $("#setting_sub_item_detail").html(response);
//        }
//    });
//}

//function loanPayableSetup() {
//    $.ajax({
//        type: "GET",
//        url: "/LoanPayable/LoanPayableList",
//        success: function (response) {
//            $("#setting_sub_item_detail").html(response);
//        }
//    });
//}
