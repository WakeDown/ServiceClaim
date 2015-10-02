using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceClaim.Models
{
    public class ServiceIssuePeriodItemMonth
    {
        public DateTime Date { get; set; }

        public int Year => Date.Year;

        public int Month => Date.Month;

        public IEnumerable<ServiceIssuePeriodItem> Periods { get; set; }

        public ServiceIssuePeriodItemMonth(DateTime date, IEnumerable<ServiceIssuePeriodItem> periods)
        {
            Date = date;
            Periods = periods;
        }

    }
}