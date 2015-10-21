using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using ServiceClaim.Objects;

namespace ServiceClaim.Models
{
    public class ServiceIssuePlan:DbModel
    {
        public int Id { get; set; }
        public int IdServiceIssue { get; set; }
        public int IdServiceIssueType { get; set; }
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public int ClientId { get; set; }
        public int CityId { get; set; }
        public int ContractId { get; set; }

        public string IdValue => $"{PeriodStart:ddMMyyyy}-{PeriodEnd:ddMMyyyy}";

        public ServiceIssuePlan()
        {
        }

       public ServiceIssuePlan(int idServiceIssue, int idServiceIssueType, DateTime periodStart, DateTime periodEnd)
        {
            IdServiceIssue = idServiceIssue;
            IdServiceIssueType = idServiceIssueType;
            PeriodStart = periodStart;
            PeriodEnd = periodEnd;
        }

        public bool Save(out ResponseMessage responseMessage)
        {
            Uri uri = new Uri(String.Format("{0}/ServiceIssuePlan/Save", OdataServiceUri));
            string json = JsonConvert.SerializeObject(this);
            bool result = PostJson(uri, json, out responseMessage);
            return result;
        }

        public static bool SaveList(IEnumerable<ServiceIssuePlan> list,out ResponseMessage responseMessage)
        {
            Uri uri = new Uri(String.Format("{0}/ServiceIssuePlan/SaveList", OdataServiceUri));
            string json = JsonConvert.SerializeObject(list);
            bool result = PostJson(uri, json, out responseMessage);
            return result;
        }

        public static ServiceIssuePlan Get(int id)
        {
            Uri uri = new Uri(String.Format("{0}/ServiceIssuePlan/Get?id={1}", OdataServiceUri, id));
            string jsonString = GetApiClient().DownloadString(uri);
            var model = JsonConvert.DeserializeObject<ServiceIssuePlan>(jsonString);

            return model;
        }

        public static ServiceIssuePlan Get(int idServiceIssue, int idServiceIssueType)
        {
            Uri uri = new Uri(String.Format("{0}/ServiceIssuePlan/Get?idServiceIssue={1}&idServiceIssueType={2}", OdataServiceUri, idServiceIssue, idServiceIssueType));
            string jsonString = GetApiClient().DownloadString(uri);
            var model = JsonConvert.DeserializeObject<ServiceIssuePlan>(jsonString);

            return model;
        }

        public static IEnumerable<ServiceIssuePlan> GetList(DateTime periodStart, DateTime periodEnd, string engeneerSid = null)
        {
            Uri uri = new Uri(
                $"{OdataServiceUri}/ServiceIssuePlan/GetList?periodStart={periodStart:yyyy-MM-dd}&periodEnd={periodEnd:yyyy-MM-dd}&engeneerSid={engeneerSid}");
            string jsonString = GetApiClient().DownloadString(uri);
            var model = JsonConvert.DeserializeObject<IEnumerable<ServiceIssuePlan>>(jsonString);

            return model;
        }

        public static IEnumerable<ServiceIssuePeriodItem> GetPeriodMonthList(int year, int month)
        {
            Uri uri = new Uri(String.Format("{0}/ServiceIssuePlan/GetPeriodMonthList?year={1}&month={2}", OdataServiceUri, year, month));
            string jsonString = GetApiClient().DownloadString(uri);
            var model = JsonConvert.DeserializeObject<IEnumerable<ServiceIssuePeriodItem>>(jsonString);

            return model;
        }
        public static SelectList GetPeriodSelectionList(int year, int month)
        {
            return new SelectList(GetPeriodMonthList(year, month), "ListValue", "ListName");
        }
        public static IEnumerable<ServiceIssuePeriodItem> GetPeriodMonthCurPrevNextList(int year, int month)
        {
            Uri uri = new Uri(String.Format("{0}/ServiceIssuePlan/GetPeriodMonthCurPrevNextList?year={1}&month={2}", OdataServiceUri, year, month));
            string jsonString = GetApiClient().DownloadString(uri);
            var model = JsonConvert.DeserializeObject<IEnumerable<ServiceIssuePeriodItem>>(jsonString);

            return model;
        }
        

        public static SelectList GetPeriodMonthCurPrevNextSelectionList(int year, int month)
        {
            return new SelectList(GetPeriodMonthCurPrevNextList(year, month), "ListValue", "ListName");
        }

        public static IEnumerable<ServiceIssuePeriodItemMonth> GroupByMonthes(IEnumerable<ServiceIssuePeriodItem> dates)
        {
            var monthes = GetMonthesFromItemArray(dates);
            var list = new List<ServiceIssuePeriodItemMonth>();

            foreach (var mon in monthes)
            {
                var periods = dates.Where(x => x.StartDate.Year == mon.Value.Year && x.StartDate.Month == mon.Value.Month);
                list.Add(new ServiceIssuePeriodItemMonth(mon.Value, periods));
            }
            
            return list;
        }

        public static IDictionary<string, DateTime> GetMonthesFromItemArray(IEnumerable<ServiceIssuePeriodItem> dates)
        {
            DateTime minDay = dates.Min(x => x.StartDate);
            DateTime maxDay = dates.Max(x => x.EndDate);
            IDictionary<string, DateTime> monthes = new Dictionary<string, DateTime>();
            DateTime curDay = minDay;
            while (curDay <= maxDay)
            {
                if (!monthes.ContainsKey(curDay.ToString("yyyy-MM")))
                {
                    monthes.Add(curDay.ToString("yyyy-MM"), new DateTime(curDay.Year, curDay.Month, 1));
                }

                curDay = curDay.AddDays(1);
            }

            return monthes;
        }
    }
}