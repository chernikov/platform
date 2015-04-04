function MacrocycleList()
{

    var _this = this;

    var nameChanged = false;
    this.init = function ()
    {
        $("#CreateMacrocycle").click(function ()
        {
            _this.createMacrocycle();
            return false;
        });

        $("#CreateMacrocycleBtn").click(function () {
            _this.createMacrocycleSubmit();
        });

        $(".macrocycle-week").click(function ()
        {
            var item = $(this).closest(".macrocycle-week-wrapper");
            $(".macrocycle-name").show();
            $(this).hide();
            $.ajax({
                type: "GET",
                url: "/admin/Macrocycle/GetWeekList",
                data: { id: item.data("id") },
                beforeSend : function() {
                    $(".select-macrocycle").remove();
                },
                success: function (data)
                {
                    item.append(data);

                    $("#SelectNumber").keyup(function (e) {
                        if (e.which == 27)
                        {
                            $(".select-macrocycle").remove();
                            $("div", item).show();
                        }
                    });

                    $("#SelectNumber").change(function ()
                    {
                        var val = $("#SelectNumber").val();
                        var text = $("#SelectNumber option[value='" + val + "']").text();
                        $(".select-macrocycle").remove();
                       
                        $.ajax({
                            type: "GET",
                            url: "/admin/Macrocycle/SetWeek",
                            data: { id: item.data("id"), val: val },
                            success: function (data)
                            {
                                (".select-macrocycle").remove();
                                $("div", item).text(data);
                            }
                        });
                        $("div", item).show();
                    });
                }
            });
        });

        $(".macrocycle-name").click(function () {
            var item = $(this).closest(".macrocycle-name-wrapper");
            $(".macrocycle-name").show();
            $(this).hide();
            $.ajax({
                type: "GET",
                url: "/admin/Macrocycle/GetWeekName",
                data: { id: item.data("id") },
                beforeSend: function () {
                    $(".select-macrocycle").remove();
                },
                success: function (data)
                {
                    item.append(data);
                    $("#Name").keyup(function (e) {
                        if (e.which == 27) {
                            $(".select-macrocycle").remove();
                            $("div", item).show();
                        }
                        if (e.which == 13) {
                            var val = $("#Name").val();
                            $.ajax({
                                type: "GET",
                                url: "/admin/Macrocycle/SetWeekName",
                                data: { id: item.data("id"), name : val },
                                success: function (data)
                                {
                                    $(".select-macrocycle").remove();
                                    $("div", item).text(data);
                                }
                            });
                            $("div", item).show();
                        }
                    });
                }
            });
        });
    }

    
    this.createMacrocycle = function ()
    {
        $.ajax({
            type: "GET",
            url: $("#CreateMacrocycle").attr("href"),
            success: function (data)
            {
                $("#MacrocycleModalWrapper").html(data);
                _this.initPopup();
                $("#MacrocycleModal").modal('show');
            }
        });
    }

    this.createMacrocycleSubmit = function ()
    {
        $.ajax({
            type: "POST",
            url: "/admin/Macrocycle/Create",
            data : $("#CreateMacrocycleForm").serialize(),
            success: function (data)
            {
                $("#MacrocycleModalWrapper").html(data);
                _this.initPopup();
            }
        });
    }

    this.initPopup = function ()
    {
        if ($("#WeekID").length > 0)
        {
            $("#WeekID").change(function () {
                if (!nameChanged) {
                    $.ajax({
                        type: "GET",
                        url: "/admin/Macrocycle/GetWeekInfo",
                        data: { id: $("#WeekID").val() },
                        success: function (data) {
                            $("#Name").val(data);
                        }
                    });
                }
            });

            $("#Name").blur(function () {
                if ($("#Name").val() == "") {
                    nameChanged = false;
                } else {
                    nameChanged = true;
                }
            });

            $("#WeekID").change();
            $("#CreateMacrocycleBtn").show();
        } else {
            $("#CreateMacrocycleBtn").hide();
        }
        
    }
}


var macrocycleList = null;

$().ready(function () {
    macrocycleList = new MacrocycleList();
    macrocycleList.init();
});