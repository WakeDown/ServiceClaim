using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceClaim.Models
{
    public class ServiceIssuePeriodItem
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
       public int DaysDiff => Math.Abs(Convert.ToInt32((StartDate - EndDate).TotalDays)) + 1;
        public string ListName => $"{StartDate:dd.MM.yy} - {EndDate:dd.MM.yy}";
        public string ListValue => $"{StartDate:dd.MM.yyyy}|{EndDate:dd.MM.yyyy}";
            public string IdValue => $"{StartDate:ddMMyyyy}-{EndDate:ddMMyyyy}";

        public IEnumerable<ServiceIssuePlan> GetServiceIssueList()
        {
           return ServiceIssuePlan.GetList(StartDate, EndDate);
        }

        public static IEnumerable<ServiceIssuePlan> GetServiceIssueList(DateTime startDate, DateTime endDate, string engeneerSid = null)
        {
            return ServiceIssuePlan.GetList(startDate, endDate, engeneerSid);
        }

        public static IEnumerable<ServiceIssuePlaningItem> GetServiceIssueCitiesList(DateTime startDate, DateTime endDate, string engeneerSid = null)
        {
            return ServiceIssuePlan.GetCitiesList(startDate, endDate, engeneerSid);
        }

        public static IEnumerable<ServiceIssuePlaningItem> GetServiceIssueAddressList(DateTime startDate, DateTime endDate, int? idCity=null, string engeneerSid = null)
        {
            return ServiceIssuePlan.GetAddressList(startDate, endDate,idCity, engeneerSid);
        }

        public static IEnumerable<ServiceIssuePlaningItem> GetServiceIssueClientList(DateTime startDate, DateTime endDate, int? idCity = null, string address=null, string engeneerSid = null)
        {
            return ServiceIssuePlan.GetClientList(startDate, endDate, idCity, address, engeneerSid);
        }
        public static IEnumerable<ServiceIssuePlaningItem> GetServiceIssueDeviceIssueList(DateTime startDate, DateTime endDate, int? idCity = null, string address = null, int? idClient=null, string engeneerSid = null)
        {
            return ServiceIssuePlan.GetDeviceIssueList(startDate, endDate, idCity, address, idClient, engeneerSid);
        }
    }
}