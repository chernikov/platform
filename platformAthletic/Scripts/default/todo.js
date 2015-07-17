'use strict';
function Todo() {
    var _this = this;

    this.init = function () {
        var hash = window.location.hash;
        if (hash.length > 0) {
            var id = hash.substr("#todo-".length);
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

        if (typeof (schedule) != "undefined" && hash.length > 0) {
            schedule.onSetSelect = function () {
                _this.clear();
            }
        }

        if (typeof (extendedDashboard) != "undefined" && hash.length > 0) {
            extendedDashboard.onSbcChange = function () {
                _this.clear();
            }
        }

        $(".todoItem").click(function () {
            var href = $(this).data("href");
            var current = window.location.pathname;
            var hash = window.location.hash;
            if (href.indexOf(current) != -1) {
                if (href == current+hash) {
                    window.location.reload();
                } else {
                    window.location = href;
                }
            } else {
                window.open(href, "_self");
            }
        });

        $(document).on("click", ".forbitBtn", function () {
            var message = $(this).data("message");
            _this.showInfo(message);
            return false;
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