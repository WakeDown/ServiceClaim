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
        [HttpGet]
        public ActionResult Planing(DateTime? date, string serviceEngeneerSid)
        {
            if (!date.HasValue) return RedirectToAction("Planing", new { date = $"{DateTime.Now:yyyy-MM-dd}" });

            //ViewBag.IssueCityList = ServiceIssue.GetPlaningCityList(date.Value, serviceEngeneerSid);
            TempData["serviceEngeneerSid"] = serviceEngeneerSid;
            return View();
        }
        [HttpPost]
        public ActionResult Planing()
        {
            //if (!month.HasValue) return RedirectToAction("Planing", new { month = $"{DateTime.Now:yyyy-MM}" });

            DateTime date = Request.Form["date"] != null ? DateTime.Parse(Request.Form["date"]) : DateTime.Now;
            string serviceEngeneerSid = Request.Form["engeneersSelect"] ?? String.Empty;
            //TempData["serviceEngeneerSid"] = serviceEngeneerSid;
            //ViewBag.IssueCityList = ServiceIssue.GetPlaningCityList(date, serviceEngeneerSid);
            return RedirectToAction("Planing", new { date = date.ToString("yyyy-MM-dd"), serviceEngeneerSid = serviceEngeneerSid});
            return View();
        }

        [HttpPost]
        public JsonResult GetCityList(DateTime? date, string[] serviceEngeneerSid, bool? planed = null)
        {
            if (!date.HasValue) date = DateTime.Now;
            string serviceEngeneerSidIds = serviceEngeneerSid != null
                ? String.Join(",", serviceEngeneerSid)
                : String.Empty;
            var list = ServiceIssue.GetPlaningCityList(date.Value, serviceEngeneerSidIds, planed: planed);
            return Json(list);
        }

        [HttpPost]
        public JsonResult GetAddressList(DateTime? date, int? idCity, bool? planed = null)
        {
            if (!idCity.HasValue) return Json(new { });
            if (!date.HasValue) date = DateTime.Now;
            var list = ServiceIssue.GetPlaningAddressList(date.Value, idCity.Value, planed:planed);
            return Json(list);
        }

        [HttpPost]
        public JsonResult GetClientList(DateTime? date, int? idCity, string address, bool? planed = null)
        {
            if (!idCity.HasValue) return Json(new { });
            if (!date.HasValue) date = DateTime.Now;
            var list = ServiceIssue.GetPlaningClientList(date.Value, idCity.Value, address, planed: planed);
            return Json(list);
        }

        [HttpPost]
        public JsonResult GetDeviceIssueList(DateTime? date, int? idCity, string address, int? idClient, bool? planed = null)
        {
            if (!idCity.HasValue) return Json(new { });
            if (!idClient.HasValue) return Json(new { });
            if (!date.HasValue) date = DateTime.Now;
            var list = ServiceIssue.GetPlaningDeviceIssueList(date.Value, idCity.Value, address, idClient.Value, planed: planed);
            return Json(list);
        }

        [HttpPost]
        public JsonResult SaveServiceIssuePlan(int[] serviceIssueIdList, string periodStr)
        {
            if (serviceIssueIdList == null || !serviceIssueIdList.Any()) return Json(new { error = "Не указана заявка" });
            var dts = periodStr.Split('|');
            if (!dts.Any()) return Json(new { error = "Не указан период" });

            DateTime periodStart = DateTime.Parse(dts[0]);
            DateTime periodEnd = DateTime.Parse(dts[1]);
            var planIssueList = new List<ServiceIssuePlan>();
            foreach (int id in serviceIssueIdList)
            {
                ServiceIssuePlan plan = new ServiceIssuePlan(id, 1, periodStart, periodEnd);
                planIssueList.Add(plan);
            }
            
            try
            {
                ResponseMessage responseMessage;
                bool result = ServiceIssuePlan.SaveList(planIssueList, out responseMessage);
                if (!result) throw new Exception(responseMessage.ErrorMessage);
                string idList = responseMessage.IdArr;
                //ResponseMessage responseMessage;
                //bool result = plan.Save(out responseMessage);
                //if (!result) throw new Exception(responseMessage.ErrorMessage);
                //plan.Id = responseMessage.Id;
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
            return Json(new {});
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

        [HttpPost]
        public JsonResult GetPlanServiceIssueList(DateTime startDate, DateTime endDate, string engeneerSid)
        {
            var list = ServiceIssuePeriodItem.GetServiceIssueList(startDate, endDate, engeneerSid);
            return Json(list);
        }

        [HttpPost]
        public JsonResult GetPlanServiceIssueCitiesList(DateTime startDate, DateTime endDate, string engeneerSid)
        {
            var list = ServiceIssuePeriodItem.GetServiceIssueCitiesList(startDate, endDate, engeneerSid);
            return Json(list);
        }

        [HttpPost]
        public JsonResult GetPlanServiceIssueAddresList(DateTime startDate, DateTime endDate, int? idCity = null, string engeneerSid=null)
        {
            var list = ServiceIssuePeriodItem.GetServiceIssueAddressList(startDate, endDate, idCity: idCity, engeneerSid:engeneerSid);
            return Json(list);
        }

        [HttpPost]
        public JsonResult GetPlanServiceIssueClientList(DateTime startDate, DateTime endDate, int? idCity = null, string address = null, string engeneerSid = null)
        {
            var list = ServiceIssuePeriodItem.GetServiceIssueClientList(startDate, endDate, idCity: idCity, address: address, engeneerSid: engeneerSid);
            return Json(list);
        }

        [HttpPost]
        public JsonResult GetPlanServiceIssueDeviceIssueList(DateTime startDate, DateTime endDate, int? idCity = null, string address = null, int? idClient=null, string engeneerSid = null)
        {
            var list = ServiceIssuePeriodItem.GetServiceIssueDeviceIssueList(startDate, endDate, idCity: idCity, address: address, idClient: idClient, engeneerSid: engeneerSid);
            return Json(list);
        }

        //public JsonResult CheckServiceIssueIsExists(int idServiceIssue, int idServiceIssueType)
        //{
        //    var model = ServiceIssuePlan.Get(idServiceIssue, idServiceIssueType);
        //    return Json(model);
        //}
    }
}