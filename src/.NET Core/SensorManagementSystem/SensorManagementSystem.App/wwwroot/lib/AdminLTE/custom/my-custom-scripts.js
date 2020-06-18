// Submit logout form
$(document).ready(function () {
	$('#logout-link').click(function () {
		$('#logout-form').submit();
	});
});

window.onscroll = function () { scrollFunction(); };

function scrollFunction() {
    if (document.body.scrollTop > 20 || document.documentElement.scrollTop > 20) {
        document.getElementById("back-to-top").style.display = "block";
    } else {
        document.getElementById("back-to-top").style.display = "none";
    }
}

$(function () {
    $("#back-to-top").click(function () {
        $("html, body").animate({ scrollTop: 0 }, "medium");
        return false;
    });
});