$(document).ready(function () {
    $("svg polygon").each(function () {
        $(this).attr("data-html", "true")
            .attr("data-toggle", "tooltip")
            .attr("class", "apartment");
    });
});