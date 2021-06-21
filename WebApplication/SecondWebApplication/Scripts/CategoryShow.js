$(document).ready(function () {
    var page_number = $("#index_data").attr("data-page");
    var categ_id = $("#index_data").attr("data-categid");
    var sortparam = $("#index_data").attr("data-sortparam");
    var sortdirection = $("#index_data").attr("data-sortdirection");


    $("#sort_title_btn img").attr("src", stateToSrc($("#sort_title_btn img").attr("data-state")));
    $("#sort_user_btn img").attr("src", stateToSrc($("#sort_user_btn img").attr("data-state")));

    $("#sort_title_btn").click(function () {
        var state = $(this).find("img").attr("data-state");
        if (state === "") {
            $(this).find("img").attr("src", "/Images/sort-up.png");
            $(this).find("img").attr("data-state", "asc");
            sortparam = "name";
            sortdirection = "asc";

            // Clear the other sort filter
            $("#sort_user_btn img").attr("data-state", "");
            $("#sort_user_btn img").attr("src", "");
        } else if (state === "asc") {
            $(this).find("img").attr("src", "/Images/sort-down.png");
            $(this).find("img").attr("data-state", "desc");
            sortdirection = "desc";
        } else {
            $(this).find("img").attr("src", "");
            $(this).find("img").attr("data-state", "");
            sortparam = "";
            sortdirection = "";
        }

        refreshPage();
    });

    $("#sort_user_btn").click(function () {
        var state = $(this).find("img").attr("data-state");
        if (state === "") {
            $(this).find("img").attr("src", "/Images/sort-up.png");
            $(this).find("img").attr("data-state", "asc");
            sortparam = "creator";
            sortdirection = "asc";

            // Clear the other sort filter
            $("#sort_title_btn img").attr("data-state", "");
            $("#sort_title_btn img").attr("src", "");
        } else if (state === "asc") {
            $(this).find("img").attr("src", "/Images/sort-down.png");
            $(this).find("img").attr("data-state", "desc");
            sortdirection = "desc";
        } else {
            $(this).find("img").attr("src", "");
            $(this).find("img").attr("data-state", "");
            sortparam = "";
            sortdirection = "";
        }

        refreshPage();
    });

    function refreshPage() {
        console.log(sortparam, sortdirection);
        location.href = '/Category/Show/' + categ_id + '/?page=' + page_number + '&sortparam=' + sortparam + '&sortdirection=' + sortdirection;
    }
});


function stateToSrc(state) {
    if (state == "") return "";
    if (state == "asc") return "/Images/sort-up.png";
    return "/Images/sort-down.png";
}