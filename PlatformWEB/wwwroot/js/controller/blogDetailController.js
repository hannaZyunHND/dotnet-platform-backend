R.BlogDetail = {
    Init: function() {
        R.BlogDetail.location_id = R.CurrentLocationId();
        R.BlogDetail.culture = R.Culture();
        R.BlogDetail.LoadListProduct();
        R.BlogDetail.AddViewCountArticle();
        R.BlogDetail.AddTableBorder();
        R.BlogDetail.RegisterEvent();
        
        
    },
    RegisterEvent: function () {
        $('.toc_title').off('click').on('click', function () {
            $('.toc_list').toggle();
        })
        $('.open-modal-coverage').off('click').on('click', function () {
            //data-coverage
            var coverages = $(this).data('coverage');
            //alert(coverages)
            $.get(`/Zone/ModalListOfCountries?zoneIds=${coverages}`).then((response) => {
                $('#modal-coverage ul').html('').html(response);
                $('#modal-coverage').modal('show')
                R.BlogDetail.RegisterEvent();
            })
        })

    },
    LoadListProduct: function() {

        $('product').each(function(element) {
            var el = $(this);
            var product_list = $(this).data('id-list');
            var params = {
                product_ids: product_list,
                location_id: R.BlogDetail.location_id
            }
            var url = R.BlogDetail.culture + "/Blog/ProductsInArticle";
            $.post(url, params, function(response) {
                console.log(response);
                el.replaceWith(response);

                $('.sim-slide').slick({
                    dots: true,
                    infinite: false,
                    speed: 300,
                    slidesToShow: 2,
                    slidesToScroll: 1,
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
                })

                
                R.BlogDetail.RegisterEvent();
            })
        })
    },
    AddViewCountArticle: function() {

        var id_san_pham = $('.detail-container').data('id');
        var url = "/Extra/CreateViewCount";
        var params = {
            objectId: id_san_pham,
            type: 'article'
        }
        $.post(url, params, function(response) {
            console.log(response);
        })

    },
    AddTableBorder: function () {
        document.querySelectorAll(".tab-content table").forEach(item => {
            item.classList.add("table");
            item.classList.add("table-bordered");
        })
    },
    
}



$(function() {
    R.BlogDetail.Init()
})
