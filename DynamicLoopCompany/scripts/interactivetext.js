$(document).ready(function () {
    $('.main-content .highlight').mouseover(function (e) {
        var popUp = $("#p" + this.id + ":hidden");
        if (popUp.length == 0)
            return;
        popUp.show();
        if (e.pageX > 200) {
            popUp.css("left", (e.pageX - 200) + "px");
        }
        else {
            popUp.css("left", "0px");
        }
        popUp.css("top", (e.pageY + 20) + "px");
    });
    
    $('.main-content .highlight').mouseout(function (e) {
        var popUp = $("#p" + this.id);
        popUp.hide();
    });
});