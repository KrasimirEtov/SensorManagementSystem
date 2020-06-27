let gm_map;
// Initialize and add the map
function initMap() {

	const options_googlemaps = {
		minZoom: 4,
		zoom: 10,
		center: new google.maps.LatLng(42.654331, 23.353332),
		maxZoom: 16,
		mapTypeId: google.maps.MapTypeId.ROADMAP,
		streetViewControl: false
	}

	gm_map = new google.maps.Map(document.getElementById('sensor-map'), options_googlemaps)

	let markers = [];

	$.getJSON("/UserSensor/GetUserSensorCoordinates", function (data) {
		for (let i = 0; i < data.length; i++) {
			let latLng = new google.maps.LatLng(data[i].latitude,
				data[i].longitude);

			markers[i] = new google.maps.Marker({
				position: latLng,
				map: gm_map,
				animation: google.maps.Animation.DROP,
				icon: '/images/map-marker.png'
			});

			let formattedDate = data[i].createdOn.replace(/(\d{4})\-(\d{2})\-(\d{2}).*/, '$3-$2-$1');
			let contentString =
				'<p>Name: ' + '<span class="h6 text-primary">' + data[i].name + '</span></p>' +
				'<p>Created on: <span class="h6 text-danger">' + formattedDate + '</span></p>' +
				'<p>Sensor type: <span class="h6 text-warning">' + data[i].measureType + '</span></p>';

			markers[i].setAnimation(google.maps.Animation.BOUNCE);
			markers[i].addListener('click', function () {
				if (markers[i].getAnimation() !== null) {
					markers[i].setAnimation(null);
				} else {
					markers[i].setAnimation(google.maps.Animation.BOUNCE);
				}
			});

			if (!data[i].isPublic) {
				markers[i].setIcon('/images/red-pushpin.png');
				contentString += '<p>Status: <span class="h6 text-danger"> Private <i class="fas fa-user-secret"><i></span></p>';
			} else {
				markers[i].setIcon('/images/grn-pushpin.png');
				contentString += '<p>Status: <span class="h6 text-success"> Public <i class="fas fa-globe"></i></span></p>';
			}
			contentString += '<p class="h6"><a href="/UserSensor/Edit/' + data[i].id + '" class="badge badge-info">Edit Sensor</a></p>';



			let infowindow = new google.maps.InfoWindow({
				content: contentString
			});
			markers[i].addListener('click', function () {
				infowindow.open(gm_map, markers[i]);
			});
		}

		let markerCluster = new MarkerClusterer(gm_map, markers);
	});
}