$().ready(function () {
	var minValue = $('#SensorMinRangeValue').val();
	var maxValue = $('#SensorMaxRangeValue').val();

	$('#CustomMinRangeValue').ionRangeSlider({
		skin: "round",
		min: minValue,
		max: maxValue,
		from: minValue,
		type: 'single',
		step: 0.5,
		postfix: ' sec',
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
		min: minValue,
		max: maxValue,
		from: maxValue,
		type: 'single',
		step: 0.5,
		postfix: ' sec',
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

	function validateMinSliderValues(obj) {
		// obj is minSlider
		maxSlider = $("#CustomMaxRangeValue").data("ionRangeSlider");

		let isValid = true;
		let callUpdate = $("#max-range-validation-message").attr("hidden") === undefined;
		let validationMessage;

		if (obj.from > maxSlider.result.from) {
			isValid = false;
			validationMessage = "Value should be less than " + maxSlider.result.from;
		}

		if (!isValid) {
			$("#min-range-validation-message").removeAttr("hidden");
			$("#min-range-validation-message").text(validationMessage);
			$("#submit-create-button").attr("disabled", true);
		}
		else {
			$("#min-range-validation-message").attr("hidden", true);
			$("#submit-create-button").removeAttr("disabled");

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
			validationMessage = "Value should be more than " + minSlider.result.from;
		}

		if (!isValid) {
			$("#max-range-validation-message").removeAttr("hidden");
			$("#max-range-validation-message").text(validationMessage);
			$("#submit-create-button").attr("disabled", true);
		}
		else {
			$("#max-range-validation-message").attr("hidden", true);
			$("#submit-create-button").removeAttr("disabled");

			if (callUpdate) {
				minSlider.update();
			}
		}
	}
});