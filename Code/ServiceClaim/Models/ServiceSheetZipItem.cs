using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using ServiceClaim.Objects;

namespace ServiceClaim.Models
{
    public class ServiceSheetZipItem:DbModel
    {
        public int Id { get; set; }
        public int ServiceSheetId { get; set; }
        public string PartNum { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public int? ZipClaimUnitId { get; set; }
        public DateTime DateCreate { get; set; }
        public string CreatorSid { get; set; }
        public bool Installed { get; set; }
        public string InstalledSid { get; set; }
        public string InstalledCancelSid { get; set; }
        public int InstalledServiceSheetId { get; set; }

        public static ServiceSheetZipItem Get(int id)
        {
            Uri uri = new Uri(String.Format("{0}/ServiceSheetZipItem/Get?id={1}", OdataServiceUri, id));
            string jsonString = GetJson(uri);
            var model = JsonConvert.DeserializeObject<ServiceSheetZipItem>(jsonString);
            return model;
        }

        private void FillSelf(ServiceSheetZipItem model)
        {
            Id = model.Id;
            ServiceSheetId = model.ServiceSheetId;
            PartNum = model.PartNum;
            Name = model.Name;
            Count = model.Count;
            ZipClaimUnitId = model.ZipClaimUnitId;
            DateCreate = model.DateCreate;
            CreatorSid = model.CreatorSid;
        }

        public bool Save(out ResponseMessage responseMessage)
        {
            Uri uri = new Uri(String.Format("{0}/ServiceSheetZipItem/Save", OdataServiceUri));
            string json = JsonConvert.SerializeObject(this);
            bool result = PostJson(uri, json, out responseMessage);
            return result;
        }

        public static bool Delete(int id, out ResponseMessage responseMessage)
        {
            Uri uri = new Uri(String.Format("{0}/ServiceSheetZipItem/Close?id={1}", OdataServiceUri, id));
            string json = String.Empty;
            bool result = PostJson(uri, json, out responseMessage);
            return result;
        }

        public static bool SetInstalled(int id, int idServiceSheet, out ResponseMessage responseMessage, bool? installed = true)
        {
            Uri uri = new Uri($"{OdataServiceUri}/ServiceSheetZipItem/SetInstalled?id={id}&idServiceSheet={idServiceSheet}&installed={installed}");
            string json = String.Empty;
            bool result = PostJson(uri, json, out responseMessage);
            return result;
        }
    }
}