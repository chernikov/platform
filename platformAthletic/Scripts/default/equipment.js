function Equipment() {
    var _this = this;

    this.init = function ()
    {
        
        $("#EquipmentWrapper").mCustomScrollbar({ theme: "minimal-dark" });

        $(".equipmentItem").click(function () {
            var checkbox = $("input", $(this));
            checkbox.prop("checked", !checkbox.prop("checked"));
        })
    }
}

var equipment = null;
$(function () {
    equipment = new Equipment();
    equipment.init();
});