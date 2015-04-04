function AboutUs() {

    this.init = function () {

        $('.query-gallery').queryGallery({
            autoScroll: true,
            autoScrollDelay: 15000
        });

        $("#buttonNext").click(function () {
            window.location = "/join";
        });
    };
}

var aboutUs = null;
$().ready(function () {
    aboutUs = new AboutUs();
    aboutUs.init();
});