function Index() {
    var _this = this;

    this.init = function ()

    {
        $(".testimonials .element").first().removeClass("hidden");
        //fix 
        $(".arrow.prev").click(function (e)
        {
            var currentItem = null;
            $(".testimonials .element").each(function (i, item) {

                if (!$(this).hasClass("hidden")) {
                    currentItem = $(this);
                }
            });
            if (currentItem.prev().length == 1)
            {
                var prev = currentItem.prev();
            } else {
                var prev = $(".testimonials .element").last();
            }
            $(".testimonials .element").addClass("hidden");
            prev.removeClass("hidden");
        });

        if ($(".testimonials .element").length < 2) {
            $(".testimonials .arrow-wrapper").hide();
        }

        $(".arrow.next").click(function (e) {
            var currentItem = null;
            $(".testimonials .element").each(function (i, item) {

                if (!$(this).hasClass("hidden")) {
                    currentItem = $(this);
                }
            });
            if (currentItem.next().length == 1) {
                var next = currentItem.next();
            } else {
                var next = $(".testimonials .element").first();
            }
            $(".testimonials .element").addClass("hidden");
            next.removeClass("hidden");
        });

        $('.bxslider').bxSlider({
            auto: true,
            pause: 3000,
            autoStart : true
        });
    }
}

var index = null;
$(function () {
    index = new Index();
    index.init();
});