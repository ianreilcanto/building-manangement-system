function displayModal() {
    var thumbnailId = event.target.id;
    var modal = document.getElementById(thumbnailId + "Modal");
    var img = document.getElementById(thumbnailId);
    var modalImg = modal.getElementsByTagName('img')[0];
    var captionText = document.getElementById("caption");

    modal.style.display = "block";
    modalImg.src = img.src;
    modalImg.alt = img.alt;
    captionText.innerHTML = img.alt;
    $('.lSAction').css("display", "none");
    $('.lSPager').css("display", "none");
    $('.homepage').css("overflow", "hidden");

    $.ajax({
        url: "/BMBuilding/BMBuildingGetSvgByMediaId",
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({ mediaId: thumbnailId, buildingId: $("#BuildingId").val() }),
        success: function (result) {
            $(".svg").prepend(result.svgData);

            $.getScript('/js/bmbuildingpage/setsvg.js', function () {
                $.getScript('/js/bmbuildingpage/svg.js', function () { });
            });
        }
    });

}

function closeModal() {
    var closeId = event.target.id;
    closeId = closeId.replace('Close', '');
    var modal = document.getElementById(closeId + "Modal");
    modal.style.display = "none";
    $('.lSAction').css("display", "block");
    $('.lSPager').css("display", "inline-block");
    $('.lSPager').css("text-align", "center");
    $('.lSSlideOuter').css("text-align", "center");
    $('.lSpg').css("text-align", "center");
    $('.homepage').css("overflow", "auto");
}