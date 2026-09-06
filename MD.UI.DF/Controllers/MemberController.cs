using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WFM.BAL.Services;
using WFM.DAL;
using WFM.UI.DF.Models;

namespace WFM.UI.DF.Controllers
{
    public class MemberController : Controller
    {
        private ApplicationUserManager _userManager;
        private readonly MDMemberService memberService = new MDMemberService();

        public MemberController()
        {
        }

        public MemberController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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
        public ActionResult Index(int? id)
        {
            MD_Member division = new MD_Member();
            if (id != null)
            {
                division = memberService.GetMemberById(id);
            }
            return View(division);
        }

        public ActionResult GetList()
        {
            List<MD_Member> list = memberService.GetMemberList();

            List<MemberViewModel> modelList = new List<MemberViewModel>();

            foreach (var item in list)
            {
                modelList.Add(new MemberViewModel() { Id = item.Id, StatusId = item.StatusId, Name = item.Name });
            }

            return Json(new { data = modelList }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SaveOrUpdate(MD_Member model)
        {
            string newData = string.Empty, oldData = string.Empty;

            try
            {
                int id = model.Id;
                MD_Member member = null;
                if (model.Id == 0)
                {
                    member = new MD_Member
                    {
                        Name = model.Name,
                        StatusId = 1
                    };
                }
                else
                {
                    member = memberService.GetMemberById(model.Id);
                    member.Name = model.Name;
                }

                memberService.SaveOrUpdate(member);

                TempData["Message"] = "<div id='flash-success'>Record Saved Successfully.</div>";
            }
            catch (Exception ex)
            {
                TempData["Message"] = "<span id='flash-error'>Error.</span>" + ex.InnerException;
            }

            return RedirectToAction("Index", "Member");
        }
    }
}