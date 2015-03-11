function FeatureList() {

    var _this = this;

    this.AjaxUpdateFeatureTextMoveUrl = "/admin/Feature/AjaxFeatureTextMove";
    this.AjaxUpdateFeatureCatalogOrder = "/admin/Feature/AjaxFeatureCatalogOrder";
    this.AjaxUpdateFeatureTextOrder = "/admin/Feature/AjaxFeatureTextOrder";

    this.init = function () {
        _this.initDraggable();

        _this.initSortableCatalog($(".featureList"));
        _this.initSortableText($(".featureTextList"));
    }

    this.initSortableCatalog = function (item) {
        item.sortable({
            placeholder: 'feature-placeholder ui-state-highlight',
            stop: function (event, ui) {
                var sortingInfo = [];
                $("> .featureItem", $(this)).each(function () {
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
                    url: _this.AjaxUpdateFeatureCatalogOrder,
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

    this.initSortableText = function (item) {
        item.sortable({
            placeholder: 'feature-placeholder ui-state-highlight',
            stop: function (event, ui) {
                var sortingInfo = [];
                $("> .featureTextItem", $(this)).each(function () {
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
                    url: _this.AjaxUpdateFeatureTextOrder,
                    data: ajaxData,
                    success: function (data) {
                        if (data.result == "error") {
                            $(this).sortable('cancel');
                            alert("Error");
                        }
                    },
                    error: function () {
                        alert("Internal Error");
                    }
                });
            }
        });
    }

    this.initDraggable = function () {
        $(".featureItem").droppable({
            accept: '.subFeatureItem',
            drop: function (event, ui) {
                var id = ui.draggable.parent().data("id");
                var moveTo = $(this).data("id");

                var ajaxData = { id: id, moveTo: moveTo };
                if (id == moveTo) {
                    return false;
                }
                $.ajax({
                    type: "POST",
                    url: _this.AjaxUpdateFeatureTextMoveUrl,
                    data: ajaxData,
                    success: function (data) {
                        if (data.result == "ok") {
                            window.location.reload();
                        }
                        if (data.result == "error") {
                            $(this).droppable('cancel');
                        }
                    },
                    error: function () {
                        $(this).droppable('cancel');
                        alert("Internal error");
                    }
                });
            }
        });

        $(".subFeatureItem").draggable({
            helper: function (event) {
                return $("<div class='ui-widget-move'>Move to new branch...</div>");
            },
            start: function (event, ui) {
                $(".featureItem").addClass("highlight");
            },
            stop: function (event, ui) {
                $(".featureItem").removeClass("highlight");
            }
        });
    }
}

var featureList;
$().ready(function () {
    featureList = new FeatureList();
    featureList.init();
});