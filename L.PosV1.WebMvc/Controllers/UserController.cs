using L.PosV1.DataAccess.Repo;
using L.PosV1.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace L.PosV1.WebMvc.Controllers
{
    public class UserController : Controller
    {
        protected IUserRepo UserRepo;
        public UserController(IUserRepo _UserRepo)
        {
            UserRepo = _UserRepo;
        }

        // GET: User
        public ActionResult Index()
        {
            IList<User> lstUser = UserRepo.GetAll();
            return View(lstUser);
        }

        // GET: User/Details/5
        public ActionResult Details(string id)
        {
            User usr = UserRepo.GetBy(x => x.tID == id);
            return View(usr);
        }

        // GET: User/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        [HttpPost]
        public ActionResult Create(User collection)
        {
            try
            {
                // TODO: Add insert logic here
                UserRepo.BeginTransaction();
                UserRepo.Save(collection);
                UserRepo.Commit();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        // GET: User/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: User/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: User/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: User/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
