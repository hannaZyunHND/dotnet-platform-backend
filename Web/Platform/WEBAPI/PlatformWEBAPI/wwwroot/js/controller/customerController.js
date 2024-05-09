R.Customer = {
    Init: function () {
        R.Customer.CurrentLocationId = R.CurrentLocationId();
        R.Customer.RegisterEvent();
        R.Customer.GetMySim();
        R.Customer.GetOrders();
    },
    RegisterEvent: function () {
        $('.choose-top-up').off('click').on('click', function () {
            var product_id = $(this).data('id');
            var order_id = $(this).data('order-id');
            var order_iccid = $(this).data('order-iccid');
            var request = {
                product_id: product_id,
                order_id: order_id,
                order_iccid: order_iccid
            }
            R.Customer.GetTopUps(request);
            
        })
        $('.inner-buy-top-up-now').off('click').on('click', function () {
            var url = $(this).data('product-url');

            //Luu du lieu orderID vao localStorage
            localStorage.setItem('onPickingOrder', $(this).data('order-id'));
            //Redirect sang trang
            window.location.href = `/${url}.html`;
        })
        $('.add-quantity').off('click').on('click', function () {
            var id = $(this).closest('.sim-item').data('id');
            var carts = R.LoadCart();
            carts.forEach((element) => {
                if (element.id == id)
                    element.quantity++;
            })
            //Doi text
            var current = parseInt($(this).parent().find('.inner-quantity').text());
            $(this).parent().find('.inner-quantity').html('').html(current+1);
            //Save carts
            localStorage.setItem('sim-carts', JSON.stringify(carts));
            //console.log(R.LoadCart())

        })
        $('.minus-quantity').off('click').on('click', function () {
            var id = $(this).closest('.sim-item').data('id');
            var carts = R.LoadCart();
            carts.forEach((element) => {
                if (element.id == id)
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
            }
            

        })
        $('.remove-sim-item').off('click').on('click', function () {
            var id = $(this).closest('.sim-item').data('id');
            //Xoa ca khoi
            $(this).closest('.sim-item').remove();
            R.RemoveFromCart(id);
        })
        $('.btn-buy-all-carts').off('click').on('click', function () {
            var carts = R.LoadCart();
            var productIds = carts.map(e => e.productId).toString();
            console.log(productIds);
            var url = `buy-all-cart/${productIds}`;
            window.location.href = url;
            
        })
        $('.open-modal-coverage').off('click').on('click', function () {
            //data-coverage
            var coverages = $(this).data('coverage');
            //alert(coverages)
            $.get(`/Zone/ModalListOfCountries?zoneIds=${coverages}`).then((response) => {
                $('#modal-coverage ul').html('').html(response);
                $('#modal-coverage').modal('show')
                R.Customer.RegisterEvent();
            })
        })
        $('.buy-iccid-top-up').off('click').on('click', function () {
            //alert('AN')
            var serial = $(this).data('serial');
            var modalText = $(this).data('modal-text');
            modalText = modalText.replace('[SERIAL]', serial);
            //Kiem tra xem serial co kha dung khong
            $.get('/Order/CheckSerialNumber?serialNumber=' + serial).then((response) => {
                if (response) {
                    // Luu vao session storage
                    sessionStorage.setItem('current_serial', JSON.stringify(response));
                    Swal.fire({
                        title: "Success!",
                        text: modalText,
                        icon: "success"
                    }).then((result) => {
                        if (result.isConfirmed) {
                            //DOWN GIA
                            window.location.href = "/"
                        }
                    });
                }
                R.Customer.RegisterEvent();
            })
        })
    },
    GetMySim: function () {
        var url = "/Customer/GetProductsByCustomerId"
        var currentCustomer = R.LoadCustomerInfo();
        if (currentCustomer) {
            var request = { customerId: currentCustomer.id, localtionId: 0 };
            console.log(request)

            if (currentCustomer) {
                $.post(url, request).then((response) => {
                    $('#tab-esim-1').html('').html(response);
                    R.Customer.RegisterEvent();
                })
            }
        }
        
    },
    GetTopUps: function (request) {
        $.post('/Customer/GetTopUpsModalDetail', request).then((response) => {
            $('#modalBuySim').html('').html(response);
            $("#modalBuySim").modal('show');

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
            R.Customer.RegisterEvent();
        }).catch((err) => { });
    },
    GetOrders: function () {
        var carts = R.LoadCart();
        console.log(carts)
        var products = carts.map(e => e.productId);
        var url = "/Customer/GetProductsInCartByProductIs";
        $.post(url, { products: products }).then((response) => {
            $('#tab-esim-2').html('').html(response)

            $('.sim-item').each((index, element) => {
                var id = $(element).data('id');
                //Tim kiem trong gio hang
                var cartItem = carts.find(r => r.id == id);
                if (cartItem) {
                    $(element).find('.inner-quantity').text(cartItem.quantity)
                }

            })
            R.Customer.RegisterEvent();

        })
    }
}
R.Customer.Init();