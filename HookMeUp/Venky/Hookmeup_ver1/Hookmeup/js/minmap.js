function setLatLong(currentlat, currentlng) {
    console.log('inside setLatLong ' + currentlat + ' ' + currentlng);
    var myCenter = { destination: new google.maps.LatLng(currentlat, currentlng) };

}
//var myCenter = { destination: new google.maps.LatLng(40.712915, -74.007511) };

//Create the map then make 'displayDirections' request
$('#page-map').live("pagebeforecreate", function() {
	console.log("Inside Page Map INIT");
    $('#map_canvas').gmap(
    		{ 'center' : myCenter.destination, 
    		      'zoom' : 17, 
    		      'mapTypeControl' : false,
    		      'navigationControl' : false,
    		      'streetViewControl' : false, 
    		      'mapTypeId' : google.maps.MapTypeId.ROADMAP,
    		      'panControl' : false,
    		      'zoomControl' : false,
    		      'overviewMapControl' : false,
    		      'rotateControl' : false,
    		      'scrollwheel' : false,
    		      'draggable' : false,
    		      'disableDoubleClickZoom' : true
    		    })
    			.bind('init', function(evt, map) 
    					{ 
    				console.log("Inside Bind Init");
    						$('#map_canvas').gmap('addMarker', 
    								{ 'position': map.getCenter(), 
    									'animation' : google.maps.Animation.DROP 
    								})
    								.click(function()
    										{
    									$.mobile.changePage($('#IM'), {});
    								});
    						//console.log("Before Page Show Refresh");
    						//$('#map_canvas').gmap('refresh');
    					});
    refreshMap();

});

function refreshMap() {
    $('#page-map').live("pageshow", function () {
        console.log("Inside Page Show Refresh");
        $('#map_canvas').gmap('refresh');
    });
}
