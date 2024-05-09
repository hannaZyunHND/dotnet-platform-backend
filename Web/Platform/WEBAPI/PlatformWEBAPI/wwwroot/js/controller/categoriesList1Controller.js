
R.CategoriesList1 = {
    Init: function () {
        R.CategoriesList1.culture = R.Culture();
        var selected = $('.select-page').data('page-index');
        //alert(selected);
        $('.select-page').val(selected);
        R.CategoriesList1.RegisterEvent();
    },
    RegisterEvent: function () {

        $('.preview_page').off('click').on('click', function () {
            var zone_id = $(this).data('zone-id');
            var page_index = $(this).data('page-index');
            var to_page = page_index - 1;
            var page_size = $(this).data('page-size');
            var min_page = $(this).data('min-page');
            var type = $(this).data('type');
            var alias = $(this).data('alias')
            if (page_index > min_page) {
                R.CategoriesList1.ViewMore(zone_id, to_page, page_size, type, alias);
            }


        });
        $('.next_page').off('click').on('click', function () {
            var zone_id = $(this).data('zone-id');
            var page_index = $(this).data('page-index');
            var to_page = page_index + 1;
            var page_size = $(this).data('page-size');
            var max_page = $(this).data('max-page');
            var type = $(this).data('type');
            var alias = $(this).data('alias');
            if (page_index < max_page) {
                R.CategoriesList1.ViewMore(zone_id, to_page, page_size, type, alias);
            }

        });
        $('#buttonLoadMore').off('click').on('click', function () {
            
            let _this = this;
            let pageIndex = $(_this).data('page');
            let ZoneId = $(_this).data('zoneId');
            let tybeView = $(_this).data('viewBy');

            let total = $(_this).children().html() - 10;
            $.get("/Product/ListProductOldRenewal?pageIndex=" + (pageIndex + 1) + "&pageSize=10&ZoneId=" + ZoneId + "&typeView=" + tybeView + "", function (response) {
                $('#ListProduct').append(response);
                $(_this).data('page', pageIndex + 1)
                if (response.trim() == "")
                    $('#buttonLoadMore').css('display', 'none');

                else
                    $('#buttonLoadMore').css('display', 'inline');

            })
        });
        $('.btnBanLai').off('click').on('click', function () {
            
            let _this = this;
            let p = $(_this).data('product');
            $('#modalBanLai #avatar').html(' <img asp-append-version="true" src="' + R.StoreFilePath(true) + p.avatar +'" alt="'+ p.name+ '" />');
            $('#modalBanLai #name').html(p.name);
            $('#modalBanLai #price').html(R.FormatNumber(p.price));
            $('#modalBanLai').modal();
            $('#productId').val(p.productId);
        });
        $('.select-page').off('change').on('change', function () {
            var zone_id = $(this).data('zone-id');
            var to_page = $(this).val();
            var page_size = $(this).data('page-size');
            var type = $(this).data('type');
            var alias = $(this).data('alias');
            R.CategoriesList1.ViewMore(zone_id, to_page, page_size, type, alias);
        })
        $('input[name=collected-type]').off('change').on('change', function () {
            var price = this.value
            $('.Price-option').text(R.FormatNumber(price));
        })

    },
    ViewMore: function (zone_id, to_page, page_size, type, alias) {
        //var url = R.BlogList2.culture + "/Blog/MoreBlogs";
        ////int zone_id, int type, string filter, int page_index, int page_size
        //var params = {
        //    zone_id: zone_id,
        //    type: type,
        //    filter: "",
        //    page_index: to_page,
        //    page_size: page_size
        //}
        //$.post(url, params, function (response) {
        //    console.log(response);
        //    $('.binding-blog').html(response);
        //    $('.preview_page').data('page-index', to_page);
        //    $('.next_page').data('page-index', to_page);
        //    $('.select-page').val(to_page);
        //})
        var url = R.CategoriesList1.culture + "/" + alias + ".dc" + zone_id + ".html";
        var queryStirng = "?pageIndex=" + to_page;
        window.location.href = url + queryStirng;
    }

}

$(function () {
    R.CategoriesList1.Init();
})



