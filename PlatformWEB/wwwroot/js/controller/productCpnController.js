
$(document).ready(function () {
    //$("#itemParent--1").addClass("setSelected");
});
function changeChildZone(zoneId) {
    $('#parentZoneId').val(zoneId)
    $('#pageNumber').val(1)
    $(".zone-productCpn").removeClass("setSelected");
    $(".zone-parent").removeClass("setSelected");
    $("#itemParent-" + zoneId + "").addClass("setSelected");
    $('#childZoneId').val(0);
    $.get("/ProductComponent/ListChangeChildZone?zoneId=" + zoneId + "", function (response) {
        if (response.length > 0) {
            $('#list_child_zone').html('');
            response.map(item => {
                var result = "<div id='itemChild-"+item.id+"' class='zone_item-product zone-child' onclick='setChildZoneValue(" + item.id + ")'><span>" + item.name + "</span></div>"
                $('#list_child_zone').append(result);
            })
        }
    })



    if (zoneId == -1) {
        $.get("/ProductComponent/ProductComponentListJson?zoneId=0&zoneIdChild=0", function (response) {
            $('#listProductComponent').html('');
            if (response.length > 0) {
                response.map(item => {
                    var result = "<div class='col-md-3 col-sm-3 col-6 mb-3'>" +
                        "<div class='item-product product-item-grid' data-id='517' data-properties='1' data-viewluotxem='0'>" +
                        "<div class='product-item-grid__image'>" +
                        "<a href='" + item.linktar + "'><img src='" + item.linkImageAvatar + "' class='w-100 ' alt='" + item.title + "'></a>" +
                        "</div>" +
                        "<div class='product-item-grid__content'>" +
                        "<h4 class='product-item-grid__title'>" +
                        "<a href='" + item.linktar + "' title='" + item.title + "'>" + item.title + "</a>" +
                        "</h4><div class='product-item-grid__price'>" +
                        "<div class='product-item-grid__price-new'>" + item.priceStr + "</div></div>" +
                        "</div>" +
                        "</div>" +
                        "</div>";
                    $('#listProductComponent').append(result);
                    
                })
                $("#loadmorebutton").removeClass("unShowLoadMore");$('#buttonLoadMore').css('display', 'inline');
                $('#pageNumber').val(1);
            } else {
                $('#listProductComponent').append("<h4 class='text-center w-100'>Không có linh kiện nào</h4>");
                $("#loadmorebutton").addClass("unShowLoadMore");
                $('#pageNumber').val(1);

            }
        })
    }
}
function loadmoredata() {
    var parentId = $('#parentZoneId').val();
    var childId = $('#childZoneId').val();
    var productCpnId = $('#productCpnId').val();
    var pageNumber = parseInt($('#pageNumber').val()) + 1;
    $('#pageNumber').val(pageNumber);

    $.get("/ProductComponent/ProductComponentListJson?zoneId=" + parentId + "&zoneIdChild=" + childId + "&productCpnId=" + productCpnId + "&pageIndex=" + pageNumber +"", function (response) {
        if (response.length > 0) {
            response.map(item => {
                var result = "<div class='col-md-3 col-sm-3 col-6 mb-3'>" +
                    "<div class='item-product product-item-grid' data-id='517' data-properties='1' data-viewluotxem='0'>" +
                    "<div class='product-item-grid__image'>" +
                    "<a href='" + item.linktar + "'><img src='" + item.linkImageAvatar + "' class='w-100 ' alt='" + item.title + "'></a>" +
                    "</div>" +
                    "<div class='product-item-grid__content'>" +
                    "<h4 class='product-item-grid__title'>" +
                    "<a href='" + item.linktar + "' title='" + item.title + "'>" + item.title + "</a>" +
                    "</h4><div class='product-item-grid__price'>" +
                    "<div class='product-item-grid__price-new'>" + item.priceStr + "</div></div>" +
                    "</div>" +
                    "</div>" +
                    "</div>";
                $('#listProductComponent').append(result);

            });
            $("#loadmorebutton").removeClass("unShowLoadMore");$('#buttonLoadMore').css('display', 'inline');
            $('#pageNumber').val(pageNumber);
        } else {
            $("#loadmorebutton").addClass("unShowLoadMore");
            $('#pageNumber').val(1);
        }
    })
}
function setChildZoneValue(childZone) {
    $('#pageNumber').val(1);
    $(".zone-child").removeClass("setSelected");
    $("#itemChild-" + childZone + "").addClass("setSelected");

    $('#childZoneId').val(childZone);
    $(".zone-productCpn").removeClass("setSelected");
    $('#productCpnId').val(0);


    var parentId = $('#parentZoneId').val();
    var childId = $('#childZoneId').val();

    $.get("/ProductComponent/ListProductCpn?zoneId=" + childZone + "", function (response) {
        $('#list-proCpn').html('');
        if (response.length > 0) {
            response.map(item => {
                var result = "<div id='itemProductCpn-" + item.id + "' class='zone_item-product zone-productCpn' onclick='getCpnById(" + item.id + ")'><span>" + item.name + "</span></div>"
                $('#list-proCpn').append(result);

            })
            $("#productCpnListData").css('display', 'inline');

        } else {
            $("#productCpnListData").css('display', 'none');

        }
    })

    $.get("/ProductComponent/ProductComponentListJson?zoneId=" + parentId + "&zoneIdChild=" + childId  +"", function (response) {
        $('#listProductComponent').html('');
        if (response.length > 0) {
            response.map(item => {
                var result = "<div class='col-md-3 col-sm-3 col-6 mb-3'>" +
                    "<div class='item-product product-item-grid' data-id='517' data-properties='1' data-viewluotxem='0'>" +
                    "<div class='product-item-grid__image'>" +
                    "<a href='" + item.linktar + "'><img src='" + item.linkImageAvatar + "' class='w-100 ' alt='" + item.title + "'></a>" +
                    "</div>" +
                    "<div class='product-item-grid__content'>" +
                    "<h4 class='product-item-grid__title'>" +
                    "<a href='" + item.linktar + "' title='" + item.title + "'>" + item.title + "</a>" +
                    "</h4><div class='product-item-grid__price'>" +
                    "<div class='product-item-grid__price-new'>" + item.priceStr + "</div></div>" +
                    "</div>" +
                    "</div>" +
                    "</div>";
                $('#listProductComponent').append(result);
                
            })
            $("#loadmorebutton").removeClass("unShowLoadMore");$('#buttonLoadMore').css('display', 'inline');
            $('#pageNumber').val(1);
        } else {
            $('#listProductComponent').append("<h4 class='text-center w-100'>Không có linh kiện nào</h4>");
            $("#loadmorebutton").addClass("unShowLoadMore");
            $('#pageNumber').val(1);
        }
    })
}

