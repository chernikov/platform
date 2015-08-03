'use strict';
function Todo() {
    var _this = this;

    this.init = function () {
        var hash = window.location.hash;
        if (hash.length > 0) {
            var id = hash.substr("#todo-".length);
            _this.showTodo(id);
        }

        $(document).on("click", "#AddPlayersButton", function () {
            $("#ModalWrapper").empty();
            $(".modal-backdrop").remove();
            common.clearOnBoarding();
        });


        if (typeof (extendedDashboard) != "undefined" && hash.length > 0) {
            extendedDashboard.onSbcChange = function () {
                _this.updateTodo();
                var href = location.protocol + '//' + location.host + location.pathname;
                window.location = href;
            }
        }

        $(document).on("click", ".todoItem", function () {
            window.location= $(this).data("href");
            //var current = window.location.pathname;
            //var hash =.hash;
            //if (href.indexOf(current) != -1) {
            //    if (href == current + hash) {
            //        window.location.reload();
            //    } else {
            //        window.open(href, "_self");
            //        if (window.navigator.userAgent.indexOf("MSIE ") <= 0) {
            //            window.location.reload();
            //        }
                   
            //    }
            //} else {
            //    window.open(href, "_self");
            //}
        });

        $(document).on("click", ".forbitBtn", function (e) {
            var message = $(this).data("message");
            _this.showInfo(message);
            e.stopPropagation();
            return false;
        });

        $(document).on("click", "#DoneDealBtn", function () {
            $.ajax({
                type: "GET",
                url: "/Tutorial/StartTrainingDate",
                success: function (data) {
                    _this.updateTodo();
                }
            });
        });

        $(document).on("click", ".todoUpdate", function () {
            _this.updateTodo();
        });

    }

    this.showTodo = function (id) {
        $.ajax({
            type: "GET",
            data: { id: id },
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

    this.clear = function () {
        var href = location.protocol + '//' + location.host + location.pathname;
        window.location = href;
    }

    this.unwrap = function () {
        common.clearOnBoarding();
    }

    this.updateTodo = function ()
    {
        var id = $(this).data("todo");
        $.ajax({
            type: "GET",
            url: "/tutorial/todo",
            data: { id: id },
            success: function (data) {
                $(".todo-li").remove();
                $(data).insertAfter("#MainMenu li:last");
            }
        });
    }
}

var todo;
$(function () {
    todo = new Todo();
    todo.init();

})