let gm_map;
// Initialize and add the map
function initMap() {

	const options_googlemaps = {
		minZoom: 4,
		zoom: 10,
		center: new google.maps.LatLng(42.654331, 23.353332),
		maxZoom: 20,
		mapTypeId: google.maps.MapTypeId.ROADMAP,
		streetViewControl: false,
		styles: [
			{ elementType: 'geometry', stylers: [{ color: '#242f3e' }] },
			{ elementType: 'labels.text.stroke', stylers: [{ color: '#242f3e' }] },
			{ elementType: 'labels.text.fill', stylers: [{ color: '#746855' }] },
			{
				featureType: 'administrative.locality',
				elementType: 'labels.text.fill',
				stylers: [{ color: '#d59563' }]
			},
			{
				featureType: 'poi',
				elementType: 'labels.text.fill',
				stylers: [{ color: '#d59563' }]
			},
			{
				featureType: 'poi.park',
				elementType: 'geometry',
				stylers: [{ color: '#263c3f' }]
			},
			{
				featureType: 'poi.park',
				elementType: 'labels.text.fill',
				stylers: [{ color: '#6b9a76' }]
			},
			{
				featureType: 'road',
				elementType: 'geometry',
				stylers: [{ color: '#38414e' }]
			},
			{
				featureType: 'road',
				elementType: 'geometry.stroke',
				stylers: [{ color: '#212a37' }]
			},
			{
				featureType: 'road',
				elementType: 'labels.text.fill',
				stylers: [{ color: '#9ca5b3' }]
			},
			{
				featureType: 'road.highway',
				elementType: 'geometry',
				stylers: [{ color: '#746855' }]
			},
			{
				featureType: 'road.highway',
				elementType: 'geometry.stroke',
				stylers: [{ color: '#1f2835' }]
			},
			{
				featureType: 'road.highway',
				elementType: 'labels.text.fill',
				stylers: [{ color: '#f3d19c' }]
			},
			{
				featureType: 'transit',
				elementType: 'geometry',
				stylers: [{ color: '#2f3948' }]
			},
			{
				featureType: 'transit.station',
				elementType: 'labels.text.fill',
				stylers: [{ color: '#d59563' }]
			},
			{
				featureType: 'poi.business',
				stylers: [{ visibility: 'off' }]
			},
			{
				featureType: 'transit',
				elementType: 'labels.icon',
				stylers: [{ visibility: 'off' }]
			},
			{
				featureType: 'water',
				elementType: 'geometry',
				stylers: [{ color: '#17263c' }]
			},
			{
				featureType: 'water',
				elementType: 'labels.text.fill',
				stylers: [{ color: '#515c6d' }]
			},
			{
				featureType: 'water',
				elementType: 'labels.text.stroke',
				stylers: [{ color: '#17263c' }]
			}
		]
	}

	gm_map = new google.maps.Map(document.getElementById('user-sensors-map'), options_googlemaps)

	let markers = [];
	let infoWindows = [];

	$.getJSON("/Home/GetUserSensorCoordinates", function (data) {
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
			let userIdInput = document.getElementById('current-user-id');
			let loggedInUserId;
			if (userIdInput != null) {
				loggedInUserId = userIdInput.value;
			}
			let isCurrentUserSensor = loggedInUserId == data[i].userId;
			if (isCurrentUserSensor) {
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
					contentString += '<p>Status: <span class="h6 text-danger"> Private <i class="fas fa-user-secret"></i></span></p>';
				} else {
					markers[i].setIcon('/images/grn-pushpin.png');
					contentString += '<p>Status: <span class="h6 text-success"> Public <i class="fas fa-globe-americas"></i></span></p>';
				}
			}
			else {
				contentString += '<p>Status: <span class="h6 text-success"> Public <i class="fas fa-globe-americas"></i></span></p>';
			}

			infoWindows[i] = new google.maps.InfoWindow({
				content: contentString
			});

			markers[i].addListener('mouseover', function () {
				infoWindows[i].open(gm_map, markers[i]);
			});

			markers[i].addListener('mouseout', function () {
				infoWindows[i].close();
			});
		}

		let markerCluster = new MarkerClusterer(gm_map, markers);
	});
}