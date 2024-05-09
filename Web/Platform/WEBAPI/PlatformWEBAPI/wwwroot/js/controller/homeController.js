R.Home = {
    Init: function () {
        //alert('AN JS')
        R.Home.RegisterEvent();
        R.Home.culture = R.Culture();
        R.Home.currentLocationId = R.CurrentLocationId();
        R.Home.BindingTotal();
        R.Home.CountDownFlashSale();
        R.Home.DateTimePicker();
        R.Home.DoiTacSlide();
        R.Home.KeepSerialNumberInInput();
        R.Home.DownPrice();
        R.Home.WhyChooseUsSlide();
        R.Home.BaseIMG = "https://way2gocms.hndedu.com/uploads/";
        R.Home.CSSHOME();

    },
    RegisterEvent: function () {
        $('.input-search-box').off('focus').on('focus', function () {
           
            var _keyword = $(this).val();
            console.log(_keyword)
            if (!_keyword || _keyword == "") {
                $('#search-results').html('');
                $('#search-results').css('display', 'none');
            } else {
                $('#search-results').css('display', 'block');
            }
            
        })
        $(document).on('mousedown', function (event) {
            // Kiểm tra xem vị trí nhấn chuột có phải là #search-results hay không
            if (!$(event.target).closest('#search-results').length) {
                $('#search-results').css('display', 'none');
            }
        });
        
        $('.input-search-box').on('keyup', function () {
            //alert('AN')
            var _keyword = $(this).val();
            if (!_keyword || _keyword == "") {
                $('#search-results').html('');
                $('#search-results').css('display', 'none');
            }
            $(this).autocomplete({
                source: function (request, response) {
                    // Gửi yêu cầu AJAX đến server
                    $.ajax({
                        url: `/Product/ElasticFilter?keyword=${_keyword}`, // Đường dẫn tới route lấy dữ liệu
                        type: "GET",
                        dataType: "json",
                        success: function (data) {
                            // Xử lý dữ liệu trả về và hiển thị
                            $('#search-results').html('')

                            response($.map(data, function (item) {
                                if (item.itemType == "ZONE") {
                                    var html = `<div class="dropdown-item mb-2" data-url="${item.itemUrl}" data-id="${item.itemId}">
                                                    <img src="https://way2gocms.hndedu.com/uploads/${item.itemAvatar}" alt="${item.itemName}"  class="country-flag"><span>${item.itemName}</span>
                                                </div>`
                                    $('#search-results').append(html);
                                }
                                
                            }));
                            $('#search-results').css('display', 'block');

                            R.Home.RegisterEvent();
                        }
                    });
                },
                select: function (event, ui) {
                    alert('AN')
                    // Hành động khi chọn một đề xuất
                    // Ví dụ: console.log(ui.item.value);
                    console.log(ui.item);
                }
            });
        });
        //$('.input-search-box').on('keyup', () => {
        //    alert('AN')
        //})
        $('.clear-serial-number').off('click').on('click', function () {
            sessionStorage.removeItem('current_serial');
            $('.input-check-serial').val(``);
            R.Home.DownPriceReverse()
        })
        $('.dropdown-item').off('click').on('click', function () {
            $('#search-results').css('display', 'none');
            var url = $(this).data('url');
            var id = $(this).data('id');
            $.post('/Product/GetProductByZoneId', { zone_Id: id, location_id: 0 }).then((response) => {
                console.log(response)
                $('.inner-section-local-container').html('').html(response);
                $('.button-show-all').addClass('button-back-country');
                R.Home.KeepSerialNumberInInput();
                R.Home.DownPrice();
                history.pushState(null, '', `/${url}`);
                window.addEventListener('popstate', function (event) {
                    var url = '/Home/ListOfCountry';
                    $.post(url).then((response) => {
                        console.log(response)
                        $('#tab-esim-1').html('').html(response);
                        // show-country
                        const showCountry = document.querySelector("[show-country]");
                        if (showCountry) {
                            let quantityShowCountry = showCountry.getAttribute("show-country");
                            quantityShowCountry = parseInt(quantityShowCountry);

                            const listItem = showCountry.querySelectorAll(".inner-item");

                            for (let i = 0; i < quantityShowCountry; i++) {
                                if (listItem[i]) {
                                    listItem[i].classList.remove("d-none");
                                }
                            }

                            const buttonShowAll = document.querySelector("[button-show-all]");
                            const textHidden = buttonShowAll.getAttribute("text-hidden");
                            const textShow = buttonShowAll.getAttribute("text-show");
                            let isShow = false;

                            buttonShowAll.addEventListener("click", () => {
                                isShow = !isShow;

                                if (isShow) {
                                    for (let i = 0; i < listItem.length; i++) {
                                        if (listItem[i]) {
                                            listItem[i].classList.remove("d-none");
                                        }
                                    }
                                } else {
                                    for (let i = quantityShowCountry; i < listItem.length; i++) {
                                        if (listItem[i]) {
                                            listItem[i].classList.add("d-none");
                                        }
                                    }
                                }

                                buttonShowAll.innerHTML = isShow ? textShow : textHidden;
                            });
                        }
                        // End show-country
                        R.Home.RegisterEvent();
                    })
                });
                R.Home.RegisterEvent();
            }).catch((err) => { })

        })
        $('.open-modal-coverage').off('click').on('click', function () {
            //data-coverage
            var coverages = $(this).data('coverage');
            //alert(coverages)
            $.get(`/Zone/ModalListOfCountries?zoneIds=${coverages}`).then((response) => {
                $('#modal-coverage ul').html('').html(response);
                $('#modal-coverage').modal('show')
                R.Home.RegisterEvent();
            })
        })
        $('.search-diem-den').off('click').on('click', function () {
            var diem_den_region = $(this).data('url-location');
            var tuNgay = $(this).data('start');
            var denNgay = $(this).data('end');
            var url = "/diem-den/";
            if (diem_den_region.length > 0) {
                url += diem_den_region;
            }
            url += '?tuNgay=' + tuNgay
            url += '&denNgay=' + denNgay
            window.location.href = url;
            
        })
        $('._switch_region').off('click').on('click', function () {
            $(this).closest('#myTab').find('.active').removeClass('active');
            $(this).addClass('active');
            var id = $(this).data('region-id');
            R.Home.SwitchRegion($(this), id)
        })
        $('.tab-region').off('click').on('click', function() {
            $(this).closest('.swiper-wrapper').find('.swiper-slide-active').removeClass('swiper-slide-active');
            $(this).closest('.swiper-slide').addClass('swiper-slide-active');
            $(this).closest('.menu-op').find('.active').removeClass('active');
            $(this).addClass("active");

            var region_id = $(this).data('region-id');
            R.Home.SwitchRegion($(this), region_id)
        });
        $('.view-remain-product').off('click').on('click', function() {
           
        })
        $('.view-more-region-nbv').off('click').on('click', function () {
            var id = $(this).data('zone-id');
            var idex = $(this).data('index');
            var size = $(this).data('size');
            var loc = $(this).data('loc');
            var el = $('._binding_product');
            var skip = idex * size;
            R.Home.ViewMoreRegion(id, el, skip, size, idex);

        })
        $('.filter-home').off('click').on('click', function () {
            var el = $(this);
            //var pr = $(this).closest('.main-fast-search');
            var parent_id = 0;
            //var parent_id = $('#zone-current').data('id');
            var manu_id = $(this).data('manu-id');
            //var range_price = pr.find('.range-price').val();
            var min_price = 0;
            var max_price = 0
            //if (range_price != "") {
            //    var arr = range_price.split('-');
            //    min_price = arr[0];
            //    max_price = arr[1];
            //}
            var fp = [];
            $('.dynamic-filter').each(function (element) {
                var filter_item = {
                    SpectificationId: $(this).data('spec-id'),
                    Value: $(this).val()
                }
                fp.push(filter_item);
            })
            //var color_code = $('.color').val();
            var extra = $('.extra').val();
            var sort_price = 0;
            var sort_rate = 0;
            if (extra == "2")
                sort_price = 0;
            if (extra == "3")
                sort_rate = 1;
            var locationId = R.Home.currentLocationId;
            var params = {
                parentId: parent_id,
                lang_code: 'vi-VN',
                locationId: locationId,
                manufacture_id: manu_id == null ? 0 : manu_id,
                min_price: parseInt(min_price),
                max_price: parseInt(max_price),
                sort_price: sort_price,
                sort_rate: sort_rate,
                color_code: '',
                filter: null,
                filter_text: '',
                material_type: 0,
                pageNumber: 1,
                pageSize: 24
            }

            sessionStorage.setItem("homeQuerySearch", JSON.stringify(params));
            window.location.href = "/tim-kiem-nhanh";
            //load ajax
            //R.Home.BindingProductFilterd(params, el);
        })
        $('.button-show-all').off('click').on('click', function () {
            if (!$(this).hasClass('button-back-country')) {
                var url = '/Home/ListOfCountry';
                $.post(url).then((response) => {
                    console.log(response)
                    $('#tab-esim-1').html('').html(response);
                    //$('.inner-section-local-container .inner-list').attr('show-country', 300);
                    //// show-country
                    const showCountry = document.querySelector("[show-country]");
                    if (showCountry) {
                        let quantityShowCountry = showCountry.getAttribute("show-country");
                        quantityShowCountry = 300;

                        const listItem = showCountry.querySelectorAll(".inner-item");

                        for (let i = 0; i < quantityShowCountry; i++) {
                            if (listItem[i]) {
                                listItem[i].classList.remove("d-none");
                            }
                        }

                        const buttonShowAll = document.querySelector("[button-show-all]");
                        const textHidden = buttonShowAll.getAttribute("text-hidden");
                        const textShow = buttonShowAll.getAttribute("text-show");
                        let isShow = false;

                        buttonShowAll.addEventListener("click", () => {
                            isShow = !isShow;

                            if (isShow) {
                                for (let i = 0; i < listItem.length; i++) {
                                    if (listItem[i]) {
                                        listItem[i].classList.remove("d-none");
                                    }
                                }
                            } else {
                                for (let i = quantityShowCountry; i < listItem.length; i++) {
                                    if (listItem[i]) {
                                        listItem[i].classList.add("d-none");
                                    }
                                }
                            }

                            buttonShowAll.innerHTML = isShow ? textShow : textHidden;

                        });
                    }
                    // End show-country
                    R.Home.RegisterEvent();
                })

            }
        })

        // Way2Go

        $('.country-item').off('click').on('click', function() { 
            var zoneId = $(this).data('id');
            var zoneUrl = $(this).data('url');
            //alert(zoneId)
            //alert(zoneId)
            $.post('/Product/GetProductByZoneId', { zone_Id: zoneId, location_id: 0 }).then((response) => {
                console.log(response)
                $('.inner-section-local-container').html('').html(response);
                $('.button-show-all').addClass('button-back-country');
                R.Home.KeepSerialNumberInInput();
                R.Home.DownPrice();
                history.pushState(null, '', `/${zoneUrl}`);
                window.addEventListener('popstate', function (event) {
                    var url = '/Home/ListOfCountry';
                    $.post(url).then((response) => {
                        console.log(response)
                        $('#tab-esim-1').html('').html(response);
                        // show-country
                        const showCountry = document.querySelector("[show-country]");
                        if (showCountry) {
                            let quantityShowCountry = showCountry.getAttribute("show-country");
                            quantityShowCountry = parseInt(quantityShowCountry);

                            const listItem = showCountry.querySelectorAll(".inner-item");

                            for (let i = 0; i < quantityShowCountry; i++) {
                                if (listItem[i]) {
                                    listItem[i].classList.remove("d-none");
                                }
                            }

                            const buttonShowAll = document.querySelector("[button-show-all]");
                            const textHidden = buttonShowAll.getAttribute("text-hidden");
                            const textShow = buttonShowAll.getAttribute("text-show");
                            let isShow = false;

                            buttonShowAll.addEventListener("click", () => {
                                isShow = !isShow;

                                if (isShow) {
                                    for (let i = 0; i < listItem.length; i++) {
                                        if (listItem[i]) {
                                            listItem[i].classList.remove("d-none");
                                        }
                                    }
                                } else {
                                    for (let i = quantityShowCountry; i < listItem.length; i++) {
                                        if (listItem[i]) {
                                            listItem[i].classList.add("d-none");
                                        }
                                    }
                                }

                                buttonShowAll.innerHTML = isShow ? textShow : textHidden;
                            });
                        }
                        // End show-country
                        R.Home.RegisterEvent();
                    })
                });
                R.Home.RegisterEvent();
            }).catch((err) => { })
        })
        //region-item
        $('.region-item').off('click').on('click', function () {
            var zoneId = $(this).data('id');
            //alert(zoneId)
            //alert(zoneId)
            $.post('/Product/GetProductByZoneId', { zone_Id: zoneId, location_id: 0 }).then((response) => {
                console.log(response)
                $('.inner-section-region').html('').html(response);
                R.Home.KeepSerialNumberInInput();
                R.Home.DownPrice();
                //$('.button-show-all').addClass('button-back-country');
                R.Home.RegisterEvent();
            }).catch((err) => { })
        })
        $('.inner-buy-now').off('click').on('click', function () {
            var product_id = $(this).data('id');
            var simPack = $(this).closest('.sim-package-item').data('sim-pack');
            //alert(simPack)
            if (simPack == 'Package') {
                var title = $('.culture-info').data('package-alert-title');
                var desc = $('.culture-info').data('package-alert-desc');
                var button = $('.culture-info').data('package-alert-button');
                console.log(title,desc,button)
                Swal.fire({
                    title: title,
                    icon: "info",
                    html: desc,
                    showCloseButton: true,
                    showCancelButton: false,
                    focusConfirm: false,
                    confirmButtonText: button
  //                  confirmButtonAriaLabel: "Thumbs up, great!",
  //                  cancelButtonText: `
  //  <i class="fa fa-thumbs-down"></i>
  //`,
  //                  cancelButtonAriaLabel: "Thumbs down"
                });
            }
            $.post('/Product/GetProductModalDetail', { product_id: product_id }).then((response) => {
                $('#modalBuySim').html('').html(response);
                $("#modalBuySim").modal('show');
                R.Home.DownPrice();
                //Kiem tra man hinh va bien thanh nut gio hang

                if (window.innerWidth <= 768) {
                    $('.add-sim-to-cart').html('').html('<i class="fa-solid fa-cart-shopping"></i>');
                } 

                // swiperSimAvailable
                // swiperSimAvailable
                const swiperSimAvailable = document.querySelector(".swiperSimAvailable");
                if (swiperSimAvailable) {
                    const swiper = new Swiper(".swiperSimAvailable", {
                        slidesPerView: 1,
                        spaceBetween: 30,
                        navigation: {
                            nextEl: ".swiper-button-next",
                            prevEl: ".swiper-button-prev",
                        },
                        pagination: {
                            el: ".swiper-pagination",
                            clickable: true,
                        },
                        breakpoints: {
                            576: {
                                slidesPerView: 2,
                            },
                        },
                    });
                }
                R.Home.RegisterEvent();
            }).catch((err) => { });
        })
        $('.button-back-country').off('click').on('click', function () {
            // Lấy vị trí Y của class "targetClass"
            var targetOffset = $(".section-2").offset().top;
            //alert(targetOffset);

            // Cuộn lên ngay lập tức đến vị trí của class "targetClass"
            $("html, body").scrollTop(targetOffset);
            var url = '/Home/ListOfCountry';
            $.post(url).then((response) => {
                console.log(response)
                $('#tab-esim-1').html('').html(response);
                // show-country
                const showCountry = document.querySelector("[show-country]");
                if (showCountry) {
                    let quantityShowCountry = showCountry.getAttribute("show-country");
                    quantityShowCountry = parseInt(quantityShowCountry);
                    quantityShowCountry = parseInt(quantityShowCountry);
                    if (window.innerWidth <= 768) {
                        quantityShowCountry = 10
                    } else {
                        quantityShowCountry = parseInt(quantityShowCountry);
                    }
                    const listItem = showCountry.querySelectorAll(".inner-item");

                    for (let i = 0; i < quantityShowCountry; i++) {
                        if (listItem[i]) {
                            listItem[i].classList.remove("d-none");
                        }
                    }

                    const buttonShowAll = document.querySelector("[button-show-all]");
                    const textHidden = buttonShowAll.getAttribute("text-hidden");
                    const textShow = buttonShowAll.getAttribute("text-show");
                    let isShow = false;

                    buttonShowAll.addEventListener("click", () => {
                        isShow = !isShow;

                        if (isShow) {
                            for (let i = 0; i < listItem.length; i++) {
                                if (listItem[i]) {
                                    listItem[i].classList.remove("d-none");
                                }
                            }
                        } else {
                            for (let i = quantityShowCountry; i < listItem.length; i++) {
                                if (listItem[i]) {
                                    listItem[i].classList.add("d-none");
                                }
                            }
                        }

                        buttonShowAll.innerHTML = isShow ? textShow : textHidden;
                        
                    });
                }
// End show-country
                R.Home.RegisterEvent();
            })
            
        })
        $('.add-sim-to-cart').off('click').on('click', function () {
            var product_id = $(this).data('id');
            var message = $(this).data('message');
            var title = $(this).data('title');
            var button = $(this).data('message-button');
            var $detail = $(this).closest('.sim-package-detail')
            var type = ""
            var price = 0
            var serialNumber = ""
            if ($detail.length > 0) {
                var lookSimType = $(this).data('sim-type');
                if (lookSimType == "Physical Sim") {
                    if (serialNumber == "") { type = "SIM" }
                    else { type = "TOPUP" }
                }
                if (lookSimType == "E-Sim") {
                    type = "ESIM"
                }
            }
            var $price = $(this).data('price');
            price = $price
            if ($price.length > 0) {
                
            }
            var obj = {
                productId: product_id,
                quantity: 1,
                serialNumber: serialNumber,
                totalPrice: price,
                type: type
            }
           
            //Add to cart
            R.AddToCart(product_id, obj, message, title, button);
            
        })
        $('.tab-1').off('click').on('click', function () {
            var url = '/Home/ListOfCountry';
            $.post(url).then((response) => {
                console.log(response)
                $('#tab-esim-1').html('').html(response);
                // show-country
                const showCountry = document.querySelector("[show-country]");
                if (showCountry) {
                    let quantityShowCountry = showCountry.getAttribute("show-country");
                    quantityShowCountry = parseInt(quantityShowCountry);

                    const listItem = showCountry.querySelectorAll(".inner-item");

                    for (let i = 0; i < quantityShowCountry; i++) {
                        if (listItem[i]) {
                            listItem[i].classList.remove("d-none");
                        }
                    }

                    const buttonShowAll = document.querySelector("[button-show-all]");
                    const textHidden = buttonShowAll.getAttribute("text-hidden");
                    const textShow = buttonShowAll.getAttribute("text-show");
                    let isShow = false;

                    buttonShowAll.addEventListener("click", () => {
                        isShow = !isShow;

                        if (isShow) {
                            for (let i = 0; i < listItem.length; i++) {
                                if (listItem[i]) {
                                    listItem[i].classList.remove("d-none");
                                }
                            }
                        } else {
                            for (let i = quantityShowCountry; i < listItem.length; i++) {
                                if (listItem[i]) {
                                    listItem[i].classList.add("d-none");
                                }
                            }
                        }

                        buttonShowAll.innerHTML = isShow ? textShow : textHidden;
                    });
                }
                // End show-country
                R.Home.RegisterEvent();
            })
        })
        $('.tab-2').off('click').on('click', function () {
            var url = '/Home/ListOfRegion';
            $.post(url).then((response) => {
                console.log(response)
                $('#tab-esim-2').html('').html(response);
                
                R.Home.RegisterEvent();
            })
        })
        $('.input-check-serial').on('focusout', function (event) {
            // Code xử lý khi input này focus out
            event.stopImmediatePropagation(); // Ngăn chặn sự kiện khác được gọi
            var serial = $('.input-check-serial').val();
            

            R.Home.RequestSerialNumber(serial)
        });
        $('.input-check-serial').keypress(function (event) {

            
            // Kiểm tra nếu phím được nhấn là phím Enter
            if (event.which === 13) {
                event.preventDefault(); // Ngăn không cho hành động mặc định khi nhấn Enter (ví dụ như submit form)
                event.stopImmediatePropagation(); // Ngăn chặn sự kiện khác được gọi

                var serial = $('.input-check-serial').val();
                R.Home.RequestSerialNumber(serial)
            }
        });
        $('.btn-check-serial').off('click').on('click', function () {
            var serial = $('.input-check-serial').val();
            R.Home.RequestSerialNumber(serial)
                
        })
    },
    RequestSerialNumber: function (serial) {
        if (serial) {
            $.get('/Order/CheckSerialNumber?serialNumber=' + serial).then((response) => {
                console.log(response)
                if (response) {
                    // Luu vao session storage
                    sessionStorage.setItem('current_serial', JSON.stringify(response));
                    Swal.fire({
                        title: "Success!",
                        text: `Verified Serial Number Successful! You get a discount on each subsequent order when you reuse the SIM Card!`,
                        icon: "success"
                    }).then((result) => {
                        if (result.isConfirmed) {
                            //DOWN GIA

                            R.Home.KeepSerialNumberInInput();
                            R.Home.DownPrice();
                        }
                    });
                }
                else {

                    Swal.fire({
                        title: "Error!",
                        text: `Serial Number is not correct! Please try again`,
                        icon: "error"
                    }).then((result) => {
                        if (result.isConfirmed) {
                            sessionStorage.removeItem('current_serial');
                            $('.input-check-serial').val(``);
                            R.Home.DownPriceReverse();
                        }
                    });


                }
                R.Home.RegisterEvent();
            })
        }
        else {
            sessionStorage.removeItem('current_serial');
            $('.input-check-serial').val(``);
            R.Home.DownPriceReverse();
        }
    },

    CSSHOME: function () {
        $('.section-23 .inner-desc p').each((i, v) => {
            if (i == 0) {
                $(v).css('text-align', 'center');
            }
        })
    },

    KeepSerialNumberInInput: function () {
        if (sessionStorage.getItem('current_serial')) {
            var currentSerial = JSON.parse(sessionStorage.getItem('current_serial'));
            console.log(currentSerial);
            var serial = currentSerial.serialNumber;
            console.log(typeof (serial))
            //console.log(currentSerial)
            $('.input-check-serial').val(`${serial}`);
        }
    },
    DownPrice: function () {
        if (sessionStorage.getItem('current_serial')) {
            var currentSerial = JSON.parse(sessionStorage.getItem('current_serial'));
            //$('.input-check-serial').val(currentSerial);

            $('.sim-package-item').each((index, element) => {
                //Tim li gia
                if ($(element).data('sim-pack') != 'Package' && $(element).data('sim-type') != "E-Sim") {
                    $(element).find('.current-price').css('text-decoration', 'line-through');
                    $(element).find('.current-price').find('small').css('display','none');
                    $(element).find('.down-price').addClass('discount-price');
                    $(element).find('.down-price').removeClass('down-price');
                }
                
                
                
            })
            //sim-package-detail
            $('.sim-package-detail').each((index, element) => {
                //Tim li gia
                if ($(element).data('sim-pack') != 'Package' && $(element).data('sim-type') != "E-Sim") {
                    $(element).find('.current-price').css('text-decoration', 'line-through');
                    $(element).find('.down-price').addClass('discount-price');
                    $(element).find('.down-price').removeClass('down-price');
                    $(element).find('.hide-current-price').css('display', 'none');

                }
                

            })
        }
    },
    DownPriceReverse: function () {
        if (!sessionStorage.getItem('current_serial')) {
            var currentSerial = JSON.parse(sessionStorage.getItem('current_serial'));
            //$('.input-check-serial').val(currentSerial);

            $('.sim-package-item').each((index, element) => {
                //Tim li gia
                if ($(element).data('sim-pack') != 'Package' && $(element).data('sim-type') != "E-Sim") {
                    $(element).find('.current-price').css('text-decoration', 'unset');
                    $(element).find('.current-price').find('small').css('display','block')
                    $(element).find('.discount-price').addClass('down-price');
                    $(element).find('.down-price').removeClass('discount-price');
                }



            })
            //sim-package-detail
            $('.sim-package-detail').each((index, element) => {
                //Tim li gia
                if ($(element).data('sim-pack') != 'Package' && $(element).data('sim-type') != "E-Sim") {
                    $(element).find('.current-price').css('text-decoration', 'line-through');
                    $(element).find('.down-price').addClass('discount-price');
                    $(element).find('.down-price').removeClass('down-price');
                    $(element).find('.hide-current-price').css('display', 'none');

                }


            })
        }
    },
    DateTimePicker: function () {
        $('input.jsDateRange').daterangepicker({
            opens: 'left',
            autoApply: true,
            locale: {
                format: 'DD/MM/YYYY'
            }
        }, function (start, end, label) {
            $('.search-diem-den').data('start', start.format('YYYY-MM-DD'))
            $('.search-diem-den').data('end', end.format('YYYY-MM-DD'))
            R.Home.RegisterEvent()
        });
    },
    SwitchRegion: function(el, region_id) {
        var url = R.Home.culture + "/Home/SwitchRegion";
        var params = {
            region_id: region_id
        }
        $.post(url, params, function(response) {
            el.closest('.container').find("._binding_product").html('').html(response);
            var total = el.closest('.container').find('.set-total').data('total');

            //console.log(total);
            var url = el.closest('.container').find('.set-total').data('url');
            
            R.Home.BindingUrlOld();
            R.Home.BindingTotal();
            R.Extra.BindingExtraToProduct();
        })
    },
    BindingUrlOld: function () {
        $('.set-url-old-link').each(function (element) {
            var url = $(this).data('link');
            $(this).closest('.container').find('.url-old-link').attr('href',url);

        })
        $(this).closest('.container').find('.remain-product').closest('.view-remain-product').show();
    },
    BindingProductFilterd: function (params, el) {
        //FilterShomepectificationInZoneListProductList
        $.post(R.Home.culture + '/Product/FilterSpectificationInZoneListProductList', params, function (response) {
            //console.log(response);
            //el.closest('.container').find('._binding_product').html(response);
            //var new_element = el.closest('.container').find('._binding_product');
            //R.Extra.BindingExtraToProductInElement(new_element);
            //R.Product.BindingTotal();
            //R.Product.RegisterEvent();
        })
    },
    ViewMoreRegion: function(id, el, skip, size, indexer) {
        //int zone_parent_id, int locationId, int skip, int size
        var params = {
            zone_parent_id: id,
            locationId: R.Home.currentLocationId,
            skip: size,
            size: size
        }
        $.post(R.Home.culture + '/Home/ViewMoreRegion', params, function(response) {
            el.closest('.container').find('._binding_product').append(response);
            $('.view-more-region-nbv').data('index', indexer + 1);
            //el.closest('.container').find('.view-remain-product').hide();
            //el.hide();
            //R.Home.BindingTotal();
            R.Extra.BindingExtraToProduct();
            R.Home.RegisterEvent();
        })
    },
    BindingTotal: function() {
        $('.set-total').each(function(el) {
            var total = $(this).data('total');
            var id = $(this).data('id');
            //console.log(total);
            if (total <= 9)
                $(this).closest('.container').find('.remain-product').html("<b>" + 0 + "</b>");
            else {
                var remain = total - 9;
                $(this).closest('.container').find('.remain-product').html("<b>" + remain + "</b>");
                $(this).closest('.container').find('.remain-product').closest('.view-remain-product').show();
                $(this).closest('.container').find('.remain-product').closest('.view-remain-product').data('size', total - 9);
                $(this).closest('.container').find('.remain-product').closest('.view-remain-product').data('id', id);
            }

        })
    },
    CountDownFlashSale: function() {
        var flashSales = $('.tab-flashsale');
        flashSales.each((i,v) => {
            var endTime = $(v).data('end-time');
            console.log(endTime)
            R.CountDown(endTime, v);
        })

    },
    DoiTacSlide: function () {
        $('.doitac-slide-wrapper').slick({
            dots: true,
            infinite: false,
            speed: 300,
            slidesToShow: 2,
            slidesToScroll: 2,
            responsive: [
                //{
                //    breakpoint: 1024,
                //    settings: {
                //        slidesToShow: 3,
                //        slidesToScroll: 3,
                //        infinite: true,
                //        dots: true
                //    }
                //},
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
    WhyChooseUsSlide: function () {
        $('.pc-none .row').slick({
            dots: true,
            infinite: false,
            speed: 300,
            slidesToShow: 4,
            slidesToScroll: 4,
            responsive: [
                //{
                //    breakpoint: 1024,
                //    settings: {
                //        slidesToShow: 3,
                //        slidesToScroll: 3,
                //        infinite: true,
                //        dots: true
                //    }
                //},
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
    }


}



$(function () {
    R.Home.Init()
});
