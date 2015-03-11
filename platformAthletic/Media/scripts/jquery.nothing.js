
(function ($) {

    $.fn.Nothing = function (settings) {
        // JScrollPane "class" - public methods are available through $('selector').data('jsp')
        function NothingToDo(elem, s)
        {
            alert(elem);
        }
        return this.each(
            function () {
                var elem = $(this),
                    jspApi = elem.data('nothing');

                $("script", elem).filter('[type="text/javascript"],:not([type])').remove();
                jspApi = new NothingToDo(elem, settings);
                elem.data('nothing', jspApi);
            }
        );
    };

})(jQuery, this);