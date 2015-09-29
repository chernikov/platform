'use strict';
function AddPlayers() {
    var _this = this;

    this.init = function ()
    {
        

        $(document).on('click', "#ChooseOption, #CancelUploadFile", function () {
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
        $(document).on('click', "#ImportPlayerButton, #BackToUploadFile", function () {
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

        $(document).on('click', '#ConfirmUploadPlayers', function () {
            $("#ConfirmUploadPlayers").prop("disabled", true);
            var data = $("#AddPlayersForm").serialize();
            $.ajax({
                type: 'POST',
                url: "/dashboard/SubmitUploadFile",
                data: data + "&firstCheck=true",
                success: function (data) {
                    $('#modalUploadFile').modal('hide');
                    $('.modal-backdrop.fade.in').remove();
                    $('#ModalWrapper').html(data);
                }
            });
        });

        $(document).on("click", "#SubmitUploadPlayers", function () {
            var data = $("#AddPlayersForm").serialize();
            $.ajax({
                type: "POST",
                url: "/dashboard/SubmitUploadFile",
                data: data,
                beforeSend: function () {
                    $("#SubmitUploadPlayers").attr("disabled", "disabled");
                },
                success: function (data) {
                    if (data.result === "success") {
                        $("#modalUploadFile").modal('hide');
                        $(".modal-backdrop.fade.in").remove();
                        $.ajax({
                            type: "POST",
                            url: "/dashboard/UploadSuccess",
                            data: {count : data.count},
                            success: function (data) {
                                $("#ModalWrapper").html(data);
                            }
                        });
                    }
                    else {
                        $("#AddPlayersBodyWrapper").html(data);
                    }
                },
                complete: function () {
                    $("#SubmitUploadPlayers").removeAttr("disabled");
                }
            })
        });

        $(document).on("change", "select.first-column, select.second-column, select.third-column", function (eve) {
            var current_select = this;
            var to_change_select;
            var headers = [];
            $("#modalUploadFile select").each(function () {
                if (current_select !== this && $(this).val() === $(current_select).val()) {
                    to_change_select = this;
                }
                headers.push($(this).val());
            });

            if ($.inArray("FirstName", headers) === -1) {
                $(to_change_select).val('FirstName');
            }
            else if ($.inArray("LastName", headers) === -1) {
                $(to_change_select).val('LastName');
            }
            else if ($.inArray("Email", headers) === -1) {
                $(to_change_select).val('Email');
            }
            //$("#ListOfFields .first-column").each(function (eve) {
            //    var $input = $(this).children("input");
            //    var $span = $(this).children("span");
            //    var change_column = $("select.first-column").val();
            //    var player_index = $(this).siblings("input#Players_index").val();
            //    var new_input_id = "Players_" + player_index + "__Value_" + change_column;
            //    var new_input_name = "Players[" + player_index + "].Value." + change_column;
            //    var new_input_placeholder = change_column.match(/[A-Z][a-z]+/g);
            //    $.each(new_input_placeholder, function (index, item) {
            //        new_input_placeholder[index] = item.toUpperCase();
            //    });
            //    new_input_placeholder = new_input_placeholder.join(" ") + " *";
            //    $input.attr("id", new_input_id);
            //    $input.attr("name", new_input_name);
            //    $input.attr("placeholder", new_input_placeholder);
            //    $span.attr("data-valmsg-for", new_input_name);
            //});
            changeColumnElemetsData(".first-column");
            changeColumnElemetsData(".second-column");
            changeColumnElemetsData(".third-column");
        });

        function changeColumnElemetsData(header_selector) {
            $("#ListOfFields " + header_selector).each(function (eve) {
                var $input = $(this).children("input");
                var $span = $(this).children("span");
                var change_column = $("select" + header_selector).val();
                var player_index = $(this).siblings("input#Players_index").val();
                var new_input_id = "Players_" + player_index + "__Value_" + change_column;
                var new_input_name = "Players[" + player_index + "].Value." + change_column;
                var new_input_placeholder = change_column.match(/[A-Z][a-z]+/g);
                $.each(new_input_placeholder, function (index, item) {
                    new_input_placeholder[index] = item.toUpperCase();
                });
                new_input_placeholder = new_input_placeholder.join(" ") + " *";
                $input.attr("id", new_input_id);
                $input.attr("name", new_input_name);
                $input.attr("placeholder", new_input_placeholder);
                $span.attr("data-valmsg-for", new_input_name);
            });
        }


    }//end init func

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
                    buttonText: "SELECT FILE",
                    auto: false,
                    multi: false,
                    height: 30,
                    uploadLimit: 1,
                    onUploadComplete: function (file) {
                        //alert('The file ' + file.name + ' finished processing.');
                    },
                    onUploadSuccess: function (file, data, response) {
                        $("#ModalImportPlayer").modal("hide");
                        $(".modal-backdrop.fade.in").remove();
                        $("#ModalWrapper").html(data);
                    }
                });

                $(document).on("click", "#UploadFile", function () {
                    jQuery('#FileUpload').uploadify('upload');
                });

                $("#ModalImportPlayer").modal();

            }
        });
    }
    this.showChooseOption = function () {
        $.ajax({
            url: "/dashboard/ChooseOption",
            type: "GET",
            success: function (data) {
                $("#ModalWrapper").html(data);
                $(".modal-backdrop.fade.in").remove();
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