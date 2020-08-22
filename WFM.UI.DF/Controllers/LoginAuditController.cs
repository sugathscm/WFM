using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WFM.BAL.Services;
using WFM.UI.DF.Models;
using WFM.UI.DF.ModelsView;

namespace WFM.UI.DF.Controllers
{
    public class LoginAuditController : Controller
    {
        private ApplicationDbContext context;
    
        public LoginAuditController()
        {
            context = new ApplicationDbContext();
        }
        // GET: DataAudit
        public ActionResult Index()
        {
            var users = (from user in context.Users
                         select new
                         {
                             UserId = user.Id,
                             Username = user.UserName,
                             Email = user.Email
                         }).ToList().Select(p => new UsersInRoleViewModel()

                         {
                             UserId = p.UserId,
                             Username = p.Username,
                             Email = p.Email
                         });

            LoginAuditViewModel loginDataViewModel = new LoginAuditViewModel
            {
                UsersInRoleViewModels = users.ToList()
            };

            return View(loginDataViewModel);
        }

        public ActionResult GetList(Guid userId)
        {
            var list = CommonService.GetLoginAuditByUser(userId);

            List<LoginAuditViewModel> modelList = new List<LoginAuditViewModel>();

            foreach (var item in list)
            {
                modelList.Add(new LoginAuditViewModel()
                {
                    UserId = item.UserId.Value,
                    UserName = "",
                    IPAddress = item.IPAddress,
                    DateLogged = item.DateLogged.ToString()
                });
            }

            return Json(new { data = modelList }, JsonRequestBehavior.AllowGet);
        }
    }

}