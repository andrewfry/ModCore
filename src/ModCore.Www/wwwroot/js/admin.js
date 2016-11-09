$(document).ready(function () {
    RegisterMenuEvents();
    RegisterThemeEvents();
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
    $(".menu-extend-icon").click(function (e) {
        if ($(".sidebar-main").hasClass("extended") || $(".body-wrapper").hasClass("extended")) {
            $(".sidebar-main").removeClass("extended");
            $(".body-wrapper").removeClass("extended");
        }
        else {
            $(".sidebar-main").addClass("extended");
            $(".body-wrapper").addClass("extended");
        }
    })
    $("#btnUserNavMenu").click(function (e) {
        e.preventDefault();
        $(".user-nav").toggleClass("extended");
        $(this).toggleClass("extended");
    })

}

function RegisterThemeEvents(){
    $(".btn-select-theme").click(function (e) {
        e.preventDefault();
        var theme = $(this).data("theme");
       
        $.ajax({
            url: 'Theme/SetTheme',
            type: 'POST',
            data: { themeName: theme },
            success: function (result) {           
                 $("#ThemeList").html(result.html);
                 RegisterThemeEvents();
            }
        });
    })
    
}