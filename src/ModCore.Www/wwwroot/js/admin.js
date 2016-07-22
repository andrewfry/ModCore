$(document).ready(function () {
    setupMenu();
});



function setupMenu() {
    $(".menu-item ").click(function () {
        if ($(this).hasClass("has-sub") === false) {
            return;
        }

        if ($(this).hasClass("open")) {

            $(this).removeClass("open");
            $(this).children(".menu-group").slideUp(175);
        }
        else {
            $(this).addClass("open");
            $(this).children(".menu-group").slideDown(175);
        }

    });
}