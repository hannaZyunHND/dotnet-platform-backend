R.FlashSale = {
    Init: function () {
        R.FlashSale.culture = R.Culture();
        //R.Extra.BindingExtraToProduct();
        R.FlashSale.CountDownFlashSale('.timer-flash-sale');
        R.FlashSale.RegisterEvent();
    },
    RegisterEvent: function () {
        $(".tab-flash-sale .link-tab-flash-sale").click(function () {
            $(".tab-flash-sale .link-tab-flash-sale").removeClass("active");
            $(this).addClass("active");
            var f_id = $(this).data('id');
            var f_start_time = $(this).data('start-time');
            var f_end_time = $(this).data('end-time');
            var f_status = $(this).data('status');

            $('.timer-flash-sale22').last().clone().appendTo('.flash_sale_timer');



            R.FlashSale.SwitchFlashSale(f_id, f_start_time, f_end_time, f_status);
        });
        $('.flash-sale-view-more').off('click').on('click', function () {
            var affected_id = 0;
            $(".link-tab-flash-sale").each(function (element) {
                if ($(this).hasClass('active'))
                    affected_id = $(this).data('id');
            })
            var pageIndex = $(this).data('page-index');
            var pageSize = $(this).data('page-size');
            R.FlashSale.ViewMore(affected_id, pageIndex, pageSize);
        });
        $('.picking-flash-sale').off('click').on('click', function() {
            var fl_id = $(this).data('flashsaleid');
            var title = $(this).attr('title');
            $('.picking-flash-sale').each((i, v) => {
                $(v).removeClass('category-tags__active');
            })
            $(this).addClass('category-tags__active');
            $('.section-head__title').text(title);
            R.FlashSale.SwitchFlashSale(fl_id);
        })
    },
    SwitchFlashSale: function (id, start_time, end_time, status) {
        var params = {
            fSaleId: id
        }
        var url = "/FlashSale/SwitchFlashSaleActive";
        $.post(url, params, function (response) {
            $('._binding_flashsale').html('').html(response);
            $('.products-grid__item').each((i, v) => {
                $(v).css('display', 'block');
            })
            //R.Extra.BindingExtraToProduct();
            $('.flash-sale-view-more').data('page-index', 1);
            R.FlashSale.RegisterEvent();
        })
    },
    CountDownFlashSale: function (el) {
        var end_time = $(el).data('end-time');
        R.FlashSale.CountDown(end_time, el);

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

            $(el).find('#day').text(days + ' day');
            $(el).find('#hour').text(hours + ' h');
            $(el).find('#minute').text(minutes + ' m');
            $(el).find('#second').text(seconds + ' s');

            if (distance < 0) {

                //clearInterval(x);
                document.getElementsByClassName("flash-sales").style.display = "none";

            }

        }
        // Update the count down every 1 second
        var x = setInterval(timesale, 1000);


        //var x = setInterval(timesale, 1000);
    },
    ViewMore: function (id, page_index, page_size) {
        //int fSaleId, int pageIndex, int pageSize
        var params = {
            fSaleId: id,
            pageIndex: page_index + 1,
            pageSize: page_size
        }
        var url = "/FlashSale/SwitchFlashSaleActive";
        $.post(url, params, function (response) {
            console.log(response);
            $('._binding_flashsale').append(response);
            $('.flash-sale-view-more').data('page-index', page_index + 1);
            $('.flash-sale-view-more').data('page-size', page_size);
            R.Extra.BindingExtraToProduct();
            R.FlashSale.RegisterEvent();
        })
    }
}

$(function () {
    R.FlashSale.Init();
})