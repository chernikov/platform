function FaqList() {

    var _this = this;

    this.AjaxUpdateFaqOrder = "/admin/Faq/AjaxFaqOrder";

    this.init = function () {
        _this.initSortable($(".faqList"));
    }

    this.initSortable = function (item)
    {
        item.sortable({
            placeholder: 'faq-placeholder ui-state-highlight',
            stop: function (event, ui) {
                var sortingInfo = [];
                $("> .faqItem", $(this)).each(function () {
                    sortingInfo.push($(this).data("id"));
                });
                var isNeedUpdate = false;
                var itemId = ui.item.data("id");

                var ajaxData = null;
                for (var i = sortingInfo.length; i--;) {
                    if (sortingInfo[i] != itemId) {
                        continue;
                    }
                    ajaxData =
                    {
                        id: itemId,
                        replaceTo: i + 1
                    };
                    isNeedUpdate = true;
                    break;
                }

                if (!isNeedUpdate) {
                    return;
                }

                $.ajax({
                    type: "POST",
                    url: _this.AjaxUpdateFaqOrder,
                    data: ajaxData,
                    success: function (data)
                    {
                        if (data.result == "error") {
                            $(this).sortable('cancel');
                            alert("Error");
                        }
                    },
                    error: function ()
                    {
                        alert("Internal Error");
                    }
                });
            }
        });
    }
}

var faqList;
$().ready(function () {
    faqList = new FaqList();
    faqList.init();
});