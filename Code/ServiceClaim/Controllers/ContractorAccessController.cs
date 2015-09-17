using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SelectPdf;
using ServiceClaim.Models;
using ServiceClaim.Objects;

namespace ServiceClaim.Controllers
{
    public class ContractorAccessController : BaseController
    {
        [HttpGet]
        public ActionResult List()
        {
            if (!CurUser.HasAccess(AdGroup.ServiceClaimContractorAccess, AdGroup.ServiceControler)) RedirectToAction("AccessDenied", "Error");
            //var list = UserList.GetUserSelectionList(AdGroup.ZipClaimClient);
            var list = ContractorAccess.GetList().OrderBy(x => x.City).ThenBy(x => x.OrgName).ThenBy(x => x.Name);
            return View(list);
        }

        [HttpGet]
        public ActionResult New()
        {
            if (!CurUser.HasAccess(AdGroup.ServiceClaimContractorAccess, AdGroup.ServiceControler)) RedirectToAction("AccessDenied", "Error");
            //var model = new ClientAccess();
            return View();
        }

        [HttpPost]
        public ActionResult New(ContractorAccess model)
        {
            if (!CurUser.HasAccess(AdGroup.ServiceClaimContractorAccess)) RedirectToAction("AccessDenied", "Error");
            try
            {
                ResponseMessage responseMessage;
                bool complete = model.Save(out responseMessage);
                if (!complete) throw new Exception(responseMessage.ErrorMessage);

                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return View("New", model);
            }
        }

        //[HttpPost]
        //public JsonResult GetOrgs()
        //{
        //    var list = (IDictionary<string, string>)ContractorAccess.GetOrgList();
        //    return Json(list);
        //}

        public ActionResult AccessSheetPdf(int id)
        {
            if (!CurUser.HasAccess(AdGroup.ServiceClaimContractorAccess, AdGroup.ServiceControler)) RedirectToAction("AccessDenied", "Error");
            HtmlToPdf converter = new HtmlToPdf();

            string url = Url.Action("AccessSheet", new { id = id });
            var leftPartUrl = String.Format("{0}://{1}:{2}", Request.RequestContext.HttpContext.Request.Url.Scheme, Request.RequestContext.HttpContext.Request.Url.Host, Request.RequestContext.HttpContext.Request.Url.Port);
            url = String.Format("{1}{0}", url, leftPartUrl);
            PdfDocument doc = converter.ConvertUrl(url);
            MemoryStream stream = new MemoryStream();
            doc.Save(stream);
            return File(stream.ToArray(), "application/pdf");
        }

        public ActionResult AccessSheet(int id)
        {
            var model = new ContractorAccess(id);
            return View(model);
        }

        [HttpPost]
        public void Delete(int id)
        {
            if (!CurUser.HasAccess(AdGroup.ServiceClaimContractorAccess)) RedirectToAction("AccessDenied", "Error");
            try
            {
                ResponseMessage responseMessage;
                bool complete = ContractorAccess.Delete(id, out responseMessage);
                if (!complete) throw new Exception(responseMessage.ErrorMessage);
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
            }
        }
    }
}