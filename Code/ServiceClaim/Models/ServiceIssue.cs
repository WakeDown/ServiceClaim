using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

        public static IEnumerable<ServiceIssuePlaningItem> GetPlaningCityList(DateTime month)
        {
            Uri uri = new Uri(String.Format("{0}/ServiceIssue/GetPlaningCityList?month={1:yyyy-MM-dd}", OdataServiceUri, month));
            string jsonString = GetJson(uri);
            var model = JsonConvert.DeserializeObject<IEnumerable<ServiceIssuePlaningItem>>(jsonString);
            return model;
        }

        public static IEnumerable<ServiceIssuePlaningItem> GetPlaningAddressList(DateTime month, int idCity)
        {
            Uri uri = new Uri(String.Format("{0}/ServiceIssue/GetPlaningAddressList?month={1:yyyy-MM-dd}&idCity={2}", OdataServiceUri, month, idCity));
            string jsonString = GetJson(uri);
            var model = JsonConvert.DeserializeObject<IEnumerable<ServiceIssuePlaningItem>>(jsonString);
            return model;
        }

        public static IEnumerable<ServiceIssuePlaningItem> GetPlaningClientList(DateTime month, int idCity, string address)
        {
            Uri uri = new Uri(String.Format("{0}/ServiceIssue/GetPlaningClientList?month={1:yyyy-MM-dd}&idCity={2}&address={3}", OdataServiceUri, month, idCity, address));
            string jsonString = GetJson(uri);
            var model = JsonConvert.DeserializeObject<IEnumerable<ServiceIssuePlaningItem>>(jsonString);
            return model;
        }

        public static IEnumerable<ServiceIssuePlaningItem> GetPlaningDeviceList(DateTime month, int idCity, string address, int idClient)
        {
            Uri uri = new Uri(String.Format("{0}/ServiceIssue/GetPlaningDeviceList?month={1:yyyy-MM-dd}&idCity={2}&address={3}&idClient={4}", OdataServiceUri, month, idCity, address, idClient));
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

        
    }
}