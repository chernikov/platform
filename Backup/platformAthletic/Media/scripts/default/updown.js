function UpDown()
{
    var _this = this;

    var step = 5;

    this.init = function ()
    {
        $(".up").click(function () {
            _this.changeValue($(this).closest(".updown").parent().find("input"), step);
        });

        $(".down").click(function () {
            _this.changeValue($(this).closest(".updown").parent().find("input"), -step);
        });

        $(".updownkeypress").keydown(function (e)
        {
            if (e.keyCode == 38)
            {
                _this.changeValue($(this), step);
                return false;
            }
            if (e.keyCode == 40) {
                _this.changeValue($(this), -step);
                return false;
            }
        });
    }

    function isNumeric(n) {
        return !isNaN(parseFloat(n)) && isFinite(n);
    }

    this.changeValue = function (input, diff)
    {
        var val = input.val();

        if (isNumeric(val))
        {
            val = parseInt(val);
            val = val + diff;
        }
        input.val(val);
        if (typeof (team) != "undefined")
        {
            team.setValue(input);
        }
        if (typeof (player) != "undefined")
        {
            var val = input.val();
            var name = input.attr("name");
            player.setField(val, name);
        }
    }
}



var updown = null;
$().ready(function () {
    updown = new UpDown();
    updown.init();
});