using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WFM.BAL.Services;
using WFM.UI.DF.Models;

namespace WFM.UI.DF.Controllers
{
    [Authorize]
    public class MinistryController : Controller
    {
        private ApplicationUserManager _userManager;
        private readonly MDMinistryService mdMinistryService = new MDMinistryService();


        public MinistryController()
        {
        }

        public MinistryController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: Member
        public ActionResult GetList()
        {
            var list = mdMinistryService.GetMinistryList(null,null);

            return Json(new { data = list }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult View(int? id)
        {
            var ministry = mdMinistryService.GetMinistryList(null, id).SingleOrDefault();
            ViewBag.LineAgencies = mdMinistryService.GetMinistryLineAgencies(null, id);
            return View(ministry);
        }
    }
}