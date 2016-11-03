$(document).ready(function () {
    RegisterMenuEvents();
});



//function setupMenu() {
//    $(".menu-item ").click(function () {
//        if ($(this).hasClass("has-sub") === false) {
//            return;
//        }

//        if ($(this).hasClass("open")) {

//            $(this).removeClass("open");
//            $(this).children(".menu-group").slideUp(175);
//        }
//        else {
//            $(this).addClass("open");
//            $(this).children(".menu-group").slideDown(175);
//        }

//    });
//}

function RegisterMenuEvents() {
    //$(".menu-extend-icon").click(function (e) {
    //    if ($(".sidebar-main").hasClass("extended") || $(".body-wrapper").hasClass("extended")) {
    //        $(".sidebar-main").removeClass("extended");
    //        $(".body-wrapper").removeClass("extended");
    //    }
    //    else {
    //        $(".sidebar-main").addClass("extended");
    //        $(".body-wrapper").addClass("extended");
    //    }
    //})
    //$("#btnUserNavMenu").click(function (e) {
    //    e.preventDefault();
    //    $(".user-nav").toggleClass("extended");
    //    $(this).toggleClass("extended");
    //})

    $(".menu-item ").click(function () {
        if ($(this).hasClass("has-sub") == false) {
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

