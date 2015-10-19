using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceClaim.Models
{
    public class ServiceIssuePlaningItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int IssuesCount { get; set; }
        public string ShortName { get; set; }
        public string IssuesIdList { get; set; }

        public ServiceIssuePlaningItem(int id, string name, int issuesCount, string shortName)
        {
            Id = id;
            Name = name;
            IssuesCount = issuesCount;
            ShortName = shortName;
        }
    }
}