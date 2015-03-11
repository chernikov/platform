function Checkbox() {

    this.init = function () {

        $('.checkbox-wrp').each(function () {
            var $this = $(this);
            var checkbox = $this.find('input');
            var checkboxImage = $this.find('.checkbox-image');

            if (checkbox.is(':checked')) {
                checkboxImage.css('background-position', '0 -26px');
                console.log(true);
            } else {
                checkboxImage.css('background-position', '0 0');
                console.log(false);
            }
        });
        

        $('.checkbox-wrp').click(function (e) {
            var $this = $(this);
            var checkbox = $this.find('input');
            var checkboxImage = $this.find('.checkbox-image');

            if (checkbox.is(':checked')) {
                checkbox.attr('checked', false);
                checkboxImage.css('background-position', '0 0');
            } else {
                checkbox.attr('checked', true);
                checkboxImage.css('background-position', '0 -26px');
            }
            e.stopPropagation();
        });
    };
}

var checkbox = null;
$().ready(function () {
    checkbox = new Checkbox();
    checkbox.init();
});