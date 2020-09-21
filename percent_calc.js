function percentToDecimal(totalBalance, percent) {

    let balance = Number(totalBalance);
    let discountPercent = Number(percent);

    return Number((100 / discountPercent) * balance);
}

function decimalToPercent(totalBalance, discountAmount) {

    let balance = Number(totalBalance);
    let discountBalance = Number(discountAmount);

    return Number((discountBalance / balance) * 100);
}