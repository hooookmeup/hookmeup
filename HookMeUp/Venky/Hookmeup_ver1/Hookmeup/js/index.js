var myLoc;
var myLoc2;
var isDeviceReady = false;
var userNetworkStatus = false;
var networkSpeed;
var userId;
var lastLatitude;
var lastLongitude;
var currentLatitude;
var currentLongitude;
var currentAltitude;
var currentAccuracy;
var currentAltitudeAccuracy;
var currentHeading;
var currentSpeed;
var currentTimeStamp;
var deviceName;
var deviceUUID;
var devicePlatform;
var deviceversion;
var phonoID ;
var userIsActive = false;
// Wait for Cordova to load
//
function onLoad() {
    document.addEventListener("deviceready", onDeviceReady, false);
    console.log("inside onLoad");   
}
// Cordova is ready
//
function onDeviceReady() {
    console.log("inside on Device Ready");
    isDeviceReady = true;
    console.log("isDeviceReady ? " + isDeviceReady);
    userNetworkStatus = isUserOnline();
    if (userNetworkStatus) {
        console.log('Network Status : ' + userNetworkStatus);
        switch (networkSpeed)
        {
            case 'Unknown connection':
                alert("Slow Network Speed");
                break;
            case 'Cell 2G connection':
                alert("Sub Optimal Network Speed");
                break;
        }
        if (isUserInLocalStorage()) {
            getUserInfo(userId, {
                onDone: function () {
                    console.log("inside getUserInfo callback function");
                    if (userIsActive) {
                        console.log("Calling Prepare Home Page now");
                        prepareHomePage();
                    }
                    else {
                        // Add code in here to redirect to correct registration Page based on Enum
                        console.log("Changing to Registration Page now");
                        $.mobile.changePage("#registration");
                    }

                }
            });
        }
        else {
            console.log("user Not in local storage so navigating to login page");
            $.mobile.changePage("#dialogPage");
        }
    }
    else {
        console.log('Network Status : ' + userNetworkStatus);
        alert('No network connection Detected;');
    }
}

function prepareHomePage() {
    if (isDeviceReady) {
        if (!getDeviceInfo()) {
            console.log("Could not get Device Information");
            //navigator.notification.alert("Could not get Device Info");
        }
        else {
            console.log("We are past getting device Info " + deviceName);
        }
        preparePhono({
            onDone: function () {
                console.log("Phono ID is " + phonoID);
            }
        });
        console.log("Update User Parameters " + userId + " " + deviceUUID + " " + deviceName + " " + deviceversion + " " + devicePlatform + " " + phonoID);
        updateUserInfo(userId, deviceUUID, deviceName, deviceversion, devicePlatform, phonoID, {
            onDone: function () {
                console.log("Update user Successful");
            }
        });

        var options = { maximumAge: 3000, timeout: 5000, enableHighAccuracy: true };
        watchID = navigator.geolocation.watchPosition(onSuccess, onWatchError, options);
        console.log("WatchID: " + watchID);
    }
}


function preparePhono(obj) {
    var phonoObject = $.phono({
        apiKey: "aaad85ef33babbe84d0a6b0b15d08076",
        onReady: function(event) {
            alert("My Phono ID is : " + this.sessionId);
            phonoID = this.sessionId;
            //obj.onDone();
        },
        onUnready: function () {
            alert("Disconnected");
        }
    });
    console.log("Connected to Phono? " + phonoObject.connected());
    //obj.onDone();
}

function updateUserInfo(myUserId, mydeviceId, mydeviceName, mydeviceVersion, mydevicePlatform, mychatId, obj) {
    console.log("inside UpdateUserinfo");
    var usersService2 = new Services.Users();
    usersService2.saveUser({
        data: { userId: myUserId, deviceId: mydeviceId, deviceName: mydeviceName, deviceVersion: mydeviceVersion, devicePlatform: mydevicePlatform, chatId: mychatId },
        onDone: function (user) {
            console.log("saveUserInfo : " + user.userId);
            console.log("saveUserInfo : " + user.latitude);
            console.log("saveUserInfo : " + user.longitude);
            console.log("saveUserInfo : " + user.chatId);
            obj.onDone();
        }
    });
}

function clearWatch() {
    if (watchID != null) {
        console.log("WatchID: " + watchID);
        navigator.geolocation.clearWatch(watchID);
        watchID = null;
    }
}

// onError Callback receives a PositionError object
//
function onWatchError(error) {
    alert('code: '    + error.code    + '\n' +
          'message: ' + error.message + '\n');
}

function onSuccess(position) {
    console.log("inside onSuccess");
    if (getPosition(position)) {
        //console.log("GetPosition returned : " + getPosition(position));
        updateUserLocation(userId, currentLatitude, currentLongitude, {
            onDone: function () {
                console.log("Setting Lat Long for Map Page");
                setLatLong(currentLatitude, currentLongitude);
                $.mobile.changePage("#page-map", { reloadPage: true });
            }
        });
    }
    else {
        navigator.notification.alert("No Location Information");
        $.mobile.changePage("#dialogPage");
    }
}

