$(function () {
    $('#SideMenuToggle').click(function () {
        $('.side-menu-container .navbar-nav').toggleClass('slide-in');
        $('.side-body').toggleClass('body-slide-in');
    });
});