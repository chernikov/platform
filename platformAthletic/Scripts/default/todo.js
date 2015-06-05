'use strict';
function Todo() {
    var _this = this;

    this.init = function () {
        var item = window.location.hash;
        var id = item.substr("#todo-".length);
        _this.showTodo(id);

        $(document).on("click", "#PrintAll", function () {
            _this.clear();
        });

        $(document).on("click", "#AddPlayersButton", function () {
            $("#ModalWrapper").empty();
            $(".modal-backdrop").remove();
            $(".tutorial-highlight > *").unwrap();
            $(".tutorial-manipulate").removeClass("tutorial-manipulate");
        });


        if (typeof (schedule) != "undefined")
        {
            schedule.onSetSelect = function () {
                _this.clear();
            }
        }

        if (typeof (schedule) != "undefined") {
            schedule.onSetSelect = function () {
                _this.clear();
            }
        }

        if (typeof (extendedDashboard) != "undefined") {
            extendedDashboard.onSbcChange = function () {
                _this.clear();
            }
        }

        $(document).on("click", ".forbitBtn", function (e) {
            var message = $(this).data("message");
            _this.showInfo(message);
            e.stopPropagation();
            return false;

        });

        $("#ShowTestModeInfo").click(function () {
            _this.showInfo("Test mode means the site is populated with sample players and data. Nothing you do or change while in test mode will be saved. When you are ready to begin using the site for your school, exit test mode and follow the To-Do list items on the left hand side.");
        });

        $(".todo-list a").click(function () {
            var href = $(this).attr("href");
            window.location = href;
            window.location.reload();
        });


    }

    this.showTodo = function (id)
    {
        $.ajax({
            type: "GET",
            data : { id : id},
            url: "/Tutorial/TodoSnippet",
            success: function (data) {
                $("#ModalWrapper").html(data);
                $("#modalTutorial").modal();
                $('#modalTutorial').on('hidden.bs.modal', function () {
                    $(".tutorial-highlight > *").unwrap();
                    $(".tutorial-manipulate").removeClass("tutorial-manipulate");
                });
            }
        })
    }

    this.showInfo = function (message) {
        $.ajax({
            type: "GET",
            data: { message: message },
            url: "/Tutorial/Info",
            success: function (data) {
                $("#ModalWrapper").html(data);
                $("#modalTutorial").modal();
            }
        })
    }

    this.clear = function ()
    {
        var href = location.protocol + '//' + location.host + location.pathname;
        window.location = href;
    }

}

var todo;
$(function () {
    todo = new Todo();
    todo.init();
})