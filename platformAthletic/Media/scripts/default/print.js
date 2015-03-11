function PrintObj() {

    var _this = this;
    
    this.init = function () {
        $(".print").live("click", function () {
            window.print();
        });
    }
}


var printObj = null;
$().ready(function () {
    printObj = new PrintObj();
    printObj.init();
});
