function Console() {
    this.log = function (message) {
    };
}

var console = new Console();

function insertParam(key, value) {
    key = escape(key);
    value = escape(value);

    var kvp = document.location.search.substr(1).split('&');

    var i = kvp.length;
    var x;
    while (i--) {
        x = kvp[i].split('=');

        if (x[0] == key) {
            x[1] = value;
            kvp[i] = x.join('=');
            break;
        }
    }

    if (i < 0) { kvp[kvp.length] = [key, value].join('='); }

    //this will reload the page, it's likely better to store this until finished
    document.location.search = kvp.join('&');
}

function removeParam(key) {
    key = escape(key);

    var kvp = document.location.search.substr(1).split('&');

    var i = kvp.length;
    var x;
    while (i--) {
        x = kvp[i].split('=');

        if (x[0] == key) {
            kvp.splice(i, 1);
            break;
        }
    }

    //this will reload the page, it's likely better to store this until finished
    document.location.search = kvp.join('&');
}

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

function InitTinyMCE(item) {
    var _this = this;

    tinymce.init({
        selector: item,
        plugins: [
        "advlist autolink lists link image charmap print preview anchor",
        "searchreplace visualblocks code fullscreen",
        "insertdatetime media table contextmenu paste"
        ],
        toolbar: "insertfile undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image",

        // Example content CSS (should be your site CSS)
        content_css: "/Media/css/tiny.css"
    });
}

function InitUpload(item, multiple, url, oncomplete, extensions, title) {
    if (extensions == null) {
        extensions = [];
    }
    if (typeof (qq) == 'undefined') {
        LoadCssDynamically("/Media/css/fineuploader.css");
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


function Common() 
{
    var _this = this;

    this.init = function () {
        $(document).on('click', '.delete-action', function () {
            return confirm("Вы действительно хотите удалить?");
        });

        InitTinyMCE(".htmltext");

        $('.datepicker').datepicker();
        if ($.fn.colorpicker) {
            $('.colorpicker').colorpicker();
        }

    };
}

var common;
$().ready(function () {
    common = new Common();
    common.init();
});