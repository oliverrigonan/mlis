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

        public ActionResult CustomerLoanDetail()
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

        public ActionResult CollectorList()
        {
            return View();
        }

        public ActionResult LoanList()
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

        public ActionResult LoanDetail()
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

        public ActionResult CollectionList()
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

        public ActionResult CollectionDetail()
        {
            return View();
        }

        public ActionResult UserList()
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

        public ActionResult UserDetail()
        {
            return View();
        }

        public ActionResult Reports()
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
    }
}