$(() => {
	"use strict";

	const connection = new signalR.HubConnectionBuilder()
		.configureLogging(signalR.LogLevel.None)
		.withUrl("/sensorStoreHub")
		.build();

	connection.on("ReceiveAuthenticatedUsersMessage", function (message) {
		toastr.info(message);
	});

	connection.start();
});