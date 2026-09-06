using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using WFM.BAL.Services;
using WFM.UI.DF.Models;
using WFM.UI.DF.ModelsView;

namespace WFM.UI.DF.Controllers
{
    public class AccountUserController : BaseController
    {
        private ApplicationDbContext context;
        private readonly EmployeeService employeeService;

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountUserController()
        {
            context = new ApplicationDbContext();
            employeeService = new EmployeeService();
        }

        public AccountUserController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;

        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
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


        // GET: AccountUser
        public ActionResult Index()
        {
            var usersWithRoles = (from user in context.Users
                                  select new
                                  {
                                      UserId = user.Id,
                                      Username = user.UserName,
                                      Email = user.Email,
                                      RoleNames = (from userRole in user.Roles
                                                   join role in context.Roles on userRole.RoleId
                                                   equals role.Id
                                                   select role.Name).ToList(),
                                      LockoutEnabled = user.LockoutEnabled,
                                  }).ToList().Select(p => new UsersInRoleViewModel()
                                  {
                                      UserId = p.UserId,
                                      Username = p.Username,
                                      Email = p.Email,
                                      Role = string.Join(",", p.RoleNames),
                                      Employee = ((employeeService.GetEmployeeByUserId(p.UserId)) != null)?(employeeService.GetEmployeeByUserId(p.UserId)).Name:"",
                                      LockoutEnabled = p.LockoutEnabled ? "Yes" : "No"
                                  });
            return View(usersWithRoles);
        }



        [AllowAnonymous]
        [HttpGet]
        public ActionResult ResetPassword() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            // Get the logged-in user details
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null) return RedirectToAction("Login");

            // Change the password using built-in Identity logic
            //var result = await _userManager.ChangePasswordAsync(user.Id, model.CurrentPassword, model.ConfirmPassword);


            //var user1 = await UserManager.FindByIdAsync(user.Id);

            var token = await UserManager.GeneratePasswordResetTokenAsync(user.Id);

            var result = await UserManager.ResetPasswordAsync(user.Id, token, model.ConfirmPassword);

            if (result.Succeeded)
            {
                // Refresh cookie security stamps to avoid immediate logout
                //await _signInManager.RefreshSignInAsync(user);
                TempData["SuccessMessage"] = "Your password has been changed.";
                return RedirectToAction("Index", "Home");
            }

            //foreach (var error in result.Errors)
            //{
            //    ModelState.AddModelError(string.Empty, error.Description);
            //}
            return View(model);
        }


        [HttpPost]
        [Authorize]
        //[ValidateAntiForgeryToken]
        public ActionResult DeactivateUser(string id)
        {
            var errors = ModelState.Where(x => x.Value.Errors.Count > 0).Select(x => new { x.Key, x.Value.Errors });
            if (ModelState.IsValid)
            {
                var roleStore = new RoleStore<IdentityRole>(new ApplicationDbContext());
                var roleMngr = new RoleManager<IdentityRole>(roleStore);

                var existingUser = UserManager.FindById(id);
                if (existingUser != null)
                {
                    //var token = await UserManager.GeneratePasswordResetTokenAsync(existingUser.Id);
                    existingUser.LockoutEnabled = true;
                    UserManager.Update(existingUser);
                }

                return Json(new { Status = 1, Message = "" });
            }

            return Json(new { Status = 0, Message = "" });
        }


        [HttpPost]
        [Authorize]
        //[ValidateAntiForgeryToken]
        public ActionResult ActivateUser(string id)
        {
            var errors = ModelState.Where(x => x.Value.Errors.Count > 0).Select(x => new { x.Key, x.Value.Errors });
            if (ModelState.IsValid)
            {
                var roleStore = new RoleStore<IdentityRole>(new ApplicationDbContext());
                var roleMngr = new RoleManager<IdentityRole>(roleStore);

                var existingUser = UserManager.FindById(id);
                if (existingUser != null)
                {
                    //var token = await UserManager.GeneratePasswordResetTokenAsync(existingUser.Id);
                    existingUser.LockoutEnabled = false;
                    UserManager.Update(existingUser);
                }

                return Json(new { Status = 1, Message = "" });
            }

            return Json(new { Status = 0, Message = "" });
        }
    }
}