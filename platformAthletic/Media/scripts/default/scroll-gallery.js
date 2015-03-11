// Utility
if (typeof Object.create !== 'function') {
    Object.create = function (obj) {

        function F() { };
        F.prototype = obj;
        return new F();
    };
}

(function ($, window, document, undefined) {
    var Gallery = {
        init: function (options, elem) {
            var self = this,
                galleryItem = $(elem).find('.container > div'),
                leftArrow = $(elem).find('.icon-arrow-left'),
                rightArrow = $(elem).find('.icon-arrow-right'),
                container = $(elem).find('.container'),
                content = $(elem).find('.content'),
                count = galleryItem.length,
                itemWidth = galleryItem.outerWidth(true);

            self.elem = elem;
            self.$elem = $(elem);

            container.css({ "width": itemWidth * count });

            self.display(container, content, leftArrow, rightArrow);

            leftArrow.click(function () {
                self.moveRight(itemWidth, container);
            });

            rightArrow.click(function () {
                self.moveLeft(itemWidth, container);
            });

            self.options = $.extend({}, $.fn.queryGallery.options, options);
        },

        display: function (container, content, leftArrow, rightArrow) {
            if (container.width() < content.outerWidth(true)) {
                leftArrow.css('display', 'none');
                rightArrow.css('display', 'none');
                content.css('margin', 0);
            }
        },

        moveLeft: function (itemWidth, container) {
            var self = this,
                animationSpeed = ([self.options.speed]);

            container.stop(true, true).animate({
                left: -1 * itemWidth
            }, animationSpeed[0], function () {
                // Animation complete.
                var obj = self.$elem.find('.container > div', self.$elem.find('.container')).first();
                obj.detach();
                container.css({ "left": "0" });
                obj.appendTo(self.$elem.find('.container'));
            });
        },

        moveRight: function (itemWidth, container) {
            var self = this,
                obj = self.$elem.find('.container > div', container).last(),
                animationSpeed = ([self.options.speed]);

            obj.detach();
            container.css({ "left": -1 * itemWidth });
            obj.prependTo(self.$elem.find('.container'));
            container.stop().animate({
                left: 0
            }, animationSpeed[0]);
        }
    };

    $.fn.queryGallery = function (options) {
        return this.each(function () {

            var gallery = Object.create(Gallery);
            gallery.init(options, this);
        });
    };

    $.fn.queryGallery.options = {
        speed: 300
    };

})(jQuery, window, document);