//let selectors = {
//    rightSideBarButton: $("#rightSideBarButton"),
//    leftSideBarButton: $("#leftSideBarButton"),
//    rightSideNav: $("#right-sidenav"),
//    leftSideNav: $("#left-sidenav"),
//    pageContent: $(".page-content"),
//    pageWrapper: $(".page-wrapper")
//};

//selectors.rightSideBarButton.on("click", function (e) {
//    e.preventDefault();

//    let pageWrapperWidth = selectors.pageWrapper.width();
//    let rightSideNavWidth = selectors.rightSideNav.width();
//    let width = pageWrapperWidth - rightSideNavWidth;

//    console.log(pageWrapperWidth, rightSideNavWidth);

//    if (selectors.rightSideNav.hasClass("slideInRight")) {

//        selectors.rightSideNav.removeClass('slideInRight fast');

//        selectors.rightSideNav.addClass('slideOutRight fast');

//        selectors.pageWrapper.css("width", "100%");

//        selectors.pageWrapper.css("transition", "width .8s");

//    } else if (selectors.rightSideNav.hasClass("slideOutRight")) {

//        selectors.rightSideNav.removeClass('slideOutRight fast');

//        selectors.rightSideNav.addClass('slideInRight fast');

//        selectors.pageWrapper.css("width", width);

//        selectors.pageWrapper.css("transition", "width .8s");

//    } else {

//        selectors.rightSideNav.css('display', 'block');

//        selectors.rightSideNav.addClass('slideInRight fast');

//        selectors.pageWrapper.css("width", width);

//        selectors.pageWrapper.css("transition", "width .8s");

//    }

//    $('input[type="checkbox"]').on("change", function (e) {
//        e.preventDefault();

//        let value = $(this).attr("value");

//        if (value == "true") {
//            $(this).attr("value", "false");
//        }

//        if (value == "false") {
//            $(this).attr("value", "true");
//        }
//    });
//});