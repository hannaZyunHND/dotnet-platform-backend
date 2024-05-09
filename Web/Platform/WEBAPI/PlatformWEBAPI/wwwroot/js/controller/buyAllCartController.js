R.BuyAllCart = {
    Init: function () {
        R.BuyAllCart.TotalPrice = 0;
        R.BuyAllCart.RegisterEvent();
        R.BuyAllCart.LoadProductQuantity();
        R.BuyAllCart.LoadProductTotalPrice();
        console.log(R.LoadCart())
        R.BuyAllCart.PaymentShowcase = null;
        R.BuyAllCart.CurrentUrl = window.location.href;
        if (window.location.href.includes('local') || window.location.href.includes('demo')) {

            $('.cod-hide').removeClass('cod-hide');
        }

        R.BuyAllCart.current_location = R.CurrentLocationId();
        R.BuyAllCart.PickupPoints = []
        R.BuyAllCart.LoadAllLocations();
        R.BuyAllCart.CheckPaymentStatus();


        R.BuyAllCart.LoadPickupPoint(R.BuyAllCart.PickupPoints);
        R.BuyAllCart.Email = "";
        R.BuyAllCart.PickUpPoint = "";

        R.BuyAllCart.CurrrentUser = R.LoadCustomerInfo();
        console.log(R.BuyAllCart.CurrrentUser)
        if (R.BuyAllCart.CurrrentUser) {
            R.BuyAllCart.Email = R.BuyAllCart.CurrrentUser.email;
            $('.input-email').val(R.BuyAllCart.Email)
        }

        R.BuyAllCart.PaymentMethod = "";
        R.BuyAllCart.Voucher = null;
        R.BuyAllCart.LoadEmailItemButton();
        //R.BuyAllCart.InitSlick();
        
    },
    RegisterEvent: function () {
        $('form').submit(false);
        $('.add-quantity').off('click').on('click', function () {
            var id = $(this).closest('.sim-item').data('id');
            var carts = R.LoadCart();
            carts.forEach((element) => {
                if (element.productId == id)
                    element.quantity++;
            })
            //Doi text
            var current = parseInt($(this).parent().find('.inner-quantity').text());
            $(this).parent().find('.inner-quantity').html('').html(current + 1);
            //Save carts
            localStorage.setItem('sim-carts', JSON.stringify(carts));
            R.BuyAllCart.LoadProductTotalPrice();
            //console.log(R.LoadCart())

        })
        $('.minus-quantity').off('click').on('click', function () {
            var id = $(this).closest('.sim-item').data('id');
            var carts = R.LoadCart();
            carts.forEach((element) => {
                if (element.productId == id)
                    if (element.quantity > 1)
                        element.quantity--;
            })
            //Doi text
            var current = parseInt($(this).parent().find('.inner-quantity').text());
            if (current > 1) {
                $(this).parent().find('.inner-quantity').html('').html(current - 1);
                //Save carts
                localStorage.setItem('sim-carts', JSON.stringify(carts));
                //console.log(R.LoadCart())
                R.BuyAllCart.LoadProductTotalPrice();
            }


        })
        $('.remove-sim-item').off('click').on('click', function () {
            var id = $(this).closest('.sim-item').data('id');
            //Xoa ca khoi
            $(this).closest('.sim-item').remove();
            R.RemoveFromCart(id);
        })


        // copy from BuyAllCart
        $('.order-complete').off('click').on('click', function () {

            //Tao ra mot khach hang Id voi random 8 ky tu
            var currentCustomer = R.LoadCustomerInfo();
            var isCreatedAccount = false;
            console.log(currentCustomer)
            var customerRandomId = "";
            if (currentCustomer) {
                customerRandomId = currentCustomer.randomId;
            } else {
                customerRandomId = R.BuyAllCart.RandomString();
                isCreatedAccount = true;
            }

            //Ghi nhan don hang voi Id do

            var totalPrice = $('.total-price').data('price');
            var productId = $('.total-price').data('id');
            var orderCode = R.BuyAllCart.RandomString();

            //Luu session ma order de doi status khi update hoac xoa khi khong thanh cong
            sessionStorage.setItem('currentOrderCode', orderCode);

            var serialNumber = "";
            if (sessionStorage.getItem('current_serial')) {
                serialNumber = JSON.parse(sessionStorage.getItem('current_serial')).serialNumber;
            }

            var orderId = 0;
            if (localStorage.getItem('onPickingOrder')) {
                orderId = parseInt(localStorage.getItem('onPickingOrder'))
            }
            debugger
            var currentUrl = window.location.href; // Giả định đây là URL hiện tại có query parameters
            var urlWithoutQueryParameters = new URL(window.location.href);
            urlWithoutQueryParameters.search = ''; // Loại bỏ tất cả query parameters

            currentUrl = urlWithoutQueryParameters.origin + urlWithoutQueryParameters.pathname;
            var lookSimType = $('.sim-package-detail').data('sim-type');
            var type = "";
            if (lookSimType == "Physical Sim") {
                if (serialNumber == "") { type = "SIM" }
                else { type = "TOPUP" }
            }
            if (lookSimType == "E-Sim") {
                type = "ESIM"
            }
            if (type == "ESIM") {
                R.BuyAllCart.PickUpPoint = "ESIM"
            }
            if (type == "TOPUP") {
                R.BuyAllCart.PickUpPoint = "TOP-UP"
            }
            var quantity = parseInt($('.inner-quantity').text() ?? "0");
            console.log(quantity);
            if (quantity == 0 || isNaN(quantity)) {
                quantity = 1;
            }
            var carts = R.LoadCart();
            //alert(currentUrl);
            var request = {
                customerRandomId: customerRandomId,
                totalPrice: totalPrice,
                productId: productId,
                orderCode: orderCode,
                serialNumber: serialNumber,
                email: R.BuyAllCart.Email,
                pickupPoint: R.BuyAllCart.PickUpPoint,
                isCreatedAccount: isCreatedAccount,
                orderNote: `Pickup Point: ${R.BuyAllCart.PickUpPoint}`,
                returnUrl: currentUrl,
                productsInfo:carts
            }
            var checker = true;
            if (!R.BuyAllCart.PaymentShowcase) {
                $('.small-warning-payment').css('display', 'block');
                checker = false;
            }
            if (!R.BuyAllCart.Email) {
                $('.small-warning-email').css('display', 'block');
                checker = false;
            }
            if (!R.BuyAllCart.PickUpPoint) {
                $('.small-warning-pickup-points').css('display', 'block');
                checker = false;
            }
            $('input[type=checkbox]').each((i, v) => {
                if ($(v).prop('checked') == false) {
                    $(v).closest('.custom-checkbox-container').find('.warning-checkbox').css('display', 'block');
                }
                if ($(v).prop('checked') == true) {
                    $(v).closest('.custom-checkbox-container').find('.warning-checkbox').css('display', 'none');
                }
            })
            $('input[type=checkbox]').each((i, v) => {
                if ($(v).prop('checked') == false) {
                    $(v).closest('.custom-checkbox-container').find('.warning-checkbox').css('display', 'block');
                }
                if ($(v).prop('checked') == true) {
                    $(v).closest('.custom-checkbox-container').find('.warning-checkbox').css('display', 'none');
                }
            })
            $('input[type=checkbox]').each((i, v) => {
                if ($(v).prop('checked') == false) {
                    checker = false;
                }
                return;
            })
            //console.log(request);
            if (checker == true) {
                //R.BuyAllCart.RequestPaypal(request);
                window.sessionStorage.setItem("currentRequest", JSON.stringify())
                console.log(R.BuyAllCart.PaymentShowcase)
                var orderInfoStorage = {
                    showcase: R.BuyAllCart.PaymentShowcase,
                    email: R.BuyAllCart.Email
                }
                sessionStorage.setItem("orderInfoStorage", JSON.stringify(orderInfoStorage));

                sessionStorage.setItem("orderInfomation", JSON.stringify(request));
                if (R.BuyAllCart.PaymentShowcase.method == "onepay") {
                    R.BuyAllCart.RequestOnePay(request);
                }
                if (R.BuyAllCart.PaymentShowcase.method == "paypal") {
                    R.BuyAllCart.RequestPaypal(request);
                }
                if (R.BuyAllCart.PaymentShowcase.method == "cod") {
                    R.BuyAllCart.RequestOrderV2(request);
                }

            }
            else {
                $('html, body').animate({
                    scrollTop: $('.payment-info').offset().top
                }, 800); // Thời gian cuộn (milliseconds)
                return null;
            }

            //


            //Luu Id khach hang vao LocalStorage


            //Tien hanh doi menu voi khach hang

            // Tao ra mot don hang voi ma san pham, gia tien 

        })

        $('.inner-button-choose').off('click').on('click', function () {
            var _img = $(this).closest('.inner-item').find('img').attr('src');
            var _text = $(this).closest('.inner-item').find('.inner-title').text();
            var _showcase = $(this).closest('.inner-item').find('.inner-title').data('showcase');
            R.BuyAllCart.PaymentShowcase = {
                img: _img,
                text: _text,
                method: _showcase
            }
            R.BuyAllCart.ChangePaymentShowcase();
            $('#modalChoosePayment').modal('hide')
        })
        $('#card-holder-number').on('keyup', function () {
            R.BuyAllCart.OnFormatCardNumber();
        })
        $('#card-holder-name').on('keyup', function () {
            R.BuyAllCart.OnFormatCardName();
        })
        $('#card-holder-date').on('keyup', function () {
            R.BuyAllCart.OnFormatCardExpired();
        })
        $('#card-holder-cvv').on('keyup', function () {
            R.BuyAllCart.OnFormatCVV();
        })
        $('.form-add-card').on('submit', function (e) {
            e.preventDefault();
            var checker = true; //Kiem tra the
            if (checker) {
                var cardNo = $('#card-holder-number').val();
                var cardNoShowing = `**** ${cardNo.split(' ')[cardNo.split(' ').length - 1]}`
                R.BuyAllCart.PaymentShowcase = {
                    img: "assets/images/icons/icon-mastercard.svg",
                    text: cardNoShowing,
                    method: "CARD"
                }
                R.BuyAllCart.ChangePaymentShowcase()
            }
            $('#modalAddPayment').modal('hide');
            $('#modalChoosePayment').modal('hide');
            return null;
        })
        $('.pickup-point-item').off('click').on('click', function () {
            var checkIcon = `<i class="fa-solid fa-circle-check"></i>`;
            $('.pickup-point-item').each((i, v) => {
                if ($(v).find('i')) {
                    var i = $(v).find('i')
                    i.remove()
                }
            })
            $(this).append(checkIcon)

        })
        $('.input-pickup-point').off('keyup').on('keyup', function () {
            var _value = $(this).val();
            var filterd = R.BuyAllCart.PickupPoints.filter(element => element.toLowerCase().includes(_value.toLowerCase()));
            R.BuyAllCart.LoadPickupPoint(filterd)
        })
        $('.btn-save-info-pickup').off('click').on('click', function () {
            var email = $('.input-email').val();

            R.BuyAllCart.Email = email;

            R.BuyAllCart.LoadEmailItemButton();
            //R.BuyAllCart.LoadPickupItemButton();


        })
        $('.btn-save-info-pickup-location').off('click').on('click', function () {

            var pickupPoint = "";
            var i = $('.pickup-point-item').find('i');
            if (i) {
                pickupPoint = $(i).closest('a').text();
            }

            R.BuyAllCart.PickUpPoint = pickupPoint;
            //alert(R.BuyAllCart.PickUpPoint);
            //R.BuyAllCart.LoadEmailItemButton();
            R.BuyAllCart.LoadPickupItemButton();
        })
        $('.btn-save-voucher-code').off('click').on('click', function () {
            //Sua nhanh voucher su dung cho Serial number
            //Ve sau se phai tao lai sau
            var modal = $(this).closest('.modal');
            var voucher_code = $(this).closest('.modal').find('.input-voucher').val();
            var url = `Order/CheckSerialNumber?serialNumber=${voucher_code}`;
            $.get(url).then((response) => {
                console.log(response);
                if (response) {
                    sessionStorage.setItem('current_serial', JSON.stringify(response));
                    R.BuyAllCart.SerialNumber = response.serialNumber;
                    Swal.fire({
                        title: "Success!",
                        text: `Verified Serial Number Successful! You get a discount on each subsequent order when you reuse the SIM Card!`,
                        icon: "success"
                    }).then((result) => {
                        if (result.isConfirmed) {
                            $(modal).modal('hide');
                            $('.button-serial-modal-text').text(response.serialNumber)
                            //R.BuyAllCart.LoadProductTotalPrice();
                            R.BuyAllCart.KeepSerialNumberInDetail();
                        }
                    });
                }
            })
        })
    },
    InitSlick: function () {
        $('.slide-buy-cart-item').slick({
            dots: true,
            infinite: false,
            speed: 300,
            slidesToShow: 2,
            slidesToScroll: 2,
            responsive: [
                {
                    breakpoint: 1024,
                    settings: {
                        slidesToShow: 2,
                        slidesToScroll: 2,
                        infinite: true,
                        dots: true
                    }
                },
                {
                    breakpoint: 600,
                    settings: {
                        slidesToShow: 1,
                        slidesToScroll: 1
                    }
                },
                {
                    breakpoint: 480,
                    settings: {
                        slidesToShow: 1,
                        slidesToScroll: 1
                    }
                }
                // You can unslick at a given breakpoint now by adding:
                // settings: "unslick"
                // instead of a settings object
            ]
        });
    },
    LoadProductQuantity: function () {
        var carts = R.LoadCart();
        $('.sim-item').each((index, element) => {
            carts.forEach((c) => {
                if (c.productId == $(element).data('id')) {
                    $(element).find('.inner-quantity').text(c.quantity)
                }
            })
        })
    },
    LoadProductTotalPrice: function () {
        R.BuyAllCart.TotalPrice = 0;
        var carts = R.LoadCart();
        $('.sim-item').each((index, element) => {
            carts.forEach((c) => {
                if (c.productId == $(element).data('id')) {
                    var priceEach = $(element).data('price-each');
                    console.log(c.quantity * priceEach)
                    R.BuyAllCart.TotalPrice = R.BuyAllCart.TotalPrice + (c.quantity * priceEach);
                }
            })
        })

        //alert(R.BuyAllCart.TotalPrice)
        //Parse tong to html
        $('.total-price').html(`VND ${R.FormatNumber(R.BuyAllCart.TotalPrice)}`);
        $('.total-price').data('price', parseFloat(R.BuyAllCart.TotalPrice.toFixed(2)))
        $('.inner-mobile-price').data('price', parseFloat(R.BuyAllCart.TotalPrice.toFixed(2)))
        $('.inner-mobile-price-detail').html(`${R.FormatNumber(R.BuyAllCart.TotalPrice)}`)
    },

    UpdatePrice: function () {
        var quantity = parseInt($('.inner-quantity').text());
        var totalPrice = parseInt($('.total-price').data('price'));
        //alert(quantity)
        //alert(totalPrice)
        totalPrice = totalPrice * quantity;
        /*alert(totalPrice)*/
        //Change 

        $('.total-price').data('price', totalPrice);
        $('.total-price').text('').text(`VND ${R.FormatNumber(totalPrice)}`)
        R.BuyAllCart.RegisterEvent();
    },
    CheckPaymentStatus: function () {

        var $_culture_mess = $('#culture-modal-payment');

        var success_title = $_culture_mess.data('success-title')
        var success_left = $_culture_mess.data('success-left')
        var success_right = $_culture_mess.data('success-right')
        var success_nomail = $_culture_mess.data('success-nomail')
        var error_title = $_culture_mess.data('error-title');
        var error_desc = $_culture_mess.data('error-desc');
        var button = $_culture_mess.data('button');
        var request = null;
        var requestString = sessionStorage.getItem("orderInfomation");
        if (requestString) {
            request = JSON.parse(requestString)
        }
        //Check status voi Onepay
        var url = new URL(window.location.href);

        // Kiểm tra xem query parameter `vpc_TxnResponseCode` có tồn tại không
        // Lấy giá trị của query parameter `vpc_TxnResponseCode`
        const txnResponseCode = url.searchParams.get('vpc_TxnResponseCode');
        const payPalToken = url.searchParams.get('token');
        const payerId = url.searchParams.get('PayerID');
        console.log(txnResponseCode)
        //currentOrderCode
        var currentOrderrCode = sessionStorage.getItem('currentOrderCode');
        //Xoa luon currrentOrderCcode 

        // Kiểm tra giá trị
        if (txnResponseCode && currentOrderrCode && request) {
            if (txnResponseCode === '0') {

                R.BuyAllCart.RequestOrderV2(request);
                ////Thanh cong roi
                //var checker = false;
                //var message = "";
                //if (R.LoadCustomerInfo()) {
                //    checker = true;
                //}

                ////Luu localStorage
                //if (checker == false) {
                //    message = `${success_left} ${customerInfo.email}. ${success_right}`;
                //}
                //else {
                //    message = `${success_nomail}`;
                //}
                ////R.ShowLoadingScreen();
                //Swal.fire({
                //    title: success_title,
                //    text: message,
                //    icon: "success",
                //    confirmButtonText: button
                //}).then((result) => {
                //    if (result.isConfirmed) {
                //        // Chuyển sang trang web khác

                //        localStorage.removeItem('onPickingOrder');
                //        sessionStorage.removeItem('current_serial')
                //        window.location.href = '/my-sim';
                //        sessionStorage.removeItem('currentOrderCode');
                //        sessionStorage.removeItem('current_serial');
                //        R.HideLoadingScreen();
                //    }
                //});

            } else {
                message = error_desc
                Swal.fire({
                    title: error_title,
                    text: message,
                    icon: "error",
                    confirmButtonText: button
                }).then((result) => {
                    sessionStorage.removeItem('currentOrderCode');
                    sessionStorage.removeItem('current_serial');
                    sessionStorage.removeItem('orderInfomation');

                });

            }
        }

        if (payPalToken && payerId && request) {
            //Kiem tra xem token thanh cong hay that bai
            let url = `/Order/PayPalCallbackAsync?token=${payPalToken}&PayerID=${payerId}`
            $(document).ajaxSend(function () {
                // Hiển thị màn hình loading ở đây
                R.ShowLoadingScreen();
            });
            $.get(url).then((response) => {
                console.log(response)
                R.BuyAllCart.RequestOrderV2(request);
                //R.HideLoadingScreen();
                ////Thanh cong roi
                //var checker = false;
                //var message = "";
                //if (R.LoadCustomerInfo()) {
                //    checker = true;
                //}

                ////Luu localStorage
                //if (checker == false) {
                //    message = `${success_left} ${customerInfo.email}. ${success_right}`;
                //}
                //else {
                //    message = `${success_nomail}`;
                //}
                ////R.ShowLoadingScreen();
                //Swal.fire({
                //    title: success_title,
                //    text: message,
                //    icon: "success",
                //    confirmButtonText: button
                //}).then((result) => {
                //    if (result.isConfirmed) {
                //        // Chuyển sang trang web khác

                //        localStorage.removeItem('onPickingOrder');
                //        sessionStorage.removeItem('current_serial')
                //        window.location.href = '/my-sim';
                //        sessionStorage.removeItem('currentOrderCode');
                //        sessionStorage.removeItem('current_serial');

                //    }
                //});
            }).catch((err) => {
                message = error_desc
                Swal.fire({
                    title: error_title,
                    text: message,
                    icon: "error",
                    confirmButtonText: button
                }).then((result) => {
                    sessionStorage.removeItem('currentOrderCode');
                    sessionStorage.removeItem('current_serial');
                });
            })
        }

    },
    LoadAllLocations: function () {
        $.get('/Home/GetLocations').then((response) => {
            console.log(response);
            response.forEach((v) => {
                R.BuyAllCart.PickupPoints.push(v.name);
            })
            R.BuyAllCart.LoadPickupPoint(R.BuyAllCart.PickupPoints);

        })
    },
    KeepSerialNumberInDetail: function () {
        if (sessionStorage.getItem('current_serial')) {
            $('.sim-package-detail').each((index, element) => {
                //Tim li gia
                if ($(element).data('sim-pack') != 'Package' && $(element).data('sim-type') != "E-Sim") {
                    var currentSerial = JSON.parse(sessionStorage.getItem('current_serial'));
                    $('.button-serial-modal-text').text(currentSerial.serialNumber)
                    $('.inner-change-item-ccid').removeClass('inner-change-item-ccid');
                    //after-price
                    var price = $('.after-price').data('price');
                    var afterPrice = price - 15000;
                    $('.after-price').data('price', afterPrice);
                    $('.after-price').data('serial-number', currentSerial.serialNumber);
                    var unit = $('.after-price').data('unit');
                    var _html = `${unit} ${R.FormatNumber(afterPrice)}`;
                    $('.after-price').text(_html.replace(",", ".").replace(',', '.'))
                    $('.inner-mobile-price').html(_html.replace(",", ".").replace(',', '.'));


                    $(element).find('.current-price').css('text-decoration', 'line-through');
                    $(element).find('.down-price').removeClass('down-price');
                    $(element).find('.hide-current-price').css('display', 'none');

                    $('.inner-quantity').text('').text('1');
                    $('.inner-quantity').closest('li').remove();
                }
            })
            //R.BuyAllCart.PickUpPoint
            R.BuyAllCart.PickUpPoint = "TOP UP"
            $('.pickup-block').css('display', 'none');
            //console.log(currentSerial)
        }
    },
    ChangePaymentShowcase: function () {
        var html = `
            <img src="${R.BuyAllCart.PaymentShowcase.img}"> <span>${R.BuyAllCart.PaymentShowcase.text}</span>
        `
        $('.payment-show-case').html('').html(html);
        $('.small-warning-payment').css('display', 'none');
        R.BuyAllCart.RegisterEvent();
    },
    OnFormatCardNumber: function () {

        var input = document.getElementById('card-holder-number');
        if (input) {
            // Xóa các ký tự không phải số
            let formattedValue = input.value.replace(/\D/g, '');

            // Chia thành các phần có 4 số, cách nhau bởi dấu cách
            formattedValue = formattedValue.replace(/(\d{4})/g, '$1 ').trim();

            // Giới hạn độ dài tối đa là 19 ký tự (bao gồm cả dấu cách)
            if (formattedValue.length > 19) {
                formattedValue = formattedValue.slice(0, 19);
            }

            // Cập nhật giá trị vào ô input
            input.value = formattedValue;
        }

    },
    OnFormatCardName: function () {
        let input = $('#card-holder-name');
        // Chỉ giữ lại các ký tự là chữ cái và khoảng trắng
        let formattedValue = input.val().replace(/[^a-zA-Z\s]/g, '');

        // Giới hạn độ dài tối đa là 50 ký tự
        if (formattedValue.length > 50) {
            formattedValue = formattedValue.slice(0, 50);
        }

        // Cập nhật giá trị vào ô input
        input.val(formattedValue);
    },
    OnFormatCardExpired: function () {
        let input = $('#card-holder-date');
        // Chỉ giữ lại các ký tự số và dấu /
        let formattedValue = input.val().replace(/[^\d/]/g, '');

        // Giới hạn độ dài tối đa là 7 ký tự (bao gồm cả dấu /)
        if (formattedValue.length > 7) {
            formattedValue = formattedValue.slice(0, 7);
        }

        // Chèn dấu / sau khi nhập xong tháng nếu chưa có
        if (formattedValue.length >= 2 && formattedValue.indexOf('/') === -1) {
            formattedValue = formattedValue.slice(0, 2) + '/' + formattedValue.slice(2);
        }

        // Cập nhật giá trị vào ô input
        input.val(formattedValue);
    },
    OnFormatCVV: function () {
        let input = $('#card-holder-cvv');
        // Chỉ giữ lại các ký tự số
        let formattedValue = input.val().replace(/\D/g, '');

        // Giới hạn độ dài tối đa là 4 ký tự
        if (formattedValue.length > 4) {
            formattedValue = formattedValue.slice(0, 4);
        }

        // Cập nhật giá trị vào ô input
        input.val(formattedValue);
    },
    RandomString: function () {
        var chuoi = '';
        var kiTu = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
        for (var i = 0; i < 8; i++) {
            chuoi += kiTu.charAt(Math.floor(Math.random() * kiTu.length));
        }
        return chuoi;
    },
    RequestOrder: function (request) {
        console.log(request)

        var $_culture_mess = $('#culture-modal-payment');

        var success_title = $_culture_mess.data('success-title')
        var success_left = $_culture_mess.data('success-left')
        var success_right = $_culture_mess.data('success-right')
        var success_nomail = $_culture_mess.data('success-nomail')
        var error_title = $_culture_mess.data('error-title');
        var error_desc = $_culture_mess.data('error-desc');
        var button = $_culture_mess.data('button');
        var url = "/Order/CreateOrderVersionMinify";
        // Kích hoạt màn hình loading trước khi gửi yêu cầu AJAX
        $(document).ajaxSend(function () {
            // Hiển thị màn hình loading ở đây
            R.ShowLoadingScreen();
        });

        $.post(url, request)
            .then((response) => {
                // Xử lý phản hồi ở đây
                console.log(response)
                //Thanh cong roi
                var checker = false;
                var message = "";
                if (R.LoadCustomerInfo()) {
                    checker = true;
                }
                var customerInfo = {
                    randomId: request.customerRandomId,
                    id: parseInt(response),
                    email: R.BuyAllCart.Email,
                    pickupPoint: R.BuyAllCart.PickUpPoint
                }
                //Save Local Storage
                localStorage.setItem('customerInfo', JSON.stringify(customerInfo));
                //Luu localStorage
                if (checker == false) {
                    message = `${success_left} ${customerInfo.email}. ${success_right}`;
                }
                else {
                    message = success_nomail;
                }

                Swal.fire({
                    title: success_title,
                    text: message,
                    icon: "success",
                    confirmButtonText: button
                }).then((result) => {
                    if (result.isConfirmed) {
                        // Chuyển sang trang web khác

                        localStorage.removeItem('onPickingOrder');
                        sessionStorage.removeItem('current_serial')
                        sessionStorage.removeItem('currentOrderCode');
                        sessionStorage.removeItem('current_serial');
                        window.location.href = '/my-sim';
                    }
                });
            })
            .catch((err) => {
                // Xử lý lỗi ở đây
                console.log(err)
            })
            .always(() => {
                // Ẩn màn hình loading sau khi nhận phản hồi hoặc xảy ra lỗi
                R.HideLoadingScreen();
            });
    },
    RequestOrderV2: function (request) {
        console.log(request)

        var $_culture_mess = $('#culture-modal-payment');

        var success_title = $_culture_mess.data('success-title')
        var success_left = $_culture_mess.data('success-left')
        var success_right = $_culture_mess.data('success-right')
        var success_nomail = $_culture_mess.data('success-nomail')
        var error_title = $_culture_mess.data('error-title');
        var error_desc = $_culture_mess.data('error-desc');
        var button = $_culture_mess.data('button');
        var url = "/Order/CreateOrderVersionMinifyV2";
        // Kích hoạt màn hình loading trước khi gửi yêu cầu AJAX
        $(document).ajaxSend(function () {
            // Hiển thị màn hình loading ở đây
            R.ShowLoadingScreen();
        });

        $.post(url, request)
            .then((response) => {
                // Xử lý phản hồi ở đây
                console.log(response)
                //Thanh cong roi
                var checker = false;
                var message = "";
                if (R.LoadCustomerInfo()) {
                    checker = true;
                }
                var customerInfo = {
                    randomId: request.customerRandomId,
                    id: parseInt(response),
                    email: R.BuyAllCart.Email,
                    pickupPoint: R.BuyAllCart.PickUpPoint
                }
                //Save Local Storage
                localStorage.setItem('customerInfo', JSON.stringify(customerInfo));
                //Luu localStorage
                if (checker == false) {
                    message = `${success_left} ${customerInfo.email}. ${success_right}`;
                }
                else {
                    message = success_nomail;
                }

                Swal.fire({
                    title: success_title,
                    text: message,
                    icon: "success",
                    confirmButtonText: button
                }).then((result) => {
                    if (result.isConfirmed) {
                        localStorage.removeItem('onPickingOrder');
                        sessionStorage.removeItem('current_serial')
                        sessionStorage.removeItem('currentOrderCode');
                        sessionStorage.removeItem('current_serial');
                        sessionStorage.removeItem('orderInfomation');
                        window.location.href = '/my-sim';
                    }
                });
            })
            .catch((err) => {
                // Xử lý lỗi ở đây
                console.log(err)
            })
            .always(() => {
                // Ẩn màn hình loading sau khi nhận phản hồi hoặc xảy ra lỗi
                R.HideLoadingScreen();
            });
    },
    RequestPaypal: function (request) {

        var url = "/Order/ProcessPaypal";

        $.post(url, request).then(response => {
            window.location.href = response;
        }).catch((err) => {
            // Xử lý lỗi ở đây
            console.log(err)
        })
            .always(() => {
                // Ẩn màn hình loading sau khi nhận phản hồi hoặc xảy ra lỗi
                R.HideLoadingScreen();
            });


        //var url = "/Order/CreateOrderVersionMinify";
        //// Kích hoạt màn hình loading trước khi gửi yêu cầu AJAX
        //$(document).ajaxSend(function () {
        //    // Hiển thị màn hình loading ở đây
        //    R.ShowLoadingScreen();
        //});

        //$.post(url, request)
        //    .then((response) => {
        //        // Xử lý phản hồi ở đây
        //        console.log(response)
        //        //Thanh cong roi
        //        var checker = false;
        //        var message = "";
        //        if (R.LoadCustomerInfo()) {
        //            checker = true;
        //        }
        //        var customerInfo = {
        //            randomId: request.customerRandomId,
        //            id: parseInt(response),
        //            email: R.BuyAllCart.Email,
        //            pickupPoint: R.BuyAllCart.PickUpPoint
        //        }
        //        //Save Local Storage
        //        localStorage.setItem('customerInfo', JSON.stringify(customerInfo));
        //        var url = "/Order/ProcessPaypal";

        //        $.post(url, request).then(response => {
        //            window.location.href = response;
        //        }).catch((err) => {
        //            // Xử lý lỗi ở đây
        //            console.log(err)
        //        })
        //            .always(() => {
        //                // Ẩn màn hình loading sau khi nhận phản hồi hoặc xảy ra lỗi
        //                R.HideLoadingScreen();
        //            });
        //        ////Luu localStorage
        //        //if (checker == false) {
        //        //    message = `We created for you a account with email ${customerInfo.email}. Please check your Email to get Receipt and update adj in future!`;
        //        //}
        //        //else {
        //        //    message = "Success!Thank you for support us!";
        //        //}

        //        //Swal.fire({
        //        //    title: "Success!",
        //        //    text: message,
        //        //    icon: "success"
        //        //}).then((result) => {
        //        //    if (result.isConfirmed) {
        //        //        // Chuyển sang trang web khác

        //        //        localStorage.removeItem('onPickingOrder');
        //        //        sessionStorage.removeItem('current_serial')
        //        //        window.location.href = '/my-sim';
        //        //    }
        //        //});
        //    })
        //    .catch((err) => {
        //        // Xử lý lỗi ở đây
        //        console.log(err)
        //    })
        //    .always(() => {
        //        // Ẩn màn hình loading sau khi nhận phản hồi hoặc xảy ra lỗi
        //        R.HideLoadingScreen();
        //    });


    },
    RequestOnePay: function (request) {

        $.post('/Order/ProcessOnePay', request).then((response) => {
            //Tao 1 orderCode tam thoi

            sessionStorage.setItem('currentOrderCode', request.orderCode);
            window.location.href = response;
        })

        //var url = "/Order/CreateOrderVersionMinify";
        //// Kích hoạt màn hình loading trước khi gửi yêu cầu AJAX
        //$(document).ajaxSend(function () {
        //    // Hiển thị màn hình loading ở đây
        //    R.ShowLoadingScreen();
        //});

        //$.post(url, request)
        //    .then((response) => {
        //        // Xử lý phản hồi ở đây
        //        console.log(response)
        //        //Thanh cong roi
        //        var checker = false;
        //        var message = "";
        //        if (R.LoadCustomerInfo()) {
        //            checker = true;
        //        }
        //        var customerInfo = {
        //            randomId: request.customerRandomId,
        //            id: parseInt(response),
        //            email: R.BuyAllCart.Email,
        //            pickupPoint: R.BuyAllCart.PickUpPoint
        //        }
        //        //Save Local Storage
        //        localStorage.setItem('customerInfo', JSON.stringify(customerInfo));
        //        $.post('/Order/ProcessOnePay', request).then((response) => {
        //            //Tao 1 orderCode tam thoi

        //            sessionStorage.setItem('currentOrderCode', request.orderCode);
        //            window.location.href = response;
        //        })
        //        ////Luu localStorage
        //        //if (checker == false) {
        //        //    message = `We created for you a account with email ${customerInfo.email}. Please check your Email to get Receipt and update adj in future!`;
        //        //}
        //        //else {
        //        //    message = "Success!Thank you for support us!";
        //        //}

        //        //Swal.fire({
        //        //    title: "Success!",
        //        //    text: message,
        //        //    icon: "success"
        //        //}).then((result) => {
        //        //    if (result.isConfirmed) {
        //        //        // Chuyển sang trang web khác

        //        //        localStorage.removeItem('onPickingOrder');
        //        //        sessionStorage.removeItem('current_serial')
        //        //        window.location.href = '/my-sim';
        //        //    }
        //        //});
        //    })
        //    .catch((err) => {
        //        // Xử lý lỗi ở đây
        //        console.log(err)
        //    })
        //    .always(() => {
        //        // Ẩn màn hình loading sau khi nhận phản hồi hoặc xảy ra lỗi
        //        R.HideLoadingScreen();
        //    });


        ///*
        //    var request = {
        //        customerRandomId: customerRandomId,
        //        totalPrice: totalPrice,
        //        productId: productId,
        //        orderCode: orderCode,
        //        serialNumber: serialNumber,
        //        email: R.BuyAllCart.Email,
        //        pickupPoint: R.BuyAllCart.PickUpPoint,
        //        isCreatedAccount: isCreatedAccount,
        //        orderNote: `Pickup Point: ${R.BuyAllCart.PickUpPoint}`
        //    }
        //*/
    },
    LoadPickupPoint: function (arrPickupPoints) {

        var ul = $('.inner-list-ul');
        ul.html('')

        arrPickupPoints.forEach((element) => {
            var html = `
                <li >
                    <a href="javascript:void(0)" class="pickup-point-item">
                        <span class="pickup-point-value">${element}</span>
                    </a>
                </li>
            `
            ul.append(html);
        })
        R.BuyAllCart.RegisterEvent();
    },
    LoadPickupItemButton: function () {

        if (R.BuyAllCart.PickUpPoint != "") {
            var html = `Pickup Point: ${R.BuyAllCart.PickUpPoint}`
            $('.inner-desc-thong-tin-bo-sung-pickup-point').html('').html(html);
            $('#modalNhamSim').modal('hide');
            $('.small-warning-pickup-points').css('display', 'none');
        }
        R.BuyAllCart.RegisterEvent();

    },
    LoadEmailItemButton: function () {
        var customer = R.LoadCustomerInfo();
        if (customer) {
            R.BuyAllCart.Email = customer.email;
            //R.BuyAllCart.PickUpPoint = customer.pickupPoint.trim().replace('\\n', '');
        }
        if (R.BuyAllCart.Email != "") {
            var html = `Email: ${R.BuyAllCart.Email}`
            $('.inner-desc-thong-tin-bo-sung-email').html('').html(html);
            $('#modalNhamSim').modal('hide');
            //modalEmailNhanSim
            $('#modalEmailNhanSim').modal('hide');

            $('.small-warning-email').css('display', 'none');
        }
        R.BuyAllCart.RegisterEvent();
    },

    



}
R.BuyAllCart.Init();