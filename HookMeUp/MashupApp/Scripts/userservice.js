/// <reference path="jquery-1.7.1.js" />
var Services = Services || {};
var Model = Model || {};

var SERVERURL = "http://localhost:49488/api/Users";
//var SERVERURL = "http://hookmeup.cloudapp.net/api/Users";


jQuery(function () {

    $.support.cors = true;

    var usersService = new Services.Users();
    var questionService = new Services.Questions();

    $("#Validate").click(function () {
        usersService.validate({
            data: { userName: "manusarin@msn.com", password: "test2" },
            onDone: function (user) {
                if (user && user instanceof Model.User) {
                    var model = $.extend(new Model.User(), user)
                    alert(Model.ErrorCodes[model.statusCode]);
                }
            }
        });
    });

   
    $("#getOne").click(function () {
        usersService.getUser({
            data: { userId: "F42ED74733238A1D30DB78AE9EDCD136" },
            onDone: function (user) {
                if (user && user instanceof Model.User) {
                    var model = $.extend(new Model.User(), user)
                    alert(model.userName);
                }
            }
        });
    });

    $("#getAll").click(function () {
        questionService.getAll({
            data: { userId: "31A577162E73956E037642E1834238D8", refresh: true },
            onDone: function (questions) {
                $.each(questions, function ()
                { alert(this.name); })
            }
        });
    });

   
   
   

    $("#post").click(function () {
        //var user =  {"userName":"venky2@pb.com","password":"","isAdmin":false,"gender":"Male","isActive":true,"latitude":40.663684953578155,"longitude":-73.95388311386364,"chatId":"","newPassword":"","deviceId":"e0101010d38bde8e6740011221af335301010333","deviceName":"iPhone 5","deviceVersion":"6","devicePlatform":"iOS","devicePhGapVersion":"2.0.0","isOnline":true,"facebookId":null,"appState":5}

        var user = { userName: "venkatesh96@hotmail.com", gender: "male", firstname: "Venky", lastname: "pal", facebookId: "100007087234376", locale: "en_US", appState: "3" };

        //user.userId = "909E96738621748AF0FE9D1F05E2EBA8";
        //user.latitude = "40.67173219537789";
        //user.longitude = "-73.93385581741408";
        //user.gender = "Male";
        //user.appState=Model.AppStateEnum.TCComplete;


        usersService.saveUser({
            data: user,
            onDone: function (user) { alert(user.userName) }
        });
    });

    //$("#post").click(function () {
    //    var data = {
    //        userId: "B6A15E4C0042ED822F9567D8099E1616",
    //        questionId: "30ba7eb0-547d-4b5d-8dd2-8b45b38fe0f2",
    //        answer : "1"
    //    };

    //    questionService.saveAnswer({
    //        data: data,
    //        onDone: function (questionid) { alert(questionid) }
    //    });
    //});

    //$("#post").click(function () {
    //    usersService.changePassword({
    //        data: { userName: "hello@pb.com", password: "test1", newPassword: "test1" },
    //        onDone: function (user) { alert(user.userName) }
    //    });
    //});

    //$("#post").click(function () {
    //    usersService.getProximity({
    //        data: { userId: "909E96738621748AF0FE9D1F05E2EBA8", latitude: "40.6638639966358", longitude: "-73.9528746032739" },
    //        onDone: function (users) { $.each(users,function()
    //        { alert(this.userName); })
    //        }
    //    });
    //});


});

// ErrorCodes

Model.ErrorCodes =
    {
        401: "Unauthorised",
        404: "Not Found",
        408: "Request time out",
        500: "Internal Server Error",
        501: "NotImplemnted",

    };

// model 
Model.AppStateEnum = 
    {
        NoSate:0,  
        AccountCreated: 1,
        UserInfoUpdated:2,
        AgeCheckComplete: 3,
        QuestionaireCompleted:4,
        PrefernceComplete:5, 
        TCComplete:6
    }

//Models 
Model.User = function () {
    var self = this;
    self.userId = "";
    self.userName = "";
    self.password = "";
    self.isAdmin = false;
    self.gender = "";
    self.isActive = false;
    self.latitude = 0;;
    self.longitude = 0;;
    self.chatId = "";
    self.newPassword = "";
    self.deviceId = "";
    self.deviceName = "";
    self.deviceVersion = "";
    self.devicePlatform = "";
    self.devicePhGapVersion = "";
    self.isOnline = false;
    self.facebookId = "";
    self.appState = Model.AppStateEnum.NoSate;
    self.firstname = "";
    self.lastname = "";
    self.locale = "";
    self.statusCode = 0;
}

Model.Question = function () {
    var self = this;
    self.questionid = "";
    self.name = "";
    self.isAnsreq = true;
    self.sectionName = "";
    self.options = [];
    self.answer = "";
}

Model.Option = function () {
    var self = this;
    self.key = 0;
    self.value = ""; 
}


//Client proxies

//Users class 

