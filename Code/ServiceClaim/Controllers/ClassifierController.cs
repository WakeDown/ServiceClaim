using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using ServiceClaim.Models;
using ServiceClaim.Objects;

namespace ServiceClaim.Controllers
{
    public class ClassifierController : BaseController
    {
        [HttpPost]
        public ActionResult Attributes(ClassifierAttributes attrs)
        {
            if (!CurUser.HasAccess(AdGroup.ServiceClaimClassifier, AdGroup.ServiceControler)) RedirectToAction("AccessDenied", "Error");
            try
            {
                ResponseMessage responseMessage;
                bool complete = attrs.Save(out responseMessage);
                if (!complete) throw new Exception(responseMessage.ErrorMessage);
            }
            catch (Exception ex)
            {
                TempData["ServerErrorAttr"] = ex.Message;
                return RedirectToAction("List");
            }

            return RedirectToAction("List");
        }

        public ActionResult List()
        {
            if (!CurUser.HasAccess(AdGroup.ServiceClaimClassifier, AdGroup.ServiceControler)) RedirectToAction("AccessDenied", "Error");

            return View();
        }

        public ActionResult ExportExcel()
        {
            if (!CurUser.HasAccess(AdGroup.ServiceClaimClassifier)) RedirectToAction("AccessDenied", "Error");
            MemoryStream stream=new MemoryStream(new byte[0]);
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        [HttpPost]
        public ActionResult ImportExcel()
        {
            if (!CurUser.HasAccess(AdGroup.ServiceClaimClassifier)) RedirectToAction("AccessDenied", "Error");
            //if (!user.UserCanEdit()) return RedirectToAction("AccessDenied", "Error");
            int id = 0;
            if (Request.Files.Count > 0)
            {
                try
                {
                    for (int i = 0; i < 1; i++)
                    {
                        var file = Request.Files[i];
                        if (file.ContentLength <= 0)
                        {
                            throw new ArgumentException("Файл не выбран. Выберите файл!");
                        }

                        if (Path.GetExtension(file.FileName) != ".xlsx" && Path.GetExtension(file.FileName) != ".xls")
                        {
                            throw new ArgumentException("Файл не был загружен. Формат файла отличается от XLS и XLSX.");
                        }

                        byte[] fileData = null;
                        using (var br = new BinaryReader(file.InputStream))
                        {
                            fileData = br.ReadBytes(file.ContentLength);
                        }


                        //var doc = new Document() { Data = fileData, Name = file.FileName };
                        ResponseMessage responseMessage;
                        Classifier.SaveFromExcel(fileData, out responseMessage);
                        //bool complete = doc.Save(out responseMessage);
                        //if (!complete) throw new Exception(responseMessage.ErrorMessage);
                        //TempData["noPdf"] = noPdf;
                    }
                }
                catch (Exception ex)
                {
                    TempData["ServerErrorExcel"] = ex.Message;
                    return RedirectToAction("List");
                }
            }
            return RedirectToAction("List");
        }
    }
}