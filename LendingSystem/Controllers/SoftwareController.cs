using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LendingSystem.Controllers
{
    public class SoftwareController : Controller
    {
        // GET: Software
        public ActionResult Index()
        {
            return View();
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