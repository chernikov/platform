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
            var selectVal = $(elem).val(),
                selectWidth = $(elem).width(),
                selectHeight = $(elem).height(),
                paddingVal = 0;

            self.elem = elem;
            self.$elem = $(elem);

            var selectTextVal = $(elem).find('option[value=' + selectVal + ']').text();

            self.options = $.extend({}, $.fn.querySelect.options, options);
            
            var paddingString = self.options.padding.split(' ');
            for (var i = 0; i < 4; i++) {
                if (paddingString[i] != '0') {
                    var paddingSlice = paddingString[i].slice(0, -2);
                    var paddingInt = parseInt(paddingSlice);
                    paddingVal = paddingVal + paddingInt;
                }
            }

            $(elem).css({
                'width': selectWidth + 'px',
                'height': selectHeight + 'px',
                'position': 'relative',
                'z-index': 10,
                'opacity': 0
            });
            
            var wrapper = $("<span>");
            wrapper.attr("class", $(elem).attr("class"));
            wrapper.css({
                'display': 'inline-block',
                'height': selectHeight + 'px'
            });
            $(elem).wrapAll(wrapper);

            $(elem).before('<span></span>');
            $(elem).parent().find('span').css({
                'position': 'absolute',
                'padding': self.options.padding,
                'height': selectHeight + 'px',
                'line-height': selectHeight + 'px',
                'width': (selectWidth - paddingVal) + 'px',
                'top': 0
            });

            $(elem).parent().css({
                'position': "relative"
            });

            $(elem).parent().find('span').html(selectTextVal);

            $(elem).change(function () {
                var val = $(elem).val();
                var textVal = $(elem).find('option[value=' + val + ']').text();
                $(elem).parent().find('span').html(textVal);
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