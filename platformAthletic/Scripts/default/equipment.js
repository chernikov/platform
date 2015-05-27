function Equipment() {
    var _this = this;

    this.init = function ()
    {
        $("#EquipmentWrapper").mCustomScrollbar({ theme: "minimal-dark" });
    }
}

var equipment = null;
$(function () {
    equipment = new Equipment();
    equipment.init();
});