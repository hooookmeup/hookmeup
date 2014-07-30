var chats = {};
var connectionUrl = window.location.protocol + "//app.phono.com/http-bind";
var provisioningUrl = undefined;
var chatString = "en2fr@bot.talk.google.com";
var gw = "gw-v6.d.phono.com";
var sessionID;
var newChatID;

$('#IM').live('pageinit', function () {
    console.log("Inside IM Init Function");

});

function urlParam(name) {
        var results = new RegExp('[\\?&]' + name + '=([^&#]*)').exec(window.location.href);
        if (!results) { return undefined; }
        return decodeURIComponent(results[1]) || undefined;
    }
function createNewPhono(obj) {
        //Clone a phono div
    console.log("Inside Create New Phono Call");
        if (connectionUrl.indexOf("file:") == 0) {
            connectionUrl = "http://app.phono.com/http-bind";
        }

        // Do we have URL parameters to override here?
       // if (audioType == "auto" && urlParam("audio") != undefined) audioType = urlParam("audio");
        if (urlParam("connectionUrl") != undefined) connectionUrl = urlParam("connectionUrl");
        if (urlParam("provisioningUrl") != undefined) provisioningUrl = urlParam("provisioningUrl");
        if (urlParam("chat") != undefined) chatString = urlParam("chat");
        if (urlParam("gateway") != undefined) gw = urlParam("gateway");


        //console.log("audioType = " + audioType);
        //console.log("charString = " + chatString);

        chats[newChatID] = $.phono({
            //apiKey: "C17D167F-09C6-4E4C-A3DD-2025D48BA243",
            apiKey: "AAAD85EF-33BA-BBE8-4D0A-6B0B15D08076",
            connectionUrl: connectionUrl,
            provisioningUrl: provisioningUrl,
            gateway: gw,

            onReady: function (event) {
                var baseUrl = window.location.href.substring(0, window.location.href.indexOf('?'));
                sessionID = this.sessionId;
                console.log("Create New Phono : " + sessionID);
                console.log("New ChatID inside onReady : " + chats[newChatID]);
                console.log("ChatID : " + newChatID);
                obj.onDone();
            },
            onUnready: function (event) {
                console.log("Phono disconnected");
            },
            onError: function (event) {
                console.log(event.reason);
            },
            messaging: {
                onMessage: function (event, message) {
                    var JID = message.from.split("/");
                    console.log("Message from " + JID[0] + " [" + message.body + "]");
                    routeMessage(chatID, "incoming", JID[0], message.body);
                }
            }
        });
        console.log("New ChatID after Phono init " + chats[newChatID]);
        console.log("ChatID : " + newChatID);
    }

//Routes a message to the appropriate chat or creates a new chat
function routeMessage(phonoID, type, jid, message) {
        var theChat = "";
        $.each(chats, function (key, value) {
            if (value == jid) {
                theChat = key;
                var newMsg = chatMessage(jid, message, "inbound");
                var chatDiv = $("#" + theChat);
                renderNewMessage(chatDiv, newMsg);
                chatDiv.find(".chatTxtInput").focus();
                return;
            }
        });

        //Did we find a chat?
        if (!theChat.length) {
            console.log("Starting a new Chat");
            $.mobile.changePage("#IM");
            createNewChat(phonoID, jid, message);
        }
    }

//Outputs an IM message
function renderNewMessage(chat, msg) {
        chat.find(".chatBox").append(msg);
        chat.find(".chatBox").attr({ scrollTop: chat.find(".chatBox").attr("scrollHeight") });
    }

//Creates a new chat
function createNewChat(phonoID, to, message) {
        //clone a chat box
        var phonoDiv = $("#" + phonoID);
        var newChatID = createChatDiv(phonoID);
        var chatDiv = $("#" + newChatID);
        chatDiv.find(".chatID").text(newChatID);
        chatDiv.find(".chatDetail").html(to);

        if (message.length) {
            var newMsg = chatMessage(to, message, "inbound");
            renderNewMessage(chatDiv, newMsg);
        }

        console.log("[" + phonoDiv.attr('id') + "] [" + newChatID + "] Chat started with " + to);

        chats[newChatID] = to;
    }

