function Faq() {
    var _this = this;

    this.init = function () {
        $("#FaqContent").mCustomScrollbar({ theme: "minimal-dark" });
    }
}
var faq = null;
$(function () {
    faq = new Faq();
    faq.init();
});
