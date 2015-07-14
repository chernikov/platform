function ModalAttendance()
{
    var _this = this;

    this.init = function () {

        $("#CalendarButton").click(function () {
            var date = $("#CurrentDate").data("date");
            $.ajax({
                type: "GET",
                url: "/dashboard/Calendar",
                data: {
                    date: date,
                    selectedDate: date
                },
                success: function (data) {
                    $("#ModalWrapper").html(data);
                    $("#modalCalendar").modal();
                }
            })
        });

        $(document).on("click", ".attendanceModalMonth", function ()
        {
            var date = $(this).data("date");
            var selectedDate = $(this).data("selected");
            if (typeof (date) != "undefined")
            {
                $.ajax({
                    type: "GET",
                    url: "/dashboard/CalendarBody",
                    data: {
                        date: date,
                        selectedDate: selectedDate
                    },
                    success: function (data)
                    {
                        $("#ModalCalendarBodyWrapper").html(data);
                    }
                });
            }
        });

        $(document).on("click", ".selectDate", function ()
        {
            var href = window.location;
            var value = $(this).data("value");
            window.location = $.param.querystring(window.location.href, 'selectedDate=' + value);
        });
    }

    

}

var modalAttendance = null;

$(function () {
    modalAttendance = new ModalAttendance();
    modalAttendance.init();
});