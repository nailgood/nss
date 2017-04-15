$(window).load(function () {
     fnLoadCharacter();
});
var lchars;
$('#ctl00_ph_txtPageTitle').keyup(function (e) {
    lchars = $(this).val().length;
    $("#rqtitle").text(lchars);

});
$('#ctl00_ph_txtMetaDescription').keyup(function (e) {
    lchars = $(this).val().length;
    $("#rqdesc").text(lchars);

});
$('#ctl00_ph_txtMetaTitle').keyup(function (e) {
    lchars = $(this).val().length;
    $("#rqmetatitle").text(lchars);

});
fnLoadCharacter = function () {
        if ($('#ctl00_ph_txtPageTitle').val() != undefined) {
            lchars = $('#ctl00_ph_txtPageTitle').val().length;
            $("#rqtitle").text(lchars);
        }
        if ($('#ctl00_ph_txtMetaDescription').val() != undefined) {
            lchars = $('#ctl00_ph_txtMetaDescription').val().length;
            $("#rqdesc").text(lchars);
        }
        if ($('#ctl00_ph_txtMetaTitle').val() != undefined) {
            lchars = $('#ctl00_ph_txtMetaTitle').val().length;
            $("#rqmetatitle").text(lchars);
        }
}