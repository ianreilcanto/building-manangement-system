$(document).ready(function () {
    //For svg hover
    $("svg polygon").tooltip({
        'container': 'body',
        'placement': 'auto'
    });

    $('g').each(function () {

        var group = this;
        $polygon = $(this).children("polygon");

        $polygon.hover(function () {
            var aptId = $(group).attr("apt-id");
            if (aptId > 0) {
                var name = $("#Name" + aptId).val();
                var size = $("#Size" + aptId).val();
                var noOfRooms = $("#NoOfRooms" + aptId).val();
                var price = $("#Price" + aptId).val();

                var statusId = $("#Status" + aptId).val();
                var status = $("#StatusName" + statusId).val();
                var color = $("#StatusColor" + statusId).val();
                var withPdf = $("#hasPdf" + aptId).val();

                var statusPdf = withPdf == "True" ? "Yes" : "No";

                var tooltipMessage = "<table width=\"100%\"><tr><td>Name:</td><td>" + name
                + "</td></tr><tr><td>Status:</td><td>" + status
                + "</td></tr><tr><td>Size:</td><td>" + size
                + "</td></tr><tr><td>No. of Rooms:</td><td>" + noOfRooms
                + "</td></tr><tr><td>Price:</td><td>" + parseFloat(price).toFixed(2).replace(/\B(?=(\d{3})+(?!\d))/g, ",") + "</td></tr>"
                + "<tr><td>With PDF:</td><td>" + statusPdf + "</td></tr>"
                + "</table>";

                $(group).children("polygon").attr("data-original-title", tooltipMessage)
                .attr("class", "apartment status-" + statusId)
                .tooltip("fixTitle")
                .tooltip("show");
            } else {
                $(group).children("polygon").attr("style", "display: none")
            }

        }, function () {
            $(this).tooltip("hide");
        });

        $(this).click(function () {
            DownloadPDF(this);
        });
    });


});

function DownloadPDF(thisValue) {
    // var apartmentId = event.target.id;
    var aptId = $(thisValue).attr("apt-id");
    $.ajax({

        url: "/BMBuilding/DownloadApartmentPDF/" + aptId,
        dataType: 'json',
        contentType: 'application/json',
        type: "POST"

    }).then(function (responseData) {
        if (responseData.pdf != null) {          
            window.open(responseData.pdf,"_blank");
            
        }
        else {
            //alert(responseData.message);
        }
    });
}