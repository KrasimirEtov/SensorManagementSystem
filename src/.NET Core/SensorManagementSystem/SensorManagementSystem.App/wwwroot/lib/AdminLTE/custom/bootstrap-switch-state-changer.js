$().ready(function () {
	$("input[data-bootstrap-switch]").each(function (obj) {
		$(this).bootstrapSwitch('state', $(this).prop('checked'));
	});

	//$("#IsAlarmOn").data("bootstrapSwitch").click(function () {
	//	if ($("#IsAlarmOn").is(":checked")) {
	//		console.log('checked');
	//	}
	//});
	$("#IsAlarmOn").data("bootstrapSwitch").onSwitchChange(function (obj) {
		if (obj.currentTarget.checked) {
			$("#min-range-picker").removeAttr("hidden");
			$("#max-range-picker").removeAttr("hidden");
		} else {
			$("#min-range-picker").attr("hidden", true);
			$("#max-range-picker").attr("hidden", true);
			$("#submit-create-button").removeAttr("disabled");
		}
	});
});