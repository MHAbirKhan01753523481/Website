(function () {

    const selectors = {
        subTotalAmount: $("#subTotalAmount"),
        discountPercent: $("#discountPercent"),
        discountAmount: $("#discountAmount"),
        paymentType: $("#paymentType"),
        paymentAmount: $("#paymentAmount"),
        addPaymentBtn: $("#addPaymentBtn"),
        paymentsTableBody: $("#paymentsTableBody"),
        billingItemTableBody: $("#billingItemTableBody")
    };

    class Sales {
        constructor(ledgerId, ledgerName, billingItems, payments, subTotal, discount, total, paid, due) {
            this.ledgerId = ledgerId;
            this.ledgerName = ledgerName;
            this.billingItems = billingItems;
            this.payments = payments;
            this.subTotal = subTotal;
            this.discount = discount;
            this.total = total;
            this.paid = paid;
            this.due = due;
        }
    }

    class PaymentDescription {
        constructor(methodId, methodName, amount) {
            this.methodId = methodId;
            this.methodName = methodName;
            this.amount = amount;
        }
    }

    class BillingItem {
        constructor(particularId, particularName, quantity, price, itemTotal) {
            this.particularId = particularId;
            this.particularName = particularName;
            this.quantity = quantity;
            this.price = price;
            this.itemTotal = itemTotal;
        }
    }

    let payments = [];
    let billingItems = [];

    selectors.discountPercent.on("change", function (e) {
        e.preventDefault();

        let percent = $(this).val();

        let subTotalAmount = Number(selectors.subTotalAmount.text());

        if (subTotalAmount > 0) {
            let discountAmount = percentToDecimal(subTotalAmount, percent);

            selectors.discountAmount.val(discountAmount);
        }
    });

    selectors.discountAmount.on("change", function (e) {
        e.preventDefault();

        let amount = $(this).val();

        let subTotalAmount = Number(selectors.subTotalAmount.text());

        if (subTotalAmount > 0) {
            let discountPercent = decimalToPercent(subTotalAmount, amount);

            selectors.discountPercent.val(discountPercent);
        }
    });

    selectors.addPaymentBtn.on("click", function (e) {
        e.preventDefault();

        let methodId = selectors.paymentType.select2("data").id;
        let methodName = selectors.paymentType.select2("data").text;
        let amount = Number(selectors.paymentAmount.val());

        if (methodId !== undefined && methodName !== undefined && amount !== 0) {

            let paymentDescription = new PaymentDescription(methodId, methodName, amount);

            payments.push(paymentDescription);

            SeedPayments();

        }
    });

    function SeedBillingItems() {
        selectors.billingItemTableBody.html(" ");

        for (var i = 0; i < billingItems.length; i++) {
            let tableRow = `<tr>
                                <td>${++i}</td>
                                <td>${billingItems[i].particularName}</td>
                                <td>${billingItems[i].quantity}</td>
                                <td>${billingItems[i].price}</td>
                                <td>${billingItems[i].itemTotal}</td>
                                <td><i class="far fa-trash-alt"></i></td>
                            </tr>`;

            selectors.paymentsTableBody.append(tableRow);
        }
    }

    function SeedPayments() {

        selectors.paymentsTableBody.html(" ");

        for (var i = 0; i < payments.length; i++) {
            let tableRow = `<tr>
                                <td>${payments[i].methodName}</td>
                                <td>${payments[i].amount}</td>
                                <td><i class="far fa-trash-alt"></i></td>
                            </tr>`;

            selectors.paymentsTableBody.append(tableRow);
        }

    }
})();