Services.Users = function () { }
Services.Users.prototype = function () {

    var validate = function (obj) {

        $.ajax({ //my ajax request
            url: SERVERURL + "/Login",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify(obj.data),
            success: function (data) {
                var user = toModel(data);
                obj.onDone(user);
            },
            error:function(data)
            {
                var user = toErrorModel(data);
                obj.onDone(user);
            }
        });
     },

    getUser = function (obj) {
        $.ajax({
            url: SERVERURL + "/GetById",
            type:"GET",
            data: { id: obj.data.userId },
            success: function (data) {
                var user = toModel(data);
                obj.onDone(user);
            },
            cache: false,
            error: function (data) {
                var user = toErrorModel(data);
                obj.onDone(user);
            }
        });

    },

    saveUser = function (obj) {
        var ACTION = "";
        (typeof obj.data.userId === "undefined" || obj.data.userId === "") ? ACTION = "/Add": ACTION = "/Update";

        if (ACTION === "/Update") {
            $.get(SERVERURL + "/GetById", { id: obj.data.userId }, function (data) {
                var user = toModel(data);
                user = $.extend(user, obj.data);
                $.ajax({ //my ajax request
                    url: SERVERURL + ACTION,
                    type: "POST",
                    contentType: "application/json",
                    data: JSON.stringify(user),
                    success: function (data) {
                        var user = toModel(data);
                        obj.onDone(user);
                    },
                    error: function (data) {
                        var user = toErrorModel(data);
                        obj.onDone(user);
                    }
                });

            })
        }
        else {

            $.ajax({ //my ajax request
                url: SERVERURL + ACTION,
                type: "POST",
                contentType: "application/json",
                data: JSON.stringify(obj.data),
          
                success: function (data) {
                    var user = toModel(data);
                    obj.onDone(user);
                },
                error: function (data) {
                    var user = toErrorModel(data);
                    obj.onDone(user);
                }
            });

        }
        
    },


    changePassword = function (obj) {
        $.ajax({ //my ajax request
            url: SERVERURL + "/ChangePassword",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify(obj.data),
            cors: true,
            success: function (data) {
                var user = toModel(data);
                obj.onDone(user);
            },
            error: function (data) {
                var user = toErrorModel(data);
                obj.onDone(user);
            }
        });
    },

    getProximity = function (obj) {

        $.ajax({
            url: SERVERURL + "/GetProximityUsers",
            type: "GET",
            data: { id: obj.data.userId },
            success: function (data) {
                var users = [];
                $.each(data, function () {
                    var user = toModel(this);
                    users.push(user);
                });
                obj.onDone(users);
            },
            cache: false,
                error:function(data)
                {
                    var user = toErrorModel(data);
                    obj.onDone(user);
                }
        });

    },

    toModel = function (data) {
        var user = new Model.User();
        user.userId = data.UserId;
        user.userName = data.UserName;
        user.isAdmin = data.IsAdmin;
        user.gender = data.Gender;
        user.isActive = data.IsActive;
        user.latitude = data.Latitude;
        user.longitude = data.Longitude;
        user.chatId = data.ChatId;
        user.deviceId = data.DeviceId;
        user.deviceName = data.DeviceName;
        user.deviceVersion = data.DeviceVersion;
        user.devicePlatform = data.DevicePlatform;
        user.devicePhGapVersion = data.DevicePhGapVersion;
        user.isOnline = data.IsOnline;
        user.facebookId = data.FacebookId;
        user.appState = data.AppState;
        user.firstname = data.FirstName;
        user.lastname = data.LastName;
        user.locale = data.Locale;
        user.statusCode = 0; 
        return user;
    } , 

    toErrorModel = function (errorcode)
    {
        var user = new Model.User();
        user.statusCode = errorcode.status; 
        return user;
    }

return {
    validate: validate,
    getUser: getUser,
    saveUser: saveUser,
    changePassword: changePassword,
    getProximity: getProximity
};
}();






// Questions
Services.Questions = function () { }
Services.Questions.prototype = function () {

    var getAll = function (obj) {
        $.ajax({
            url: SERVERURL + "/Questions",
            type: "GET",
            data: { id: obj.data.userId, refresh: obj.data.refresh },
            success: function (data) {
                var questions = [];
                $.each(data, function () {
                    var question = toModel(this);
                    questions.push(question);
                });
                obj.onDone(questions);
            },
            cache: false
        });

    } ,

    saveAnswer = function (obj) {
        $.ajax({ //my ajax request
            url: SERVERURL + "/SetAnswer",
            type: "POST",
            contentType: "application/json",
            //data: { userId: obj.data.id, questionId: obj.data.questionId, answer: obj.data.answer },
            data: JSON.stringify(obj.data),
            cors: true,
            success: function (data) {
                //var question = toModel(data);
                obj.onDone(obj.data.questionId);
            }
        });
    }

    toModel = function (data) {
        var question = new Model.Question();
        question.questionid = data.QuestionId;
        question.name = data.Name;
        question.isAnsreq = data.IsAnswerReq;
        question.sectionName = data.SectionName;
        $.each(data.Options, function () {
            var option = toOptionModel(this);
            question.options.push(option);
        });
        question.isPositive = data.IsPositive;
        return question;
    }

    toOptionModel = function (data) {
        var option = new Model.Option();
        option.key = data["<Name>k__BackingField"];
        option.value = data["<Value>k__BackingField"];

        return option;
    }


    return {

        getAll: getAll ,
        saveAnswer:saveAnswer

    };
}();


