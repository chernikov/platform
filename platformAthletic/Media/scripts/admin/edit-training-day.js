function EditTrainingDay() {
    var _this = this;

    this.ajaxEditCellInfo = "/admin/TrainingDay/EditCellInfo";
    this.ajaxEditCellExercise = "/admin/TrainingDay/EditCellExercise";
    this.ajaxEditCellValue = "/admin/TrainingDay/EditCellValue";
    this.ajaxGetCellInfo = "/admin/TrainingDay/GetCellInfo";
    this.ajaxClearCell = "/admin/TrainingDay/ClearCell";

    this.init = function () {
        $(".infoCell").live("click", function () {
            var id = $(this).data("id");
            _this.showEditInfoCell(id);
        });

        $(".exerciseCell").live("click", function () {
            var id = $(this).data("id");
            _this.showEditExerciseCell(id);
        });

        $(".valueCell").live("click", function () {
            var id = $(this).data("id");
            _this.showEditValueCell(id);
        });

        $(".primaryTextBtn").live("click", function () {
            $(".valuePrimaryText").toggle();
        });

        $("#saveCellModal").live("click", function () {
            _this.submitFormEditCell();
        });

        $("#clearCellModal").live("click", function () {
            _this.clearCell();
        });

      /*  $(".cell").each(function () {
            $(this).text($(this).data("id"));
        })*/
    }

    this.showEditInfoCell = function (id)
    {
        $.ajax({
            type: "GET",
            url: _this.ajaxEditCellInfo,
            data: { id: id, trainingDayID: $("#TrainingDayViewID").val() },
            success: function (data) {
                $("#cellModalWrapper").html(data);
                $("#cellModal").modal('show');
            }
        });
    }

    this.showEditExerciseCell = function (id)
    {
        $.ajax({
            type: "GET",
            url: _this.ajaxEditCellExercise,
            data: { id: id, trainingDayID: $("#TrainingDayViewID").val() },
            success: function (data)
            {
                $("#cellModalWrapper").html(data);
                $("#cellModal").modal('show');
            }
        });
    }

    this.showEditValueCell = function (id) {
        $.ajax({
            type: "GET",
            url: _this.ajaxEditCellValue,
            data: { id: id, trainingDayID: $("#TrainingDayViewID").val() },
            success: function (data) {
                $("#cellModalWrapper").html(data);
                $("#cellModal").modal('show');
                $(".valuePrimaryText").hide();
            }
        });

    }
    this.submitFormEditCell = function () {
        var ajaxData = $("#EditCellForm").serialize();
        var otherAjaxData = {
            cellID: $("#CellID").val(),
            trainingDayID: $("#TrainingDayID").val()
        };
        var url = $("#EditCellForm").attr("action");
        $.ajax({
            type: "POST",
            url: url,
            data: ajaxData,
            success: function () {
                _this.UpdateCell(otherAjaxData);
            }
        });
    }

    this.clearCell = function () {
        var ajaxData = {
            cellID: $("#CellID").val(),
            trainingDayID: $("#TrainingDayID").val()
        };
        $.ajax({
            type: "POST",
            url: _this.ajaxClearCell,
            data: ajaxData,
            success: function () {
                $(".cell[data-id='" + ajaxData.cellID + "']").text("");
                $("#cellModal").modal('hide');
            }
        });
    }

    this.UpdateCell = function (ajaxData) {
        $.ajax({
            type: "POST",
            url: _this.ajaxGetCellInfo,
            data: ajaxData,
            success: function (data) {
                if (data.result == "ok") {
                    $(".cell[data-id='" + ajaxData.cellID + "']").text(data.data);
                    $("#cellModal").modal('hide');
                }
            }
        });
    }


}

var editTrainingDay = null;
$().ready(function () {
    editTrainingDay = new EditTrainingDay();
    editTrainingDay.init();

});