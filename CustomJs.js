var CardItem = [];
var ProductNames = [];
var ProductPrice = [];
var Images = [];
var ClickId;
var Duplicates = [];
var FinalAmount;
var totalCount;
var CurentTableAvailableTable = -1;
function FindQuantity(n) {
    var qty = 0;
    for (var i = 0; i < Duplicates.length; i++) {
        if (Duplicates[i] == n) {
            qty++;
        }
    }
    return qty;
}

function BuildSketch() {
    var TableData = "";
    FinalAmount = 0;
    totalCount = 0;

    for (var i = 0; i < CardItem.length; i++) {
        var qty = FindQuantity(CardItem[i]);
        TableData += "<tr>";
        TableData += "<td>" + ProductNames[i] + "</td>";
        TableData += "<td>" + "<img src='" + Images[i] + "' alt='' height='52'>" + "</td > ";
        TableData += "<td>" + "<input type='number' class='item-count form-control addCount' id ='" + CardItem[i]+"' value='" + qty + "'>" + "</td>";
        TableData += "<td>" + "<input type='hidden' class=' HiddenInput' value='" + CardItem[i] + "'>" + "</td>";
        TableData += "<td>" + ProductPrice[i] * qty + "</td>";
        TableData += "<td></td>";
        TableData 
        TableData += "<td>" + "<button type='button' class = 'canDelete btn btn-danger' DeleteId =" + i + "><i class='far fa-trash-alt'></i></button></td>";
        TableData += "</tr>";
        FinalAmount += ProductPrice[i] * qty;

        totalCount += qty;

        
    }

    $("#totaCountItem").html(totalCount);
    $("#Mydata").html(TableData);
    $("#TotalAmount").text(FinalAmount);
    console.log("Heare");
   

   
}

//Clear Card

//$("#clearCardTotal").click(function () {
//    CardItem = [];
//    CardItem.splice(CardItem,1);
//    BuildSketch();
//});


//< p class='d-inline-block align-middle mb-0 product-name' >" + ProductNames[i] + "</p >

function AddIntoDuplicates(range, value) {
    console.log(value);
    console.log(range);
    for (var i = 0; i < range; i++) {
        Duplicates.push(value);
    }
}

function RemoveFromDuplicates(value) {
    var NewDuplicates = [];
    for (var i = 0; i < Duplicates.length; i++) {
        if (Duplicates[i] != value) {
            NewDuplicates.push(Duplicates[i]);
        }
    }
    Duplicates = NewDuplicates;
}

function SetNewValue(something, NewValue) {
    RemoveFromDuplicates(something);
    AddIntoDuplicates(NewValue, something);
    BuildSketch();
    

}

$(document).on("click", ".item-count", function () {
    var qty = 0;
    for (var i = 0; i < CardItem.length; i++) {
        var qty = FindQuantity(CardItem[i]);
        var totalCart = ProductPrice[i]*qty;
        


    }
});

function GetAvailableTable(date, IsEndTime) {

    $.ajax({
        url: "/ClientView/GetAvailableTable",
        method: "GET",
        data: ({ "date": date}),
        dataType: "JSON",
        success: function (response) {
            if (IsEndTime == false) {
                $("#AvailableTable").text(response != null ? response : "Something Went Wrong");
            }
            else {
                CurentTableAvailableTable = response;
            }
        }
    })
}

