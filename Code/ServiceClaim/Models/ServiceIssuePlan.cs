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

        public static IEnumerable<ServiceIssuePlan> GetList(DateTime periodStart, DateTime periodEnd)
        {
            Uri uri = new Uri(String.Format("{0}/ServiceIssuePlan/GetList?periodStart={1:yyyy-MM-dd}&periodEnd={2:yyyy-MM-dd}", OdataServiceUri, periodStart, periodEnd));
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

        public static IDictionary<string, DateTime> GetMonthesFromItemArray(IEnumerable<ServiceIssuePeriodItem> dates)
        {
            DateTime minDay = dates.Min(x => x.StartDate);
            DateTime maxDay = dates.Min(x => x.EndDate);
            IDictionary<string, DateTime> monthes = new Dictionary<string, DateTime>();
            DateTime curDay = minDay;
            while (curDay <= maxDay)
            {
                if (!monthes.Any() || !monthes.ContainsKey(minDay.ToString("yyyy-MM")))
                {
                    monthes.Add(minDay.ToString("yyyy-MM"), new DateTime(minDay.Year, minDay.Month, 1));
                }

                curDay = curDay.AddDays(1);
            }

            return monthes;
        }
    }
}