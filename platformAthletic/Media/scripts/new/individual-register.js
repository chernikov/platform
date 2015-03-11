function IndividualRegister()
{
    var _this = this;

    this.init = function () {
        $('.mySelectBoxClass').customSelect();


        $('.checkbox-wrp').each(function () {
            var $this = $(this);
            var checkbox = $this.find('input');
            var checkboxImage = $this.find('.checkbox-image');

            if (checkbox.is(':checked')) {
                checkboxImage.css('background-position', '0 0');
            } else {
                checkboxImage.css('background-position', '0 -93px');
            }
        });

        $('.checkbox-wrp').click(function (e)
        {
            var $this = $(this);
            var checkbox = $this.find('input');
            var checkboxImage = $this.find('.checkbox-image');
            if (checkbox.attr('checked') == "checked") {
                checkbox.removeAttr('checked');
                checkboxImage.css('background-position', '0 -93px');
            } else {
                checkbox.attr('checked', "checked");
                checkboxImage.css('background-position', '0 0');
            }
            return false;
        });

        $("#StartFreeTrial").click(function () {
            $(this).attr("disabled", "disabled");
            $("#RegisterForm").submit();
        });
    }
}

var individualRegister = null;

$(function () {
    individualRegister = new IndividualRegister();
    individualRegister.init();
});