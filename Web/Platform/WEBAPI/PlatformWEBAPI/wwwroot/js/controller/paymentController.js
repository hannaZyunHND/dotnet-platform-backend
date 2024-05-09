R.Payment = {
    Init: function () {
        R.Payment.OrderInfo = {};
        R.Payment.LoadData();
    },
    RegisterEvent: function () { },
    LoadData: function () {
        
        var orderInfoString = sessionStorage.getItem("orderInfoStorage");
        if (orderInfoString) {
            R.Payment.OrderInfo = JSON.parse(orderInfoString);
            console.log(R.Payment.OrderInfo)
            $('.customer-email').text(R.Payment.OrderInfo.Email);
        }

    }
}
R.Payment.Init();