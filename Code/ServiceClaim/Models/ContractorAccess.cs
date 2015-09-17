using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using ServiceClaim.Objects;

namespace ServiceClaim.Models
{
    public class ContractorAccess: DbModel
    {
        public int Id { get; set; }
        public string AdSid { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string OrgName { get; set; }
        public string City { get; set; }
        public string OrgSid { get; set; }
        public string Email { get; set; }

        public ContractorAccess() { }

        public ContractorAccess(int id)
        {
            Uri uri = new Uri(String.Format("{0}/ContractorAccess/Get?id={1}", OdataServiceUri, id));
            string jsonString = GetJson(uri);
            var model = JsonConvert.DeserializeObject<ContractorAccess>(jsonString);
            FillSelf(model);
        }

        private void FillSelf(ContractorAccess model)
        {
            Id = model.Id;
            Name = model.Name;
            AdSid = model.AdSid;
            Login = model.Login;
            Password = model.Password;
            OrgName = model.OrgName;
            City = model.City;
            OrgSid = model.OrgSid;
            Email = model.Email;
        }

        public static IEnumerable<ContractorAccess> GetList()
        {
            Uri uri = new Uri(String.Format("{0}/ContractorAccess/GetList", OdataServiceUri));
            string jsonString = GetJson(uri);
            var model = JsonConvert.DeserializeObject<IEnumerable<ContractorAccess>>(jsonString);

            return model;
        }

        public bool Save(out ResponseMessage responseMessage)
        {
            Uri uri = new Uri(String.Format("{0}/ContractorAccess/Save", OdataServiceUri));
            string json = JsonConvert.SerializeObject(this);
            bool result = PostJson(uri, json, out responseMessage);
            return result;
        }

        public static bool Delete(int id, out ResponseMessage responseMessage)
        {
            Uri uri = new Uri(String.Format("{0}/ContractorAccess/Close?id={1}", OdataServiceUri, id));
            string json = String.Empty;//String.Format("{{\"id\":{0}}}",id);
            bool result = PostJson(uri, json, out responseMessage);
            return result;
        }

        public static IEnumerable<KeyValuePair<string, string>> GetOrgList()
        {
            Uri uri = new Uri(String.Format("{0}/ContractorAccess/GetOrgList", OdataServiceUri));
            string jsonString = GetJson(uri);
            var model = JsonConvert.DeserializeObject<IDictionary<string, string>>(jsonString);

            return model;
        }

        public static SelectList GetOrgSelectionList(bool addNewItem = true)
        {
            var list = new List<KeyValuePair<string, string>>();
            list.AddRange(GetOrgList().OrderBy(x=>x.Value));
            if (addNewItem)list.Add(new KeyValuePair<string, string>("new", "Создать новую..."));

            return new SelectList(list, "Key", "Value");
        }
    }
}