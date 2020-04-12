using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WFM.BAL;
using WFM.DAL;
using WFM.UI.DF;
using WFM.UI.ModelsView;

namespace WFM.UI.Controllers
{
    public class EmployeeController : Controller
    {
        private ApplicationUserManager _userManager;

        public EmployeeController()
        {

        }
        public EmployeeController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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

        // GET: Employee
        public ActionResult Index(int? id)
        {
            Employee employee = new Employee();

            using (WorkFlowEntities entities = new WorkFlowEntities())
            {
                if(id !=null)
                {
                    employee = entities.Employees.Where(e => e.Id == id).SingleOrDefault();
                }
                var listData = entities.Designations.Select(s => new { Id = s.Id, Value = s.Name }).ToList();
                ViewBag.ListObject = new SelectList(listData, "Id", "Value");
            }
                return View(employee);
        }

        public ActionResult GetList()
        {
            using (WorkFlowEntities entities = new WorkFlowEntities())
            {
                var list = entities.Employees.OrderBy(o => o.Name).ToList();
                List<EmployeeView> modelList = new List<EmployeeView>();
                foreach (var item in list)
                {
                    modelList.Add(new EmployeeView()
                    {
                        Id = item.Id,
                        IsActive = item.IsActive,
                        Title = item.Title,
                        Name = item.Name,
                        Mobile = item.Mobile,
                        Email = item.Email,
                        FixedLine = item.FixedLine,
                        DesignationName = (item.DesignationId == 0) ? "" : entities.Designations.Where(o => o.Id == item.DesignationId).SingleOrDefault().Name,
                    });
                }
                return Json(new { data = modelList }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SaveOrUpdate(Employee model)
        {
            string newData = string.Empty, oldData = string.Empty;
            using (WorkFlowEntities entities = new WorkFlowEntities())
            {
                try
                {
                    int id = model.Id;
                    Employee employee = null;
                    Employee oldEmployee = null;
                    if (model.Id == 0)
                    {
                        employee = new Employee
                        {
                            Title = model.Title,
                            Name = model.Name,
                            Mobile = model.Mobile,
                            Email = model.Email,
                            FixedLine = model.FixedLine,
                            IsActive = true,
                            DesignationId = model.DesignationId
                        };

                        entities.Employees.Add(employee);
                        entities.SaveChanges();

                        oldEmployee = new Employee();
                        oldData = new JavaScriptSerializer().Serialize(oldEmployee);
                        newData = new JavaScriptSerializer().Serialize(employee);
                    }
                    else
                    {
                        employee = entities.Employees.Where(o => o.Id == model.Id).SingleOrDefault();
                        oldEmployee = entities.Employees.Where(o => o.Id == model.Id).SingleOrDefault();

                        oldData = new JavaScriptSerializer().Serialize(new Employee()
                        {
                            Id = oldEmployee.Id,
                            Title = oldEmployee.Title,
                            Name = oldEmployee.Name,
                            Mobile = oldEmployee.Mobile,
                            Email = oldEmployee.Email,
                            FixedLine = oldEmployee.FixedLine,
                            DesignationId = oldEmployee.DesignationId,
                            IsActive = oldEmployee.IsActive
                        });

                        employee.Title = model.Title;
                        employee.Name = model.Name;
                        employee.Mobile = model.Mobile;
                        employee.Email = model.Email;
                        employee.FixedLine = model.FixedLine;
                        employee.DesignationId = model.DesignationId;
                        bool Example = Convert.ToBoolean(Request.Form["IsActive.Value"]);
                        employee.IsActive = model.IsActive;

                        newData = new JavaScriptSerializer().Serialize(new Employee()
                        {
                            Id = employee.Id,
                            Title = employee.Title,
                            Name = employee.Name,
                            Mobile = employee.Mobile,
                            Email = employee.Email,
                            FixedLine = employee.FixedLine,
                            DesignationId = employee.DesignationId,
                            IsActive = employee.IsActive
                        });

                        entities.Entry(employee).State = System.Data.Entity.EntityState.Modified;
                        entities.SaveChanges();
                    }

                    CommonService.SaveDataAudit(new DataAudit()
                    {
                        Entity = "Employee",
                        NewData = newData,
                        OldData = oldData,
                        UpdatedOn = DateTime.Now,
                        UserId = new Guid(User.Identity.GetUserId())
                    });

                    TempData["Message"] = "<div id='flash-success'>Record Saved Successfully.</div>";
                }
                catch (Exception ex)
                {
                    TempData["Message"] = "<span id='flash-error'>Error.</span>" + ex.InnerException;
                }
            }

            return RedirectToAction("Index", "Employee");
        }






    }
}