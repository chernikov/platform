function Console() {
    this.log = function (message) {
    };
}

var console = new Console();

function LoadCssDynamically(fileName) {
    var fileref = $('<link>');
    fileref.attr("rel", "stylesheet");
    fileref.attr("type", "text/css");
    fileref.attr("href", fileName);
    $("head").append(fileref);
}

function LoadJsDynamically(fileName) {
    var fileref = $('<script>');
    fileref.attr("type", "text/javascript");
    fileref.attr("src", fileName);

    $("head").append(fileref);
}

function InitUpload(item, multiple, url, oncomplete, extensions, title) {
    if (extensions == null) {
        extensions = [];
    }
    if (typeof (qq) == 'undefined') {
        LoadCssDynamically("/Media/css/fineuploader_default.css");
        LoadJsDynamically("/Media/scripts/jquery.fineuploader-3.0.js");
    }

    var obj = new qq.FineUploader({
        element: item,
        multiple: multiple,
        request: {
            endpoint: url,
        },
        text: {
            uploadButton: title
        },
        callbacks: {
            onComplete: oncomplete,
        },
        validation: {
            allowedExtensions: extensions
        }
    });
}


function UpdateOnline() {
    SendUpdateOnline();
}

function SendUpdateOnline() {
    $.getJSON("/online", function (data) { });
    setTimeout(UpdateOnline, 60000);
}

function Common() {
    var _this = this;
    this.init = function ()
    {
        if ($('.dropdown-styled').length > 0) {
            _this.StyleDropDown($('.dropdown-styled'));
        }
        Cufon.set('fontFamily', 'MyMyriad').replace('.myriad');

        $(".user-logo").click(function () {
            alert("as");
        });
    };

    this.StyleDropDown = function (selector) {
        if ($.fn.querySelect == undefined) {
            LoadJsDynamically("/Media/scripts/default/styled-select-list.js");
        }
        selector.querySelect({
            padding: '0 30px 0 10px'
        });

    }
}

var common = null;
$().ready(function () {
    common = new Common();
    common.init();
    SendUpdateOnline();
});