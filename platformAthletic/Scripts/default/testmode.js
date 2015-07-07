function Testmode() {
    var _this = this;

    this.init = function () {
        $(document).on("click", ".forbitBtn", function () {
            var message = $(this).data("message");
            _this.showInfo(message);
            return false;
        });


        $("#ShowTestModeInfo").click(function () {
            _this.showInfo("Test mode means the site is populated with sample players and data. Nothing you do or change while in test mode will be saved. When you are ready to begin using the site for your school, exit test mode and follow the To-Do list items on the left hand side.");
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
}

var testmode;
$(function () {
    testmode = new Testmode();
    testmode.init();
})