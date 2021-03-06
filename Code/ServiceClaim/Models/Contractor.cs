﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using ServiceClaim.Objects;

namespace ServiceClaim.Models
{
    public class Contractor:DbModel
    {
        public string Sid { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string Inn { get; set; }
        public int ClaimCount { get; set; }
        public string ListName => $"{Name} ({ClaimCount})";

        public static IEnumerable<Contractor> GetServiceList(int? idContractor = null, string contractorName = null, int? idContract = null, string contractNumber = null, int? idDevice = null, string deviceName = null)
        {
            Uri uri = new Uri(String.Format("{0}/Contractor/GetServiceList?idContractor={1}&contractorName={2}&idContract={3}&contractNumber={4}&idDevice={5}&deviceName={6}", OdataServiceUri, idContractor, contractorName, idContract, contractNumber, idDevice, deviceName));
            string jsonString = GetJson(uri);
            var model = JsonConvert.DeserializeObject<IEnumerable<Contractor>>(jsonString);
            return model;
        }

        public static IEnumerable<Contractor> GetServiceClaimFilterList()
        {
            Uri uri = new Uri(
                $"{OdataServiceUri}/Contractor/GetServiceClaimFilterList");
            string jsonString = GetJson(uri);
            var model = JsonConvert.DeserializeObject<IEnumerable<Contractor>>(jsonString);
            return model;
        }

        public Contractor()
        {
            
        }
    }
}