function onError(error) {
    alert('code: ' + error.code + '\n' +
            'message: ' + error.message + '\n');
}


function getPosition(Pos) {
    console.log('inside getPosition');
    var myposition = '{"' + 'Latitude": ' +'"'+ Pos.coords.latitude + '"'+ ',' +
                    '"'+ 'Longitude": ' + '"'+ Pos.coords.longitude + '"'+ ',' +
                    '"' + 'Altitude": ' + '"' + Pos.coords.altitude + '"' + ',' +
                    '"' + 'Accuracy": ' + '"' + Pos.coords.accuracy + '"' + ',' +
                    '"' + 'Altitude Accuracy": ' + '"' + Pos.coords.altitudeAccuracy + '"' + ',' +
                    '"' + 'Heading": ' + '"' + Pos.coords.heading + '"' + ',' +
                    '"' + 'Speed": ' + '"' + Pos.coords.speed + '"' + ',' +
                    '"' + 'Timestamp": ' + '"' + Pos.timestamp + '"' + '}';
    console.log(myposition);
    console.log("Getting Pos data");
    //console.log('inside Parse ' + JSON.parse(myposition));
    currentLatitude = Pos.coords.latitude;
    currentLongitude = Pos.coords.longitude;
    currentAltitude = Pos.coords.altitude;
    currentAccuracy = Pos.coords.accuracy;
    currentAltitudeAccuracy = Pos.coords.altitudeAccuracy;
    currentHeading = Pos.coords.heading;
    currentSpeed = Pos.coords.speed;
    currentTimeStamp = Pos.timestamp;
    return true;
}
function getDeviceInfo() {
    var deviceDetails = 'Device Name: ' + device.name + '<br />' +
                        'Device Cordova: ' + device.cordova + '<br />' +
                        'Device Platform: ' + device.platform + '<br />' +
                        'Device UUID: ' + device.uuid + '<br />' +
                        'Device Version: ' + device.version + '<br />';
    console.log(deviceDetails);
    deviceName = device.name;
    devicePlatform = device.platform;
    deviceUUID = device.uuid;
    deviceversion = device.version;
    return true;
}
function checkConnection() {
    var networkState = navigator.connection.type;
    var states = {};
    states[Connection.UNKNOWN] = 'Unknown connection';
    states[Connection.ETHERNET] = 'Ethernet connection';
    states[Connection.WIFI] = 'WiFi connection';
    states[Connection.CELL_2G] = 'Cell 2G connection';
    states[Connection.CELL_3G] = 'Cell 3G connection';
    states[Connection.CELL_4G] = 'Cell 4G connection';
    states[Connection.NONE] = 'No network connection';
    return states[networkState];
}

function isUserOnline() {
    console.log("Checking connection Status");
    var onlinestatus = checkConnection();
    if (onlinestatus === 'No network connection') {
        console.log(onlinestatus);
        networkSpeed = onlinestatus;
        return false;
    }
    else {
        console.log(onlinestatus);
        networkSpeed = onlinestatus;
        return true;
    }
}

function isUserInLocalStorage() {
    console.log('Inside checking Local Storage for User');
    if (window.localStorage["userId"] != undefined) {
        console.log("UserID in Local Storage is " + window.localStorage["userId"]);
        userId = window.localStorage["userId"];
        return true;
    }
    else {
        console.log("UserID not in Local Storage ");
        return false;
    }
}

function updateUserLocation(ID, myLat, myLng, obj) {
    console.log("inside UpdateUserLocation");
    console.log("User last Postions from : " + lastLatitude + " and " + lastLongitude + " to " + myLat + " and " + myLng);
    if (!(lastLatitude === myLat) || (!(lastLongitude === myLng))) {
        console.log("User moved from : " + lastLatitude + " and " + lastLongitude + " to " + myLat + " and " + myLng);
        var usersService1 = new Services.Users();
        usersService1.saveUser({
            data: { userId: ID, latitude: myLat, longitude: myLng },
            onDone: function (user) {
                console.log("updateUserLoc : " + user.userId);
                console.log("UpdateUserLoc : " + user.latitude);
                console.log("UpdateUserLoc : " + user.longitude);
                obj.onDone();
            }
        });
    }
    else {
        obj.onDone();
    }
}

function getUserInfo(id, obj) {
    console.log("Inside getUserInfo");
        var usersService = new Services.Users();
        usersService.getUser({
            data: { userId: id },
            onDone: function (user) {
                if (user && user instanceof Model.User) {
                    var model = $.extend(new Model.User(), user)
                    //alert(model.userName);
                    console.log("UserName: " + model.userName);
                    console.log("isActive: " + model.isActive);
                    console.log("Latitude: " + model.latitude);
                    console.log("Longitude: " + model.longitude);
                    userIsActive = model.isActive;
                    lastLatitude = model.latitude;
                    lastLongitude = model.longitude;
                    obj.onDone();
                }
            }
        });
    }



