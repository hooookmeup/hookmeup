var myCenter;
//var minZoomLevel = 14;
var myPos;
var me;
//var phonoObject;

function setLatLong(userid, currentlat, currentlng) {
    console.log('inside setLatLong ' + currentlat + ' ' + currentlng);
    //myCenter = { 'center': currentlat + ',' + currentlng };
    me = userid;
    myCenter = new google.maps.LatLng(currentlat, currentlng);
    console.log("Before updateMap inside setLatLng");
    //updateMap();
    getProximityUsers(userid, currentlat, currentlng);
}

function getProximityUsers(user, currentlat, currentlng) {
    var usersService = new Services.Users();
    usersService.getProximity({
        data: { userId: user, latitude: currentlat, longitude: currentlng },
        onDone: function (users) {
            $.each(users, function () {
                updateMap(this);
            })
        }
    });
}
//Create the map then make 'displayDirections' request
$('#page-map').live("pageinit", function() {
    console.log("Inside Page Map INIT");
    //updateMap();
    //prepareUser();
});

function updateMap(obj) {
    console.log(myCenter);
    $('#map_canvas').gmap(
    		{
    		    'center': myCenter,
    		    'minZoom': 10,
    		    'maxZoom': 16,
    		    'mapTypeControl': false,
    		    'navigationControl': false,
    		    'streetViewControl': false,
    		    'mapTypeId': google.maps.MapTypeId.ROADMAP,
    		    'panControl': false,
    		    'zoomControl': true,
    		    'overviewMapControl': false,
    		    'rotateControl': false,
    		    'scrollwheel': false,
    		    'draggable': false,
    		    'disableDoubleClickZoom': true
    		})
    addMarkers(obj);
    //			.bind('init', function (evt, map) {
    //			    console.log("Inside Bind Init");
    //			   $('#map_canvas').gmap('addMarker',
    //                        {
    //                            'tags': 'myPos',
    //                            'position': myCenter, //map.getCenter(),
    //                            'animation': google.maps.Animation.DROP,
    //                            'bounds': true
    //                        })
    //                        .click(function () {
    //                            $.mobile.changePage($('#IM'), {});
    //                        });
    //			    console.log("Marker Added");
    //			    refreshMap();
    //			});
    //console.log("before calling RefreshMap inside updateMap");
    //refreshMap();
}

function addMarkers(obj) {
    console.log("Adding Marker : " + obj.userId + " and me is " + me);
    if (obj.userId == me) {
        console.log(" inside Clear for : " + obj.userId);
        //$('#map_canvas').gmap('find', 'markers', { 'property': 'tags', 'value': me }, function (marker, found) {
            //console.log('marker found : ' + marker);
            $('#map_canvas').gmap('clear', 'markers')[me];
        //});
    }
    
    $('#map_canvas').gmap('addMarker',
        {
            'tags': obj.userId,
            'position': new google.maps.LatLng(obj.latitude, obj.longitude),
            //'animation': google.maps.Animation.DROP,
            'bounds': true
        })
    .click(function (event) {
        self.openInfoWindow({ 'content': 'TEXT_AND_HTML_IN_INFOWINDOW' }, event.userid);
    });
   // refreshMap();
}

function refreshMap() {
    //$('#map_canvas').gmap('clear', 'markers');
    console.log("Inside Page Show Refresh");
    $('#map_canvas').gmap('find', 'markers', { 'property': 'tags', 'value': me }, function (marker, found) {
        console.log('marker found : ' + marker);
        //$('#map_canvas').gmap('clear', 'markers');
        marker.setPosition(myCenter);
        $('#map_canvas').gmap('get', 'map').panTo(myCenter);
    });
    //console.log("right before refresh");
    //$('#map_canvas').gmap('refresh');
}

function prepareUser() {
    console.log("Before calling Create New Phono and inside PrepareUser");
    createNewPhono({
        onDone: function () {
            console.log("Phono ID is " + phonoID);
            console.log("Update User Parameters " + userId + " " + deviceUUID + " " + deviceName + " " + deviceversion + " " + devicePlatform + " " + phonoID);
            updateUserInfo(userId, deviceUUID, deviceName, deviceversion, devicePlatform, phonoID, phoneGapVersion, {
                onDone: function () {
                    console.log("Update user Successful");
                }
            });
        }
    });
}

function updateUserInfo(myUserId, mydeviceId, mydeviceName, mydeviceVersion, mydevicePlatform, mychatId, myPhoneGapVersion, obj) {
    console.log("inside UpdateUserinfo");
    var usersService = new Services.Users();
    usersService.saveUser({
        data: { userId: myUserId, deviceId: mydeviceId, deviceName: mydeviceName, deviceVersion: mydeviceVersion, devicePlatform: mydevicePlatform, chatId: mychatId, devicePhGapVersion: myPhoneGapVersion },
        onDone: function (user) {
            if (user && user instanceof Model.User) {
                var model = $.extend(new Model.User(), user)
                console.log("saveUserInfo : " + model.userId);
                console.log("saveUserInfo : " + model.latitude);
                console.log("saveUserInfo : " + model.longitude);
                console.log("saveUserInfo : " + model.chatId);
                obj.onDone();
            }

        }
    });
}
