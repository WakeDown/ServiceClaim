﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceClaim.Objects
{
    public class ResponseMessage
    {
        public string ErrorMessage { get; set; }
        public int Id { get; set; }
        public string RedirectUrl { get; set; }
    }
}