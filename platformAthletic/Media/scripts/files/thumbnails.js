function Thumbnails() {
    var page = 0;

    var _this = this;
    
    var errorDialog;

    this.prefix = "thumbnail_";
    this.DialogName = "#ModalFileDialog";

    this.AjaxLoadFileList = "/admin/Files/LoadFiles";
    this.AjaxUpdateFile = "/admin/Files/UpdateFile";
    this.AjaxRemoveFile = "/admin/Files/RemoveFile";
    this.AjaxLoadMimeType = "/admin/Files/GetMimeImage";
    this.AjaxLoadSmallMimeType = "/admin/Files/GetMimeSmallImage";
    this.LoadingImage = "/Media/images/loading.gif";
    this.defaultMimeImage = "/Media/images/mime/default.png";
    this.defaultMimeSmallImage = "/Media/images/mime/small/default.png";

    this.container = function () {
        return $(".file-list");
    }

    this.init = function () {
        _this.load();

        _this.dialogInstance().dialog({
            autoOpen: false,
            width: "450px",
            resizable: false,
            modal: true
        });

        errorDialog = $("<div>").attr("title", "Ошибки");

        errorDialog.dialog({
            autoOpen: false,
            width: "450px",
            resizable: false,
            modal: true
        });

        $("#LoadFiles").click(function () {
            _this.load();
        });

        $("#cancelDialogFile").click(function () {
            _this.close();
        });

        $("#editDialoglFile").click(function () {
            _this.update();
        });

    }

    this.getById = function (id) {
        return $("#" + _this.prefix + id);
    }

    this.dialogInstance = function () {
        return $(_this.DialogName);
    }

    this.open = function () {
        _this.dialogInstance().dialog("open");
        _this.buttonEnable(true);
        _this.clearErrors();
    }

    this.close = function () {
        _this.dialogInstance().dialog("close");
    }


    this.populate = function (data) {
        var item = _this.getById(data.ID);

        var extension = data.FullPath.substring(data.FullPath.lastIndexOf("."));
        $("#ID", _this.dialogInstance()).attr("value", data.ID);

        $("#Extension", _this.dialogInstance()).text(extension);
        $("#MimeType", _this.dialogInstance()).text(data.MimeType);
        $("#Name", _this.dialogInstance()).attr("value", data.Name);
        $("#FullPath", _this.dialogInstance()).attr("href", data.FullPath);
        $("#Preview", _this.dialogInstance()).attr("src", $(".preview img", item).attr("src"));
        $("#Preview", _this.dialogInstance()).attr("class", $(".preview img", item).attr("class"));
    }


    this.create = function (data) {
        var thumbnail = $("<div/>").attr("class", "thumbnail-item").attr("id", this.prefix + data.ID).prependTo(_this.container());
        var toolbox = _this.createToolBox(data);
        toolbox.appendTo(thumbnail);
        var container = $("<div/>").attr("class", "container").appendTo(thumbnail);
        var mimetype = $("<div/>").attr("class", "MimeType").text(data.MimeType).hide().appendTo(container);
        var previewDiv = $("<div/>").attr("class", "preview").appendTo(container);
        if (data.PreviewPath != "") {
            var image = $("<img/>").attr("src", data.PreviewPath).appendTo(previewDiv);
        } else {
            var image = $("<img/>").attr("class", "loading").attr("src", _this.LoadingImage).appendTo(previewDiv);
            ajaxData = {
                mimeType: data.MimeType
            }
            $.ajax({
                type: "POST",
                url: _this.AjaxLoadMimeType,
                data: ajaxData,
                success: function (data) {
                    image.attr("class", "mime-icon").attr("src", data.resource);
                    image.attr("height", "200");
                    image.attr("width", "200");
                },

                error: function () {
                    image.attr("class", "mime-icon").attr("src", _this.defaultMimeImage);
                    image.attr("height", "200");
                    image.attr("width", "200");
                }
            });
        }

        thumbnail.hover(
                    function () {
                        toolbox.show();
                    },
                    function () {
                        toolbox.hide();
                    }
                );

        $("<div/>").attr("class", "Name").text(data.Name).appendTo(container);
    }

    this.createToolBox = function (data) {
        var object = $("<div/>").attr("class", "toolbox").hide();
        var editButton = $("<div/>").attr("class", "edit").appendTo(object);
        var deleteButton = $("<div/>").attr("class", "delete").appendTo(object);
        var pasteLinkButton = $("<div/>").attr("class", "paste-link").appendTo(object);
        if (data.PreviewPath != "") {
            var pasteImageButton = $("<div/>").attr("class", "paste").appendTo(object);
            pasteImageButton.click(function () {
                _this.pasteAsImage(data);
            });
        }
        editButton.click(function () {
            _this.edit(data);
        });
        deleteButton.click(function () {
            _this.remove(data.ID);
        });

        pasteLinkButton.click(function () {
            _this.pasteSubDescriptionAsLink(data);
        });

        return object;
    }

    this.edit = function (data) {
        _this.populate(data);
        _this.open();
    }

    this.update = function () {
        ajaxData = {
            ID: $("#ID", _this.dialogInstance()).attr("value"),
            Name: $("#Name", _this.dialogInstance()).attr("value"),
            Active : true
        }
        $.ajax({
            type: "POST",
            url: _this.AjaxUpdateFile,
            beforeSend: function () {
                _this.clearErrors();
                _this.buttonEnable(false);
            },
            data: ajaxData,
            success: function (data) {
                _this.buttonEnable(true);
                if (data.result == "ok") {
                    _this.updateImageData(data.data);
                    _this.close();
                }
                if (data.result == "error") {
                    _this.showErrors(data.errors);
                }
            },
            error: function () {
                _this.buttonEnable(true);
                alert("Внутренняя ошибка");
            }

        });
    }

    this.remove = function (Id) {
        if (confirm("Удалить этот файл?")) {
            ajaxData = {
                id: Id
            }
            $.ajax({
                type: "POST",
                url: _this.AjaxRemoveFile,
                data: ajaxData,
                success: function (data) {
                    if (data.result == "ok") {
                        _this.getById(Id).fadeOut(300);
                    }
                    if (data.result == "error") {
                        _this.showErrors(data.errors);
                    }
                },
                error: function () {
                    alert("Внутренняя ошибка");
                }
            });
        }
    }

    this.updateImageData = function (data) {
        var item = _this.getById(data.ID);
        var toolbox = $(".toolbox", item);
        toolbox.empty();
        var editButton = $("<div/>").attr("class", "edit").appendTo(toolbox);
        var deleteButton = $("<div/>").attr("class", "delete").appendTo(toolbox);
        var pasteLinkButton = $("<div/>").attr("class", "paste-link").appendTo(toolbox);
        if (data.PreviewPath != "") {
            var pasteImageButton = $("<div/>").attr("class", "paste").appendTo(toolbox);
            pasteImageButton.click(function () {
                _this.pasteAsImage(data);
            });
        }
        editButton.click(function () {
            _this.edit(data);
        });
        deleteButton.click(function () {
            _this.remove(data.ID);
        });
        pasteLinkButton.click(function () {
            _this.pasteAsLink(data);
        });
       
        $(".Name", item).text(data.Name);
    }

    this.load = function () {
        ajaxData = {
            page: page
        }
        $.ajax({
            type: "POST",
            url: _this.AjaxLoadFileList,
            data: ajaxData,

            success: function (data) {
                if (data.result == "ok") {
                    $.each(data.data, function (i, item) {
                        _this.create(item);
                    });
                    page++;
                    if (page >= data.pageCount) {
                        $("#LoadFiles").parent().hide();
                    }
                }
                if (data.result == "error") {
                    _this.showErrors(data.errors);
                }
            },

            error: function () {
                alert("Внутренняя ошибка");
            }
        });
    }

    this.pasteAsImage = function (data) {
        $('.InputContent').tinymce().execCommand('mceInsertContent', true, '<img src="' + data.FullPath + '" alt="' + data.Name + '">');
    }

    this.pasteAsLink = function (big_data) {
        ajaxData = {
            mimeType: big_data.MimeType
        }
        $.ajax({
            type: "POST",
            url: _this.AjaxLoadSmallMimeType,
            data: ajaxData,
            success: function (data) {
                _this.continuePasteLink(big_data, data.small);
            },

            error: function () {
                _this.continuePasteLink(big_data, _this.defaultMimeSmallImage);
            }
        });
    }

    this.continuePasteLink = function (data, icon) {
        $('.InputContent').tinymce().execCommand('mceInsertContent', true, '<a href="' + data.FullPath + '" target="_blank"><img src="' + icon + '" alt="' + data.Name + '"/>' + data.Name + '(Скачать)</a>');
    }
    this.clearErrors = function () {
        $(".modal_error").empty();
    }

    this.createError = function (item) {
        var obj = $("#modal_errorFiles"+ item.PropertyName, _this.dialogInstance());
        $("<div>").attr("class", "line err").text(item.Message).appendTo(obj);

    }

    this.showErrors = function (errors) {
        errorDialog.empty();
        $.each(errors, function (i, item) {
            $("<div>").attr("class", "line err").text(item.Message).appendTo(errorDialog);
        });
        errorDialog.dialog("open");
    }


    this.buttonEnable = function (value) {
        if (value) {
            $("#editDialoglFile").removeAttr("disabled");
            $("#cancelDialogFile").removeAttr("disabled");
        } else {
            $("#editDialoglFile").attr("disabled", true);
            $("#cancelDialogFile").attr("disabled", true);
        }
    }
}

var thumbnails;
$().ready(function () {
    thumbnails = new Thumbnails();
    thumbnails.init();
});