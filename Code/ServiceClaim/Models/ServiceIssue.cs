using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using ServiceClaim.Objects;

namespace ServiceClaim.Models
{
    public class ServiceIssue : DbModel
    {
        public int Id { get; set; }
        public int IdClaim { get; set; }
        public string SpecialistSid { get; set; }
        public string Descr { get; set; }
        public DateTime DatePlan { get; set; }

        public ServiceIssue()
        {
            DatePlan = DateTime.Now;
        }

        private void FillSelf(ServiceIssue model)
        {
            Id = model.Id;
            IdClaim = model.IdClaim;
            SpecialistSid = model.SpecialistSid;
            Descr = model.Descr;
            DatePlan = model.DatePlan;
        }

        public static IEnumerable<ServiceIssuePlaningItem> GetPlaningCityList(DateTime month, string serviceEngeneerSid = null, bool? planed = null, int? clientId = null)
        {
            Uri uri = new Uri($"{OdataServiceUri}/ServiceIssue/GetPlaningCityList?month={month:yyyy-MM-dd}&serviceEngeneerSid={serviceEngeneerSid}&planed={planed}&idClient={clientId}");
            string jsonString = GetJson(uri);
            var model = JsonConvert.DeserializeObject<IEnumerable<ServiceIssuePlaningItem>>(jsonString);
            return model;
        }

        public static IEnumerable<ServiceIssuePlaningItem> GetPlaningAddressList(DateTime month, int idCity, string serviceEngeneerSid = null, bool? planed = null)
        {
            Uri uri = new Uri(
                $"{OdataServiceUri}/ServiceIssue/GetPlaningAddressList?month={month:yyyy-MM-dd}&idCity={idCity}&serviceEngeneerSid={serviceEngeneerSid}&planed={planed}");
            string jsonString = GetJson(uri);
            var model = JsonConvert.DeserializeObject<IEnumerable<ServiceIssuePlaningItem>>(jsonString);
            return model;
        }

        public static IEnumerable<ServiceIssuePlaningItem> GetPlaningClientList(DateTime month, int? idCity = null, string address = null, string serviceEngeneerSid = null, bool? planed = null)
        {
            Uri uri = new Uri(
                $"{OdataServiceUri}/ServiceIssue/GetPlaningClientList?month={month:yyyy-MM-dd}&idCity={idCity}&address={address}&serviceEngeneerSid={serviceEngeneerSid}&planed={planed}");
            string jsonString = GetJson(uri);
            var model = JsonConvert.DeserializeObject<IEnumerable<ServiceIssuePlaningItem>>(jsonString);
            return model;
        }
        public static IEnumerable<ServiceIssuePlaningItem> GetPlaningEngeneerList(DateTime month, string serviceEngeneerSid = null, bool? planed = null)
        {
            Uri uri = new Uri(
                $"{OdataServiceUri}/ServiceIssue/GetPlaningEngeneerList?month={month:yyyy-MM-dd}&serviceEngeneerSid={serviceEngeneerSid}&planed={planed}");
            string jsonString = GetJson(uri);
            var model = JsonConvert.DeserializeObject<IEnumerable<ServiceIssuePlaningItem>>(jsonString);
            return model;
        }
        
        public static IEnumerable<ServiceIssuePlaningItem> GetPlaningDeviceIssueList(DateTime month, int idCity, string address, int idClient, string serviceEngeneerSid = null, bool? planed = null)
        {
            Uri uri = new Uri(
                $"{OdataServiceUri}/ServiceIssue/GetPlaningDeviceIssueList?month={month:yyyy-MM-dd}&idCity={idCity}&address={address}&idClient={idClient}&serviceEngeneerSid={serviceEngeneerSid}&planed={planed}");
            string jsonString = GetJson(uri);
            var model = JsonConvert.DeserializeObject<IEnumerable<ServiceIssuePlaningItem>>(jsonString);
            return model;
        }

        public static ServiceIssuePlaningResult GetPlaningList(DateTime month)
        {
            Uri uri = new Uri(String.Format("{0}/ServiceIssue/GetPlaningList?month={1:yyyy-MM-dd}", OdataServiceUri, month));
            string jsonString = GetJson(uri);
            var model = JsonConvert.DeserializeObject<ServiceIssuePlaningResult>(jsonString);
            return model;
        }

        public static SelectList GetEngeneerSelectionList()
        {
            return new SelectList(GetEngeneerList(), "Key", "Value");
        }

        public static IEnumerable<KeyValuePair<string, string>> GetEngeneerList(string orgSid = null, bool appendAllItem = false)
        {
            if (orgSid == "all") orgSid=null;
            Uri uri = new Uri($"{OdataServiceUri}/ServiceIssue/GetEngeneerList?orgSid={orgSid}");
            string jsonString = GetJson(uri);
            var list = new List<KeyValuePair<string, string>>();
            var model = JsonConvert.DeserializeObject<IEnumerable<KeyValuePair<string, string>>>(jsonString);
            if (appendAllItem && model.Count() > 1) list.Add(new KeyValuePair<string, string>("all", "все инженеры"));
            list.AddRange(model);
            return list;
        }
    }
}