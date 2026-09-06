using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WFM.BAL.Services;
using System.Web.Script.Serialization;
using WFM.DAL;
using WFM.UI.DF.Models;

namespace WFM.UI.DF.Controllers
{
    public class RelationshipController : Controller
    {
        private ApplicationUserManager _userManager;
        private readonly MDMemberService memberService = new MDMemberService();

        public RelationshipController()
        {
        }

        public RelationshipController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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

        // GET: Relationship
        public ActionResult Index()
        {
            ViewBag.MemberList = memberService.GetMemberList();
            return View();
        }

        public ActionResult GetListByPerson(int? personId)
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SaveOrUpdateRelationship(RelationshipViewModel model)
        {
            string newData = string.Empty, oldData = string.Empty;

            try
            {
                int id = model.Id;
                MD_MemberRelationship memberRelationship = null;

                if (model.Id == 0)
                {
                    memberRelationship = new MD_MemberRelationship
                    {
                        MemberId = model.MemberId,
                        RelatedMemberId = model.RelatedMemberId,
                    };
                }

                memberService.SaveOrUpdate(memberRelationship);

                TempData["Message"] = "<div id='flash-success'>Record Saved Successfully.</div>";
            }
            catch (Exception ex)
            {
                TempData["Message"] = "<span id='flash-error'>Error.</span>" + ex.InnerException;
            }

            return RedirectToAction("Index", "Designation");
        }
    }
}