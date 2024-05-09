
var R = {
    Init: function () {
        //R.ClearCart();
        //R.LoadCart();
        //R.FirstLoad();
        
        R.LoadLanguages();
        R.CheckingCustomerInfo();
        R.RegisterEvent();
    },
    RegisterEvent: function () {
        $('html').off('click').on('click', function (event) {
            // Kiểm tra nếu phần tử được click không phải là '.inner' và không phải là con của '.inner'
            if (!$(event.target).closest('.menu-mobile').length) {
                
                if ($('.non-login').hasClass('active')) {
                    $('.non-login').css('display', 'none');
                    $('.non-login').removeClass('active');
                }
                if ($('.logged-in').hasClass('active')) {
                    $('.logged-in').css('display', 'none');
                    $('.logged-in').removeClass('active');
                }
                // Thay đổi thuộc tính CSS của '.inner'
                
            }
        });
        //open-modal-coverage
        $('.mobile-menu-account').off('click').on('click', function (event) {
            //Kiem tra xem da login chua
            event.stopPropagation()
            var loginChecker = R.CheckCustomerLoggedIn();
            if (!loginChecker) {
                $('.menu-mobile.non-login').css('display', 'block');
                $('.non-login').addClass('active')
            }
            else {
                $('.menu-mobile.logged-in').css('display', 'block');
                $('.logged-in').addClass('active')
            }
            
            
        })
        $('.change-language-link').off('click').on('click', function () {
            var languagesString = localStorage.getItem("Languages");
            if (languagesString) {
                var languages = JSON.parse(languagesString);
                //set tat ca ve false
                languages.forEach((element) => { element.isActive = false });
                //set cho cai duoc chon  = true
                languages.forEach((element) => {

                    if (element.languageCode.includes($(this).data('language-code'))) {
                        element.isActive = true;
                        
                    }
                })
                
                //set lai localStorage
                localStorage.setItem("Languages", JSON.stringify(languages));
                R.BindingLanguages();

                var cultureCode = $(this).data('language-code');
                //Thay doi ngon ngu va load lai trang
                var url = `/Home/SetLanguage?culture=${cultureCode}`;
                $.get(url).then((response) => {
                    window.location.reload();
                })
            }
        })
        $('.link-login').off('click').on('click', function () {
            $("#modalLogin").modal('show');
        })
        $('.btn-login').off('click').on('click', function () {
            var email = $('.input-login-email').val();
            var password = $('.input-login-password').val();
            //Tien hanh dang nhap
            var url = "/Customer/DoLogin";
            $.post(url, { email: email, password: password }).then(function (response) {
                console.log(response);
                var customerInfo = {
                    randomId: response.pcName,
                    id: response.id,
                    email: response.email,
                    pickupPoint: ""
                }
                if (response.email == "huy.bc@aptechlearning.edu.vn") {
                    //set cookie giam gia 10%
                    document.cookie = "discount=10; expires=Thu, 29 Mar 2025 12:00:00 UTC; path=/";

                }
                //Tien hanh xu ly user mac dinh
                //Save Local Storage
                localStorage.setItem('customerInfo', JSON.stringify(customerInfo));
                Swal.fire({
                    title: "Success!",
                    text: `Đăng nhập thành công!`,
                    icon: "success"
                }).then((result) => {
                    if (result.isConfirmed) {
                        R.CheckingCustomerInfo();
                        $("#modalLogin").modal('hide');
                    }
                });
            }).fail(function (jqXHR, textStatus, errorThrown) {
                Swal.fire({
                    title: "LỖI!",
                    text: `Đăng nhập không thành công! Kiểm tra lại tài khoản và mật khẩu`,
                    icon: "error"
                }).then((result) => {
                    if (result.isConfirmed) {
                        $("#modalLogin").modal('hide');
                    }
                });
                
            });
        })
        $('.link-logout').off('click').on('click', function () {
            localStorage.removeItem('customerInfo');
            window.location.href = "/";
        })
        $('.link-sign-up').off('click').on('click', function () {
            //Swal.fire({
            //    title: "Đừng lo!",
            //    text: `Tài khoản của bạn sẽ được đăng ký tự động khi mua đơn hàng đầu tiên. Chúc bạn mua sắm vui vẻ!`,
            //    icon: "success"
            //})
            $('#modalSignup').modal('show');
        })
        $('.btn-sign-up').off('click').on('click', function () {
            var email = $('.input-signup-email').val();
            var password = $('.input-signup-password').val();
            var confirm = $('.input-signup-confirm-password').val();
            if (password == confirm) {
                var url = "/Customer/DoSignUp";
                $.post(url, { email: email, password: password }).then(function (response) {
                    var customerInfo = {
                        randomId: password,
                        id: response,
                        email: email,
                        pickupPoint: ""
                    }
                    //Save Local Storage
                    localStorage.setItem('customerInfo', JSON.stringify(customerInfo));
                    Swal.fire({
                        title: "Success!",
                        text: `Đăng ký tài khoản thành công!`,
                        icon: "success"
                    }).then((result) => {
                        if (result.isConfirmed) {
                            R.CheckingCustomerInfo();
                            $('#modalSignup').modal('hide');
                        }
                    });
                }).fail(function (error) {
                    Swal.fire({
                        title: "LỖI!",
                        text: `Tài khoản đã tồn tại!`,
                        icon: "error"
                    }).then((result) => {
                        if (result.isConfirmed) {
                            $("#modalSignup").modal('hide');
                        }
                    });
                })
            }
        })
        $('.change-password').off('click').on('click', function () {
            $('#modalChangePassword').modal('show');
        })

        $('.btn-change-password').off('click').on('click', function () {
            var oldpass = $('.input-change-old-pass').val();
            var newpass = $('.input-change-new-pass').val();
            var confirm = $('.input-change-new-confirm-pass').val();
            var customer = R.LoadCustomerInfo();
            var email = customer.email;
            if (newpass == confirm) {
                var url = '/Customer/ChangePassword';
                $.post(url, { email: email, password: newpass, oldPassword: oldpass }).then(function (response) {
                    var customerInfo = R.LoadCustomerInfo();
                    customerInfo.randomId = newpass;
                    //Tien hanh xu ly user mac dinh
                    //Save Local Storage
                    localStorage.setItem('customerInfo', JSON.stringify(customerInfo));
                    Swal.fire({
                        title: "Success!",
                        text: `Đổi mật khẩu thành công!`,
                        icon: "success"
                    }).then((result) => {
                        $('#modalChangePassword').modal('hide');
                    })
                }).fail(function (err) {
                    Swal.fire({
                        title: "LỖI!",
                        text: `Có lỗi khi thay đổi mật khẩu. Vui lòng thử lại!`,
                        icon: "error"
                    }).then((result) => {
                        if (result.isConfirmed) {
                            $("#modalLogin").modal('hide');
                        }
                    });
                   
                });
            }
            
        })
            // modalChangePassword
    },
    AddToCart: function (id,obj, message, title, button) {
        var productObj = obj
        //Load cart
        var products = [];
        if (localStorage.getItem('sim-carts')) {
            products = JSON.parse(localStorage.getItem('sim-carts'));

        }
        products.push(productObj);
        //Save cart
        localStorage.setItem('sim-carts', JSON.stringify(products));
        Swal.fire({
            title:title,
            text: message,
            icon: "success",
            confirmButtonText: button
        });

    },
    LoadCart: function () {
        var products = [];
        if (localStorage.getItem('sim-carts')) {
            products = JSON.parse(localStorage.getItem('sim-carts'));

        }
        return products;
    },
    RemoveFromCart: function (id) {
        var products = R.LoadCart();
        if (products.length > 0) {
            // Tìm sản phẩm
            var finded;
            $.each(products, function (index, element) {
                if (element.productId == id) {
                    finded = element;
                    return false; // Dừng vòng lặp khi tìm thấy sản phẩm
                }
            });
            console.log(finded);
            products.splice(products.indexOf(finded), 1);
        }
        localStorage.setItem('sim-carts', JSON.stringify(products));
    },
    LoadCustomerInfo: function () {
        var customerInfoString = localStorage.getItem("customerInfo");
        if (customerInfoString) {
            return JSON.parse(customerInfoString);
        }
    },
    CheckCustomerLoggedIn: function () {
        var customerInfo = R.LoadCustomerInfo();
        console.log(customerInfo)
        if (customerInfo)
            return true;
        else
            return false;
    },
    CheckingCustomerInfo: function () {
        var customerInfo = R.LoadCustomerInfo();
        console.log(customerInfo)
        if (customerInfo) {
            $('.menu-my-sim').removeAttr('style')
            $('.menu-my-account').removeAttr('style')
            $('.menu-my-account').find('.account-customer-name').text(customerInfo.email)
            $('.menu-my-account').find('.inner-sub-menu').css('display','block')
            //$('.menu-my-account').find('.account-customer-name').attr("href", "/my-sim");
            $('.menu-my-account').find('.account-register-link').css("display", "none");
            $('.menu-register').css('display', 'none');
            $('.mobile-menu-account').find('span').text(customerInfo.email)
        }
    },

    LoadLanguages: function () {
        //if (!localStorage.getItem("Languages")) {
        //    var url = "/Home/GetAllLanguages";
        //    $.post(url).then((response) => {
        //        console.log(response)
        //        //Save to localstorage
        //        localStorage.setItem("Languages", JSON.stringify(response));
        //        R.BindingLanguages();
        //    })
        //} else {
        //    R.BindingLanguages();
        //}
        var url = "/Home/GetAllLanguages";
        $.post(url).then((response) => {
            console.log(response)
            //Save to localstorage
            localStorage.setItem("Languages", JSON.stringify(response));
            R.BindingLanguages();
        })
        
        
    },
    BindingLanguages: function () {
        var languagesString = localStorage.getItem("Languages");
        if (languagesString) {
            var languages = JSON.parse(languagesString);
            // Parse ra HTML
            console.log(languages)
            // select-languge-button
            // Do ra nut
            $('.select-languge-button span').text('').text(languages.find(element => element.isActive == true).name);

            // do ra modal truoc
            var html = "";
            languages.forEach((element) => {
                var active = "";
                if (element.isActive) {
                    active = `<i class="fa-solid fa-circle-check"></i>`;
                }
                html += `
                    <li>
                        <a href="javascript:void(0)" class="change-language-link" data-language-code="${element.languageCode}">
                            <span>${element.name}</span>
                            ${active}
                        </a>
                    </li>
                `;
            })
            $('#modalChangeLanguage ul').html('').html(html);
            var mobile_html = `<img src="${languages.find(element => element.isActive == true).flag}" class="mobile-flag-icon" \>`;
            $('.select-languge-button-mobile').html('').html(mobile_html);
            R.RegisterEvent();
        }
    },

    Test: function () {
        var test = 1;
        //console.log(test);
    },
    Culture: function () {
        //var culture = window.location.pathname.split('/')[1];
        //return ("/" + culture);
        return ("");
    },
    FormatNumber: function (num) {
        return num.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,');
    },
    ClearCart: function () {
        var arrProduct = [];
        localStorage.setItem('arrProduct', JSON.stringify(arrProduct));
    },
    GroupBy: function (xs, key) {
        return xs.reduce(function (rv, x) {
            (rv[x[key]] = rv[x[key]] || []).push(x);
            return rv;
        }, {});
    },
    CurrentLocationId: function () {
        var location_id = $('#current-local').data('id');
        return location_id;
    },
    RemoveCartItem: function (id) {
        var r = JSON.parse(localStorage.getItem("arrProduct"));
        for (var i = 0; i < r.length; i++) {
            if (r[i].product_id == id)
                r.splice(i, 1);
        }
        localStorage.setItem('arrProduct', JSON.stringify(r));

    },
    StoreFilePath: function (isThumb) {
        // string store_url = "https://jhcms.migroup.asia", string root = "/uploads"
        var store_url = "https://cmsdidongxanh.migroup.asia";
        var root = "/uploads";
        if (isThumb)
            return store_url + root + "/thumb";
        else
            return store_url + root;
    },
    FirstLoad: function () {
        var r = sessionStorage.getItem("locationFirstLoad");
        if (r == null)
            sessionStorage.setItem("locationFirstLoad", "1");
    },
    CountDown: function (date, el) {
        var countDownDate = new Date(date).getTime();
        
        function timesale() {

            // Get today's date and time
            var now = new Date().getTime();
            //console.log(countDownDate);

            // Find the distance between now and the count down date
            var distance = countDownDate - now;

            // Time calculations for days, hours, minutes and seconds
            var days = Math.floor(distance / (1000 * 60 * 60 * 24));
            var hours = Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
            var minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
            var seconds = Math.floor((distance % (1000 * 60)) / 1000);

            $(el).find('.countdown-day').text(days);
            $(el).find('.countdown-hour').text(hours);
            $(el).find('.countdown-minute').text(minutes);
            $(el).find('.countdown-second').text(seconds);

            if (distance < 0) {

                //clearInterval(x);
                document.getElementsByClassName("flash-sales").style.display = "none";

            }
            
        }
        // Update the count down every 1 second
        var x = setInterval(timesale, 1000);
      

        //var x = setInterval(timesale, 1000);
    },
    ShowLoadingScreen: function () {
        $(".loading-screen").fadeIn();
    },
    HideLoadingScreen: function () {
        $(".loading-screen").fadeOut();
    }
}
$(function () {
    R.Init();
})
