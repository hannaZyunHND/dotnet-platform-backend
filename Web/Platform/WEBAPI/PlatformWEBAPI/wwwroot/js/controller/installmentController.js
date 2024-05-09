R.Installment = {
    Init: function () { 
        R.Installment.culture = R.Culture(); 
        R.Installment.RegisterEvent();
        R.Installment.SelectedBank = 0;
        R.Installment.SelectedBankName = "";
        R.Installment.SelectedPeriod = 0;
        R.Installment.CalculateInfo = [];
        R.Installment.SelectedBankInstallmentDetailId = 0;
        R.Installment.CurrentTraGopInfo = {};
    },

    RegisterEvent: function () {
        $('.plus').off('click').on('click', function () {
            var quantity = $(this).parent().find('.quantity').val();
            $(this).parent().find('.quantity').val(parseInt(quantity) + 1);
        });
        $('.minus').off('click').on('click', function () {
            var affected_id = $(this).closest('.item').data('id');
            var quantity = $(this).parent().find('.quantity').val();
            $(this).parent().find('.quantity').val(parseInt(quantity) - 1);    
        });            
        $('.quantity').off('focusout').on('focusout', function () {  
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
            } 
        });
        $('input[type=radio][name=bank-radio]').off('change').on('change', () => {
            var checked = $('input[name=bank-radio]:checked').val();
            var productId = $('.productId').data('id');
            R.Installment.SelectedBankName = $('input[name=bank-radio]:checked').data('name');
            
            var bankId = checked
            R.Installment.SelectedBank = checked;
            R.Installment.GetPeriods(productId, bankId);
        })
        $('#ky-han-slb').off('change').on('change', function () {
            var period = $(this).val();
            R.Installment.SelectedPeriod = period;
            var productId = $('.productId').data('id');
            var bankId = R.Installment.SelectedBank;
            R.Installment.GetPeriodsDetail(productId, bankId, period)
        })
        $('#tra-truoc-slb').off('change').on('change', function () {
            R.Installment.SelectedBankInstallmentDetailId = $(this).val();
            R.Installment.ShowHideDetailTable();
        })
        $('.tinh-thanh').off('change').on('change', function () {
            var locationType = 'quan_huyen';
            var parent = $(this).val();
            R.Installment.PickingProvince(locationType, parent);
        });
        $('.quan-huyen').off('change').on('change', function () {
            var locationType = 'phuong_xa';
            var parent = $(this).val();
            R.Installment.PickingProvince(locationType, parent);
        });
        $('.cart-detail').off('submit').on('submit', function () {
            //Lay ra thong tin cua khach hang
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
            console.log(customer_infomation)
            var extra_infomation = [];
            $(this).find('.extra-infomation').each(function (element) {
                if ($(this).is(':checked'))
                    extra_infomation.push($(this).data('content'));
            })
            var product_infomation = [];
            var vouch = $(this).find('#Coupon').data('code');
            var voupr = $(this).find('span.data-voucher').data('value');
            var vouty = $(this).find('span.data-voucher').data('type');
            $('.cart-detail__item').each(function (element) {
                var el = $(this);
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
                var order_type = 4;
                var order_source_id = 0;
                if (el.data('is-flash-sale') > 0) {
                    order_type = 3;
                    order_source_id = el.data('is-flash-sale');
                }
                //Color

                var product_infomation_item = {
                    ProductId: el.data('id'),
                    Name: el.data('name'),
                    LogPrice: el.data('sale-price'),
                    Quantity: parseInt(el.find('#quantity').val()),
                    OrderSourceType: order_type,
                    OrderSourceId: order_source_id,
                    Voucher: vouch,
                    VoucherPrice: voupr,
                    VoucherType: vouty,
                    Promotions: promotion
                }
                console.log(product_infomation_item)
                product_infomation.push(product_infomation_item);
            });
            //$('#modal-don-hang').modal('show');
            R.Installment.RenderModalConfirmOrder(customer_infomation, extra_infomation, product_infomation);
            return false;
        });
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
            var totalProce = 0;
            product_infomation.forEach(function (element) {
                $('.table-report-item').last().clone().insertAfter('.table-report-item:last');
                var row = $('.table-report-item').last();
                row.find('.product-id').text(element.ProductId);
                row.find('.product-name').text(element.Name);
                row.find('.product-quantity').text(element.Quantity);
                row.find('.product-price').text(R.FormatNumber(element.LogPrice));
                row.find('.product-total-price').text(R.FormatNumber(element.LogPrice * element.Quantity));
                totalProce += element.LogPrice * element.Quantity;
                var promo_html = '';
                element.Promotions.forEach(function (p) {
                    promo_html += '<li>' + p.LogName + '</li>';
                })
                row.find('.product-promotion-ul').empty().append(promo_html);
                row.css('display', 'table-row');
                row.addClass('table-report-item-append');
            })
            //$('#order-price').data('price', order_price);
            modals.find('.total-payment').html(R.FormatNumber(totalProce) + 'vnd');
            modals.find('.code-promotion').html($('.mess-coupon').data('codepromotion'));
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
            modals.find('.btn-save').off('click').on('click', function () {
                var orders = {
                    OrderCode: code,
                    Customer: customer_infomation,
                    Products: product_infomation,
                    Extras: extra_infomation,
                    CodePromotion: $('.mess-coupon').data('codepromotion'),
                    ValuePromotion: prvalue,
                    InstallmentDetail: R.Installment.CurrentTraGopInfo
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
    GetPeriods: function (productId, bankId) {
        R.Installment.SelectedPeriod = 0;
        R.Installment.SelectedBankInstallmentDetailId = 0;
        R.Installment.CalculateInfo = [];
        var url = "/Installment/GetPeriods?productId=" + productId + "&bankId=" + bankId + "";
        $.get(url, (response) => {
            
            var htm = "";
            htm += '<option hidden>Chọn kỳ hạn</option>'
            response.forEach((v) => {
                htm += '<option value="'+v+'">'+v+' Tháng</option>'
            })
            console.log(htm);
            $('#ky-han-slb').html('').html(htm);
            R.Installment.ShowHideDetailTable();
            R.Installment.RegisterEvent();
        })
    },
    GetPeriodsDetail: function (productId, bankId, period) {
        R.Installment.SelectedBankInstallmentDetailId = 0;
        R.Installment.CalculateInfo = [];
        var url = "/Installment/GetBankPeriodDetail?productId=" + productId + "&bankId=" + bankId + "&period=" + period + "";
        $.get(url, (response) => {
            console.log(response)
            R.Installment.CalculateInfo = response;
            var htm = "";
            htm += '<option hidden>Chọn số tiền trả trước</option>'
            response.forEach((v) => {
                if (v.paymentFirstPercent > 0 && v.paymentFirst <= 0) {
                    var calculated = R.Installment.CalculateFirstPriceMoney(v.paymentFirstPercent);
                    htm += '<option value="' + v.id + '">' + R.FormatNumber(calculated) + ' đ</option>'
                }
                if (v.paymentFirstPercent <= 0 && v.paymentFirst > 0) {
                    htm += '<option value="' + v.id + '">' + R.FormatNumber(v.paymentFirst) + ' đ</option>'
                }
            })
            $('#tra-truoc-slb').html('').html(htm);
            R.Installment.ShowHideDetailTable();
            R.Installment.RegisterEvent();
        })
    },
    CalculateFirstPriceMoney: function (percent) {
        var price = $('.cart-detail__item-price').data('price');
        return price * percent / 100;
    },
    ShowHideDetailTable: function () {
        var el = $(".cart-detail-boxbank__table");
        if (R.Installment.SelectedBankInstallmentDetailId <= 0) {
            el.css('display', 'none');
        }
        else {
            el.css('display', '');
            //so thang tra gop
            $('.so-thang-tra-gop').html('').html(R.Installment.SelectedPeriod + ' Tháng');
            //so tra gop moi thang
            R.Installment.CalcualteTraGopMoiThang()

        }
    },
    CalcualteTraGopMoiThang: function () {
        var price = $('.cart-detail__item-price').data('price');
        console.log(R.Installment.CalculateInfo)
        console.log(R.Installment.SelectedBankInstallmentDetailId)
        var selected_id = 0;
        for (var i = 0; i < R.Installment.CalculateInfo.length; i++) {
            if (R.Installment.CalculateInfo[i].id == parseInt(R.Installment.SelectedBankInstallmentDetailId))
                selected_id = i;
        }
        var v = R.Installment.CalculateInfo[selected_id];
        if (v !== 'undefined') {
            console.log(v);
            var traTruoc = 0;
            if (v.paymentFirstPercent > 0 && v.paymentFirst <= 0) {
                traTruoc = R.Installment.CalculateFirstPriceMoney(v.paymentFirstPercent)
            }
            if (v.paymentFirstPercent <= 0 && v.paymentFirst > 0) {
                traTruoc = v.paymentFirst;
            }
            var gopMoiThang = 0;
            gopMoiThang = Math.round(((price - traTruoc) + ((price - traTruoc) * v.interestPercent)) / R.Installment.SelectedPeriod);
            var tongTraGop = 0;
            tongTraGop = Math.round(traTruoc + (price - traTruoc) + (price - traTruoc) * v.interestPercent);
            gopMoiThang = Math.round(gopMoiThang / 1000) * 1000;
            tongTraGop = Math.round(tongTraGop / 1000) * 1000;
            var chenhLech = 0;          
            chenhLech = tongTraGop - price;
            $('.ngan-hang-tra-gop').html('').html(R.Installment.SelectedBankName);
            $('.so-tien-tra-truoc').html('').html(R.FormatNumber(traTruoc));
            $('.gop-moi-thang').html('').html(R.FormatNumber(gopMoiThang));
            $('.tong-tien-tra-gop').html('').html(R.FormatNumber(tongTraGop));
            $('.chenh-lech-tra-thang').html('').html(R.FormatNumber(chenhLech));

            R.Installment.CurrentTraGopInfo.NganHang = R.Installment.SelectedBankName;
            R.Installment.CurrentTraGopInfo.SoThangTraGop = R.Installment.SelectedPeriod;
            R.Installment.CurrentTraGopInfo.TraTruoc = traTruoc;
            R.Installment.CurrentTraGopInfo.TraGopMoiThang = gopMoiThang;
            R.Installment.CurrentTraGopInfo.TongSoTien = tongTraGop;
            R.Installment.CurrentTraGopInfo.ChenhLech = chenhLech;
            R.Installment.RegisterEvent();
        }
        
    }


}
$(function () {
    R.Installment.Init()
})

