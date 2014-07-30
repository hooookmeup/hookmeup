using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using SS.DL.AzureTableStorage;

namespace Hookmeup.Models
{
    public class UserModel
    {
        [Key]
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
        public string Gender { get; set; }
        public bool IsActive { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string ChatId {get;set;}
        public string newPassword { get; set; }

        public string DeviceId {get;set;}
        public string DeviceName {get;set;}
        public string DeviceVersion {get;set;}
        public string DevicePlatform {get;set;}
        public string DevicePhGapVersion {get;set;}
        public bool IsOnline { get; set; }
        public string FacebookId { get; set; }
        
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Locale { get; set; }

        public AppStateType AppState { get; set; }
        public ResultCode StatusCode { get; set; }
        

        internal static UserModel FromEntity(User user)
        {
            return new UserModel { UserId = user.UserId, UserName = user.UserName, IsAdmin = user.IsAdmin, Gender = user.Gender,
                IsActive = user.IsActive, Latitude = user.Latitude, Longitude = user.Longitude, ChatId = user.ChatId , DeviceId = user.DeviceId , DeviceName = user.DeviceName,
                        DeviceVersion = user.DeviceVersion , DevicePlatform = user.DevicePlatform , 
                        DevicePhGapVersion = user.DevicePhGapVersion , IsOnline = user.IsOnline , FacebookId= user.FacebookId , AppState = user.AppState , FirstName = user.FirstName,
                        LastName = user.LastName , Locale =  user.Locale};
        }

        internal static User FromModel(UserModel model)
        {
            return new User { UserPartition = "Users", UserId = model.UserName, UserName = model.UserName, Password = model.Password, IsAdmin = model.IsAdmin, 
                Gender = model.Gender, IsActive = model.IsActive, Longitude = model.Longitude, Latitude = model.Latitude, ChatId = model.ChatId , DeviceId = model.DeviceId
                , DeviceName = model.DeviceName , DeviceVersion = model.DeviceVersion , DevicePlatform = model.DevicePlatform , 
                DevicePhGapVersion = model.DevicePhGapVersion , IsOnline = model.IsOnline , FacebookId = model.FacebookId , AppState = model.AppState, 
                FirstName = model.FirstName , LastName = model.LastName , Locale= model.Locale};
        }

        internal static User FromModel(UserModel model, User user)
        {
            if (!String.IsNullOrEmpty(model.UserName))
                user.UserName = model.UserName;

            if (!String.IsNullOrEmpty(model.Password))
            user.Password = model.Password;

            
            user.IsActive = model.IsActive;

            user.IsAdmin = model.IsAdmin;

            if (!String.IsNullOrEmpty(model.Gender))
                user.Gender = model.Gender;


            user.Latitude = model.Latitude;
            user.Longitude = model.Longitude;
            
            if (!String.IsNullOrEmpty(model.ChatId))
                user.ChatId = model.ChatId;


            if (!String.IsNullOrEmpty(model.DeviceId))
                user.DeviceId = model.DeviceId;

            if (!String.IsNullOrEmpty(model.DeviceName))
                user.DeviceName = model.DeviceName;
            if (!String.IsNullOrEmpty(model.DeviceVersion))
                user.DeviceVersion = model.DeviceVersion;
            if (!String.IsNullOrEmpty(model.DevicePlatform))
                user.DevicePlatform = model.DevicePlatform;
            if (!String.IsNullOrEmpty(model.DevicePhGapVersion))
                user.DevicePhGapVersion = model.DevicePhGapVersion;

            if (!string.IsNullOrEmpty(model.FacebookId))
                user.FacebookId = model.FacebookId;

            user.IsOnline = model.IsOnline;

            if (model.AppState != AppStateType.NoState)
                user.AppState = model.AppState; 


            return user;
        }

        internal static List<UserModel> FromEntity(List<User> users)
        {
            return (from user in users select FromEntity(user)).ToList<UserModel>(); 

        }
    }
}