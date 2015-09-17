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
    public class ClientAccessController : BaseController
    {
        public ActionResult List()
        {
            if (!CurUser.HasAccess(AdGroup.ServiceClaimClientAccess, AdGroup.ServiceControler)) RedirectToAction("AccessDenied", "Error");
            //var list = UserList.GetUserSelectionList(AdGroup.ZipClaimClient);
            var list = ClientAccess.GetList().OrderBy(x=>x.Name);
            return View(list);
        }

        [HttpGet]
        public ActionResult New()
        {
            if (!CurUser.HasAccess(AdGroup.ServiceClaimClientAccess, AdGroup.ServiceControler)) RedirectToAction("AccessDenied", "Error");
            //var model = new ClientAccess();
            return View();
        }

        [HttpPost]
        public ActionResult New(ClientAccess model)
        {
            if (!CurUser.HasAccess(AdGroup.ServiceClaimClientAccess)) RedirectToAction("AccessDenied", "Error");
            try
            {
                ResponseMessage responseMessage;
                //dep.Creator = new Employee(){AdSid = GetCurUser().Sid};
                string ctrIdStr = Request.Form["ctrList"];
                int idClientEtalon;
                int.TryParse(ctrIdStr, out idClientEtalon);
                model.IdClientEtalon = idClientEtalon;
                bool complete = model.SaveNew(out responseMessage);
                if (!complete) throw new Exception(responseMessage.ErrorMessage);

                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return View("New", model);
            }
        }

        [HttpPost]
        public ActionResult Edit(ClientAccess model)
        {
            if (!CurUser.HasAccess(AdGroup.ServiceClaimClientAccess)) RedirectToAction("AccessDenied", "Error");
            try
            {
                ResponseMessage responseMessage;
                bool complete = model.Update(out responseMessage);
                if (!complete) throw new Exception(responseMessage.ErrorMessage);

                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return View("Edit", model);
            }
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (!CurUser.HasAccess(AdGroup.ServiceClaimClientAccess, AdGroup.ServiceControler)) RedirectToAction("AccessDenied", "Error");
            var model = new ClientAccess(id);
            return View(model);
        }

        [HttpPost]
        public JsonResult GetCtors(string contractorName)
        {
            var list = Contractor.GetServiceList(contractorName: contractorName);
            return Json(list);
        }

        public ActionResult ClientSheetPdf(int id)
        {
            if (!CurUser.HasAccess(AdGroup.ServiceClaimClientAccess, AdGroup.ServiceControler)) RedirectToAction("AccessDenied", "Error");
            HtmlToPdf converter = new HtmlToPdf();

            string url = Url.Action("ClientSheet", new { id = id });
            var leftPartUrl = String.Format("{0}://{1}:{2}", Request.RequestContext.HttpContext.Request.Url.Scheme, Request.RequestContext.HttpContext.Request.Url.Host, Request.RequestContext.HttpContext.Request.Url.Port);
            url = String.Format("{1}{0}", url, leftPartUrl);
            PdfDocument doc = converter.ConvertUrl(url);
            MemoryStream stream = new MemoryStream();
            doc.Save(stream);
            return File(stream.ToArray(), "application/pdf");
        }

        public ActionResult ClientSheet(int id)
        {
            var model = new ClientAccess(id);
            return View(model);
        }

        [HttpPost]
        public void Delete(int id)
        {
            if (!CurUser.HasAccess(AdGroup.ServiceClaimClientAccess)) RedirectToAction("AccessDenied", "Error");
            try
            {
                ResponseMessage responseMessage;
                bool complete = ClientAccess.Delete(id, out responseMessage);
                if (!complete) throw new Exception(responseMessage.ErrorMessage);
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
            }
        }
    }
}