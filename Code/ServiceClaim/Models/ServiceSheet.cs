﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using ServiceClaim.Objects;

namespace ServiceClaim.Models
{
    public class ServiceSheet:DbModel
    {
        public int Id { get; set; }
        public int IdClaim { get; set; }
        public int IdClaim2ClaimState { get; set; }
        public bool? ProcessEnabled { get; set; }
        public bool? DeviceEnabled { get; set; }
        public bool? ZipClaim { get; set; }
        public string ZipClaimNumber { get; set; }
        public int? CounterMono { get; set; }
        public int? CounterColor { get; set; }
        public bool NoTechWork { get; set; }
        public int? CounterTotal { get; set; }
        public bool? NoCounter { get; set; }
        public string Descr { get; set; }
        public bool? CounterUnavailable { get; set; }
        public string CounterDescr { get; set; }
        public int DeviceId { get; set; }
        public Device Device { get; set; }
        public ClassifierCaterory DeviceClassifierCaterory { get; set; }
        public int WorkTypeId { get; set; }
        public WorkType WorkType { get; set; }
        public string EngeneerSid { get; set; }
        public string AdminSid { get; set; }
        public int? TimeOnWorkMinutes { get; set; }
        public EmployeeSm Admin { get; set; }
        public EmployeeSm Creator { get; set; }
        public EmployeeSm Engeneer { get; set; }
        public string ClientSdNum { get; set; }

        public ServiceSheet()
        {
        }

        public static ServiceSheet Get(int id)
        {
            Uri uri = new Uri(String.Format("{0}/ServiceSheet/Get?id={1}", OdataServiceUri, id));
            string jsonString = GetJson(uri);
            var model = JsonConvert.DeserializeObject<ServiceSheet>(jsonString);
            return model;
        }

        //private void FillSelf(ServiceSheet model)
        //{
        //    Id = model.Id;
        //    IdClaim = model.IdClaim;
        //    IdClaim2ClaimState = model.IdClaim2ClaimState;
        //    ProcessEnabled = model.ProcessEnabled;
        //    DeviceEnabled = model.DeviceEnabled;
        //    ZipClaim = model.ZipClaim;
        //    ZipClaimNumber = model.ZipClaimNumber;
        //    CounterMono = model.CounterMono;
        //    CounterColor = model.CounterColor;
        //    CounterTotal = model.CounterTotal;
        //    NoCounter = model.NoCounter;
        //    Descr = model.Descr;
        //    CounterUnavailable = model.CounterUnavailable;
        //}

        public static ListResult<ServiceSheet> GetList(int? idClaim = null, int? idClaim2ClaimState = null)
        {
            Uri uri = new Uri($"{OdataServiceUri}/ServiceSheet/GetList?idClaim={idClaim}&idClaim2ClaimState={idClaim2ClaimState}");
            string jsonString = GetJson(uri);
            var model = JsonConvert.DeserializeObject<ListResult<ServiceSheet>>(jsonString);
            return model;
        }

        //public bool Save(out ResponseMessage responseMessage)
        //{
        //    Uri uri = new Uri(String.Format("{0}/ServiceSheet/Save", OdataServiceUri));
        //    string json = JsonConvert.SerializeObject(this);
        //    bool result = PostJson(uri, json, out responseMessage);
        //    return result;
        //}
    }
}