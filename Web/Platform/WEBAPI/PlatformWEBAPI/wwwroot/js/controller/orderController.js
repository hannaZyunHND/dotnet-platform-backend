R.Order = {
    Init: function () {
        //R.Order.OrderDetailInLocalStorage();

        R.Order.culture = R.Culture();
        R.Order.LoadOrderDetail();
        //R.Order.LoadOrderCombo();
        R.Order.RegisterEvent();

    },

    RegisterEvent: function () {

        $('.extra-infomation').off('change').on('change', function () {

            $('.extra-infomation').each(function (i, v) {
                console.log(v);
                $(v).prop('checked', false);
            })
            $(this).prop('checked', true);
            if (!$(this).hasClass('company-bill') || !$(this).hasClass('bank-transfer')) {
                $('.info-cty').css('display', 'none');
            }

        })
        $('.getOderDetailByCode').off('click').on('click', function () {

            var orderCode = $('#txtCodeOrder').val();
            R.Order.GetOrderByCode(orderCode);
        })
        $('.company-bill').off('change').on('change', function () {
            $('.info-cty').css('display', 'none');
            $('.extra-infomation').each(function (i, v) {
                console.log(v);
                $(v).prop('checked', false);
            })
            $(this).prop('checked', true);
            if ($(this).prop('checked') == true) {
                $('.info-company').css('display', 'block');
                $('.info-company').css('display', '');
            }


        })
        //bank-transfer
        $('.bank-transfer').off('change').on('change', function () {
            $('.info-cty').css('display', 'none');
            $('.extra-infomation').each(function (i, v) {
                console.log(v);
                $(v).prop('checked', false);
            })
            $(this).prop('checked', true);
            if ($(this).prop('checked') == true) {
                $('.info-bank').css('display', 'block');
                $('.info-bank').css('display', '');
            }


        })

        $('.plus').off('click').on('click', function () {
            var affected_id = $(this).closest('.item').data('id');
            var quantity = $(this).parent().find('.quantity').val();
            $(this).parent().find('.quantity').val(parseInt(quantity) + 1);
            var el = $(this).closest('.item');
            //Save lai vao localstorage
            var r = JSON.parse(localStorage.getItem("arrProduct"));
            r.find(function (element) {
                if (element.product_id == affected_id) {

                    //alert("Chinh no")
                    element.quantity = parseInt(quantity) + 1;
                }

            })
            ///console.log(r);
            //re-save local
            localStorage.setItem("arrProduct", JSON.stringify(r));
            R.LoadCart();
            R.Order.CalculatePriceProductItem(el)
        });
        $('.minus').off('click').on('click', function () {
            var affected_id = $(this).closest('.item').data('id');
            var quantity = $(this).parent().find('.quantity').val();
            $(this).parent().find('.quantity').val(parseInt(quantity) - 1);
            var el = $(this).closest('.item');
            //Save lai vao localstorage
            var r = JSON.parse(localStorage.getItem("arrProduct"));
            r.find(function (element) {
                if (element.product_id == affected_id) {
                    //alert("Chinh no")
                    if (element.quantity > 0) {
                        element.quantity = parseInt(quantity) - 1;
                    }
                }
            })
            //console.log(r);

            //re-save local
            localStorage.setItem("arrProduct", JSON.stringify(r));
            R.LoadCart();
            R.Order.CalculatePriceProductItem(el);
        });
        $('.choose-promotion').off('change').on('change', function () {
            var id = $(this).closest('.item-promotion').data('product-id');
            var tr_item = '#item-' + id;
            var el = $(tr_item);
            //nsole.log('is Checked  in event')
            R.Order.CalculatePriceProductItem(el);
        });
        $('#link-clear-cart').off('click').on('click', function () {
            R.ClearCart();
            R.LoadCart();
        });
        $('.remove-cart-item').off('click').on('click', function () {
            var remove_id = $(this).closest('.item').data('id');
            $(this).closest('.item').remove();

            //Xoa trong localstorage
            R.RemoveCartItem(remove_id)
            R.Order.CalculateTotalOrders();
            R.LoadCart();
        });
        $('.choosen-item').off('change').on('change', function () {
            R.Order.CalculateTotalOrders();
        });
        $('#order-form').off('submit').on('submit', function (e) {
            //Lay ra thong tin cua khach hang
            e.preventDefault();
            var r = JSON.parse(localStorage.getItem("arrProduct"));
            var tinh = $(this).find('.tinh-thanh option:selected').html();
            var quan = $(this).find('.quan-huyen option:selected').html();
            var phuong = $(this).find('.phuong-xa option:selected').html();
            var nha = $(this).find('.so-nha').val();
            var address = nha + " - " + phuong + " - " + quan + " - " + tinh;
            var gender = $("input[name='gender-radio']:checked").val();
           
            var customer_infomation = {
                Gender: gender,
                Name: $(this).find('.name').val(),
                PhoneNumber: $(this).find('.phone-number').val(),
                Note: $(this).find('.note').val(),
                Address: address,
                Id: 0
            }
            var extra_infomation = [];
            $(this).find('.extra-infomation').each(function (element) {
                if ($(this).is(':checked'))
                    extra_infomation.push($(this).data('content'));
            })
            var product_infomation = [];
            var vouch = $(this).find('.ma-giam-gia').data('ma');
            var voupr = $(this).find('.ma-giam-gia').data('gia');
            var vouty = $(this).find('.ma-giam-gia').data('option');
            $('.item').each(function (element) {
                var el = $(this);
                if (el.find('.choosen-item').is(':checked')) {
                    var promotion = [];
                    el.find('.choose-promotion').each(function (cp) {
                        if ($(this).is(':checked')) {
                            var i = {
                                PromotionId: $(this).data('id'),
                                LogType: $(this).data('type'),
                                LogValue: $(this).data('value'),
                                LogName: $(this).data('name')
                            }
                            promotion.push(i)
                        }
                    })
                    //flash sale
                    var order_type = 1;
                    var order_source_id = 0;
                    if (el.data('is-flash-sale') > 0) {
                        order_type = 3;
                        order_source_id = el.data('is-flash-sale');
                    }
                    //Color
                    var totalPrice = 0;
                    r.forEach((v, i) => {
                        if (v.product_id == el.data('id')) {
                            totalPrice = v.totalPrice
                        }
                    })
                    
                    var product_infomation_item = {
                        ProductId: el.data('id'),
                        Name: el.data('name'),
                        LogPrice: totalPrice,
                        TotalPrice: totalPrice,
                        Quantity: parseInt(el.find('.quantity').val()),
                        SLNguoiLon: parseInt(el.find('.li-so-nguoi-lon').val()),
                        SLTreEm: parseInt(el.find('.li-so-tre-em').val()),
                        SLEmBe: parseInt(el.find('.li-so-em-be').val()),
                        OrderSourceType: order_type,
                        OrderSourceId: order_source_id,
                        Voucher: vouch,
                        VoucherPrice: voupr,
                        VoucherType: vouty,
                        Promotions: promotion
                    }
                    console.log(product_infomation_item)
                    product_infomation.push(product_infomation_item);
                }

            });
            //$('#modal-don-hang').modal('show');
            R.Order.RenderModalConfirmOrder(customer_infomation, extra_infomation, product_infomation);
            return false;
        });
        $('.tinh-thanh').off('change').on('change', function () {
            var locationType = 'quan_huyen';
            var parent = $(this).val();
            R.Order.PickingProvince(locationType, parent);
        });
        $('.quan-huyen').off('change').on('change', function () {
            var locationType = 'phuong_xa';
            var parent = $(this).val();
            R.Order.PickingProvince(locationType, parent);
        });
        $('.cbx-affectedd-all').off('change').on('change', function () {
            if ($(this).is(':checked') == true)
                R.Order.CheckAllCheckbox();
            else
                R.Order.UnCheckAllCheckbox();

        });
        $('.quantity').off('focusout').on('focusout', function () { 
            var affected_id = $(this).closest('.item').data('id');
            var quantity = $(this).val();
            if (quantity != undefined && quantity != "" && quantity != null) { 
                let _quantity = parseInt(quantity);
                let _maxQuantity = parseInt($(this).data('quantit'));
                if (_quantity < 1) {
                    _quantity = 1; 
                } 
                if (_quantity > _maxQuantity) {
                    _quantity = _maxQuantity;
                } 
                $(this).val(_quantity); 
                var el = $(this).closest('.item'); 
                var r = JSON.parse(localStorage.getItem("arrProduct"));
                r.find(function (element) {
                    if (element.product_id == affected_id) { 
                        element.quantity = _quantity;
                    }

                }) 
                localStorage.setItem("arrProduct", JSON.stringify(r));
                R.LoadCart();
                R.Order.CalculatePriceProductItem(el);
            } 
        });
        $('#btnCoupon').off('click').on('click', function () {
            var textInput = $('#Coupon').val();
            var url = "/Order/GetCouPonByCode?code= " + textInput + "";
            var parrent = $(this).closest(".total-pay");
            var price = $("#order-sum").data('price')
            parrent.find("#Coupon").data('code', textInput)
            $.get(url, function (data) {
                if (data != null) {
                    parrent.find(".mess-coupon").show();
                    parrent.find(".mess-coupon").addClass("alert-success");
                    parrent.find(".mess-coupon").removeClass("alert-danger");
                    parrent.find(".mess-coupon").text("Mã được áp dụng : " + data.name);
                    parrent.find("span.data-voucher").attr("data-value", data.valueDiscount);
                    parrent.find("span.data-voucher").attr("data-type", data.discountOption);
                    parrent.find(".mess-coupon").data('codepromotion', data.name);
                    var valDis = parseInt(data.valueDiscount);
                    var ThanhToan = parseInt(price);
                    if (data.discountOption == 2) {
                        parrent.find("#order-discount").text(R.FormatNumber(valDis) + "đ");
                        if (valDis > 0) {
                            var tongtien = ThanhToan - valDis;
                            parrent.find("#order-price").text(R.FormatNumber(tongtien) + "đ")
                            parrent.find("#order-price").data('price', tongtien)
                        }
                    } else if (data.discountOption == 1) {
                        parrent.find("#order-discount").text(data.valueDiscount + "%");
                        if (valDis > 0) {
                            var tongTien = Math.ceil(ThanhToan - (ThanhToan / 100 * valDis));
                            parrent.find("#order-price").text(R.FormatNumber(tongTien))
                            parrent.find("#order-price").data('price', tongTien)
                        }
                    }
                }
                else {
                    parrent.find(".mess-coupon").show();
                    parrent.find(".mess-coupon").addClass("alert-danger");
                    parrent.find(".mess-coupon").removeClass("alert-success");
                    parrent.find(".mess-coupon").text("Mã không tồn tại");
                    parrent.find(".coupon > span.data-voucher").attr("data-value", 0);
                    parrent.find(".coupon > span.data-voucher").attr("data-type", 0);
                    parrent.find("#order-discount").text(0 + "đ");
                    parrent.find("#order-price").text(R.FormatNumber(price));
                    parrent.find("#order-price").data('price', price)
                    parrent.find(".mess-coupon").data('codepromotion', "")
                    parrent.find("#Coupon").data('code', "")
                }
            })
        });

        $('.btn-add').off('click').on('click', function () {
            var inputEl = $(this).closest('.cart-detail__item-amount').find('.ip-sl');
            var affected_id = $(this).closest('.item').data('id');
            var old_value = parseInt($(this).closest('.cart-detail__item-amount').find('.ip-sl').val());
            
            
            $(this).closest('.cart-detail__item-amount').find('.ip-sl').val(old_value + 1);
            var el = $(this).closest('.item');
            //Save lai vao localstorage
            var r = JSON.parse(localStorage.getItem("arrProduct"));
            r.find(function (element) {
                if (element.product_id == affected_id) {
                    if (inputEl.hasClass('li-so-nguoi-lon')) {
                        element.quantityNguoiLon = old_value + 1;
                        var price_gia = inputEl.data('gia');
                        var old_gia = inputEl.closest('.item').find('.item-sum').data('sale-price');
                        inputEl.closest('.item').find('.item-sum').data('sale-price', old_gia + price_gia);
                        inputEl.closest('.item').find('.item-sum').text(R.FormatNumber(old_gia + price_gia));
                        element.totalPrice = old_gia + price_gia;
                    }
                        
                    if (inputEl.hasClass('li-so-tre-em')) {
                        element.quantityTreEm = old_value + 1;
                        var price_gia = inputEl.data('gia');
                        var old_gia = inputEl.closest('.item').find('.item-sum').data('sale-price');
                        inputEl.closest('.item').find('.item-sum').data('sale-price', old_gia + price_gia);
                        inputEl.closest('.item').find('.item-sum').text(R.FormatNumber(old_gia + price_gia));
                        element.totalPrice = old_gia + price_gia;
                    }
                        
                    if (inputEl.hasClass('li-so-em-be')) {
                        element.quantityEmBe = old_value + 1;
                        var price_gia = inputEl.data('gia');
                        var old_gia = inputEl.closest('.item').find('.item-sum').data('sale-price');
                        inputEl.closest('.item').find('.item-sum').data('sale-price', old_gia + price_gia);
                        inputEl.closest('.item').find('.item-sum').text(R.FormatNumber(old_gia + price_gia));
                        element.totalPrice = old_gia + price_gia;
                    }
                        
                }

            })
            
            ///console.log(r);
            //re-save local
            localStorage.setItem("arrProduct", JSON.stringify(r));
            R.LoadCart();
            R.Order.CalculateTotalOrders();
            //R.Order.CalculatePriceProductItem(el)
        })
        $('.btn-sub').off('click').on('click', function () {
            var inputEl = $(this).closest('.cart-detail__item-amount').find('.ip-sl');
            var affected_id = $(this).closest('.item').data('id');
            var old_value = parseInt($(this).closest('.cart-detail__item-amount').find('.ip-sl').val());
            console.log(old_value)
            if ((inputEl.hasClass('li-so-nguoi-lon') && old_value > 1) || (!inputEl.hasClass('li-so-nguoi-lon') && old_value > 0)) {
                $(this).closest('.cart-detail__item-amount').find('.ip-sl').val(old_value - 1);
                var el = $(this).closest('.item');
                //Save lai vao localstorage
                var r = JSON.parse(localStorage.getItem("arrProduct"));
                r.find(function (element) {
                    if (element.product_id == affected_id) {

                        if (inputEl.hasClass('li-so-nguoi-lon')) {
                            element.quantityNguoiLon = old_value - 1;
                            var price_gia = inputEl.data('gia');
                            var old_gia = inputEl.closest('.item').find('.item-sum').data('sale-price');
                            inputEl.closest('.item').find('.item-sum').data('sale-price', old_gia - price_gia);
                            inputEl.closest('.item').find('.item-sum').text(R.FormatNumber(old_gia - price_gia));
                            element.totalPrice = old_gia - price_gia;
                        }

                        if (inputEl.hasClass('li-so-tre-em')) {
                            element.quantityTreEm = old_value - 1;
                            var price_gia = inputEl.data('gia');
                            var old_gia = inputEl.closest('.item').find('.item-sum').data('sale-price');
                            inputEl.closest('.item').find('.item-sum').data('sale-price', old_gia - price_gia);
                            inputEl.closest('.item').find('.item-sum').text(R.FormatNumber(old_gia - price_gia));
                            element.totalPrice = old_gia - price_gia;
                        }

                        if (inputEl.hasClass('li-so-em-be')) {
                            element.quantityEmBe = old_value - 1;
                            var price_gia = inputEl.data('gia');
                            var old_gia = inputEl.closest('.item').find('.item-sum').data('sale-price');
                            inputEl.closest('.item').find('.item-sum').data('sale-price', old_gia - price_gia);
                            inputEl.closest('.item').find('.item-sum').text(R.FormatNumber(old_gia - price_gia));
                            element.totalPrice = old_gia - price_gia;
                        }
                    }

                })
                ///console.log(r);
                //re-save local
                localStorage.setItem("arrProduct", JSON.stringify(r));
                R.LoadCart();
                R.Order.CalculateTotalOrders();
                //R.Order.CalculatePriceProductItem(el)
            }

        })
    },
    ReCalculatePriceTour: function (el) {

    },
    CheckOnlyCheckboxInfo: function (el) {

    },
    CheckAllCheckbox: function () {
        $('.choosen-item').each(function (element) {
            $(this).prop('checked', true);
        })

    },
    UnCheckAllCheckbox: function () {
        $('.choosen-item').each(function (element) {
            $(this).prop('checked', false);
        })
    },
    
    LoadOrderDetail: function () {
        var r = JSON.parse(localStorage.getItem("arrProduct"));
        console.log(r);
        //$('#link-target-cart').data('product_ids'), product_ids.toString());
        var product_ids = $('#link-target-cart').data('product_ids')
        var productList = [];
        r.forEach((v) => {
            productList.push(v.product_id)
        })
        var params = {
            product_ids: productList.join(',')
        }
        //Load ajax
        $.post(R.Order.culture + "/Order/LoadOrderDetail", params, function (response) {
            console.log(response);
            $('._binding_order').html('').html(response);
            R.Order.CheckAllCheckbox();
            R.Order.OrderDetailInLocalStorage();

        })
    },

    LoadOrderCombo: function () {
        var r = JSON.parse(localStorage.getItem("arrProduct"));
        //$('#link-target-cart').data('product_ids'), product_ids.toString());
        var product_ids = $('#link-target-cart').data('product_ids')
        var params = {
            product_ids: product_ids
        }
        //Load ajax
        $.post(R.Order.culture + "/Order/LoadOrderCombo", params, function (response) {
            $('.products-combo').html('').html(response);
            R.Order.CheckAllCheckbox();
            R.Order.OrderDetailInLocalStorage();

        })
    },
    //Kiem tra Order trong LocalStorage
    OrderDetailInLocalStorage: function () {
        var r = JSON.parse(localStorage.getItem("arrProduct"));
        console.log(r);
        r.forEach(function (element) {
            var product_id = element.product_id;
            var el = "#item-" + product_id;
            console.log(element.totalPrice);
            $(el).find('.quantity').val(element.quantity);
            //var sale_price = parseInt($(el).find('.price-new').data('sale-price'));
            $(el).find('.item-sum').text(R.FormatNumber(element.totalPrice));
            $(el).find('.item-sum').data('sale-price', element.totalPrice);
            $(el).find('.li-so-nguoi-lon').val(element.quantityNguoiLon);
            $(el).find('.li-so-tre-em').val(element.quantityTreEm);
            $(el).find('.li-so-em-be').val(element.quantityEmBe);

            if (element.couponUse != null && element.couponUse.length > 0) {
                $(el).find('.ma-giam-gia').text('(Mã giảm giá: ' + element.couponUse + ')');
                $(el).find('.ma-giam-gia').data('gia', element.couponValue);
                $(el).find('.ma-giam-gia').data('ma', element.couponUse);
                $(el).find('.ma-giam-gia').data('option', element.couponOption);
                $(el).find('.ma-giam-gia').data('totaldiscount', element.totalDiscount);
            }
            R.Order.CalculateTotalOrders();
            //R.Order.CalculatePriceProductItem($(el));
        })
        R.Order.RegisterEvent();
    },
    CalculatePriceProductItem: function (el) {
        var quantity = parseInt(el.find('.quantity').val());
        var sale_price = parseInt(el.find('.price-new').data('sale-price'));
        ////Lay phan tru dich vu
        //var id = el.data('id');
        //var tr_promotion_id = '#item-promotion-' + id;
        //var promo_el = $(tr_promotion_id);
        //var discount = 0;
        //promo_el.find('.choose-promotion').each(function (element) {
        //    if (this.checked && $(this).data('is-discount-price') == 1) {
        //        //console.log('is Checked  in function');
        //        discount += parseInt($(this).data('value'));
        //    }

        //})

        //Cong tru gia tri o day
        var price_after = sale_price * quantity - discount;
        el.find('.item-sum').text(R.FormatNumber(price_after));
        el.find('.item-sum').data('item-sum', price_after);
        R.Order.CalculateTotalOrders();
        R.Order.RegisterEvent();
    },
    CalculateTotalOrders: function () {
        //Tinh toan de ra tong tien
        var total = 0;
        var total_product_child=0;
        $('.item').each(function (element) {
            if ($(this).find('.choosen-item').is(':checked')) {
                total += parseInt($(this).find('.item-sum').data('sale-price'));
            }
        });
        var discount = 0;
        $('.item').each(function (element) {
            if ($(this).find('.choosen-item').is(':checked')) {
                var checker = parseInt($(this).find('.ma-giam-gia').data('totaldiscount'));
                if (!isNaN(checker)) {
                    discount += parseInt($(this).find('.ma-giam-gia').data('totaldiscount'));
                }
                
            }
        });
        
        //console.log(total_product_child);
        var order_before_coupon = total + discount;
        
        
        $('#order-sum').text(R.FormatNumber(order_before_coupon));
        $('#order-sum').data('price', order_before_coupon);
        $('#order-discount').text(R.FormatNumber(discount));
        $('#order-discount').data('price', discount);
        $('#order-price').text(R.FormatNumber(total));
        $('#order-price').data('price', total);
        R.Order.RegisterEvent();
    },
    RenderModalConfirmOrder: function (customer_infomation, extra_infomation, product_infomation) {

        var df = "JHN-";
        var u = '/Order/GetOrderCode';
        var prvalue = $('#order-discount').text();
        $.post(u, null, function (response) {
            //alert(response);
            var num = ''
            if (response < 10)
                num = '00' + response;
            if (response < 100 && response > 10)
                num = '0' + response;
            if (response > 100)
                num = response
            var code = df + num;
            event.preventDefault();
            var modals = $('#modal-don-hang');
            //Xoa toan bo class 
            modals.find('.table-report-item-append').each(function (element) {
                $(this).remove();
            })
            //Fill thong tin nguoi nhan
            modals.find('.customer-name').text(customer_infomation.Name);
            modals.find('.customer-phone').text(customer_infomation.PhoneNumber);
            modals.find('.customer-address').text(customer_infomation.Address);
            //Fill thong tin bo sung
            var ttbs = '';
            extra_infomation.forEach(function (element) {
                ttbs += '<li>' + element + '</li>';
            })
            modals.find('.list_extra_infomation').empty().append(ttbs);

            //Fill thong tin don hang
            //var tthh = '';
            product_infomation.forEach(function (element) {
                $('.table-report-item').last().clone().insertAfter('.table-report-item:last');
                var row = $('.table-report-item').last();
                row.find('.product-id').text(element.ProductId);
                row.find('.product-name').text(element.Name);
                row.find('.li-tbl-nglon').text(element.SLNguoiLon);
                row.find('.li-tbl-trem').text(element.SLTreEm);
                row.find('.li-tbl-embe').text(element.SLEmBe);
                row.find('.product-quantity').text(R.FormatNumber(element.Quantity));
                row.find('.product-price').text(R.FormatNumber(element.TotalPrice));
                row.find('.product-total-price').text(R.FormatNumber(R.FormatNumber(element.TotalPrice)));
                var promo_html = '';
                element.Promotions.forEach(function (p) {
                    promo_html += '<li>' + p.LogName + '</li>';
                })
                row.find('.product-promotion-ul').empty().append(promo_html);
                row.css('display', 'table-row');
                row.addClass('table-report-item-append');
            })
            //$('#order-price').data('price', order_price);
            modals.find('.total-payment').html(R.FormatNumber($('#order-price').data('price')) + 'vnd');
            modals.find('.code-promotion').html($('.ma-giam-gia').data('ma'));
            modals.find('.values-promotion').html(prvalue);
            $('#modal-don-hang').modal('show');
            //var code = '';
            //R.Order.GenOrderCode(code);
            $('.order-code').text(code);
            var now = moment(new Date()).format('DD - MM - YYYY');
            $('.order-time').text(now);
            var p_ids = [];
            product_infomation.forEach(function (element) {
                p_ids.push(element.ProductId);
            })
            var htm = $('#modal-don-hang').find('.modal-content').html();
            product_infomation.forEach((v, i) => {
                v.Quantity = 1;
            })
            modals.find('.btn-save').off('click').on('click', function () {
                var orders = {
                    OrderCode: code,
                    Customer: customer_infomation,
                    Products: product_infomation,
                    Extras: extra_infomation,
                    CodePromotion: $('.mess-coupon').data('codepromotion'),
                    ValuePromotion: prvalue
                }
                var url = R.Order.culture + '/Order/CreateOrder';
                $.post(url, orders, function (response) {
                    //alert(response);
                    p_ids.forEach(function (element) {
                        R.RemoveCartItem(element);
                    })
                    R.LoadCart();

                    $('#modal-don-hang').modal('hide');
                    $('#modal-xn').modal('show');
                    R.Order.SendMail('order', htm);
                })
            })
        })
        return false;
    },
    GetOrderByCode: function (orderCode) {
        console.log(orderCode);
        var params = {
            orderCode: orderCode
        }
        //Load ajax
        var url = R.Order.culture + '/Order/GetOrderByCode';
        $.post(url, params, function (response) {
            console.log(response);
            $('.login').html('').html(response);
            //R.Order.CheckAllCheckbox();
            //R.Order.OrderDetailInLocalStorage();

        })
    },
    PickingProvince: function (locationType, parent) {
        var el_name = "." + locationType.replace('_', '-');
        var params = {
            //(string locationType,string parent
            locationType: locationType,
            parent: parent
        }
        var url = '/Order/GetQuanHuyen'
        $.post(url, params, function (response) {
            var result = JSON.parse(response);
            //console.log(result);
            //console.log(el_name);
            $(el_name).prop('disabled', false);
            var htm = ''
            result.forEach(function (element) {
                htm += '<option value="' + element.Key + '">' + element.Value.name_with_type + '</option>'
            })
            $(el_name).html('').html(htm);
        })


    },
    GenOrderCode: function (handleData) {

    },
    SendMail: function (type, body) {
        var params = {
            type: type,
            body: body
        }
        var url = R.Order.culture + '/Extra/SendMail';
        $.post(url, params, function (response) {
            console.log(response);
        })
    }


}

$(function () {
    R.Order.Init();
})


//var gio_hang = [
//    {
//        product_id: 1,
//        quantity: 1,
//        promotion: [
//            {
//                id: 1,
//                checked: true
//            },
//            {
//                id: 2,
//                checked: false
//            }
//        ]
//    }
//]