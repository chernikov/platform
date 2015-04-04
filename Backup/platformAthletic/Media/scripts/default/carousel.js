function Carousel() {
    var _this = this;
    var img = $('.carousel-wrapper .container .item');
    var itemCount = img.length;
    var itemWidth = img.outerWidth(true);

    this.init = function () {

        /* --- create switcher --- */
        $('.carousel-wrapper .switcher ul').append('<li><span class="arrow-left"></span></li>');

        for (var i = 1; i <= itemCount; i++) {

            $('.carousel-wrapper .switcher ul').append('<li ' + (i == 1 ? 'class="selected button"' : 'class="button"') + '>' + i + '</li>');
        }

        $('.carousel-wrapper .switcher ul').append('<li><span class="arrow-right"></span></li>');

        /* --- set container width --- */
        var totalWidth = 0;
        $('.carousel-wrapper .container .item').each(function () {
            totalWidth += $(this).outerWidth(true);
        });
        $('.carousel-wrapper .container').css('width', totalWidth);

        /* --- click actions --- */
        $('.carousel-wrapper li:first-child').on('click', function () {
            _this.slideRight(1);
            _this.select('left');
        });

        $('.carousel-wrapper li:last-child').on('click', function () {
            _this.slideLeft(1);
            _this.select('right');
        });

        $('.carousel-wrapper li.button').on('click', function () {
            var $this = $(this);
            var index = $this.index();
            var currIndex = $('.carousel-wrapper ul').find('li.selected').index();
            var multiplier;
            if (currIndex < index) {
                multiplier = index - currIndex;
                _this.slideLeft(multiplier);
                _this.select('itemClick', $this);
            } else if (currIndex > index) {
                multiplier = currIndex - index;
                _this.slideRight(multiplier);
                _this.select('itemClick', $this);
            }
        });

        _this.autoSlide();
    };

    this.container = function () {
        return $('.carousel-wrapper .container');
    };

    this.slideRight = function (multiplier) {
        for (var i = 1; i <= multiplier; i++) {
            var obj = $('.carousel-wrapper .container').find('.item', _this.container()).last();
            obj.detach();
            _this.container().css({ "left": -1 * itemWidth });
            obj.prependTo(_this.container());
            _this.container().stop().animate({
                left: 0
            }, 400);
        }
    };

    this.slideLeft = function (multiplier) {
        for (var i = 1; i <= multiplier; i++) {
            _this.container().stop(true, true).animate({
                left: -1 * itemWidth
            }, 400, function () {
                // Animation complete.
                var obj = $('.carousel-wrapper .container').find('.item', _this.container()).first();
                obj.detach();
                _this.container().css({ "left": "0" });
                obj.appendTo(_this.container());
            });
        }
    };

    this.select = function (clickType, elem) {
        var selected = $('.carousel-wrapper .switcher ul').find('li.selected');
        var selIndex = selected.index();
        $('.carousel-wrapper .switcher li').removeClass('selected');
        if (clickType == 'left') {
            if (selIndex == 1) {
                $('.carousel-wrapper .switcher li:last-child').prev().addClass('selected');
            } else {
                selected.prev().addClass('selected');
            }
        } else if (clickType == 'right') {
            if (selIndex == itemCount) {
                $('.carousel-wrapper .switcher li:first-child').next().addClass('selected');
            } else {
                selected.next().addClass('selected');
            }
        } else if (clickType == 'itemClick') {
            elem.addClass('selected');
        }
    };

    this.autoSlide = function () {
        setInterval(function () {
            _this.slideLeft(1);
            _this.select('right');
        }, 15000);
    };
}

var carousel = null;
$().ready(function () {
    carousel = new Carousel();
    carousel.init();
});