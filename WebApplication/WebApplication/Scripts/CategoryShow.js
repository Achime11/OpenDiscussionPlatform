$(document).ready(function () {

    $("#sort_title_btn, #sort_user_btn").click(function () {
        var state = $(this).find("img").attr("data-state");
        if (state === "down") {
            $(this).find("img").attr("src", "/Images/sort-up.png");
            $(this).find("img").attr("data-state", "up");
        } else {
            $(this).find("img").attr("src", "/Images/sort-down.png");
            $(this).find("img").attr("data-state", "down");
        }
    });
});