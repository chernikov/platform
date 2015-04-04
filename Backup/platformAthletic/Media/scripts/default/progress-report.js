function ProgressReport() {
    _this = this;
    this.ajaxUpdateChart = "/Report/ProgressChart";
    this.ajaxGetProgressData = "/Report/GetProgressData";
    this.ajaxGetProgressPrint = "/progress-report-print";


    this.init = function () {
        $("#SelectedType").change(function () {
            $("#ProgressReportForm").submit();
        });

        $("#FieldPositionID, #UserID, #GroupID, #SelectedPeriod, #Weight, #BeginDate, #EndDate").change(function () {
            _this.updateChart();
        });

        $("#PrintButton").live("click", function () {
            _this.print();
        });
        if($.fn.datepicker)
        {
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
        var ajaxData = $("#ProgressReportForm").serialize();

        $.ajax({
            type: "GET",
            url: _this.ajaxUpdateChart,
            data: ajaxData,
            success: function (data) {
                $("#ProgressChartWrapper").html(data);
                _this.loadChart();
                colors.update();
            }
        })
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

    this.loadChart = function () {

        var ajaxData = $("#ProgressReportForm").serialize();

        $.ajax({
            type: "GET",
            url: _this.ajaxGetProgressData,
            data: ajaxData,
            success: function (source) {
                if (source.result == "ok") {
                    var arr = [];
                    arr[0] = ['Date', 'Progress'];
                    $.each(source.data, function (i, item) {
                        var d = new Date(item.Key.match(/\d+/)[0] * 1);
                        var curr_date = d.getDate();
                        var curr_month = d.getMonth() + 1;
                        var curr_year = d.getFullYear();
                        var key = curr_month + "/" + curr_date + "/" + curr_year;
                        var value = item.Value;
                        arr.push([key, value]);
                    });
                    var total = Math.round((source.data[source.data.length - 1].Value - source.data[0].Value) / 5) * 5;
                    $("#total").text(total);
                }
                var data = google.visualization.arrayToDataTable(arr);

                var options = {
                    hAxis: { title: 'Date', titleTextStyle: { color: 'red' } },
                    pointSize: 5
                };

                var chart = new google.visualization.AreaChart(document.getElementById('mainChart'));
                chart.draw(data, options);
            }
        })
    }

    this.print = function () {
        var ajaxData = $("#ProgressReportForm").serialize();

        window.open(_this.ajaxGetProgressPrint + "?" + ajaxData, "_blank");
    }

}

var progressReport = null;
$().ready(function () {
    progressReport = new ProgressReport();
    progressReport.init();

});
