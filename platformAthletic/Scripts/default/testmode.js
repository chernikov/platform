function Testmode() {
    var _this = this;

    this.init = function () {
        $(document).on("click", ".forbitBtn", function (e) {
            var message = $(this).data("message");
            _this.showInfo(message);
            e.stopPropagation();
            return false;
        });


        $("#ShowTestModeInfo").click(function () {
            //_this.showInfo("Test mode means the site is populated with sample players and data. Nothing you do or change while in test mode will be saved. When you are ready to begin using the site for your school, exit test mode and follow the To-Do list items on the left hand side.");
            _this.showInfo("Test mode means that the site is populated with fake players and data so that you can explore to your heart's delight. Once you exit test mode, changes will be erased and you will be given a blank slate to start fresh with your own players.");
        });

        $("#StopTestMode").click(function () {
            _this.showStopTestMode();
        });
    }


    this.showInfo = function (message) {
        $.ajax({
            type: "GET",
            data: { message: message },
            url: "/Tutorial/Info",
            success: function (data) {
                $("#ModalWrapper").html(data);
                $("#modalTutorial").modal();
            }
        })
    }

    this.showInfoExtended = function (message, button, callback) {
         $.ajax({
             type: "GET",
             data: {
                 message: message,
                 button: button
             },
             url: "/Tutorial/Info",
             success: function (data) {
                 $("#ModalWrapper").html(data);
                 $("#modalTutorial").modal();
                 if (callback) {
                     $('#modalTutorial').on('hidden.bs.modal', function () {
                         callback();
                     })
                 }
             }
         })
    }


    this.showStopTestMode = function ()
    {
        $.ajax({
            type: "GET",
            url: "/Tutorial/ShowStopTestMode",
            success: function (data) {
                $("#ModalWrapper").html(data);
                $("#modalStopTestMode").modal();
            }
        })
    }
}

var testmode;
$(function () {
    testmode = new Testmode();
    testmode.init();
})