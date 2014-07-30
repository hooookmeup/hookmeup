using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Hookmeup.Models;

namespace Hookmeup.Controllers
{
    public class UserController : Controller
    {
        static UserRepository rep = new UserRepository();
        //
        // GET: /User/

        public ActionResult Index()
        {
            return View(rep.GetAll());
        }

        //
        // GET: /User/Details/5

        public ActionResult Details(string id)
        {
            UserModel usermodel = rep.Get(id); 
            if (usermodel == null)
            {
                return HttpNotFound();
            }
            return View(usermodel);
        }

        //
        // GET: /User/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /User/Create

        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserModel usermodel)
        {
            if (ModelState.IsValid)
            {
                rep.Post(ref usermodel);
                return RedirectToAction("Index");
            }

            return View(usermodel);
        }

        //
        // GET: /User/Edit/5

        public ActionResult Edit(string id)
        {
            UserModel usermodel = rep.Get(id); 
            if (usermodel == null)
            {
                return HttpNotFound();
            }
            return View(usermodel);
        }

        //
        // POST: /User/Edit/5

        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserModel usermodel)
        {
            if (ModelState.IsValid)
            {
               rep.Update(usermodel);
               return RedirectToAction("Index");
            }
            return View(usermodel);
        }

        //
        // GET: /User/Delete/5

        public ActionResult Delete(string id)
        {
            UserModel usermodel = rep.Get(id); 
            if (usermodel == null)
            {
                return HttpNotFound();
            }
            return View(usermodel);
        }

        //
        // POST: /User/Delete/5

        [System.Web.Mvc.HttpPost, System.Web.Mvc.ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            rep.Delete(id);
            return RedirectToAction("Index");
        }

        public ActionResult ValidatePassword(string id)
        {
            UserModel usermodel = rep.Get(id);
            if (usermodel == null)
            {
                return HttpNotFound();
            }
            return View(usermodel);
        }

        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public UserModel ValidatePassword(string userName, string password)
        {
            UserModel model = rep.Validate(userName, password);
            if (model == null)
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));

            return model;
        }

        public ActionResult ChangePassword(string id)
        {
            UserModel usermodel = rep.Get(id);
            if (usermodel == null)
            {
                return HttpNotFound();
            }
            return View(usermodel);
        }

        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public UserModel ChangePassword(string userName, string password, string newPassword)
        {
            UserModel model = rep.Validate(userName, password);
            if (model == null)
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            model = rep.ChangePassword(model, newPassword);
            return model;
        }

        public ActionResult GetProximityUsers(string id)
        {
            UserModel usermodel = rep.Get(id);
            if (usermodel == null)
            {
                return HttpNotFound();
            }
            return View(usermodel);
        }

        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public List<UserModel> GetProximityUsers(string userid, double lat, double lon)
        {
            UserModel model = rep.Get(userid);
            if (model == null)
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            List<UserModel> models = rep.GetProximityUsers(userid, lat, lon);
            return models;
        }
    }
}