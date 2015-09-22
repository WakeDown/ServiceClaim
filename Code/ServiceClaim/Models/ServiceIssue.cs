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

        public static IEnumerable<ServiceIssuePlaningItem> GetPlaningCityList(DateTime month, string serviceEngeneerSid = null)
        {
            Uri uri = new Uri($"{OdataServiceUri}/ServiceIssue/GetPlaningCityList?month={month:yyyy-MM-dd}&serviceEngeneerSid={serviceEngeneerSid}");
            string jsonString = GetJson(uri);
            var model = JsonConvert.DeserializeObject<IEnumerable<ServiceIssuePlaningItem>>(jsonString);
            return model;
        }

        public static IEnumerable<ServiceIssuePlaningItem> GetPlaningAddressList(DateTime month, int idCity, string serviceEngeneerSid = null)
        {
            Uri uri = new Uri(
                $"{OdataServiceUri}/ServiceIssue/GetPlaningAddressList?month={month:yyyy-MM-dd}&idCity={idCity}&serviceEngeneerSid={serviceEngeneerSid}");
            string jsonString = GetJson(uri);
            var model = JsonConvert.DeserializeObject<IEnumerable<ServiceIssuePlaningItem>>(jsonString);
            return model;
        }

        public static IEnumerable<ServiceIssuePlaningItem> GetPlaningClientList(DateTime month, int idCity, string address, string serviceEngeneerSid = null)
        {
            Uri uri = new Uri(
                $"{OdataServiceUri}/ServiceIssue/GetPlaningClientList?month={month:yyyy-MM-dd}&idCity={idCity}&address={address}&serviceEngeneerSid={serviceEngeneerSid}");
            string jsonString = GetJson(uri);
            var model = JsonConvert.DeserializeObject<IEnumerable<ServiceIssuePlaningItem>>(jsonString);
            return model;
        }

        public static IEnumerable<ServiceIssuePlaningItem> GetPlaningDeviceIssueList(DateTime month, int idCity, string address, int idClient, string serviceEngeneerSid = null)
        {
            Uri uri = new Uri(
                $"{OdataServiceUri}/ServiceIssue/GetPlaningDeviceIssueList?month={month:yyyy-MM-dd}&idCity={idCity}&address={address}&idClient={idClient}&serviceEngeneerSid={serviceEngeneerSid}");
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

        public static IEnumerable<KeyValuePair<string, string>> GetEngeneerList()
        {
            Uri uri = new Uri(String.Format("{0}/ServiceIssue/GetEngeneerList", OdataServiceUri));
            string jsonString = GetJson(uri);
            var model = JsonConvert.DeserializeObject<IEnumerable<KeyValuePair<string, string>>>(jsonString);

            return model;
        }
    }
}