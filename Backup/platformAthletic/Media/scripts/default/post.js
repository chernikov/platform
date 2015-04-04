function Post() {
    var _this = this;

    this.init = function () {
        $('.scroll-pane').jScrollPane({ showArrows: true });
    };
}
var post = null;

$().ready(function () {
    post = new Post();
    post.init();
});