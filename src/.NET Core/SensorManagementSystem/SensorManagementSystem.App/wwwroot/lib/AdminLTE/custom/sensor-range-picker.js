$().ready(function () {
	var sensorMinValue = $('#SensorMinRangeValue').val();
	var sensorMaxValue = $('#SensorMaxRangeValue').val();
	var sensorPollingInterval = $('#SensorPollingInterval').val();

	var currentMinValue = $('#CustomMinRangeValue').val();
	var currentMaxValue = $('#CustomMaxRangeValue').val();
	var currentPollingInterval = $('#CustomPollingInterval').val();

	if (currentMinValue == "" && currentMaxValue == "") {
		currentMinValue = sensorMinValue;
		currentMaxValue = sensorMaxValue;
	}
	if (currentPollingInterval == 0) {
		currentPollingInterval = sensorPollingInterval;
	}

	$('#CustomMinRangeValue').ionRangeSlider({
		skin: "round",
		min: sensorMinValue,
		max: sensorMaxValue,
		from: currentMinValue,
		type: 'single',
		step: 0.5,
		postfix: ' ' + $("#MeasureUnit").val(),
		prettify: true,
		hasGrid: true,
		onChange: function (obj) {
			validateMinSliderValues(obj);
			$("#CustomMinRangeValue").val(obj.from);
		},
		onUpdate: function (obj) {
			validateMinSliderValues(obj);
		}
	});

	$('#CustomMaxRangeValue').ionRangeSlider({
		skin: "round",
		min: sensorMinValue,
		max: sensorMaxValue,
		from: currentMaxValue,
		type: 'single',
		step: 0.5,
		postfix: ' ' + $("#MeasureUnit").val(),
		prettify: true,
		hasGrid: true,
		onChange: function (obj) {
			validateMaxSliderValues(obj);
			$("#CustomMaxRangeValue").val(obj.from);
		},
		onUpdate: function (obj) {
			validateMaxSliderValues(obj);
		}
	});

	$('#CustomPollingInterval').ionRangeSlider({
		skin: "round",
		min: sensorPollingInterval,
		max: 300,
		from: currentPollingInterval,
		type: 'single',
		step: 1,
		postfix: ' sec',
		prettify: true,
		hasGrid: true,
		onChange: function (obj) {
			$("#CustomPollingInterval").val(obj.from);
		}
	});

	function validateMinSliderValues(obj) {
		// obj is minSlider
		maxSlider = $("#CustomMaxRangeValue").data("ionRangeSlider");

		let isValid = true;
		let callUpdate = $("#max-range-validation-message").attr("hidden") === undefined;
		let validationMessage;

		if (obj.from > maxSlider.result.from) {
			isValid = false;
			validationMessage = "Min Range should be less than " + maxSlider.result.from;
		}

		if (!isValid) {
			$("#min-range-validation-message").removeAttr("hidden");
			$("#min-range-validation-message").text(validationMessage);
			$("#submit-button").attr("disabled", true);
		}
		else {
			$("#min-range-validation-message").attr("hidden", true);
			$("#submit-button").removeAttr("disabled");

			if (callUpdate) {
				maxSlider.update();
			}
		}
	}

	function validateMaxSliderValues(obj) {
		// obj is maxSlider
		minSlider = $("#CustomMinRangeValue").data("ionRangeSlider");

		let isValid = true;
		let callUpdate = $("#min-range-validation-message").attr("hidden") === undefined;
		let validationMessage;

		if (obj.from < minSlider.result.from) {
			isValid = false;
			validationMessage = "Max Range should be more than " + minSlider.result.from;
		}

		if (!isValid) {
			$("#max-range-validation-message").removeAttr("hidden");
			$("#max-range-validation-message").text(validationMessage);
			$("#submit-button").attr("disabled", true);
		}
		else {
			$("#max-range-validation-message").attr("hidden", true);
			$("#submit-button").removeAttr("disabled");

			if (callUpdate) {
				minSlider.update();
			}
		}
	}
});