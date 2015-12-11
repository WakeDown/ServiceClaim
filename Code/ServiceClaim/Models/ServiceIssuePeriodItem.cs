using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceClaim.Models
{
    public class ServiceIssuePeriodItem
    {
        private DateTime _startDate;
        public DateTime StartDate
        {
            get { return _startDate; }
            set
            {
                _startDate = value;
                FillLabels();
            }
        }

        public string StartDateStr => StartDate.ToString("dd.MM.yyyy hh:mm:ss");
        private DateTime _endDate;
        public DateTime EndDate {
            get { return _endDate; }
            set
            {
                _endDate = value;
                FillLabels();
            }
        }
        public string EndDateStr => EndDate.ToString("dd.MM.yyyy hh:mm:ss");
        public int DaysDiff;
        public string ListName;
        public string ListValue;
        public string IdValue;

        public ServiceIssuePeriodItem()
        {
        }

        public ServiceIssuePeriodItem(DateTime startDate, DateTime endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
        }

        private void FillLabels()
        {
            DaysDiff = Math.Abs(Convert.ToInt32((StartDate - EndDate).TotalDays)) + 1;
            ListName = $"{StartDate:dd.MM.yy} - {EndDate:dd.MM.yy}";
            ListValue = $"{StartDate:dd.MM.yyyy}|{EndDate:dd.MM.yyyy}";
            IdValue = $"{StartDate:ddMMyyyy}-{EndDate:ddMMyyyy}";
        }
        //public int DaysDiff => Math.Abs(Convert.ToInt32((StartDate - EndDate).TotalDays)) + 1;
        // public string ListName => $"{StartDate:dd.MM.yy} - {EndDate:dd.MM.yy}";
        // public string ListValue => $"{StartDate:dd.MM.yyyy}|{EndDate:dd.MM.yyyy}";
        // public string IdValue => $"{StartDate:ddMMyyyy}-{EndDate:ddMMyyyy}";

        public IEnumerable<ServiceIssuePlan> GetServiceIssueList()
        {
           return ServiceIssuePlan.GetList(StartDate, EndDate);
        }

        public static IEnumerable<ServiceIssuePlan> GetServiceIssueList(DateTime startDate, DateTime endDate, string engeneerSid = null)
        {
            return ServiceIssuePlan.GetList(startDate, endDate, engeneerSid);
        }

        public static ServiceIssueTotal GetPlanServiceIssueTotal(DateTime startDate, DateTime endDate, string engeneerSid = null)
        {
            return ServiceIssuePlan.GetTotal(startDate, endDate, engeneerSid);
        }

        public static IEnumerable<ServiceIssuePlaningItem> GetServiceIssueCitiesList(DateTime startDate, DateTime endDate, string engeneerSid = null, int? clientId = null, bool? done = null)
        {
            return ServiceIssuePlan.GetCitiesList(startDate, endDate, engeneerSid, clientId: clientId, done: done);
        }

        public static IEnumerable<ServiceIssuePlaningItem> GetServiceIssueAddressList(DateTime startDate, DateTime endDate, int? idCity=null, string engeneerSid = null, bool? done = null)
        {
            return ServiceIssuePlan.GetAddressList(startDate, endDate,idCity, engeneerSid, done: done);
        }

        public static IEnumerable<ServiceIssuePlaningItem> GetServiceIssueClientList(DateTime startDate, DateTime endDate, int? idCity = null, string address=null, string engeneerSid = null, bool? done = null)
        {
            return ServiceIssuePlan.GetClientList(startDate, endDate, idCity, address, engeneerSid, done: done);
        }
        public static IEnumerable<ServiceIssuePlaningItem> GetServiceIssueEngeneerList(DateTime startDate, DateTime endDate, string engeneerSid = null, bool? done = null)
        {
            return ServiceIssuePlan.GetEngeneerList(startDate, endDate, engeneerSid, done: done);
        }
        public static IEnumerable<ServiceIssuePlaningItem> GetServiceIssueDeviceIssueList(DateTime startDate, DateTime endDate, int? idCity = null, string address = null, int? idClient=null, string engeneerSid = null, bool? done = null)
        {
            return ServiceIssuePlan.GetDeviceIssueList(startDate, endDate, idCity, address, idClient, engeneerSid, done: done);
        }
    }
}