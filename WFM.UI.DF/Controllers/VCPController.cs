using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WFM.BAL.Services;
using WFM.DAL;
using WFM.UI.DF.ModelsView;
using System.Configuration;
using System.Net.Configuration;
using System.Net.Mail;
using System.Net;

namespace WFM.UI.DF.Controllers
{
    public class VCPController : BaseController
    {
        private ApplicationUserManager _userManager;
        private readonly CountryService countryService = new CountryService();
        private readonly VCPService vcpService = new VCPService();

        // GET: VCP
        public VCPController()
        {
        }

        public VCPController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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

        // GET: WFM_VCP
        public ActionResult Index(int? id)
        {
            WFM_VCP vcp = new WFM_VCP();
            if (id != null)
            {
                vcp = vcpService.GetVCPById(id);
            }

            ViewBag.CountryList = countryService.GetCountryList();

            return View(vcp);
        }

        public ActionResult GetList()
        {
            var list = vcpService.GetVCPList(0);
            List<VCPView> modelList = new List<VCPView>();
            foreach (var item in list)
            {
                modelList.Add(new VCPView()
                {
                    Id = item.Id,
                    AddressLine1 = item.AddressLine1,
                    AddressLine2 = item.AddressLine2,
                    City = item.City,
                    Province = item.Province,
                    Postcode = item.Postcode,
                    Email = item.Email,
                    Website = item.Website,
                    IsActive = item.IsActive,
                    CountryName = "",
                    Name = item.Name
                });
            }

            return Json(new { data = modelList }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SaveOrUpdate(WFM_VCP model)
        {
            string newData = string.Empty, oldData = string.Empty;

            try
            {
                int id = model.Id;
                WFM_VCP vcp = null;
                WFM_VCP oldVCP = null;
                if (model.Id == 0)
                {
                    vcp = new WFM_VCP
                    {
                        Name = model.Name,
                        AddressLine1 = model.AddressLine1,
                        AddressLine2 = model.AddressLine2,
                        City = model.City,
                        Province = model.Province,
                        Postcode = model.Postcode,
                        CountryId = model.CountryId,
                        Email = model.Email,
                        Website = model.Website,
                        IsActive = true
                    };

                    oldVCP = new WFM_VCP();
                    oldData = new JavaScriptSerializer().Serialize(oldVCP);
                    newData = new JavaScriptSerializer().Serialize(vcp);
                }
                else
                {
                    vcp = vcpService.GetVCPById(model.Id);
                    oldVCP = vcpService.GetVCPById(model.Id);

                    oldData = new JavaScriptSerializer().Serialize(new WFM_VCP()
                    {
                        Id = oldVCP.Id,
                        Name = oldVCP.Name,
                        AddressLine1 = oldVCP.AddressLine1,
                        AddressLine2 = oldVCP.AddressLine2,
                        City = oldVCP.City,
                        Province = oldVCP.Province,
                        Postcode = oldVCP.Postcode,
                        CountryId = oldVCP.CountryId,
                        Email = oldVCP.Email,
                        Website = oldVCP.Website,
                        IsActive = oldVCP.IsActive
                    });

                    vcp.Name = model.Name;
                    vcp.AddressLine1 = model.AddressLine1;
                    vcp.AddressLine2 = model.AddressLine2;
                    vcp.City = model.City;
                    vcp.Postcode = model.Postcode;
                    vcp.Province = model.Province;
                    vcp.CountryId = model.CountryId;
                    vcp.Website = model.Website;
                    vcp.Email = model.Email;
                    bool Example = Convert.ToBoolean(Request.Form["IsActive.Value"]);
                    vcp.IsActive = model.IsActive;

                    newData = new JavaScriptSerializer().Serialize(new WFM_VCP()
                    {
                        Id = vcp.Id,
                        Name = vcp.Name,
                        AddressLine1 = vcp.AddressLine1,
                        AddressLine2 = vcp.AddressLine2,
                        City = vcp.City,
                        Province = vcp.Province,
                        Postcode = vcp.Postcode,
                        CountryId = vcp.CountryId,
                        Email = vcp.Email,
                        Website = vcp.Website,
                        IsActive = vcp.IsActive
                    });
                }

                vcpService.SaveOrUpdate(vcp);

                CommonService.SaveDataAudit(new DataAudit()
                {
                    Entity = "WFM_VCP",
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
            return RedirectToAction("Index", "VCP");
        }

        public ActionResult IFPEmail()
        {
            return View();
        }

        public ActionResult SendEmail(EmailModelView model)
        {
            //MailAddressCollection mailAddressCollection = new MailAddressCollection();

            SmtpSection smtpSection = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");

            var VCPList = vcpService.GetVCPList(1);

            foreach (var vcp in VCPList)
            {            
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(smtpSection.From);
                    mail.To.Add(vcp.Email);
                    mail.CC.Add(model.CC);
                    mail.Bcc.Add(model.BCC);
                    mail.Subject = model.Subject;
                    mail.Body = "Dear " + vcp.Name + 
                        ",<br /><br />" +
                        model.Body + 
                        "<br /><br/>" +
                        "I trust this email finds you well.<br /><br />" +
                        "We are pleased to inform you that we have a promising investment/ " +
                        "bidding opportunity available, with comprehensive details outlined." +
                        "As part of our valued network, we would like to formally extend an " +
                        "invitation for you to participate in this venture.<br /><br />" +
                        "Should you require any further information or clarification, please do not hesitate to reach out to us. We are more than happy to provide any additional details you may need to assist in your decision-making.<br />" +
                        "Thank you for considering this opportunity, and we look forward to the possibility of working together.<br /><br />" +
                        "Warm regards,<br />" +
                        "OSL Global(Pvt) Ltd.";

                    mail.IsBodyHtml = true;
                    //mail.Attachments.Add(new Attachment("C:\\Users\\Suga\\Downloads\\BIADP - Phase II Stage II.pdf")); //Server.MapPath(model.Attachment)));

                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new NetworkCredential(smtpSection.Network.UserName, smtpSection.Network.Password);
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }
                }
            }

            return View();
        }
    }
}