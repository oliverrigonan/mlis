using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LendingSystem.Controllers
{
    public class SoftwareController : Controller
    {
        public Data.lendingsystemDataContext db = new Data.lendingsystemDataContext();

        public ActionResult Index()
        {
            var currentUser = from d in db.MstUsers
                              where d.AspNetUserId == User.Identity.GetUserId()
                              select d;

            if (currentUser.FirstOrDefault().UserType == "Customer")
            {
                return Redirect("/Software/CustomerProfile");
            }
            else
            {
                return View();
            }
        }

        public ActionResult CustomerProfile()
        {
            var currentUser = from d in db.MstUsers
                              where d.AspNetUserId == User.Identity.GetUserId()
                              select d;

            if (currentUser.FirstOrDefault().UserType == "Admin")
            {
                return Redirect("/Software");
            }
            else
            {
                return View();
            }
        }

        public ActionResult CustomerList()
        {
            return View();
        }

        public ActionResult CollectorList()
        {
            return View();
        }

        public ActionResult LoanList()
        {
            return View();
        }

        public ActionResult LoanDetail()
        {
            return View();
        }

        public ActionResult CollectionList()
        {
            return View();
        }

        public ActionResult CollectionDetail()
        {
            return View();
        }

        public ActionResult UserList()
        {
            return View();
        }

        public ActionResult UserDetail()
        {
            return View();
        }
    }
}