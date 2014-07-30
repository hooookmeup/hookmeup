var myCenter;
var minZoomLevel = 14;
var myPos;

function setLatLong(currentlat, currentlng) {
    console.log('inside setLatLong ' + currentlat + ' ' + currentlng);
    myCenter = { 'center': currentlat + ',' + currentlng };
    updateMap();

}
//var myCenter = { destination: new google.maps.LatLng(40.712915, -74.007511) };

//Create the map then make 'displayDirections' request
$('#page-map').live("pageinit", function() {
    console.log("Inside Page Map INIT");
    updateMap();
    //console.log(myCenter.center);
    //$('#map_canvas').gmap(
    //		{ 'center' : myCenter.center, 
    //		'minZoom': 10,
    //            'maxZoom': 14,
    //		      'mapTypeControl' : false,
    //		      'navigationControl' : false,
    //		      'streetViewControl' : false, 
    //		      'mapTypeId' : google.maps.MapTypeId.ROADMAP,
    //		      'panControl' : false,
    //		      'zoomControl' : true,
    //		      'overviewMapControl' : false,
    //		      'rotateControl' : false,
    //		      'scrollwheel' : false,
    //		      'draggable' : false,
    //		      'disableDoubleClickZoom' : true
    //		    })
    //			.bind('init', function(evt, map) 
    //					{ 
    //				console.log("Inside Bind Init");
    //						$('#map_canvas').gmap('addMarker', 
    //								{ 'tags': 'home',
    //								    'position': map.getCenter(),
    //									'animation' : google.maps.Animation.DROP 
    //								})
    //								.click(function()
    //										{
    //									$.mobile.changePage($('#IM'), {});
    //								});
    //						//console.log("Before Page Show Refresh");
    //						//$('#map_canvas').gmap('refresh');
    //					});
    //console.log("before calling RefreshMap inside PageInit");
    //refreshMap();

});

function updateMap() {
    console.log(myCenter.center);
    $('#map_canvas').gmap(
    		{
    		    'center': myCenter.center,
    		    'minZoom': 10,
    		    'maxZoom': 14,
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
    			.bind('init', function (evt, map) {
    			    console.log("Inside Bind Init");
    			    $('#map_canvas').gmap('addMarker',
                            {
                                'id': 'myPos',
                                'position': map.getCenter(),
                                'animation': google.maps.Animation.DROP
                            })
                            .click(function () {
                                $.mobile.changePage($('#IM'), {});
                            });
    			    //console.log("Before Page Show Refresh");
    			    //$('#map_canvas').gmap('refresh');
    			});
    console.log("before calling RefreshMap inside updateMap");
    refreshMap();

}

//function refreshMap() {
//    $('#page-map').live("pageshow", function () {

//        console.log("Inside Page Show Refresh");
//        $('#map_canvas').gmap('refresh');
//    });
//}

function refreshMap() {
    console.log("Inside Page Show Refresh");
    $('#page-map').live("pageshow", function () {
        myPos = $('#map_canvas').gmap('get', 'markers')['myPos'];
            console.log("marker found : " + myPos);
            if (myPos) {
                marker.setPosition(map.getCenter());
                console.log("Resetting marker Center");
            }
        });
        
        $('#map_canvas').gmap('refresh');
    });
}