function getCpnById(cpnId) {
    $(".zone-productCpn").removeClass("setSelected");
    $("#itemProductCpn-" + cpnId + "").addClass("setSelected");
    $('#productCpnId').val(cpnId);
    var parentId = $('#parentZoneId').val();
    var childId = $('#childZoneId').val();

    $.get("/ProductComponent/ProductComponentListJson?zoneId=" + parentId + "&zoneIdChild=" + childId + "&productCpnId=" + cpnId +"", function (response) {
        $('#listProductComponent').html('');
        if (response.length > 0) {
            response.map(item => {
                var result = "<div class='col-md-3 col-sm-3 col-6 mb-3'>" +
                    "<div class='item-product product-item-grid' data-id='517' data-properties='1' data-viewluotxem='0'>" +
                    "<div class='product-item-grid__image'>" +
                    "<a href='" + item.linktar + "'><img src='" + item.linkImageAvatar + "' class='w-100 ' alt='" + item.title + "'></a>" +
                    "</div>" +
                    "<div class='product-item-grid__content'>" +
                    "<h4 class='product-item-grid__title'>" +
                    "<a href='" + item.linktar + "' title='" + item.title + "'>" + item.title + "</a>" +
                    "</h4><div class='product-item-grid__price'>" +
                    "<div class='product-item-grid__price-new'>" + item.priceStr + "</div></div>" +
                    "</div>" +
                    "</div>" +
                    "</div>";
                $('#listProductComponent').append(result);
                
            })
            $("#loadmorebutton").removeClass("unShowLoadMore");$('#buttonLoadMore').css('display', 'inline');
            $('#pageNumber').val(1);
        } else {
            $('#listProductComponent').append("<h4 class='text-center w-100'>Không có linh kiện nào</h4>");
            $("#loadmorebutton").addClass("unShowLoadMore");
            $('#pageNumber').val(1);
        }
    })
}
function redirectUrl(url) {
    var protocol = window.location.protocol;
    var host = window.location.host;
    var path = "/sua-chua/" + url;
    var fullUrl = protocol + "//" + host + path;
    window.location.href = fullUrl;
    //alert(fullUrl);
    
}

$('.search-sua-chua').on('keyup', (e) => {
    if (e.key === 'Enter' || e.keyCode === 13) {
        var value_sua_chua = $('.search-sua-chua').val();
        var protocol = window.location.protocol;
        var host = window.location.host;
        var path = window.location.pathname;
        var fullUrl = protocol + "//" + host + path + "?s=" + value_sua_chua;
        window.location.href = fullUrl;
    }
    
})
//$('#buttonLoadMore').off('click').on('click', function () {
//    let _this = this;
//    let pageIndex = $(_this).data('page');
//    let ZoneId = $(_this).data('zoneId');
//    let tybeView = $(_this).data('viewBy');

//    let total = $(_this).children().html() - 10;
//    $.get("/Product/ListProductOldRenewal?pageIndex=" + (pageIndex + 1) + "&pageSize=10&ZoneId =" + ZoneId + "&typeView = " + tybeView + "", function (response) {
//        $('#ListProduct').append(response);
//        $(_this).data('page', pageIndex + 1)
//        if (total > 0)
//            $(_this).children().html(total)
//    })
//});