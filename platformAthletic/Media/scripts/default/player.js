function Player() {
    _this = this;

    this.ajaxSetField = "/set-field";
    this.ajaxSetPillar = "/set-pillar";
    this.ajaxSetFieldPosition = "/set-field-position";
    this.ajaxGetTextAbove = "/get-text-above";
    this.ajaxUploadAvatar = "/upload-avatar";

    this.init = function ()
    {
        var boardHeight = $('.player-page-wrp .center').height();

        $('.board-wrp .board-content').css('height', boardHeight - 37);

        $('.double-input').blur(function () {
            $(this).removeClass('input-validation-error');
            var value = $(this).val();
            if (parseFloat(value)) {
                var name = $(this).attr("name");
                _this.setField(value, name);
            } else {
                $(this).addClass('input-validation-error');
            }
        });

        $('.int-input').blur(function () {
            $(this).removeClass('input-validation-error');
            var value = $(this).val();
            if (parseInt(value)) {
                var name = $(this).attr("name");
                _this.setField(value, name);
            } else {
                $(this).addClass('input-validation-error');
            }
        });

        $('.no-validate-input').blur(function () {
            var value = $(this).val();
            var name = $(this).attr("name");
            _this.setField(value, name);
        });

        $("#FieldPositionID").change(function ()
        {
            _this.setFieldPosition($(this).val());
        });

        _this.initPillar();
    }

    this.setField = function (value, field) {
        var ajaxData = {
            id: $("#ID").val(),
            value: value,
            field: field
        };
        $.ajax({
            type: "POST",
            url: _this.ajaxSetField,
            data: ajaxData,
            success: function () {
            }
        });
    }

    this.setFieldPosition = function (value)
    {
        var ajaxData = {
            id: $("#ID").val(),
            fieldPosition: value,
        };
        $.ajax({
            type: "POST",
            url: _this.ajaxSetFieldPosition,
            data: ajaxData,
            success: function () {
            }
        });
    }

    this.setPillar = function ()
    {
        var ajaxData = $("#PillarForm").serialize();

        $.ajax({
            type: "POST",
            url: _this.ajaxSetPillar,
            data: ajaxData,
            success: function (data) {
                $("#PillarDataWrapper").html(data);
                common.StyleDropDown($('#PillarDataWrapper .dropdown-styled'));
                _this.initPillar();
            }
        });
    }

    this.initPillar = function ()
    {
        if (colors != null) {
            colors.update();
        }
        $("#AddedDate").datepicker({
            showOtherMonths: true,
            selectOtherMonths: true
        });

        $("#PillarTypeID").change(function () {
            _this.getTextAbove($(this).val());
        });

        $("#SubmitPillarBtn").click(function () {
            _this.setPillar();
            return false;
        });
    }

    this.getTextAbove = function (id) {
        $.ajax({
            type: "GET",
            url: _this.ajaxGetTextAbove,
            data: { id: id },
            success: function (data) 
            {
                $("#TextAboveWrapper").text(data.TextAbove);
                $("#TextValue").val("");
                $("#TextValue").attr("placeholder", data.Placeholder);

            }
        });
    }
}


var player = null;
$().ready(function () {
    player = new Player();
    player.init();
});