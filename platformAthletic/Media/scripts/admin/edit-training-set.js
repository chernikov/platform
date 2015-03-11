function EditTrainingSet()
{
    var _this = this;
    this.ajaxAddNewLine = "/admin/TrainingSet/TrainingEquipment";

    this.init = function ()
    {
        $("#AddExtraLine").click(function () {
            _this.addExtraLine();
        });

        $(".remove-line").live("click", function () {
            _this.removeLine($(this));
        });
    }

    this.addExtraLine = function ()
    {
        $.ajax({
            type : "GET", 
            url : _this.ajaxAddNewLine,
            success: function (data)
            {
                var div = $("<div>").addClass("TrainingEqupmentWrapper").html(data);
                $("#TrainingSetList").append(div);
            }
        });
    }

    this.removeLine = function (item)
    {
        var item = item.closest(".TrainingEqupmentWrapper");
        item.remove();
    }
    
}

var editTrainingSet = null;
$().ready(function () {
    editTrainingSet = new EditTrainingSet();
    editTrainingSet.init();

});