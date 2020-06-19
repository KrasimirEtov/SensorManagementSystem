$().ready(function () {
	$("#sensor-form").on("submit", function (event) {
		if (!$("#sensor-form").valid()) {
			event.preventDefault();
			$("#validation-message-alert").removeAttr("hidden");
			return false;
		}
		else {
			return true;
		}
	});
});