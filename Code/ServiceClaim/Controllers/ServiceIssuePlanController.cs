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
    public class ServiceIssuePlanController : BaseController
    {
        // GET: Issue
        public ActionResult Planing(DateTime? month, string serviceEngeneerSid)
        {
            if (!month.HasValue) return RedirectToAction("Planing", new { month = $"{DateTime.Now:yyyy-MM}" });

            ViewBag.IssueCityList = ServiceIssue.GetPlaningCityList(month.Value, serviceEngeneerSid);

            return View();
        }

        [HttpPost]
        public JsonResult GetAddressList(DateTime? month, int? idCity)
        {
            if (!idCity.HasValue) return Json(new { });
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
        public JsonResult GetDeviceIssueList(DateTime? month, int? idCity, string address, int? idClient)
        {
            if (!idCity.HasValue) return Json(new { });
            if (!idClient.HasValue) return Json(new { });
            if (!month.HasValue) month = DateTime.Now;
            var list = ServiceIssue.GetPlaningDeviceIssueList(month.Value, idCity.Value, address, idClient.Value);
            return Json(list);
        }

        [HttpPost]
        public JsonResult SaveServiceIssuePlan(int serviceIssueId, string periodStr)
        {
            if (serviceIssueId <= 0) return Json(new { error = "Не указана заявка" });
            var dts = periodStr.Split('|');
            if (!dts.Any()) return Json(new { error = "Не указан период" });

            DateTime periodStart = DateTime.Parse(dts[0]);
            DateTime periodEnd = DateTime.Parse(dts[1]);
            ServiceIssuePlan plan = new ServiceIssuePlan(serviceIssueId, 1, periodStart, periodEnd);
            try
            {
                ResponseMessage responseMessage;
                bool result = plan.Save(out responseMessage);
                if (!result) throw new Exception(responseMessage.ErrorMessage);
                plan.Id = responseMessage.Id;
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
            return Json(plan);
        }

        public PartialViewResult PeriodWeekView(int? year, int? month)
        {
            var periodList = ServiceIssuePlan.GetPeriodMonthList(year.Value, month.Value);
            return PeriodWeekView(periodList);
        }

        public PartialViewResult PeriodWeekView(IEnumerable<ServiceIssuePeriodItem> periodList)
        {
            return PartialView("PeriodWeekView", periodList);
        }

        public ActionResult GetServiceIssuePlanItem(int? id)
        {
            if (!id.HasValue) return HttpNotFound();
            var issue = ServiceIssuePlan.Get(id.Value);
            return PartialView("ServiceIssuePlanItem", issue);
        }

        //public JsonResult CheckServiceIssueIsExists(int idServiceIssue, int idServiceIssueType)
        //{
        //    var model = ServiceIssuePlan.Get(idServiceIssue, idServiceIssueType);
        //    return Json(model);
        //}
    }
}