using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using SS.DL.AzureTableStorage;
using SS.Framework.Common;

namespace Hookmeup.Models
{
    public class UserRepository
    {

        private UserContext uctx = new UserContext();

        public IEnumerable<UserModel> GetAll()
        {
            var qry = from user in uctx.GetAllForPartition("Users")
                      where user.IsActive == true
                      select UserModel.FromEntity(user);

            return qry.ToList<UserModel>();
        }

        public void Update(UserModel model)
        {
            User user = uctx.GetById("Users", model.UserId);
            if (user == null) throw new Exception(); //TODO: does not exists

            bool islocChanged = IsLocationChanged(model, user);
            bool isChatChanged = IsChatIdChanged(model, user);
            bool isDeviceChanged = IsDeviceIdChanged(model, user);

            user = UserModel.FromModel(model, user);

            if (islocChanged) UpdateUserLocation(user.Latitude, user.Longitude, user.UserId);
            if (isChatChanged) UpdateUserChat(user.ChatId, user.UserId);
            if (isDeviceChanged) UpdateUserDevices(user);
            user.IsOnline = true; 

            uctx.UpdateObject(user, false);

        }

        public UserModel Get(string UserId)
        {
            User user = uctx.GetById("Users", UserId);
            if (user != null)
                return UserModel.FromEntity(user);
            else
                return null; 
        }

        public void Post(ref UserModel model)
        {
            //validate userid exists in system
            User user = uctx.GetById("Users", SimpleHash.GenerateHashKey(model.UserName));
           
            bool fbuserexists = DoesFacebookUserExists(model, user);

            if (user != null && !fbuserexists)
            {
     
                    throw new Exception(); //user id already exists . 
            }

            if (!fbuserexists)
            {
                user = UserModel.FromModel(model);
                user.IsOnline = true;
                user.IsActive = true;


                uctx.AddObject(user);
            }

            if (user.Latitude != 0 && user.Longitude != 0)
                UpdateUserLocation(user.Latitude, user.Longitude, user.UserId);

            if (!string.IsNullOrEmpty(user.ChatId))
                UpdateUserChat(user.ChatId, user.UserId);

            if (!string.IsNullOrEmpty(user.DeviceId))
                UpdateUserDevices(user);

            model = UserModel.FromEntity(user);

        }

        private static bool DoesFacebookUserExists(UserModel model, User user)
        {
            if (user!=null && !string.IsNullOrEmpty(user.FacebookId) && user.FacebookId == model.FacebookId)
            {
                return true; 
            }
            return false; 
        }

  

        public UserModel Delete(string UserId)
        {
            User user = uctx.GetById("Users", UserId);
            user.IsActive = false;
            user.IsOnline = false; 
            uctx.UpdateObject(user, false);
            return UserModel.FromEntity(user);
        }

        public UserModel Validate(string userName, string password)
        {
            User user = uctx.GetById("Users", SimpleHash.GenerateHashKey(userName));
            if (user == null) return null;  //TODO: Failed Validation

            bool rez = SimpleHash.VerifyHash(password, "MD5", user.Password);
            if (rez == false) return null; // Failed Validation
            else return UserModel.FromEntity(user);

        }

        public UserModel ChangePassword(UserModel model, string newPassword)
        {
            model.Password = newPassword;
            this.Update(model);
            return model;
        }

        public List<UserModel> GetProximityUsers(string userid, double lat, double lon)
        {
            User user = uctx.GetById("Users", userid);
            if (user == null) throw new Exception(); // does not exsist

            UserLocationContext ctx = new UserLocationContext();
            List<UserLocation> proxUserLoc = ctx.GetProximityUsers(lat, lon);

            List<User> proxUsers = uctx.GetUsers(proxUserLoc);

            List<UserModel> proxUserModels = UserModel.FromEntity(proxUsers);
            return proxUserModels;
        }


        private void UpdateUserLocation(double lat, double lon, string userId)
        {
            UserLocationContext locCtx = new UserLocationContext();
            UserLocation userloc = locCtx.GetById(lat.ToString(), lon.ToString());
            if (userloc == null)
                locCtx.AddObject(new UserLocation { Latitude = lat.ToString(), Longitude = lon.ToString(), UserId = userId, LocationTime = DateTime.Now });
            else
            {
                userloc.UserId = userId;
                userloc.LocationTime = DateTime.Now;
                locCtx.UpdateObject(userloc, false);
            }
        }

        private void UpdateUserChat(string chatId, string userId)
        {
            UserChatContext cCtx = new UserChatContext();
            UserChat userChat = cCtx.GetById(userId,chatId);
            if (userChat == null)
                cCtx.AddObject(new UserChat { UserId = userId, ChatId=chatId , ChatTime = DateTime.Now  } ) ; 
            else
            {
                userChat.ChatTime = DateTime.Now;
                cCtx.UpdateObject(userChat, false);
            }
        }


        private void UpdateUserDevices(User user)
        {
            UserDeviceContext udCtx = new UserDeviceContext();
            UserDevice userDev = udCtx.GetById(user.UserId , user.DeviceId);
            if (userDev == null)
                udCtx.AddObject(new UserDevice { UserId = user.UserId, DeviceId = user.DeviceId, DeviceName = user.DeviceName, DevicePlatform = user.DevicePlatform, DevicePhGapVersion = user.DevicePhGapVersion, DeviceVersion = user.DeviceVersion });
            else
            {
                userDev.DeviceName = user.DeviceName;
                userDev.DevicePlatform = user.DevicePlatform;
                userDev.DevicePhGapVersion = user.DevicePhGapVersion;
                userDev.DeviceVersion = user.DeviceVersion;
                udCtx.UpdateObject(userDev,false);
            }
        }

        private bool IsChatIdChanged(UserModel model, User user)
        {
            return (model.ChatId != user.ChatId);
        }

        private bool IsLocationChanged(UserModel model, User user)
        {
            return (model.Longitude != user.Longitude || model.Latitude != user.Latitude);
        }

        private bool IsDeviceIdChanged(UserModel model, User user)
        {
            return (model.DeviceId != user.DeviceId);
        }

        internal List<Question> GetQuestions(string userid , bool refresh )
        {
            QuestionsRepository qr = new QuestionsRepository();
            
            List<UserQuestion> questionIds = null; 

            if (refresh) qr.RemoveQuestions(userid);

            questionIds = qr.AssignQuestionsIds(userid);
            
            List<Question> questions = qr.GetQuestions(questionIds);

            return questions; 

        }

        internal void SetAnswer(string id, string questionId, int answer)
        {
            QuestionsRepository qr = new QuestionsRepository();
            qr.UpdateAnswer(id, questionId, answer);
            
            
            User user = uctx.GetById("Users", id);

            user.Score = CalculateScore(id);

            uctx.UpdateObject(user, false);
        }


        private CategoryScore[] CalculateScore(string id)
        {
            UserQuestionContext uqc = new UserQuestionContext();
            var ques = uqc.GetAllForPartition(id);
            
               var results = from q in ques 
                          group  q by q.CategoryId into grps
                          select new CategoryScore {CategoryId = grps.Key , Score = grps.Sum(g=>g.Answer)};

            
      
               return results.ToArray();

        }

    }
}