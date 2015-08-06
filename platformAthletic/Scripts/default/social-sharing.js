function SocialSharing()
{
    var _this = this;

    this.init = function () {
        $(".facebook").click(function () {
            var id = $(this).data("id");
            window.open("https://www.facebook.com/dialog/share?app_id=722048687941492&href=http://app.plt4m.com/user/" + id + "&display=popup&redirect_uri=http://app.plt4m.com/close", "_blank", 'height=560,width=530,scrollbars=true');
        });
        $(".google").click(function () {
            var id = $(this).data("id");
            window.open("https://plus.google.com/u/0/share?url=http://app.plt4m.com/user/" + id, "_blank", 'height=560,width=530,scrollbars=true');
        });
        $(".twitter").click(function () {
            var id = $(this).data("id");
            window.open("https://twitter.com/intent/tweet?url=http://app.plt4m.com/user/" + id, "_blank", 'height=560,width=530,scrollbars=true');
        });
        $(".linkedin").click(function () {
            var id = $(this).data("id");
            window.open("https://www.linkedin.com/shareArticle?url=http://app.plt4m.com/user/" + id, "_blank", 'height=560,width=530,scrollbars=true');
        });
        
    }
}

var socialSharing = null;
$(function () {
    socialSharing = new SocialSharing();
    socialSharing.init();
})