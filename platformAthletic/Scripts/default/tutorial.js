function Tutorial() {
    var _this = this;

    this.init = function () {
        $.ajax({
            type: "GET",
            url: "/Tutorial",
            success: function (data) {
                $("#ModalWrapper").html(data);
                $("#modalTutorial").modal();
            }
        })
    }
}

var tutorial = null;
$(function () {

    tutorial = new Tutorial();
    tutorial.init();
});