if (typeof Object.create !== 'function') {
    Object.create = function (obj) {

        function F() { };
        F.prototype = obj;
        return new F();
    };
}

(function ($, window, document, undefined) {
    var CustomSelect = {
        init: function (options, elem) {
            var selectVal = $(elem).find('select').val(),
                selectWidth = $(elem).width(),
                selectHeight = $(elem).height(),
                paddingVal = 0;

            self.elem = elem;
            self.$elem = $(elem);

            self.options = $.extend({}, $.fn.querySelect.options, options);
            
            var paddingString = self.options.padding.split(' ');
            for (var i = 0; i < 4; i++) {
                if (paddingString[i] != '0') {
                    var paddingSlice = paddingString[i].slice(0, -2);
                    var paddingInt = parseInt(paddingSlice);
                    paddingVal = paddingVal + paddingInt;
                }
            }

            $(elem).css({ 'position': 'relative' });

            $(elem).find('select').css({
                'width': selectWidth + 'px',
                'height': selectHeight + 'px',
                'position': 'relative',
                'z-index': 10
            });

            $(elem).append('<span></span>');
            $(elem).find('span').css({
                'position': 'absolute',
                'padding': self.options.padding,
                'height': selectHeight + 'px',
                'line-height': selectHeight + 'px',
                'width': (selectWidth - paddingVal) + 'px',
                'top': 0
            });

            $(elem).find('span').html(selectVal);

            $(elem).find('select').change(function () {
                var val = $(elem).find('select').val();
                $(elem).find('span').html(val);
            });
        },
    };

    $.fn.querySelect = function (options) {
        return this.each(function () {

            var customSelect = Object.create(CustomSelect);
            customSelect.init(options, this);
        });
    };

    $.fn.querySelect.options = {
        padding: '0 0 0 0'
    };


})(jQuery, window, document);