//Creates a new div to hold a chat. Returns the id of the new div
function createChatDiv(phonoID) {
        //clone a chat box
        var phonoDiv = $("#" + phonoID);
        var chatBox = phonoDiv.find(".chats");
        var newChatCtr = ($(".chatHldr").size() + 1) - 2;
        newChatID = "Chat" + newChatCtr;
        var firstChat = $(".chatHldr").first();
        var newChatDiv = firstChat.clone();
        newChatDiv.attr("id", newChatID).appendTo(chatBox).show();
        newChatDiv.find(".chatTxtInput").focus();

        return newChatID;
    }

function isIOS() {
        var userAgent = window.navigator.userAgent;
        if (userAgent.match(/iPad/i) || userAgent.match(/iPhone/i)) {
            return true;
        }
        return false;
    }

function isAndroid() {
        var userAgent = window.navigator.userAgent;
        if (userAgent.match(/Android/i)) {
            return true;
        }
        return false;
    }

// Create and return a chat message bubble
var chatMessage = function (from, body, type) {
        var result;

        if (type == 'inbound') {
            var borderStyle = "inbound";
            var fromStyle = "fromInbound";
        } else {
            var borderStyle = "outbound";
            var fromStyle = "fromOutbound";
        }

        result = $("<div>")
            .addClass("chatEntry")
            .addClass(borderStyle);
        var fromHeader = $("<div>")
            .addClass("from")
            .addClass(fromStyle)
            .html(from)
            .appendTo(result);
        var msgBody = $("<div>")
            .addClass("body")
            .html(body)
            .appendTo(result);

        return result;
    }

//DOM Event Handlers
$(".addPhono").click(function () {
        createNewPhono();
        return false;
    });

$('.closeChat').live('click', function () {
        var thisChat = $(this).closest(".chatHldr");
        var thisChatID = $(this).closest(".chatHldr").attr("id");
        thisChat.slideUp();
        chats[thisChatID] = null;
        console.log("[" + thisChatID + "] Chat closed");
    });

$('.chat').live('click', function () {
        var thisPhono = $(this).closest(".phono").attr("id");
        var chatTo = $.trim($("#" + thisPhono).find(".chatTo").val());
        createNewChat(thisPhono, chatTo, "");
    });

$('.sendMsg').live('click', function () {
        var thisPhono = $(this).closest(".phono");
        var thisChat = $(this).closest(".chatHldr");
        var msgText = thisChat.find(".chatTxtInput").val();
        var newMsg = chatMessage("You", msgText, "outgoing");
        renderNewMessage(thisChat, newMsg);
        phonos[thisPhono.attr("id")].messaging.send(chats[thisChat.attr("id")], msgText);
        thisChat.find(".chatTxtInput").val("");
        console.log("[" + thisPhono.attr('id') + "] Sending message to: " + chats[thisChat.attr('id')] + " [" + msgText + "]");
    });

$('.text').live('click', function () {
        var thisPhono = $(this).closest(".phono");
        var to = thisPhono.find(".jid").val();
        var msg = thisPhono.find(".msgBody").val();
        sendMessage(thisPhono.attr("id"), to, msg);
        thisPhono.find(".msgBody").val("");
    });

$(".logToggler").click(function () {
        if ($("#logConsole").css("height") == "25px") {
            $("#logConsole").css("height", "245px");
            $("body").css("margin-bottom", "285px");
            $(this).text("Hide Log Viewer");
        } else {
            $("#logConsole").css("height", "25px");
            $("body").css("margin-bottom", "25px");
            $(this).text("Show Log Viewer");
        }

        return false;
    });
//createNewPhono();
//}
//	});
//});

