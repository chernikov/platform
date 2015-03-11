function EditMacrocycleTable() {
    var _this = this;

    this.ajaxEditCellInfo = "/admin/Macrocycle/EditCellInfo";
    this.ajaxEditCellExercise = "/admin/Macrocycle/EditCellExercise";
    this.ajaxEditCellValue = "/admin/Macrocycle/EditCellValue";
    this.ajaxGetCellInfo = "/admin/Macrocycle/GetCellInfo";
    this.ajaxClearCell = "/admin/Macrocycle/ClearCell";

    this.init = function () {
        $(".infoCell").live("click", function () {
            var id = $(this).data("id");
            var dayID = $(this).closest(".TrainingDayWrapper").data("id");
            _this.showEditInfoCell(id, dayID);
        });

        $(".exerciseCell").live("click", function () {
            var id = $(this).data("id");
            var dayID = $(this).closest(".TrainingDayWrapper").data("id");
            _this.showEditExerciseCell(id, dayID);
        });

        $(".valueCell").live("click", function () {
            var id = $(this).data("id");
            var dayID = $(this).closest(".TrainingDayWrapper").data("id");
            _this.showEditValueCell(id, dayID);
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
    }

    this.showEditInfoCell = function (id, dayID) {
        $.ajax({
            type: "GET",
            url: _this.ajaxEditCellInfo,
            data: { id: id, trainingDayID: dayID },
            success: function (data) {
                $("#cellModalWrapper").html(data);
                $("#cellModal").modal('show');
            }
        });
    }

    this.showEditExerciseCell = function (id, dayID) {
        $.ajax({
            type: "GET",
            url: _this.ajaxEditCellExercise,
            data: { id: id, trainingDayID: dayID },
            success: function (data) {
                $("#cellModalWrapper").html(data);
                $("#cellModal").modal('show');
            }
        });
    }

    this.showEditValueCell = function (id, dayID) {
        $.ajax({
            type: "GET",
            url: _this.ajaxEditCellValue,
            data: { id: id, trainingDayID: dayID },
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
                $(".TrainingDayWrapper[data-id='" + ajaxData.trainingDayID + "'] .cell[data-id='" + ajaxData.cellID + "']").text("");
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
                    $(".TrainingDayWrapper[data-id='" + ajaxData.trainingDayID + "'] .cell[data-id='" + ajaxData.cellID + "']").text(data.data);
                    $("#cellModal").modal('hide');
                }
            }
        });
    }


}

var editMacrocycleTable = null;
$().ready(function () {
    editMacrocycleTable = new EditMacrocycleTable();
    editMacrocycleTable.init();
});