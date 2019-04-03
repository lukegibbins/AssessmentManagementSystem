$(document).ready(function () {
    history.pushState({ page: 1 }, "Results", "#nbb");
    window.onhashchange = function (event) {
        window.location.hash = "nbb";
    };
});
