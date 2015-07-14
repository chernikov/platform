'use strict';
function Todo() {
    var _this = this;

    this.init = function () {
        var item = window.location.hash;
        if (item.length > 0) {
            var id = item.substr("#todo-".length);
            _this.showTodo(id);
        }

        $(document).on("click", "#PrintAll", function () {
            _this.clear();
        });

        $(document).on("click", "#AddPlayersButton", function () {
            $("#ModalWrapper").empty();
            $(".modal-backdrop").remove();
            common.clearOnBoarding();
        });

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


        $(".todo-list-item a").click(function () {
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
                _this.unwrap();
                $("#ModalWrapper").html(data);
                $("#modalTutorial").modal();
                $('#modalTutorial').on('hidden.bs.modal', function () {
                    common.clearOnBoarding();
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

    this.unwrap = function () {
        common.clearOnBoarding();
    }
}

var todo;
$(function () {
    todo = new Todo();
    todo.init();
})