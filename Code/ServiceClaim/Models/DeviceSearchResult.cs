using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceClaim.Models
{
    public class DeviceSearchResult
    {
        public IEnumerable<Device> Devices { get; set; }
        public IEnumerable<string> Vendors { get; set; }
    }
}