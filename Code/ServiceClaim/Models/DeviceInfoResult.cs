﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceClaim.Models
{
    public class DeviceInfoResult
    {
        public string DeviceSerialNum { get; set; }
        public string ContractorStr { get; set; }
        public string ContractStr { get; set; }
        public string DeviceStr { get; set; }
        public string AddressStr { get; set; }
        public string DescrStr { get; set; }
    }
}