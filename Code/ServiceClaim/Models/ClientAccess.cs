using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using ServiceClaim.Objects;

namespace ServiceClaim.Models
{
    public class ClientAccess:DbModel
    {
        public int Id { get; set; }
        public int? IdClientEtalon { get; set; }
         public string Name { get; set; }
        public string FullName { get; set; }
        public string AdSid { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public bool ZipAccess { get; set; }
        public bool CounterAccess { get; set; }

        public ClientAccess()
        {
        }

        public ClientAccess(int id)
        {
            Uri uri = new Uri(String.Format("{0}/ClientAccess/Get?id={1}", OdataServiceUri, id));
            string jsonString = GetJson(uri);
            var model = JsonConvert.DeserializeObject<ClientAccess>(jsonString);
            FillSelf(model);
        }

        private void FillSelf(ClientAccess model)
        {
            Id = model.Id;
            IdClientEtalon = model.IdClientEtalon;
            Name = model.Name;
            FullName = model.FullName;
            AdSid = model.AdSid;
            Login = model.Login;
            Password = model.Password;
            ZipAccess = model.ZipAccess;
            CounterAccess = model.CounterAccess;
        }

        public static IEnumerable<ClientAccess> GetList()
        {
            Uri uri = new Uri(String.Format("{0}/ClientAccess/GetList", OdataServiceUri));
            string jsonString = GetJson(uri);
            var model = JsonConvert.DeserializeObject<IEnumerable<ClientAccess>>(jsonString);

            return model;
        }

        public bool SaveNew(out ResponseMessage responseMessage)
        {
            Uri uri = new Uri(String.Format("{0}/ClientAccess/SaveNew", OdataServiceUri));
            string json = JsonConvert.SerializeObject(this);
            bool result = PostJson(uri, json, out responseMessage);
            return result;
        }

        public bool Update(out ResponseMessage responseMessage)
        {
            Uri uri = new Uri(String.Format("{0}/ClientAccess/Update", OdataServiceUri));
            string json = JsonConvert.SerializeObject(this);
            bool result = PostJson(uri, json, out responseMessage);
            return result;
        }

        public static bool Delete(int id, out ResponseMessage responseMessage)
        {
            Uri uri = new Uri(String.Format("{0}/ClientAccess/Close?id={1}", OdataServiceUri, id));
            string json = String.Empty;//String.Format("{{\"id\":{0}}}",id);
            bool result = PostJson(uri, json, out responseMessage);
            return result;
        }
    }
}