'use strict';
function AddPlayers() {
    var _this = this;

    this.init = function ()
    {
        

        $(document).on('click', "#ChooseOption", function () {
            if (typeof (testmode) != "undefined") {
                testmode.showInfoExtended("You are still in Test Mode. Players added will not be saved, once you exit test mode.", "Understood", function () {
                    _this.showChooseOption();
                });
            } else {
                _this.showChooseOption();
            }
        });
        $(document).on('click', "#AddPlayersButton", function () {
            if (typeof (testmode) != "undefined") {
                testmode.showInfoExtended("You are still in Test Mode. Players added will not be saved, once you exit test mode.", "Understood", function () {
                    _this.showAddPlayers();
                });
            } else {
                _this.showAddPlayers();
            }
        });
        $(document).on('click', "#ImportPlayerButton", function () {
            if (typeof (testmode) != "undefined") {
                testmode.showInfoExtended("You are still in Test Mode. Players added will not be saved, once you exit test mode.", "Understood", function () {
                    _this.showImportPlayers();
                });
            } else {
                _this.showImportPlayers();
            }
        });

        $(document).on("click", "#AddMoreRow", function () {
            $.ajax({
                type: "GET",
                url: "/dashboard/AddPlayerItem",
                success: function (data) {
                    $("#ListOfFields").append(data);
                }
            });
        });

        $(document).on("click", ".add-player-item .remove-row", function () {
            $(this).closest(".add-player-item").remove();
        });

        $(document).on("click", "#NoBtn", function () {
            $("#FirstPanel").show();
            $("#SecondPanel").hide();
        });

        $(document).on("click", ".try-close", function () {
            $("#FirstPanel").hide();
            $("#SecondPanel").show();
        });

        $(document).on("click", "#SubmitAddPlayers", function () {
            var data = $("#AddPlayersForm").serialize();
            $.ajax({
                type: "POST",
                url: "/dashboard/AddPlayers",
                data: data,
                beforeSend: function () {
                    $("#SubmitAddPlayers").attr("disabled", "disabled");
                },
                success: function (data)
                {
                    $("#AddPlayersBodyWrapper").html(data);
                },
                complete: function () {
                    $("#SubmitAddPlayers").removeAttr("disabled");
                }
            })
        });

    }

    this.showImportPlayers = function () {
        $.ajax({
            url: "/dashboard/ImportPlayer",
            type: "GET",
            success: function (data) {
                $("#ModalWrapper").html(data);
                if ($(".modal-backdrop.fade.in").length > 0) {
                    $(".modal-backdrop.fade.in").remove();
                }

                jQuery('#FileUpload').uploadify({
                    swf: '/Scripts/uploadify.swf',
                    uploader: '/dashboard/UploadFile/',
                    auto: false,
                    multi: false,
                    onUploadComplete: function (file) {
                        alert('The file ' + file.name + ' finished processing.');
                    },
                    onUploadSuccess: function (file, data, response) {
                        alert(data);
                    }
                });

                $(document).on("click", "#UploadFile", function () {
                    jQuery('#FileUpload').uploadify('upload');
                });

                $("#ModalImportPlayer").modal();

            }
        });
    }
    //"#AddPlayersButton"
    this.showChooseOption = function () {
        $.ajax({
            url: "/dashboard/ChooseOption",
            type: "GET",
            success: function (data) {
                $("#ModalWrapper").html(data);
                $("#ModalChooseOption").modal();
            }
        });
    }
    this.showAddPlayers = function () {
        $.ajax({
            url: "/dashboard/AddPlayers",
            type: "GET",
            success: function (data) {
                $("#ModalWrapper").html(data);
                $("#modalAddPlayers").modal();
            }
        });
    }

}

var addPlayers = null;
$(function () {
    addPlayers = new AddPlayers();
    addPlayers.init();
});