$(function () {
    var x = 0;
    var s = "";

    console.log("Hello Dutch Treat");

    var contactForm = $("#contactForm");
    contactForm.hide();

    var button = $("#buyButton");
    button.on("click", function () {
        console.log("Buying Item");
    });

    var productInfo = $(".productProps li");
    productInfo.on("click", function () {
        console.log("You clicked on " + $(this).text());
    });

    var $loginToggle = $("#loginToggle");
    var $popupForm = $(".popupForm");

    $loginToggle.on("click", function () {
        $popupForm.fadeToggle(1000);
    });
});