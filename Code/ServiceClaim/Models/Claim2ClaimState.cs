using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceClaim.Models
{
    public class Claim2ClaimState
    {
        public int Id { get; set; }
        public int IdClaim { get; set; }
        public int IdClaimState { get; set; }
        public string Descr { get; set; }
        public EmployeeSm Creator { get; set; }
        public DateTime DateCreate { get; set; }
        public ClaimState State { get; set; }
        public string SpecialistSid { get; set; }
        public int IdWorkType { get; set; }
        public int? IdServiceSheet { get; set; }

        public Claim2ClaimState() { }
    }
}