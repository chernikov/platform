function Equipment() {
    var _this = this;

    this.init = function ()
    {
        $('.scroll-pane').jScrollPane({ showArrows: true });
        $('.left .menu').css('height', $('.equipment-wrp').height());

        $(".item .image").live("click", function () {
            var wrp = $(this).closest(".item");

            $(".checkbox-wrp", wrp).click();
        });
    }
}
var equipment = null;

$().ready(function () {
    equipment = new Equipment();
    equipment.init();
});