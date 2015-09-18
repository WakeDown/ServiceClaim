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

        public string ListName => $"{StartDate:dd.MM.yy} - {EndDate:dd.MM.yy}";
        public string ListValue => $"{StartDate:dd.MM.yyyy}|{EndDate:dd.MM.yyyy}";

        public IEnumerable<ServiceIssuePlan> GetServiceIssueList()
        {
           return ServiceIssuePlan.GetList(StartDate, EndDate);
        } 
    }
}