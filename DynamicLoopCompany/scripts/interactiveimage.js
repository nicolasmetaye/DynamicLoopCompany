$(document).ready(function () {
    $('.about-drawing-container .about-map-area').mouseover(function (e) {
        var popUp = $("#popup" + this.id.substring(4));
        popUp.show();
    });

    $('.about-drawing-container .about-map-area').mouseout(function (e) {
        var popUp = $("#popup" + this.id.substring(4));
        popUp.hide();
    });
});