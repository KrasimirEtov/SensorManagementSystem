$().ready(function () {
	const reloadTable = function () {
		let measureType = $("#MeasureType").val();
		let isPublic = $("#IsPublic").val();
		let isAlarmOn = $("#IsAlarmOn").val();
		let searchTerm = $("#search-filter-input").val();
		var pageIndex = $("#PageIndex").val();
		var requestUrl = '/UserSensor/ReloadUserSensorsTable';

		requestUrl += '?pageIndex=' + pageIndex;

		if (measureType != 'null' && measureType != undefined) {
			requestUrl += '&measureType=' + measureType;
		}
		if (isPublic != 'null' && isPublic != undefined) {
			requestUrl += '&isPublic=' + isPublic;
		}
		if (isAlarmOn != 'null' && isAlarmOn != undefined) {
			requestUrl += '&isAlarmOn=' + isAlarmOn;
		}
		if (searchTerm != '' && searchTerm != undefined) {
			requestUrl += '&searchTerm=' + searchTerm;
		}

		$.ajax({
			type: "GET",
			url: requestUrl,
			dataType: "html",
			success: function (response) {
				$("#user-sensor-cards").html(response);
				var hasPreviousPage = $("#HasPreviousPage").val().toLowerCase() === 'true';
				var hasNextPage = $("#HasNextPage").val().toLowerCase() === 'true';
				if (hasPreviousPage) {
					$("#previous-page-button").removeAttr("disabled");
				}
				else {
					$("#previous-page-button").attr("disabled", true);
				}
				if (hasNextPage) {
					$("#next-page-button").removeAttr("disabled");
				}
				else {
					$("#next-page-button").attr("disabled", true);
				}
				handleGaugeUpdate();
			}
		});
	}

	$("#MeasureType").change(reloadTable);
	$("#IsPublic").change(reloadTable);
	$("#IsAlarmOn").change(reloadTable);
	$("#search-filter-input").change(reloadTable);

	$("#search-filter-input").keyup(function (event) {
		if (event.keyCode === 13) {
			reloadTable();
		}
	});

	$("#search-filter-button").click(reloadTable);
	$("#previous-page-button").click(function () {
		let pageIndex = $("#PageIndex").val();
		$("#PageIndex").val(parseInt(pageIndex) - 1);
		reloadTable();
	});
	$("#next-page-button").click(function () {
		let pageIndex = $("#PageIndex").val();
		$("#PageIndex").val(parseInt(pageIndex) + 1);
		reloadTable();
	});

	function handleGaugeUpdate() {
		function getNonSwitchGaugeOptions(currentValue, minRange, maxRange) {
			return {
				value: currentValue,
				min: minRange,
				max: maxRange,
				decimals: 2,
				gaugeWidthScale: 0.1,
				pointer: true,
				pointerOptions: {
					toplength: 3,
					bottomlength: -20,
					bottomwidth: 5,
					color: '#8e8e93'
				},
				levelColors: [
					"#FF0000",
					"#00b200",
					"#FF0000"
				],
				textRenderer: function (data) {
					if (data >= parseFloat(minRange) && data <= parseFloat(maxRange)) {
						return parseFloat(data);
					} else if (data <= parseFloat(minRange)) {
						return 'Alarm! ' + data;
					} else if (data >= parseFloat(maxRange)) {
						return 'Alarm! ' + data; // fire alarm
					}
					else {
						return 'fetching';
					}
				},
				counter: true,
				relativeGaugeSize: true
			};
		}

		function getSwitchGaugeOptions(currentValue) {
			return {
				value: currentValue,
				gaugeWidthScale: 0.5,
				donut: true,
				min: 0,
				max: 1,
				relativeGaugeSize: true,
				gaugeColor: "#FF0000", // false
				levelColors: ["#00b200"], // true
				textRenderer: function (data) {
					if (data == '0') {
						return 'Off';
					} else if (data == '1') {
						return 'On';
					} else {
						return 'fetch';
					}
				}
			};
		}

		var firstGaugeInstance;
		var secondGaugeInstance;
		var thirdGaugeInstance;

		function initGaugeInstance(gaugeId, gaugeOptions) {
			return new JustGage({
				id: gaugeId,
				defaults: gaugeOptions
			});
		}

		function initGaugeOptions(isSwitch, value, minRange, maxRange) {
			if (isSwitch === 'true') {
				return getSwitchGaugeOptions(value);
			} else {
				return getNonSwitchGaugeOptions(value, minRange, maxRange);
			}
		}

		if (parseInt($("#SensorsCount").val()) === 1) {
			firstGaugeInstance = initGaugeInstance('gauge0', initGaugeOptions($("#FirstSensorIsSwitch").val().toLowerCase(), $("#FirstSensorValue").val(), $("#FirstSensorMinRange").val(), $("#FirstSensorMaxRange").val()));
			firstGaugeInstance.refresh($("#FirstSensorValue").val());
		} else if (parseInt($("#SensorsCount").val()) === 2) {
			firstGaugeInstance = initGaugeInstance('gauge0', initGaugeOptions($("#FirstSensorIsSwitch").val().toLowerCase(), $("#FirstSensorValue").val(), $("#FirstSensorMinRange").val(), $("#FirstSensorMaxRange").val()));
			secondGaugeInstance = initGaugeInstance('gauge1', initGaugeOptions($("#SecondSensorIsSwitch").val().toLowerCase(), $("#SecondSensorValue").val(), $("#SecondSensorMinRange").val(), $("#SecondSensorMaxRange").val()));
			firstGaugeInstance.refresh($("#FirstSensorValue").val());
			secondGaugeInstance.refresh($("#SecondSensorValue").val());
		} else if (parseInt($("#SensorsCount").val()) === 3) {
			firstGaugeInstance = initGaugeInstance('gauge0', initGaugeOptions($("#FirstSensorIsSwitch").val().toLowerCase(), $("#FirstSensorValue").val(), $("#FirstSensorMinRange").val(), $("#FirstSensorMaxRange").val()));
			secondGaugeInstance = initGaugeInstance('gauge1', initGaugeOptions($("#SecondSensorIsSwitch").val().toLowerCase(), $("#SecondSensorValue").val(), $("#SecondSensorMinRange").val(), $("#SecondSensorMaxRange").val()));
			secondGaugeInstance = initGaugeInstance('gauge2', initGaugeOptions($("#ThirdSensorIsSwitch").val().toLowerCase(), $("#ThirdSensorValue").val(), $("#ThirdSensorMinRange").val(), $("#ThirdSensorMaxRange").val()));
			firstGaugeInstance.refresh($("#FirstSensorValue").val());
			secondGaugeInstance.refresh($("#SecondSensorValue").val());
			thirdGaugeInstance.refresh($("#ThirdSensorValue").val());
		}

		function refreshGaugeBarInstance(instance, sensorId, sensorPollingInterval) {
			setInterval(function () {
				$.getJSON('/UserSensor/GetGaugeData?userSensorId=' + sensorId, function (data) {
					if (instance.originalValue != data.value) {
						instance.refresh(data.value);
						sendToastrAlertIfNeeded(data.name, data.value, data.minRange, data.maxRange);
					}
				});
			}, sensorPollingInterval * 1000);
		}

		if (parseInt($("#SensorsCount").val()) === 1) {
			refreshGaugeBarInstance(firstGaugeInstance, parseInt($("#FirstSensorId").val()), parseInt($("#FirstSensorPollingInterval").val()));
		} else if (parseInt($("#SensorsCount").val()) === 2) {
			refreshGaugeBarInstance(firstGaugeInstance, parseInt($("#FirstSensorId").val()), parseInt($("#FirstSensorPollingInterval").val()));
			refreshGaugeBarInstance(secondGaugeInstance, parseInt($("#SecondSensorId").val()), parseInt($("#SecondSensorPollingInterval").val()));
		} else if (parseInt($("#SensorsCount").val()) === 3) {
			refreshGaugeBarInstance(firstGaugeInstance, parseInt($("#FirstSensorId").val()), parseInt($("#FirstSensorPollingInterval").val()));
			refreshGaugeBarInstance(secondGaugeInstance, parseInt($("#SecondSensorId").val()), parseInt($("#SecondSensorPollingInterval").val()));
			refreshGaugeBarInstance(thirdGaugeInstance, parseInt($("#ThirdSensorId").val()), parseInt($("#ThirdSensorPollingInterval").val()));
		}

		function sendToastrAlertIfNeeded(sensorName, value, minRange, maxRange) {
			if (maxRange != null && value > maxRange) {
				toastr.info('Activated alarm for sensor: ' + sensorName + ' because value is above max range.');
			} else if (minRange != null && value < minRange) {
				toastr.info('Activated alarm for sensor: ' + sensorName + ' because value is below min range.');
			}
		}
	}
});