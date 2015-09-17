using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using ServiceClaim.Objects;

namespace ServiceClaim.Models
{
    public class Classifier : DbModel
    {
        //public static bool GetExcel()
        //{
        //    Uri uri = new Uri(String.Format("{0}/Classifier/GetExcel", OdataServiceUri));
        //    string json = JsonConvert.SerializeObject(data);
        //    bool result = PostJson(uri, json, out responseMessage);
        //    return result;
        //}

        public static bool SaveFromExcel(byte[] data, out ResponseMessage responseMessage)
        {
            Uri uri = new Uri(String.Format("{0}/Classifier/SaveFromExcel", OdataServiceUri));
            string json = JsonConvert.SerializeObject(data);
            bool result = PostJson(uri, json, out responseMessage);
            return result;
        }

        public static IEnumerable<ClassifierItem> GetList()
        {
            Uri uri = new Uri(String.Format("{0}/Classifier/GetList", OdataServiceUri));
            string jsonString = GetJson(uri);
            var model = JsonConvert.DeserializeObject<IEnumerable<ClassifierItem>>(jsonString);
            return model;
        }
    }
}