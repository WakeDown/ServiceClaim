using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using ServiceClaim.Objects;

namespace ServiceClaim.Models
{
    public class EmployeeSm
    {
        public string AdSid { get; set; }
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
    }
}