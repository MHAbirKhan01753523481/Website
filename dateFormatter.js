function formatDate(givenDate, order = "dd") {

    let date = new Date(givenDate);

    let dd = date.getDate();

    let mm = date.getMonth() + 1;

    var yyyy = date.getFullYear();

    if (dd < 10) {
        dd = '0' + dd;
    }

    if (mm < 10) {
        mm = '0' + mm;
    }

    if (order === "mm") {
        date = mm + '/' + dd + '/' + yyyy;
    } else {
        date = dd + '/' + mm + '/' + yyyy;
    }

    return date;
}