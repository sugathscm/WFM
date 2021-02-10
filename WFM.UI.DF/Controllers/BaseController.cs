using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WFM.BAL.Services;

namespace WFM.UI.DF.Controllers
{
    public class BaseController : Controller
    {
        private readonly ProjectTypeService projectTypeService = new ProjectTypeService();

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);

            ViewBag.ProjectTypeList = projectTypeService.GetProjectTypeList().Where(p => p.ShowInMenu == true).ToList();

        }
    }
}