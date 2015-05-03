function VideoLibrary() {
    var _this = this;

    this.init = function ()
    {
        $("#VideoContent").mCustomScrollbar({theme:"minimal-dark"});
    }
}

var videoLibrary = null;
$(function () {
    videoLibrary = new VideoLibrary();
    videoLibrary.init();
});