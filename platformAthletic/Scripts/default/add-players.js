'use strict';
function AddPlayers() {
    var _this = this;

    this.init = function ()
    {
        $("#AddPlayersButton").click(function () {
            if (typeof (testmode) != "undefined") {
                testmode.showInfoExtended("You are still in Test Mode. Players added will not be saved, once you exit test mode.", "Understood", function () {
                    _this.showAddPlayers();
                });
            } else {
                _this.showAddPlayers();
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