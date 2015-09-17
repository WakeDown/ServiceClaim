using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ServiceClaim.Models;
using ServiceClaim.Objects;

namespace ServiceClaim.Controllers
{
    [Compress]
    public class ServiceIssueController : BaseController
    {
        // GET: Issue
        public ActionResult Planing(DateTime? month)
        {
            if (!month.HasValue) return RedirectToAction("Planing", new {month=$"{DateTime.Now:yyyy-MM-dd}"});
            
            ViewBag.IssueCityList = ServiceIssue.GetPlaningCityList(month.Value);

            return View();
        }

        [HttpPost]
        public JsonResult GetAddressList(DateTime? month, int? idCity)
        {
            if (!idCity.HasValue) return Json(new {});
            if (!month.HasValue) month = DateTime.Now;
            var list = ServiceIssue.GetPlaningAddressList(month.Value, idCity.Value);
            return Json(list);
        }

        [HttpPost]
        public JsonResult GetClientList(DateTime? month, int? idCity, string address)
        {
            if (!idCity.HasValue) return Json(new { });
            if (!month.HasValue) month = DateTime.Now;
            var list = ServiceIssue.GetPlaningClientList(month.Value, idCity.Value, address);
            return Json(list);
        }

        [HttpPost]
        public JsonResult GetDeviceList(DateTime? month, int? idCity, string address, int? idClient)
        {
            if (!idCity.HasValue) return Json(new { });
            if (!idClient.HasValue) return Json(new { });
            if (!month.HasValue) month = DateTime.Now;
            var list = ServiceIssue.GetPlaningDeviceList(month.Value, idCity.Value, address, idClient.Value);
            return Json(list);
        }
    }
}