$(function () {
    let searchbarContainer = $(".searchbar-container");

    searchbarContainer.on("click", function (e) {
        let currentTarget = $(e.currentTarget);
        currentTarget.addClass("blue-outline");
    });

    searchbarContainer.on("focusout", function (e) {
        let currentTarget = e.currentTarget;
        $(currentTarget).removeClass("blue-outline");
    });
})