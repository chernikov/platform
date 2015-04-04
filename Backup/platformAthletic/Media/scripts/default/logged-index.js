function LoggedIndex() {
    _this = this;

    this.init = function () {
        var pagWrp = $('.pagination ul');
        var wrpWidth = $('.pagination').width();
        var width = pagWrp.width();
        var marginVal = (wrpWidth - width) / 2;

        pagWrp.css('margin-left', marginVal);
    }
}


var loggedIndex = null;
$().ready(function () {
    loggedIndex = new LoggedIndex();
    loggedIndex.init();
});
