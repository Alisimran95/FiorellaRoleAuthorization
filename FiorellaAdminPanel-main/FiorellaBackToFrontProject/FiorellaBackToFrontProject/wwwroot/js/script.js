$(document).ready(function () {

    $(document).on("click",
        "#add-to-basket",
        function(e) {
            $.ajax({
                type: "POST",
                url: "/Home/AddToBasket?id=" + $(this).attr("data-id"),
                success: function(res) {
                    $("#cart-list").append(res);
                    //reloading headerCartTotols
                    //$("#basket-count").text($("#cart-list tr").length - 1);

                    //Setting response count and amount to basketCount and basketAmount
                    var totalAmount = 0;

                    for (var i = 0; i < res.length; i++) {
                        totalAmount += res[i].price * res[i].count;
                    }
                    $("#basket-total-amount").text("CART $(" + totalAmount + ")");
                    $("#basket-count").text(res.length);

                }
            });
        });

    $(document).on("click",
        "#cart-delete-btn",
        function() {
            $.ajax({
                type: "POST",
                url: "/Home/Delete?id=" + $(this).attr("data-id"),
            success: function(res) {
                $("#cart-list").empty();
                $("#cart-list").append(res);
                $("#basket-count").text($("#cart-list tr").length);
            }
        });
});


    $(document).on("click",
        "#increase-btn",
        function () {
            $.ajax({
                type: "POST",
                url: "/Home/CountIncrease?id=" + $(this).attr("data-id"),
                success: function (res) {
                    $("#cart-list").empty();
                    $("#cart-list").append(res);
                    $("#basket-count").text($("#cart-list tr").length);

                }
            });
        });


    $(document).on("click",
        "#decrease-btn",
        function () {
            $.ajax({
                type: "POST",
                url: "/Home/CountDecrease?id=" + $(this).attr("data-id"),
                success: function (res) {
                    $("#cart-list").empty();
                    $("#cart-list").append(res);
                    $("#basket-count").text($("#cart-list tr").length);

                }
            });
        });





    var stop = false;
    var skip = 4;
    $(window).scroll(function () {
        if ($("#productRow").height() - $(".product-item").height() < $(document).scrollTop()){
            window.$.ajax({
                type: "GET",
                url: "/Products/Load?skip=" + skip,
                success: function (res) {
                    window.$("#productRow").append(res);
                    skip += 4;

                    var productsCount = window.$("#productsCount").val();
                    if (skip >= productsCount) {
                        window.$("#productRow").remove(res);
                    }
                }
            });
        }
    });

    //var skip = 4;

    //$(document).on('click',
    //    "#loadMore",
    //    function() {
    //        $.ajax({
    //            type: "GET",
    //            url: "/Products/Load?skip="+skip,
    //            success: function(res) {
    //                $("#productRow").append(res);
    //                    skip += 4;
    //                var productsCount = window.$("#productsCount").val();

    //                if (skip >= productsCount) {
    //                    window.$("#loadMore").remove();
    //                }
    //            }
    //        });
    //    });

    // HEADER

    $(document).on("click",
        '#search',
        function() {
            $(this).next().toggle();
        });

    $(document).on("click",
        "#mobile-navbar-close",
        function() {
            $(this).parent().removeClass("active");

        });
    $(document).on("click",
        '#mobile-navbar-show',
        function() {
            $('.mobile-navbar').addClass("active");

        });

    $(document).on('click',
        '.mobile-navbar ul li a',
        function() {
            if ($(this).children('i').hasClass('fa-caret-right')) {
                $(this).children('i').removeClass('fa-caret-right').addClass('fa-sort-down')
            } else {
                $(this).children('i').removeClass('fa-sort-down').addClass('fa-caret-right')
            }
            $(this).parent().next().slideToggle();
        });

    // SLIDER

    $(document).ready(function(){
        $(".slider").owlCarousel(
            {
                items: 1,
                loop: true,
                autoplay: true
            }
        );
      });

    // PRODUCT

    $(document).on('click',
        '.categories',
        function(e) {
            e.preventDefault();
            $(this).next().next().slideToggle();
        });

    $(document).on('click',
        '.category li a',
        function(e) {
            e.preventDefault();
            let category = $(this).attr('data-id');
            let products = $('.product-item');

            products.each(function() {
                if (category == $(this).attr('data-id')) {
                    $(this).parent().fadeIn();
                } else {
                    $(this).parent().hide();
                }
            })
            if (category == 'all') {
                products.parent().fadeIn();
            }
        });

    // ACCORDION 

    $(document).on('click',
        '.question',
        function() {
            $(this).siblings('.question').children('i').removeClass('fa-minus').addClass('fa-plus');
            $(this).siblings('.answer').not($(this).next()).slideUp();
            $(this).children('i').toggleClass('fa-plus').toggleClass('fa-minus');
            $(this).next().slideToggle();
            $(this).siblings('.active').removeClass('active');
            $(this).toggleClass('active');
        });

    // TAB

    $(document).on('click',
        'ul li',
        function() {
            $(this).siblings('.active').removeClass('active');
            $(this).addClass('active');
            let dataId = $(this).attr('data-id');
            $(this).parent().next().children('p.active').removeClass('active');

            $(this).parent().next().children('p').each(function() {
                if (dataId == $(this).attr('data-id')) {
                    $(this).addClass('active')
                }
            });
        });

    $(document).on('click',
        '.tab4 ul li',
        function() {
            $(this).siblings('.active').removeClass('active');
            $(this).addClass('active');
            let dataId = $(this).attr('data-id');
            $(this).parent().parent().next().children().children('p.active').removeClass('active');

            $(this).parent().parent().next().children().children('p').each(function() {
                if (dataId == $(this).attr('data-id')) {
                    $(this).addClass('active')
                }
            });
        });

    // INSTAGRAM

    $(document).ready(function(){
        $(".instagram").owlCarousel(
            {
                items: 4,
                loop: true,
                autoplay: true,
                responsive:{
                    0:{
                        items:1
                    },
                    576:{
                        items:2
                    },
                    768:{
                        items:3
                    },
                    992:{
                        items:4
                    }
                }
            }
        );
      });

      $(document).ready(function(){
        $(".say").owlCarousel(
            {
                items: 1,
                loop: true,
                autoplay: true
            }
        );
      });
})