function redirectWithDisclaimer(url, disclaimer) {
    var r = confirm(disclaimer);
    if (r == true) {
        window.location.href = url;
    }
}