function isVietnamesePhoneNumber(number) {
    return /(03|05|07|08|09|01[2|6|8|9])+([0-9]{8})\b/.test(number);
}
function isEmpty(str) {
    return (!str || str.length === 0);
}
function submitRequestCustomerProduct(type) {
    var phone = $('#phone').val();
    var fullname = $('#fullname').val();
    var location = $('#location option:selected').val();
    var department = $('#department option:selected').val();
    var productId = $('#productId').val();
    var dealPrice = null;
    var productIdToExchange = null;
    if (type == 2) {
        dealPrice = $('#priceModal').text().replace('.', '').replace('đ','');
        productIdToExchange = $('#oldProductId').val();
    }
    if (!isVietnamesePhoneNumber(phone)) {
        alert("Nhập đúng định dạng Số điện thoại");
        return;
    }
    if (isEmpty(fullname)) {
        alert("Nhập họ tên");
        return;
    }
    if (location == "0") {
        alert("Chọn khu vực");
        return;
    }
    if (department == "0") {
        alert("Chọn chi nhánh");
        return;
    }


    var params = {
        productId: productId, fullName: fullname, phoneNumber: phone, locationId: location, type: type, departmentId: department, dealPrice: dealPrice, productIdToExchange: productIdToExchange
    };

    $.post('/Product/AddCustomerProductOldRenewalRequest/', params, function (response) {
        if (response.result == 1)
            alert('Gửi yêu cầu thành công')
        else
            alert('Gửi yêu cầu không thành công')
        
        $('#phone').val('');
        $('#fullname').val('');
        $('#productId').val('');
        $('#modalBanLai').modal('toggle');
        $('#modalDoiMoi').modal('toggle');
    })

}

function loadProductCat(zoneId, event) {
    $('.section-head__link-item a').removeClass("setSelected");
    $(event).addClass("setSelected");
    let _this = $('#buttonLoadMore');
    let pageIndex = $(_this).data('page');
    let tybeView = $(_this).data('viewBy');

    $.get("/Product/ListProductOldRenewalJson?pageIndex=1&pageSize=10&ZoneId=" + zoneId + "&typeView= " + tybeView + "", function (response) {
        if (response.length > 0) {
            $('#ListProduct').html('');
            response.map(item => {
                let result = "<div class='products-grid__item'>" +
                    "<div class='product-item-grid'>" +
                    "<div class='product-item-grid__image'>" +
                    "<a href='" + item.linktar + "'>" +
                    "<img asp-append-version='true' src='" + item.avatar + "'" +
                    "class='w-100 ' alt='" + item.name + "' />" +
                    " </a>" +
                    "</div>" +
                    "<div class='product-item-grid__content'>" +
                    "<h4 class='product-item-grid__title'>" +
                    "<a href='chi-tiet-san-pham.html'>" + item.name + "</a>" +
                    "</h4>" +
                    "<div class='product-item-grid__price'>" +
                    "<span class='product-item-grid__price-label'>Giá thu cũ tham khảo:</span>" +
                    "<p class='product-item-grid__price-new'>" + item.priceStr + "₫</p>" +
                    "</div>" +
                    "<div class='product-item-grid__footer'>" +
                    "<a href='" + item.linktar + "' class='product-item-grid__button'>Đổi mới</a>" +
                    "<span class='product-item-grid__button btn-resell btnBanLaiLoad'" +
                    "data-product='" + item.jsonProduct + "' onclick='btnBanLaiLoad()'>Bán lại</span>" +
                    "</div>" +
                    "</div>" +
                    "</div>" +
                    "</div>";
                $('#ListProduct').append(result);
            });
            $('#buttonLoadMore').css('display', 'inline');

        }
        else {
            $('#ListProduct').html('');
            $('#ListProduct').append("<h3 class='text-center'>Không tìm thấy sản phẩm nào</h3>");
            $('#buttonLoadMore').css('display', 'none');
        }
    })
    $(_this).data('zoneId',zoneId);
    $(_this).data('page', 1)
}

function btnBanLaiLoad() {
    
    let p = $(".btnBanLaiLoad").data('product');
    $('#modalBanLai #avatar').html(' <img asp-append-version="true" src="' + p.avatar + '" alt="' + p.name + '" />');
    $('#modalBanLai #name').html(p.name);
    $('#modalBanLai #price').html(R.FormatNumber(p.price));
    $('#modalBanLai').modal();
    $('#productId').val(p.productId);
}


function openModalDoiMoi(imgUrl, title, price, productId, difference, oldProductId) {
    priceStr = $("#firstPrice").text();
    $('#imgAvtModal').attr("src", imgUrl);
    $('#titleModal').html('').html(title);
    $('#priceModal').html('').html(priceStr + ' đ');
    $('#priceDifferenceModal').html('').html(difference + ' đ');
    $('#productId').val(productId);
    $('#oldProductId').val(oldProductId);
}

function btnDoiMoiLoad() {
    let p = $(".btnDoiMoiLoad").data('product');
    $('#imgAvtModal').attr("src", p.avatar);
    $('#titleModal').html('').html(p.title);
    $('#priceModal').html('').html(p.priceReferStr + ' đ');
    $('#priceDifferenceModal').html('').html(p.differenceStr + ' đ');
    $('#productId').val(p.productId);
    $('#oldProductId').val(p.oldProductId);

}

$('#location').on('change', function () {
    let id = this.value;
    var url = "/Store/ListDeptByLocId?id=" + id + "&needView=0";
    $.get(url, function (response) {
        $('#department option').remove();
        $('#department option').append($('<option>', {
            value: 0,
            text: 'Chọn'
        }));

        response.map(item => {
            $('#department').append($('<option>', {
                value: item.departmentId,
                text: item.name
            }));

        });
    })
});


