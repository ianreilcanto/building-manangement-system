$(document).ready(function () {
    $('#list').click(function (event) {
        event.preventDefault();
        $('#buildings .item').addClass('list-group-item');
        $('.thumbnail .list-group-image').addClass('5u');
        $('.thumbnail .caption').addClass('7u');
        $('.thumbnail').css("min-height", "250px");
        $('#list').addClass('active');
        $('#grid').removeClass('active');
    });
    $('#grid').click(function (event) {
        event.preventDefault(); $('#buildings .item').removeClass('list-group-item');
        $('#buildings .item').addClass('grid-group-item');
        $('.thumbnail .list-group-image').removeClass('5u');
        $('.thumbnail .caption').removeClass('7u');
        $('.thumbnail').css("min-height", "530px");
        $('#grid').addClass('active');
        $('#list').removeClass('active');
    });
});