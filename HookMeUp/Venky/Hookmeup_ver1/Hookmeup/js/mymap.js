var map;
var myCenter = new google.maps.LatLng(40.712915, -74.007511);
var bakhra1 = new google.maps.LatLng(40.71111, -74.007811);
var bakhra2 = new google.maps.LatLng(40.71200, -74.007000);

function initialize() {
    var mapProp = {
        center: myCenter,
        zoom: 17,
        mapTypeId: google.maps.MapTypeId.ROADMAP,
        panControl: false,
        zoomControl: false,
        mapTypeControl: false,
        scaleControl: false,
        streetViewControl: false,
        overviewMapControl: false,
        rotateControl: false,
        scrollwheel: false,
        draggable: false,
        disableDoubleClickZoom: true
    };

    map = new google.maps.Map(document.getElementById("googleMap"), mapProp);

    var homemarker = new google.maps.Marker({
        position: myCenter,
        animation: google.maps.Animation.DROP,
        icon: 'css/img/small-pushpin-flag.png'
    });

    homemarker.setMap(map);

    var infowindow1 = new google.maps.InfoWindow({
        content: "<div data-theme=\"b\">\"This is you!\"</div>"
    });

    google.maps.event.addListener(homemarker, 'click', function () {
        infowindow1.open(map, homemarker);
    });

    var bakhra1marker = new google.maps.Marker({
        position: bakhra1,
        icon: 'css/img/green-round-pushpin.png'
    });

    bakhra1marker.setMap(map);
    /*
    var infowindow2 = new google.maps.InfoWindow({
        content: "Hello Bakhra1!"
    });
    */

    google.maps.event.addListener(bakhra1marker, 'click', function () {
        //infowindow2.open(map, bakhra1marker);
        $.mobile.changePage("#IM");
    });

    var bakhra2marker = new google.maps.Marker({
        position: bakhra2,
        icon: 'css/img/orange-round-pushpin.png'
    });

    bakhra2marker.setMap(map);

    var infowindow3 = new google.maps.InfoWindow({
        content: "Hello Bakhra2!"
    });

    google.maps.event.addListener(bakhra2marker, 'click', function () {
        infowindow3.open(map, bakhra2marker);
    });

    /*
    google.maps.event.addListener(map, 'click', function (event) {
        placeMarker(event.latLng);
    });
}

function placeMarker(location) {
    var marker = new google.maps.Marker({
        position: location,
        map: map,
    });
    var infowindow = new google.maps.InfoWindow({
        content: 'Latitude: ' + location.lat() + '<br>Longitude: ' + location.lng()
    });
    infowindow.open(map, marker);
    */
}

google.maps.event.addDomListener(window, 'load', initialize);

