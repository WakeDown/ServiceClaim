using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceClaim.Models
{
    public class ListResult<T>
    {
        public IEnumerable<T> List { get; set; }
        public int TotalCount { get; set; }
    }
}