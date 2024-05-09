
R.ProductDetail = {
    Init: function () {
        R.ProductDetail.RegisterEvent();
        R.ProductDetail.culture = R.Culture();
        R.ProductDetail.PaymentShowcase = null;
        R.ProductDetail.CurrentUrl = window.location.href;
        if (window.location.href.includes('local') || window.location.href.includes('demo')) {

            $('.cod-hide').removeClass('cod-hide');
        }
        
        R.ProductDetail.current_location = R.CurrentLocationId();
        R.ProductDetail.PickupPoints = []
        R.ProductDetail.LoadAllLocations();
        R.ProductDetail.CheckPaymentStatus();
        
       
        R.ProductDetail.LoadPickupPoint(R.ProductDetail.PickupPoints);
        R.ProductDetail.Email = "";
        R.ProductDetail.PickUpPoint = "";

        R.ProductDetail.CurrrentUser = R.LoadCustomerInfo();
        console.log(R.ProductDetail.CurrrentUser)
        if (R.ProductDetail.CurrrentUser) {
            R.ProductDetail.Email = R.ProductDetail.CurrrentUser.email;
            $('.input-email').val(R.ProductDetail.Email)
        }

        R.ProductDetail.PaymentMethod = "";
        R.ProductDetail.Voucher = null;
        R.ProductDetail.DiscountOption = null;
        R.ProductDetail.DiscountValue = null;
        R.ProductDetail.LoadEmailItemButton();
        R.ProductDetail.KeepSerialNumberInDetail();
        R.ProductDetail.DiscountCode = ""
        
        
        //$('#txt-time-input').date
    },
    
    RegisterEvent: function () {
        $('input[triggerTar]').off('keypress').on('keypress',function (e) {
            // Kiểm tra nếu phím được nhấn là Enter (keyCode của Enter là 13)
            if (e.which == 13) {
                // Tìm đến thẻ cha gần nhất có class là `modal`
                var parentModal = $(this).closest('.modal');
                // Trong thẻ cha này, tìm button có thuộc tính `triggerSrc` và kích hoạt sự kiện click
                parentModal.find('button[triggerSrc]').click();
            }
        });
        $('form').submit(false);
        $('.add-quantity').off('click').on('click', function () {
            //var id = $(this).closest('.sim-item').data('id');
            //var carts = R.LoadCart();
            //carts.forEach((element) => {
            //    if (element.id == id)
            //        element.quantity++;
            //})
            //Doi text
            var current = parseInt($(this).parent().find('.inner-quantity').text());
            $(this).parent().find('.inner-quantity').html('').html(current + 1);
            R.ProductDetail.UpdatePrice();
            //Save carts
            //localStorage.setItem('sim-carts', JSON.stringify(carts));
            //R.BuyAllCart.LoadProductTotalPrice();
            //console.log(R.LoadCart())

        })
        $('.minus-quantity').off('click').on('click', function () {
            //var id = $(this).closest('.sim-item').data('id');
            //var carts = R.LoadCart();
            //carts.forEach((element) => {
            //    if (element.id == id)
            //        if (element.quantity > 1)
            //            element.quantity--;
            //})
            //Doi text
            var current = parseInt($(this).parent().find('.inner-quantity').text());
            if (current > 1) {
                $(this).parent().find('.inner-quantity').html('').html(current - 1);
                ////Save carts
                //localStorage.setItem('sim-carts', JSON.stringify(carts));
                ////console.log(R.LoadCart())
                //R.BuyAllCart.LoadProductTotalPrice();
            }
            R.ProductDetail.UpdatePrice();


        })
        $('.remove-sim-item').off('click').on('click', function () {
            var id = $(this).closest('.sim-item').data('id');
            //Xoa ca khoi
            $(this).closest('.sim-item').remove();
            R.RemoveFromCart(id);
        })
        $('.custom-checkbox:last a:last').off('click').on('click', function () {
            //alert('AN')
            $('#modalEsimDevice').modal('show')
        })
        $('.order-complete').off('click').on('click', function () {

            //Tao ra mot khach hang Id voi random 8 ky tu
            var currentCustomer = R.LoadCustomerInfo();
            var isCreatedAccount = false;
            console.log(currentCustomer)
            var customerRandomId = "";
            if (currentCustomer) {
                customerRandomId = currentCustomer.randomId;
            } else {
                customerRandomId = R.ProductDetail.RandomString();
                isCreatedAccount = true;
            }
            
            //Ghi nhan don hang voi Id do

            var totalPrice = $('.total-price').data('price');
            var productId = $('.total-price').data('id');
            var orderCode = R.ProductDetail.RandomString();

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
                R.ProductDetail.PickUpPoint = "ESIM"
            }
            if (type == "TOPUP") {
                R.ProductDetail.PickUpPoint = "TOP-UP"
            }
            var quantity = parseInt($('.inner-quantity').text() ?? "0");
            console.log(quantity);
            if (quantity == 0 || isNaN(quantity)) {
                quantity = 1;
            }
            //alert(currentUrl);
            var request = {
                customerRandomId: customerRandomId,
                totalPrice: totalPrice,
                productId: productId,
                orderCode: orderCode,
                serialNumber: serialNumber,
                email: R.ProductDetail.Email,
                pickupPoint: R.ProductDetail.PickUpPoint,
                isCreatedAccount: isCreatedAccount,
                orderNote: `Pickup Point: ${R.ProductDetail.PickUpPoint}`,
                returnUrl: currentUrl,
                productsInfo: [{
                    productId: productId,
                    quantity: quantity,
                    serialNumber: serialNumber,
                    totalPrice: totalPrice / quantity,
                    type: type
                }],
                discountValue: R.ProductDetail.DiscountValue,
                discountOption: R.ProductDetail.DiscountOption,
                discountCode: R.ProductDetail.DiscountCode
            }
            var checker = true;
            if (!R.ProductDetail.PaymentShowcase) {
                $('.small-warning-payment').css('display', 'block');
                checker = false;
            }
            if (!R.ProductDetail.Email) {
                $('.small-warning-email').css('display', 'block');
                checker = false;
            }
            if (!R.ProductDetail.PickUpPoint) {
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
                //R.ProductDetail.RequestPaypal(request);
                window.sessionStorage.setItem("currentRequest", JSON.stringify())
                console.log(R.ProductDetail.PaymentShowcase)
                var orderInfoStorage = {
                    showcase : R.ProductDetail.PaymentShowcase,
                    email : R.ProductDetail.Email
                }
                sessionStorage.setItem("orderInfoStorage", JSON.stringify(orderInfoStorage));

                sessionStorage.setItem("orderInfomation", JSON.stringify(request));
                if (R.ProductDetail.PaymentShowcase.method == "onepay") {
                    R.ProductDetail.RequestOnePay(request);
                }
                if (R.ProductDetail.PaymentShowcase.method == "paypal") {
                    R.ProductDetail.RequestPaypal(request);
                }
                if (R.ProductDetail.PaymentShowcase.method == "cod") {
                    R.ProductDetail.RequestOrderV2(request);
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
            R.ProductDetail.PaymentShowcase = {
                img: _img,
                text: _text,
                method: _showcase
            }
            R.ProductDetail.ChangePaymentShowcase();
            $('#modalChoosePayment').modal('hide')
        })
        $('#card-holder-number').on('keyup', function () {
            R.ProductDetail.OnFormatCardNumber();
        })
        $('#card-holder-name').on('keyup', function () {
            R.ProductDetail.OnFormatCardName();
        })
        $('#card-holder-date').on('keyup', function () {
            R.ProductDetail.OnFormatCardExpired();
        })
        $('#card-holder-cvv').on('keyup', function () {
            R.ProductDetail.OnFormatCVV();
        })
        $('.form-add-card').on('submit', function (e) {
            e.preventDefault();
            var checker = true; //Kiem tra the
            if (checker) {
                var cardNo = $('#card-holder-number').val();
                var cardNoShowing = `**** ${cardNo.split(' ')[cardNo.split(' ').length - 1]}`
                R.ProductDetail.PaymentShowcase = {
                    img: "assets/images/icons/icon-mastercard.svg",
                    text: cardNoShowing,
                    method: "CARD"
                }
                R.ProductDetail.ChangePaymentShowcase()
            }
            $('#modalAddPayment').modal('hide');
            $('#modalChoosePayment').modal('hide');
            return null;
        })
        $('.pickup-point-item').off('click').on('click', function() {
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
            var filterd = R.ProductDetail.PickupPoints.filter(element => element.toLowerCase().includes(_value.toLowerCase()));
            R.ProductDetail.LoadPickupPoint(filterd)
        })
        $('.btn-save-info-pickup').off('click').on('click', function () {
            var email = $('.input-email').val();
            
            R.ProductDetail.Email = email;
            
            R.ProductDetail.LoadEmailItemButton();
            //R.ProductDetail.LoadPickupItemButton();
            

        })
        $('.btn-save-info-pickup-location').off('click').on('click', function () {
           
            var pickupPoint = "";
            var i = $('.pickup-point-item').find('i');
            if (i) {
                pickupPoint = $(i).closest('a').text();
            }
           
            R.ProductDetail.PickUpPoint = pickupPoint;
            //alert(R.ProductDetail.PickUpPoint);
            //R.ProductDetail.LoadEmailItemButton();
            R.ProductDetail.LoadPickupItemButton();
        })
        $('.input-voucher').on('focusout', function (event) {
            event.stopImmediatePropagation(); // Ngăn chặn sự kiện khác được gọi
            var voucher_code = $('.input-voucher').val();
            var modal = $(this).closest('.modal');
            if (voucher_code == "") {
                $(modal).modal('hide');
                $('.button-serial-modal-text').text('')
                //R.ProductDetail.LoadProductTotalPrice();
                R.ProductDetail.RemoveSerialNumberInDetail();
                return;
            }

        })
        $('.btn-save-voucher-code').off('click').on('click', function () {
            //Sua nhanh voucher su dung cho Serial number
            //Ve sau se phai tao lai sau
            var modal = $(this).closest('.modal');
            var voucher_code = $(this).closest('.modal').find('.input-voucher').val();
            R.ProductDetail.RequsetSerialNumber(voucher_code, modal)
        })
        $('.btn-save-coupon-code').off('click').on('click', function () {
            //Sua nhanh voucher su dung cho Serial number
            //Ve sau se phai tao lai sau
            var modal = $(this).closest('.modal');
            var voucher_code = $(this).closest('.modal').find('.input-coupon').val();
            var url = `Order/GetCouPonByCode?code=${voucher_code}`;
            $.get(url).then((response) => {
                console.log(response);
                if (response) {
                    R.ProductDetail.DiscountOption = response.discountOption;
                    R.ProductDetail.DiscountValue = parseInt(response.valueDiscount);
                    R.ProductDetail.DiscountCode = voucher_code;
                    var _successMsg = $('.culture-modal-payment').data('success-coupon');
                    Swal.fire({
                        title: "Success!",
                        text: _successMsg,
                        icon: "success"
                    }).then((result) => {
                        if (result.isConfirmed) {
                            $(modal).modal('hide');
                            $('.button-coupon-modal-text').text(voucher_code)
                            //R.ProductDetail.LoadProductTotalPrice();
                            R.ProductDetail.KeepCouponDetail();
                        }
                    });
                }
                else {
                    var _errMsg = $('.culture-modal-payment').data('error-coupon');
                    Swal.fire({
                        title: "Error!",
                        text: _errMsg,
                        icon: "error"
                    }).then((result) => {
                        if (result.isConfirmed) {
                            $(modal).modal('hide');
                            voucher_code.val('')
                            
                        }
                    });
                }
            })
        })
    },
    RequsetSerialNumber: function (voucher_code, modal) {
        var url = `Order/CheckSerialNumber?serialNumber=${voucher_code}`;
        console.log(voucher_code);
        if (voucher_code == "") {
            $(modal).modal('hide');
            //$('.button-serial-modal-text').text('')
            //R.ProductDetail.LoadProductTotalPrice();
            R.ProductDetail.RemoveSerialNumberInDetail();
            return;
        }
        $.get(url).then((response) => {
            console.log(response);
            if (response) {
                sessionStorage.setItem('current_serial', JSON.stringify(response));
                R.ProductDetail.SerialNumber = response.serialNumber;
                Swal.fire({
                    title: "Success!",
                    text: `Verified Serial Number Successful! You get a discount on each subsequent order when you reuse the SIM Card!`,
                    icon: "success"
                }).then((result) => {
                    if (result.isConfirmed) {
                        $(modal).modal('hide');
                        $('.button-serial-modal-text').text(response.serialNumber)
                        //R.ProductDetail.LoadProductTotalPrice();
                        R.ProductDetail.KeepSerialNumberInDetail();
                    }
                });
            }
            else {
                Swal.fire({
                    title: "Error!",
                    text: `Your serial number is not correct! Please try again!`,
                    icon: "error"
                }).then((result) => {
                    if (result.isConfirmed) {
                        $(modal).modal('hide');
                        
                        //R.ProductDetail.LoadProductTotalPrice();
                        R.ProductDetail.RemoveSerialNumberInDetail();
                    }
                });
            }
        })

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
        R.ProductDetail.RegisterEvent();
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

                R.ProductDetail.RequestOrderV2(request);
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
                R.ProductDetail.RequestOrderV2(request);
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
                R.ProductDetail.PickupPoints.push(v.name);
            })
            R.ProductDetail.LoadPickupPoint(R.ProductDetail.PickupPoints);

        })
    },
    KeepSerialNumberInDetail: function () {
        if (sessionStorage.getItem('current_serial')) {
            $('.sim-package-detail').each((index, element) => {
                //Tim li gia
                if ($(element).data('sim-pack') != 'Package' && $(element).data('sim-type') != "E-Sim") {
                    var currentSerial = JSON.parse(sessionStorage.getItem('current_serial'));
                    $('.button-serial-modal-text').text(currentSerial.serialNumber)
                    $('.input-voucher').val(currentSerial.serialNumber);
                    //$('.inner-change-item-ccid').removeClass('inner-change-item-ccid');
                    $('.inner-change-item-ccid').addClass('active');
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
                    $(element).find('.down-price').addClass('down-show');
                    $(element).find('.down-price').removeClass('down-price');
                    $(element).find('.hide-current-price').css('display', 'none');

                    $('.inner-quantity').text('').text('1');
                    $('.inner-quantity').closest('li').remove();
                }
            })
            //R.ProductDetail.PickUpPoint
            R.ProductDetail.PickUpPoint = "TOP UP"
            $('.pickup-block').css('display', 'none');
            //console.log(currentSerial)
        }
        
        R.ProductDetail.RegisterEvent();
    },
    KeepCouponDetail: function () {
        if (R.ProductDetail.DiscountOption != null) {
            //after-price
            var price = $('.after-price').data('price');
            var afterPrice = price;
            var discountPrice = afterPrice * R.ProductDetail.DiscountValue / 100
            if (R.ProductDetail.DiscountOption == 1) {
                afterPrice = afterPrice - afterPrice * R.ProductDetail.DiscountValue / 100;
            }

            $('.after-price').data('price', afterPrice);

            var unit = $('.after-price').data('unit');
            var _html = `${unit} ${R.FormatNumber(afterPrice)}`;
            $('.after-price').text(_html.replace(",", ".").replace(',', '.'))
            $('.inner-mobile-price').html(_html.replace(",", ".").replace(',', '.'));

            var _couponHtml = `- ${unit} ${R.FormatNumber(discountPrice)}`
            $('.inner-change-coupon .inner-value').html('').html(_couponHtml.replace(",", ".").replace(',', '.'))
            $('.inner-change-coupon').removeClass('inner-change-coupon');

        }
        R.ProductDetail.RegisterEvent();
    },

    RemoveSerialNumberInDetail: function () {
        var _serial_number = sessionStorage.getItem('current_serial')
        if (_serial_number) {
            //Tra lai gia ve ban dau
            sessionStorage.removeItem('current_serial');
            $('.inner-change-item-ccid').removeClass('active');
            var afterPrice = $('.after-price').data('default-price');
            $('.after-price').data('price', afterPrice);
            $('.after-price').data('serial-number', '');
            var unit = $('.after-price').data('unit');
            var _html = `${unit} ${R.FormatNumber(afterPrice)}`;
            $('.after-price').text(_html.replace(",", ".").replace(',', '.'))
            $('.inner-mobile-price').html(_html.replace(",", ".").replace(',', '.'));

            $('.sim-package-info .current-price').css('text-decoration','none');
            $('.sim-package-info .down-show').addClass('down-price');
            $('.sim-package-info .down-price').removeClass('down-show');
            var df = $('.button-serial-modal-text').data('df');
            //alert(df)
            $('.button-serial-modal-text').html(df);
            $('.pickup-block').css('display', 'block');


        }
    },
    ChangePaymentShowcase: function () {
        var html = `
            <img src="${R.ProductDetail.PaymentShowcase.img}"> <span>${R.ProductDetail.PaymentShowcase.text}</span>
        `
        $('.payment-show-case').html('').html(html);
        $('.small-warning-payment').css('display', 'none');
        R.ProductDetail.RegisterEvent();
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
                    email: R.ProductDetail.Email,
                    pickupPoint: R.ProductDetail.PickUpPoint
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
                    email: R.ProductDetail.Email,
                    pickupPoint: R.ProductDetail.PickUpPoint
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
        //            email: R.ProductDetail.Email,
        //            pickupPoint: R.ProductDetail.PickUpPoint
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
        //            email: R.ProductDetail.Email,
        //            pickupPoint: R.ProductDetail.PickUpPoint
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
        //        email: R.ProductDetail.Email,
        //        pickupPoint: R.ProductDetail.PickUpPoint,
        //        isCreatedAccount: isCreatedAccount,
        //        orderNote: `Pickup Point: ${R.ProductDetail.PickUpPoint}`
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
        R.ProductDetail.RegisterEvent();
    },
    LoadPickupItemButton: function () {
        
        if (R.ProductDetail.PickUpPoint != "") {
            var html = `Pickup Point: ${R.ProductDetail.PickUpPoint}`
            $('.inner-desc-thong-tin-bo-sung-pickup-point').html('').html(html);
            $('#modalNhamSim').modal('hide');
            $('.small-warning-pickup-points').css('display', 'none');
        }
        R.ProductDetail.RegisterEvent();
        
    },
    LoadEmailItemButton: function () {
        var customer = R.LoadCustomerInfo();
        if (customer) {
            if (R.ProductDetail.Email) {
                if (R.ProductDetail.Email != customer.email) {
                    localStorage.removeItem('customerInfo');
                }
                else {
                    R.ProductDetail.Email = customer.email;
                }
            }
            //R.ProductDetail.Email = customer.email;
            //R.ProductDetail.PickUpPoint = customer.pickupPoint.trim().replace('\\n', '');
        }
        if (R.ProductDetail.Email != "") {
            var html = `Email: ${R.ProductDetail.Email}`
            $('.inner-desc-thong-tin-bo-sung-email').html('').html(html);
            $('#modalNhamSim').modal('hide');
            //modalEmailNhanSim
            $('#modalEmailNhanSim').modal('hide');

            $('.small-warning-email').css('display', 'none');
        }
        R.ProductDetail.RegisterEvent();
    },

    LoadProductTotalPrice: function () {
        R.ProductDetail.TotalPrice = 0;

        var currentPrice = parseFloat($('.total-price').data('price'));
        R.ProductDetail.TotalPrice = currentPrice;
        if (R.ProductDetail.Voucher) {
            var discountType = R.ProductDetail.Voucher.discountOption;
            if (discountType == 1) {
                var discountPrice = R.ProductDetail.TotalPrice * R.ProductDetail.Voucher.valueDiscount / 100;
                R.ProductDetail.TotalPrice = R.ProductDetail.TotalPrice - discountPrice;
            }
            else {

                R.ProductDetail.TotalPrice = R.ProductDetail.TotalPrice - R.ProductDetail.Voucher.valueDiscount;
            }
        }
        //Cap nhat gia vao 
        $('.total-price').data('price', parseFloat(R.ProductDetail.TotalPrice.toFixed(2)));
        $('.total-price').text(`$US ${parseFloat(R.ProductDetail.TotalPrice.toFixed(2)) }`);
        
    },
    
}


$(function () {
    R.ProductDetail.Init();
})