function handleLogin() {
   
    var form = $("#loginForm");
    //disable the button so we can't resubmit while we wait
    console.log("in handle Login");
    $("#submitButton", form).attr("disabled", "disabled");
    var u = $("#username", form).val();
    var p = $("#password", form).val();
    console.log(u);
    console.log(p);
    if (u != '' && p != '') {
        var usersService = new  Services.Users();
        usersService.validate({
            data: { userName: u, password: p },
            onDone: function (user) {
                console.log("Inside ondone: Login");
                if (user && user instanceof Model.User) {
                    var model = $.extend(new Model.User(), user)
                    console.log(model.userId);
                    window.localStorage["userId"] = model.userId;
                    console.log("UserId stored in local storage");
                    prepareHomePage();
                    console.log("Navigating to Homepage");
                    $.mobile.changePage("#page-map", { reloadPage: true });
                } else {
                    navigator.notification.alert("Your login failed", function () { });
                }
                $("#submitButton").removeAttr("disabled");
            }
        });
    }
    else {
        $("#submitButton").removeAttr("disabled");
    }
        
       
        }

function logout() {
    console.log("inside Logout");
    if (window.localStorage["userId"] != undefined) {
        console.log(window.localStorage["userId"]);
        window.localStorage.removeItem("userId");
        console.log("UserId deleted from Local Storage");
        clearWatch();
    }
}

function updatePassword(uname) {
    var usersService = new Services.Users();
    usersService.changePassword({
        data: { userName: uname, password: "test1", newPassword: "test1" },
        onDone: function (user) { alert(user.userName) }
    });
}
       
function handleRegistration() {

    var name, pass, repass, age, terms;

    var init = function () {
        name = $("#newUserReg_Name").val();
        pass = $("#newUserReg_Passwd").val();
        repass = $("newUserReg_RePasswd").val();
        age = $("#chkageagree").is(':checked');
        terms = $("#chktandc").is(':checked');
    }

    var validate = function () {
        return true;
    }

    var submitData = function () {
        var usersService = new Services.Users();
        usersService.saveUser({
            data: { userName: name, password: pass },
            onDone: function (user) { alert(user.userName) }
        });
    }

    init();
    if (validate()) submitData();
}
function loginformvalidate() {
    console.log("Inside Login Form Validate");
    var validator = $("#loginForm").validate({
        rules: {
            username: "required",
            password: "required"
        },
        messages: {
            username: {
                required: "Username Required"
            },
            password: {
                required: "Password Required"
            }
        },
        errorPlacement: function (error, element) {
            if (element.is(":radio"))
                error.appendTo(element.parent());
            else if (element.is(":checkbox"))
                error.appendTo(element.parent());
            else
                //error.appendTo(element.parent());
                console.log(element.get(0).placeholder);
            ('input[id=element.get(0).id]').replace(element.get(0).placeholder, error);
        },
        submitHandler: function () {
            console.log("inside submitHandler");
            return true;
        },
        success: function (label) {
            console.log("Returning Success");
           // ("#loginForm").addClass("checked");
        }
       // highlight: function(element, errorClass) {
            //$(element).parent.find("." + errorClass).removeClass("checked");
         //   console.log("inside Highlight");
        //}
    });
}
$(function(){
    $('#submitButton').click(function () {
 //       if (!loginformvalidate()) {
            handleLogin();
            //checkPreAuth();
  //      }
        //$.mobile.changePage("#page-map");
    });
});

$(function () {
    $('#btnnewUser').click(function () {
        $.mobile.changePage("#registration")
    });
});

$(function () {
    $('#btncreateAccount').click(function () {
        handleRegistration();
        $.mobile.changePage("#updateProfile")
    });
});

$(function () {
    $('#IMCloseButton').click(function () {
        $('.ui-dialog').dialog('close')
    });
});

$(function () {
    $('#btnNext').click(function () {
        $.mobile.changePage("#questionaire1")
    });
});

$(function () {
    $('#changepassword').click(function () {
        updatePassword($("#uname").val());
        $.mobile.changePage("#dialogPage");
    });
});

$(function () {
    $('#btnlogout').click(function () {
        console.log("inside Logout Click");
        logout();
        $.mobile.changePage("#dialogPage");
    });
});


$(function () {
    $('div[data-role="dialog"]').on('pagebeforeshow', function (e, ui) {
        if (ui.prevPage.attr('id') == 'Home') {
            ui.prevPage.addClass("ui-dialog-background ");
        }
    });

    $('div[data-role="dialog"]').on('pagehide', function (e, ui) {
            $(".ui-dialog-background ").removeClass("ui-dialog-background ");
    });
});
