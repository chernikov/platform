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
            _this.showInfo("Test mode means the site is populated with sample players and data. Nothing you do or change while in test mode will be saved. When you are ready to begin using the site for your school, exit test mode and follow the To-Do list items on the left hand side.");
        });
    }
}

var testmode;
$(function () {
    testmode = new Testmode();
    testmode.init();
})