$(document).ready(function () {

    // Get Available table information
    var date = new Date().toLocaleString().replace(",", "").replace(/:.. /, " ");
    console.log(date);


    
    GetAvailableTable(date, false);

    $("#date-end").on("change", function () {
        alert();
        var value = $(this).val();
        console.log("date end " + value);
     
        //kno hocha
        // I dont know Why this is not working
        GetAvailableTable(date, true);
    });

    function DateConversion(date)
    {
        console.log("Date Show + " + date);
    }

    $("#OrderSeat").on("keyup", function () {

        var date = $("#date-end").val($(this).attr("data-dtp"));
        console.log("here Order" + date);
        if (CurentTableAvailableTable == -1) {
            DateConversion(date);
            GetAvailableTable(Date.parse(date), true);
        }
        

        if (CurentTableAvailableTable != -1) {

            var value = $(this).val();
            console.log(value + " = " + CurentTableAvailableTable);
            if (value > CurentTableAvailableTable) {
                //  alert("Not Available");
                $("#availableTableItem").text("Not Available");
                $(this).val("");
            }
        }
        
        
    });


    

    $("#confrimButton").on("click", function () {
        var FilterQuantity = [];
        var FilterPrice = [];
      
        for (var i = 0; i < CardItem.length; i++) {
            var qty = FindQuantity(CardItem[i]);
            FilterQuantity.push(qty);
            FilterPrice.push(qty * ProductPrice[i]);
            
        }
        reservationSave(FilterQuantity, FilterPrice);

     
        //console.log(FilterQuantity);
        //console.log(FilterPrice);
        // Product names in CardItems array
        // Quantity in FilterQuantity
        //Price in FilterPrice

        // complete


    });

    $(document).on("click", ".CardItems", function () {
        var something = $(this).attr("ProductId");
        
        var NewValue = $("#" + something).val();
        
        ClickId = $(this).attr("ProductId");
     
        Duplicates.push($(this).attr("ProductId"));
 
        if (FindQuantity(ClickId) == 1) {
            CardItem.push($(this).attr("ProductId"));
            ProductNames.push($(this).attr("PrdName"));
            ProductPrice.push($(this).attr("PrdPrice"));
            Images.push($(this).attr("Images"));

        }
        $("input").text("");
        SetNewValue(something, NewValue);
     
    });

    $(document).on("click", '.canDelete', function () {
       
        var Index = $(this).attr("DeleteId");
        var value = CardItem[Index];
        CardItem.splice(Index, 1);
        ProductPrice.splice(Index, 1);
        ProductNames.splice(Index, 1);
        Images.splice(Index, 1);
        //var NewDuplicates = [];
        //for (var i = 0; i < Duplicates.length; i++) {
        //    if (Duplicates[i] != value) {
        //        NewDuplicates.push(Duplicates[i]);
        //    }
        //}
        //Duplicates = NewDuplicates;
        RemoveFromDuplicates(value);
        BuildSketch();
    });

    $(document).on("change", ".addCount", function () {
        var NewQty = Number ($(this).val());
        var Value = $(this).prop("id");
       
        RemoveFromDuplicates(Value);
        AddIntoDuplicates(NewQty, Value);
        BuildSketch();

    
      
    });
    
});

//SubTotal Section

const reservationSave = (FilterQuantity, FilterPrice) => {

    let reservationData = {
        ReservationType: $("#ReservationType").val(),
        Date: $("#Date").val(),
        Adult: $("#Adult").val(),
        Child: $("#Child").val(),
        Children: $("#Children").val(),
        ChoseTimeStart: $("#ChoseTimeStart").val(),
        ChoseTimeEnd: $("#ChoseTimeEnd").val(),
        OrderSeat: $("#OrderSeat").val(),
        FirstName: $("#FirstName").val(),
        LastName: $("#LastName").val(),
        DeliveryAddress: $("#DeliveryAddress").val(),
        Address: $("#Address").val(),
        City: $("#City").val(),
        Country: $("#Country").val(),
        ZipCode: $("#ZipCode").val(),
        Email: $("#Email").val(),
        Mobile: $("#Mobile").val(),
        ProductNames: ProductNames,
        ProductPrices: FilterPrice,
        ProductQuantities: FilterQuantity,
        TotalPrices: FinalAmount,
        BookingTableNumber: $("#OrderSeat").val(),
        

    };
    console.log(reservationData);
    $.ajax({

        url: "/ClientView/SaveReserVation",
        type: "POST",
        data: reservationData,
        dataType:"JSON",
        success: function (response) {

            if (response == true) {
                toastr.success("Data Save", "Success");
                $("#refreshData").html(response);
                $("#reservationDataForm")[0].reset();

                var date = new Date().toLocaleString().replace(",", "").replace(/:.. /, " ");

                GetAvailableTable(date, false);
            }
            else {
                toastr.error("Something Data Missing", "Error!!");
            }

        }

    });

}

function SubTotal() {
    var TableData = "";
    var totalCart = 0;
    for (var item in CardItem) {
        totalCart += CardItem[item].ProductPrice * CardItem[item].qty;
        TableData += "<tr>";
        TableData += "<td>" + "SubTotal" + "</td>";
        TableData += "<td>" + totalCart + "</td>";
        TableData += "</tr>";

    }
    $("#subTotalSection").html(TableData);
}

//$(document).ready(function () {

//    GetdropdownList();
//});


//const GetdropdownList = () => {
//    $.ajax({
//        url: "/ClientView/GetDropDownReservationList",
//        method: "GET",
//        success: function (response) {

//            var dropList = "<option >Select..</option>";

//            for (var i = 0; i < response.length; i++) {
//                dropList += "<option value = " + response[i].id + ">" + response[i].id + " || " + response[i].name + "</option>";
//            }

//            $("#tableList").html(dropList);

//        }
//    });
//}