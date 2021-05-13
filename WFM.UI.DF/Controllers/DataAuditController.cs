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
    [Authorize]
    public class DataAuditController : BaseController
    {
        private ApplicationDbContext context;

        public DataAuditController()
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

            DataAuditViewModel loginDataViewModel = new DataAuditViewModel
            {
                UsersInRoleViewModels = users.ToList()
            };

            return View(loginDataViewModel);
        }

        public ActionResult GetList(Guid userId)
        {
            var list = CommonService.GetDataAuditByUser(userId);

            List<DataAuditViewModel> modelList = new List<DataAuditViewModel>();

            foreach (var item in list)
            {
                modelList.Add(new DataAuditViewModel()
                {
                    UserId = item.UserId.Value,
                    UserName = "",
                    Entity = item.Entity,
                    OldData = item.OldData,
                    NewData = item.NewData,
                    UpdatedOn = ""
                });
            }

            return Json(new { data = modelList }, JsonRequestBehavior.AllowGet);
        }

    }

}