/// <reference path="jquery-1.7.1.js" />
var Services = Services || {};
var Model = Model || {};

//var SERVERURL = "http://localhost:14582/api/Users";
var SERVERURL = "http://hookmeup.cloudapp.net/api/Users";
//jQuery(function () {

//    $.support.cors = true;

// var usersService = new Services.Users();
//    var questionService = new Services.Questions();

//    $("#Validate").click(function () {
//        usersService.validate({
//            data: { userName: "manusarin@msn.com", password: "test1" },
//            onDone: function (user) {
//                if (user && user instanceof Model.User) {
//                    var model = $.extend(new Model.User(), user)
//                    alert(model.userId);
//                }
//            }
//        });
//    });

   
//    $("#getOne").click(function () {
//        usersService.getUser({
//            data: { userId: "F42ED74733238A1D30DB78AE9EDCD136" },
//            onDone: function (user) {
//                if (user && user instanceof Model.User) {
//                    var model = $.extend(new Model.User(), user)
//                    alert(model.userName);
//                }
//            }
//        });
//    });

//    $("#getAll").click(function () {
//        questionService.getAll({
//            data: { userId: "F42ED74733238A1D30DB78AE9EDCD136" , refresh:true },
//            onDone: function (questions) {
//                $.each(questions, function ()
//                { alert(this.name); })
//            }
//        });
//    });

//    //Adding new user 

//    //$("#post").click(function () {
//    //    var user = new Model.User();
//    //    user.userName = "venky2@pb.com";
//    //    user.password = "test1";
//    //    user.latitude = "41.697841";
//    //    user.longitude = "-72.90252";
//    //    user.isActive = true;
//    //    user.gender = "Male";
//    //    user.deviceId = "12345678";
//    //    user.deviceVersion = " 2.13";
//    //    user.devicePhGapVersion = "2.134";
//    //    user.devicePlatform = "OSX";
//    //    user.deviceName = "iphone";

//    //    usersService.saveUser({
//    //        data: user,
//    //        onDone: function (user) { alert(user.userName) }
//    //    });
//    //});

//    //$("#post").click(function () {
//    //    usersService.changePassword({
//    //        data: { userName: "hello@pb.com", password: "test1", newPassword: "test1" },
//    //        onDone: function (user) { alert(user.userName) }
//    //    });
//    //});

//    $("#post").click(function () {
//        usersService.getProximity({
//            data: { userId: "C16DBCE3AF9D0F3228A6FC3E58AEF773", latitude: "41.697841", longitude: "-72.90252" },
//            onDone: function (users) { $.each(users,function()
//            { alert(this.userName); })
//            }
//        });
//    });


//});


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

Services.Users = function (test, test1) {
    this.test = test;
    this.test1 = test1;

}

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
            }
        });
     },

    getUser = function (obj) {
        $.ajax({
            url: SERVERURL + "/GetById",
            type:"GET",
            data: { id: obj.data.userId },
            success: function (data) {
                console.log("inside getUser in userService : " + data);
                var user = toModel(data);
                obj.onDone(user);
            },
            cache:false
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
                        console.log("Save User Update successful : " + data.userId);
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
                    console.log("Save User Add successful : " + data.userId);
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
            cache: false
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
            data: { id: obj.data.userId, questionId: obj.data.questionId, answer: obj.data.answer },
            cors: true,
            success: function (data) {
                var question = toModel(data);
                obj.onDone(question);
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