function loadProductRenewCat(zoneId) {
    let _this = $('#buttonLoadMore');
    let pageIndex = $(_this).data('page');
    let tybeView = $(_this).data('viewBy');
    let firstPrice = $("#firstPrice").text();

    $.get("/Product/ListProductOldRenewalGetAllJson?pageIndex=1&pageSize=50&ZoneId=" + zoneId + "&typeView= " + tybeView + "&firstPrice=" + firstPrice + "", function (response) {
        if (response.length > 0) {
            $('#ListProduct').html('');
            response.map(item => {
                let result = "<div class='products-grid__item'>" +
                    "<div class='product-item-grid'>" +
                    "<div class='product-item-grid__image'>" +
                    "<a href='" + item.linktar + "'>" +
                    "<img asp-append-version='true' src='" + item.avatar + "'" +
                    "class='w-100 ' alt='" + item.title + "' />" +
                    "</a>" +
                    "</div>" +
                    "<div class='product-item-grid__content'>" +
                    "<h4 class='product-item-grid__title'>" +
                    item.title +
                    "</h4>" +
                    "<div class='products-getnew__price'>" +
                    "<div class='products-getnew__current-price'>Giá khuyến mãi: <span>" + item.defaultPriceStr + "</div>" +
                    "<div class='products-getnew__guess-price'>Giá thu cũ tham khảo: <span>" + item.priceReferStr + "đ</span></div>" +
                    "<div class='products-getnew__promotion-price'>Trợ giá " + item.discountPercent + "%: <span>" + item.discountStr + "₫</span></div>" +
                    "<span class='products-getnew__line'></span>" +
                    "<div class='products-getnew__total-price'>" +
                    "Bù chênh lệch trả thẳng:<span>" + item.differenceStr + "₫</span>" +
                    "</div>" +
                    "</div>" +
                    "<div class='products-getnew__button'>" +
                    "<a href='#' class='btn-getnew btnDoiMoiLoad' data-toggle='modal' data-product='" + item.jsonProduct + "' data-target='#modalDoiMoi' onclick='btnDoiMoiLoad()'>Đổi ngay</a>" +
                    "</div>" +
                    "</div>" +
                    "</div>"
                "</div>";
                $('#ListProduct').append(result);
            });
        }
        else {
            $('#ListProduct').html('');
            $('#ListProduct').append("<h3 class='text-center'>Không tìm thấy sản phẩm nào</h3>");
        }
    })
}

function typeMobileVersion(price, oldProductId) {
    $.get("/Product/ListProductOldRenewalGetAllJson?pageIndex=1&pageSize=50&ZoneId=0&typeView=0&firstPrice=" + price.value + "&oldProductId=" + oldProductId, function (response) {
        if (response.length > 0) {
            $('#ListProduct').html('');
            response.map(item => {
                let result = "<div class='products-grid__item'>" +
                    "<div class='product-item-grid'>" +
                    "<div class='product-item-grid__image'>" +
                    "<a href='" + item.linktar + "'>" +
                    "<img asp-append-version='true' src='" + item.avatar + "'" +
                    "class='w-100 ' alt='" + item.title + "' />" +
                    "</a>" +
                    "</div>" +
                    "<div class='product-item-grid__content'>" +
                    "<h4 class='product-item-grid__title'>" +
                    item.title +
                    "</h4>" +
                    "<div class='products-getnew__price'>" +
                    "<div class='products-getnew__current-price'>Giá khuyến mãi: <span>" + item.defaultPriceStr + "</div>" +
                    "<div class='products-getnew__guess-price'>Giá thu cũ tham khảo: <span>" + item.priceReferStr + "đ</span></div>" +
                    "<div class='products-getnew__promotion-price'>Trợ giá " + item.discountPercent + "%: <span>" + item.discountStr + "₫</span></div>" +
                    "<span class='products-getnew__line'></span>" +
                    "<div class='products-getnew__total-price'>" +
                    "Bù chênh lệch trả thẳng:<span>" + item.differenceStr + "₫</span>" +
                    "</div>" +
                    "</div>" +
                    "<div class='products-getnew__button'>" +
                    "<a href='#' class='btn-getnew btnDoiMoiLoad' data-toggle='modal' data-product='" + item.jsonProduct + "' data-target='#modalDoiMoi' onclick='btnDoiMoiLoad()'>Đổi ngay</a>" +
                    "</div>" +
                    "</div>" +
                    "</div>"
                "</div>";
                $('#ListProduct').append(result);
            });
        }
        else {
            $('#ListProduct').html('');
            $('#ListProduct').append("<h3 class='text-center'>Không tìm thấy sản phẩm nào</h3>");
        }
    })
}