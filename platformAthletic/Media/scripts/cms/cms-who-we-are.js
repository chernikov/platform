function CmsWhoWeAre() {
    var _this = this;

    this.init = function () {
        tinymce.init({
            selector: "div.editable",
            inline: true,
            plugins: [
                "advlist autolink lists link image charmap print preview anchor",
                "searchreplace visualblocks code fullscreen",
                "insertdatetime media table contextmenu paste"
            ],
            toolbar: "insertfile undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image"
        });

        $(".CancelBtn").click(function () {
            window.location.reload();
            return false;
        });
    }

}

var cmsWhoWeAre = null;
$(function () {
    cmsWhoWeAre = new CmsWhoWeAre();
    cmsWhoWeAre.init();
});