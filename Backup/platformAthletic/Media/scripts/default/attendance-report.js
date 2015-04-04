function AttendanceReport() {
    _this = this;
    this.ajaxUpdateChart = "/Report/AttendanceChart";
    this.ajaxGetAttendanceData = "/Report/GetAttendanceData";
    this.ajaxGetAttendancePrint = "/attendance-report-print";

    this.init = function ()
    {
        $("#SelectedType, #SelectedPeriod").change(function () {
            _this.updateChart();
        });

        $("#PrintButton").live("click", function () {
            _this.print();
        });

        if ($.fn.datepicker) {
            $("#BeginDate").datepicker({
                showOtherMonths: true,
                selectOtherMonths: true,
                beforeShowDay: _this.disableBeginDays,
                onSelect: function () {
                    _this.updateChart();
                }
            });
            $("#BeginDateIcon").click(function () {
                $("#BeginDate").datepicker("show");
            });
            $("#EndDate").datepicker({
                showOtherMonths: true,
                selectOtherMonths: true,
                beforeShowDay: _this.disableEndDays,
                onSelect: function () {
                    _this.updateChart();
                }
            });
            $("#EndDateIcon").click(function () {
                $("#EndDate").datepicker("show");
            });
        }

        _this.updateChart();
    }

    this.updateChart = function () {
        var ajaxData = $("#AttendanceReportForm").serialize();

        $.ajax({
            type: "GET",
            url: _this.ajaxUpdateChart,
            data: ajaxData,
            success: function (data) {
                $("#AttendanceChartWrapper").html(data);
                _this.loadChart();
                colors.update();
            }
        })
    }

    this.loadChart = function () {

        var ajaxData = $("#AttendanceReportForm").serialize();
        $.ajax({
            type: "GET",
            url: _this.ajaxGetAttendanceData,
            data: ajaxData,
            success: function (source) {
                if (source.result == "ok") {
                    var arr = [];
                    arr[0] = ['Name', 'Attendance'];
                    $.each(source.data, function (i, item) {
                        arr.push([item.Key, item.Value]);
                    });
                    if (arr.length > 10) {
                        $("#mainChart").css({ height: 25 * arr.length });
                    }
                    var data = google.visualization.arrayToDataTable(arr);
                    var options = {
                        title: "Attendance",
                        vAxis: {
                            title: "Name",
                            textPosition: 'in'
                        },
                        hAxis: {
                            viewWindow: {
                                max: 1.1,
                                min: 0
                            },
                            format: '0%'
                        },
                    };
                    var chart = new google.visualization.BarChart(document.getElementById('mainChart'));
                    chart.draw(data, options);
                };
            }
        });
    }

    this.print = function ()
    {
        var ajaxData = $("#AttendanceReportForm").serialize();

        window.open(_this.ajaxGetAttendancePrint + "?" + ajaxData, "_blank");
    }

    this.disableBeginDays = function (date) {
        var endDate = new Date($("#EndDate").val());

        if (date >= endDate) {
            return [false];
        }
        return [true];
    }

    this.disableEndDays = function (date) {
        var beginDate = new Date($("#BeginDate").val());

        if (date <= beginDate) {
            return [false];
        }
        return [true];
    }
}

var attendanceReport = null;
$().ready(function () {
    attendanceReport = new AttendanceReport();
    attendanceReport.init();

});
