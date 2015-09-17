using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using ServiceClaim.Objects;

namespace ServiceClaim.Models
{
    public class ClassifierAttributes:DbModel
    {
        public decimal Wage { get; set; }
        public decimal Overhead { get; set; }

        public ClassifierAttributes() { }
        

        public static ClassifierAttributes Get()
        {
            Uri uri = new Uri(String.Format("{0}/Classifier/GetAttributes", OdataServiceUri));
            string jsonString = GetJson(uri);
            var model = JsonConvert.DeserializeObject<ClassifierAttributes>(jsonString);
            return model;
        }

        public bool Save(out ResponseMessage responseMessage)
        {
            Uri uri = new Uri(String.Format("{0}/Classifier/SaveAttributes", OdataServiceUri));
            string json = JsonConvert.SerializeObject(this);
            bool result = PostJson(uri, json, out responseMessage);
            return result;
        }
    }
}