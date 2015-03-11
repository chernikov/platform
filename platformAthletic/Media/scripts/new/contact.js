function Contact() {
    var _this = this;

    this.init = function () {
        $('.mySelectBoxClass').customSelect();
    }
}


var contact = null;

$(function () {
    contact = new Contact();
    contact.init();
})