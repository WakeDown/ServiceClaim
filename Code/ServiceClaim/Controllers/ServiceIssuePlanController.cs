using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
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
        public JsonResult GetCityList(DateTime? date, string[] serviceEngeneerSid, bool? planed = null, int? clientId = null, bool? seted = null)
        {
            if (!date.HasValue) date = DateTime.Now;
            string serviceEngeneerSidIds = serviceEngeneerSid != null
                ? String.Join(",", serviceEngeneerSid)
                : String.Empty;
            var list = ServiceIssue.GetPlaningCityList(date.Value, serviceEngeneerSidIds, planed: planed, clientId: clientId, seted: seted);
            return Json(list);
        }

        [HttpPost]
        public JsonResult GetAddressList(DateTime? date, string[] serviceEngeneerSid, int? idCity, bool? planed = null, int? clientId = null, bool? seted = null)
        {
            if (!idCity.HasValue) return Json(new { });
            if (!date.HasValue) date = DateTime.Now;
            string serviceEngeneerSidIds = serviceEngeneerSid != null
                ? String.Join(",", serviceEngeneerSid)
                : String.Empty;
            var list = ServiceIssue.GetPlaningAddressList(date.Value, idCity.Value, serviceEngeneerSidIds, planed:planed, clientId: clientId, seted: seted);
            return Json(list);
        }

        [HttpPost]
        public JsonResult GetClientList(DateTime? date, string[] serviceEngeneerSid, int? idCity = null, string address = null, bool? planed = null, bool? seted = null)
        {
            //if (!idCity.HasValue) return Json(new { });
            if (!date.HasValue) date = DateTime.Now;
            string serviceEngeneerSidIds = serviceEngeneerSid != null
                ? String.Join(",", serviceEngeneerSid)
                : String.Empty;
            var list = ServiceIssue.GetPlaningClientList(date.Value, serviceEngeneerSid: serviceEngeneerSidIds, idCity: idCity, address: address, planed: planed, seted: seted);
            return Json(list);
        }

        [HttpPost]
        public JsonResult GetEngeneerList(DateTime? date, string[] serviceEngeneerSid, bool? planed = null, bool? seted = null)
        {
            //if (!idCity.HasValue) return Json(new { });
            if (!date.HasValue) date = DateTime.Now;
            string serviceEngeneerSidIds = serviceEngeneerSid != null
                ? String.Join(",", serviceEngeneerSid)
                : String.Empty;
            var list = ServiceIssue.GetPlaningEngeneerList(date.Value, serviceEngeneerSid: serviceEngeneerSidIds, planed: planed, seted: seted);
            return Json(list);
        }

        [HttpPost]
        public JsonResult GetDeviceIssueList(DateTime? date, string[] serviceEngeneerSid, int? idCity, string address, int? idClient, bool? planed = null, bool? seted = null)
        {
            if (!idCity.HasValue) return Json(new { });
            //if (!idClient.HasValue) return Json(new { });
            if (!date.HasValue) date = DateTime.Now;
            string serviceEngeneerSidIds = serviceEngeneerSid != null
                ? String.Join(",", serviceEngeneerSid)
                : String.Empty;
            var list = ServiceIssue.GetPlaningDeviceIssueList(date.Value, idCity.Value, address, idClient, planed: planed, serviceEngeneerSid: serviceEngeneerSidIds, seted: seted);
            return Json(list);
        }

        [HttpPost]
        public JsonResult GetTotal(DateTime? date, string[] serviceEngeneerSid=null, bool? planed = null, bool? seted = null)
        {
            if (!date.HasValue) date = DateTime.Now;
            string serviceEngeneerSidIds = null;
            if (serviceEngeneerSid == null || (serviceEngeneerSid.Count() == 1 && String.IsNullOrEmpty(serviceEngeneerSid[0])))
            {

            }
            else
            {
                serviceEngeneerSidIds = String.Join(",", serviceEngeneerSid);
            }
            
            var model = ServiceIssue.GetTotal(date.Value, serviceEngeneerSid: serviceEngeneerSidIds);
            return Json(model);
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
        public JsonResult GetPlanServiceIssueTotal(int year, int month, string[] engeneerSid = null)
        {
            DateTime startDate = new DateTime(year, month, 1);
            DateTime endDate = new DateTime(year, month, DateTime.DaysInMonth(year, month));
            var engeneerSidArr = engeneerSid != null ? String.Join(",", engeneerSid) : String.Empty;
            var model = ServiceIssuePeriodItem.GetPlanServiceIssueTotal(startDate, endDate, engeneerSidArr);
            return Json(model);
        }

        [HttpPost]
        public JsonResult GetPlanServiceIssueList(DateTime startDate, DateTime endDate, string engeneerSid)
        {
            var list = ServiceIssuePeriodItem.GetServiceIssueList(startDate, endDate, engeneerSid);
            return Json(list);
        }

        [HttpPost]
        public JsonResult GetPlanServiceIssueCitiesList(DateTime startDate, DateTime endDate, string[] engeneerSid=null, int? clientId = null, bool? done = null)
        {
            var engeneerSidArr = engeneerSid != null ? String.Join(",", engeneerSid) : String.Empty;

            var list = ServiceIssuePeriodItem.GetServiceIssueCitiesList(startDate, endDate, engeneerSidArr, clientId, done: done);
            return Json(list);
        }

        [HttpPost]
        public JsonResult GetPlanServiceIssueAddressList(DateTime startDate, DateTime endDate, string[] engeneerSid = null, int? idCity = null, bool? done = null)
        {
            //string engeneerSidArr = null;
            var engeneerSidArr = engeneerSid != null ? String.Join(",", engeneerSid) : String.Empty;

            var list = ServiceIssuePeriodItem.GetServiceIssueAddressList(startDate, endDate, engeneerSid: engeneerSidArr, idCity:idCity, done: done);
            return Json(list);
        }

        [HttpPost]
        public JsonResult GetPlanServiceIssueAddresList(DateTime startDate, DateTime endDate, int? idCity = null, string[] engeneerSid=null, bool? done = null)
        {
            //string engeneerSidArr = null;
            var engeneerSidArr = engeneerSid != null ? String.Join(",", engeneerSid) : String.Empty;
            var list = ServiceIssuePeriodItem.GetServiceIssueAddressList(startDate, endDate, idCity: idCity, engeneerSid: engeneerSidArr, done: done);
            return Json(list);
        }

        [HttpPost]
        public JsonResult GetPlanServiceIssueClientList(DateTime startDate, DateTime endDate, int? idCity = null, string address = null, string[] engeneerSid = null, bool? done = null)
        {
            //string engeneerSidArr = null;
            var engeneerSidArr = engeneerSid != null ? String.Join(",", engeneerSid) : String.Empty;
            var list = ServiceIssuePeriodItem.GetServiceIssueClientList(startDate, endDate, idCity: idCity, address: address, engeneerSid: engeneerSidArr, done: done);
            return Json(list);
        }

        [HttpPost]
        public JsonResult GetPlanServiceIssueEngeneerList(DateTime startDate, DateTime endDate, int? idCity = null, string address = null, string[] engeneerSid = null, bool? done = null)
        {
            //string engeneerSidArr = null;
            var engeneerSidArr = engeneerSid != null ? String.Join(",", engeneerSid) : String.Empty;
            var list = ServiceIssuePeriodItem.GetServiceIssueEngeneerList(startDate, endDate, engeneerSid: engeneerSidArr, done: done);
            return Json(list);
        }

        [HttpPost]
        public JsonResult GetPlanServiceIssueDeviceIssueList(DateTime startDate, DateTime endDate, int? idCity = null, string address = null, int? idClient=null, string[] engeneerSid = null, bool? done = null)
        {
            string engeneerSidArr = null;
            //var engeneerSidArr = engeneerSid != null ? String.Join(",", engeneerSid) : String.Empty;
            var list = ServiceIssuePeriodItem.GetServiceIssueDeviceIssueList(startDate, endDate, idCity: idCity, address: address, idClient: idClient, engeneerSid: engeneerSidArr, done: done);
            return Json(list);
        }

        //public JsonResult CheckServiceIssueIsExists(int idServiceIssue, int idServiceIssueType)
        //{
        //    var model = ServiceIssuePlan.Get(idServiceIssue, idServiceIssueType);
        //    return Json(model);
        //}

        [HttpPost]
        public JsonResult GetEngeneerOrgList()
        {
            var list = ContractorAccess.GetOrgList(true);
            return Json(list);
        }

        [HttpPost]
        public JsonResult GetEngeneerSelList(string orgSid)
        {
            var list = ServiceIssue.GetEngeneerList(orgSid, true);
            return Json(list);
        }

        [HttpPost]
        public JsonResult DeleteIssueItemPlan(string planids)
        {
            var ids = planids.Split(',');
            int[] idList = Array.ConvertAll<string, int>(ids, int.Parse);
            try
            {
                ResponseMessage responseMessage;
                bool result = ServiceIssuePlan.DeleteIssueItem(idList, out responseMessage);
                if (!result) throw new Exception(responseMessage.ErrorMessage);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
            
            return Json(new {});
        }

        [HttpPost]
        public JsonResult GetPeriodMonthList(int year, int month)
        {
            var list = ServiceIssuePlan.GetPeriodMonthList(year, month);
            return Json(list);
        }
    }
}