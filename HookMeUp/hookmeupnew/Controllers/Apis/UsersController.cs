using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Hookmeup.Models;
using SS.DL.AzureTableStorage;
using SS.Framework.Common;
using System.Web.Http.Cors;

namespace Hookmeup.Controllers.Apis
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UsersController : ApiController
    {
        static UserRepository rep = new UserRepository();
        // GET api/users
        //[ActionName("DefaultAction")]
        //public IEnumerable<UserModel> Get()
        //{
        //    return rep.GetAll();
        //}

       // // GET api/users/5
       //[ActionName("DefaultAction")]
       // public UserModel Get(string id)
       // {
       //     UserModel usermodel = rep.Get(id);
       //     if (usermodel == null)
       //     {
       //         throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
       //     }
       //     return usermodel;
       // }

        [HttpGet]
        public IEnumerable<UserModel> GetAll()
        {
            IEnumerable<UserModel> rez = null; 

            try
            {
                rez= rep.GetAll();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex,true);
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError));
            }

            return rez; 
        }


       // GET api/users/5
       [HttpGet]
       public UserModel GetById(string id)
       {
           UserModel usermodel = null;
           try
           {
               Logger.LogInformaton(String.Format("GetbyId() called for Id {0}", id));
                 usermodel = rep.Get(id);
           }
           catch (Exception ex)
           {
               Logger.LogError(ex, true);
               throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError));
           }

           if (usermodel == null)
           {
               throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotImplemented));
           }
           return usermodel;
       }

        // POST api/users
        [HttpPost]
        public HttpResponseMessage Add([FromBody]UserModel model)
        {
           HttpResponseMessage rez = null ;
           try
           {
               Logger.LogInformaton(String.Format("Add() called for UserName {0}", model.UserName));
               if (this.ModelState.IsValid)
               {
                   rep.Post(ref model);
                   rez = Request.CreateResponse<UserModel>(HttpStatusCode.Created, model);
                   
               } 
               else
                   rez =  Request.CreateResponse(HttpStatusCode.BadRequest);
           }
           catch (Exception ex)
           {
               Logger.LogError(ex, true);
               throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError));
           }

           return rez; 
        }

        // PUT api/users/5
        [HttpPost]
        public HttpResponseMessage Update([FromBody]UserModel model)
        {
            HttpResponseMessage rez = null;

            try
            {
                Logger.LogInformaton(String.Format("Update() called for UserName {0}", model.UserName));
                if (this.ModelState.IsValid)
                {

                    rep.Update(model);
                    rez = Request.CreateResponse<UserModel>(HttpStatusCode.Accepted, model);
                }
                else
                     rez =  Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, true);
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError));
            }
            return rez; 
        }

        // DELETE api/users/5
        [HttpGet]
        public UserModel Remove(string id)
        {
            UserModel model = null;
            try
            {
                model = rep.Get(id);
                if (model != null)
                {
                    model = rep.Delete(id);
                    return model;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, true);
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError));
            }
            return model;

        }

        [HttpPost]
        public UserModel Login([FromBody]UserModel user)
        {
            UserModel model = null;

            try
            {
               Logger.LogInformaton(String.Format("Login() called for UserName {0}", user.UserName));
               model = rep.Validate(user.UserName,user.Password);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, true);
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError));
            }

            if (model == null)
             throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Unauthorized));
       
            return model;
        }

        [HttpPost]
        public UserModel ChangePassword([FromBody]UserModel user)
        {
            UserModel model = null;

            try
            {
                model = rep.Validate(user.UserName, user.Password);

                if(model!=null)
                    model = rep.ChangePassword(model, user.newPassword);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, true);
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError));
            }

            if (model == null)
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Unauthorized));

            return model;
        }

        [HttpGet]
        public List<UserModel> GetProximityUsers(string id)
        {
            List<UserModel> models=null;
            UserModel model = null;
            try
            {
                Logger.LogInformaton(String.Format("GetProximityUsers() called for Id {0}", id));
                if (this.ModelState.IsValid)
                {
                    model = rep.Get(id);
                    if(model!=null)
                        models = rep.GetProximityUsers(model.UserId, model.Latitude, model.Longitude);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, true);
            }
            if (model == null)
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotImplemented));

            return models;
        }

        [HttpGet]
        public List<Question> Questions(string id , bool refresh)
        {
            UserModel model = null;
            List<Question> questions = null;

            try
            {

                model = rep.Get(id);
                if (model != null)
                    questions = rep.GetQuestions(id, refresh);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, true);
            }

            if (model == null)
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotImplemented));

            return questions;
        }

  
        [HttpPost]
        public void SetAnswer([FromBody]AnswerModel model) 
        {
            UserModel mod = null;
            try
            {
                mod = rep.Get(model.UserId);
                if (mod != null)
                    rep.SetAnswer(model.UserId, model.QuestionId, model.Answer);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, true);
            }
            if (model == null)
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotImplemented));


        }

      
    }
}
