function ListTrainingSet() {
    var _this = this;
    this.ajaxRemove = "/admin/TrainingSet/Delete";

    this.init = function () {

        $(".remove").live("click", function () {
            _this.removeItem($(this));
        });
    }

    this.removeItem = function (item)
    {
        $.ajax({
            type: "GET",
            url: _this.ajaxRemove,
            data: { id: item.data("id") },
            success: function (data)
            {
                item.closest("tr").remove();
            }
        });
    }

}

var listTrainingSet = null;
$().ready(function () {
    listTrainingSet = new ListTrainingSet();
    listTrainingSet.init();
});