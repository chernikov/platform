function AddPlayers() {
    var _this = this;

    this.init = function ()
    {
        $("#AddPlayersButton").click(function () {
            $.ajax({
                url: "/dashboard/AddPlayers",
                type: "GET",
                success: function (data) {
                    $("#ModalWrapper").html(data);
                    $("#modalAddPlayers").modal();
                }
            });
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
}

var addPlayers = null;
$(function () {
    addPlayers = new AddPlayers();
    addPlayers.init();
});