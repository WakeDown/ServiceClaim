using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using ServiceClaim.Objects;

namespace ServiceClaim.Models
{
    public class Contract:DbModel
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public Contractor Contractor { get; set; }

        public Contract()
        {
            Contractor=new Contractor();
        }

        public Contract(int id):this()
        {
            //var con = GetFake(id);
            //Id = con.Id;
            //Number = con.Number;
            //Contractor = con.Contractor;
        }

        internal static object GetList(int? idContractor = null, string contractorName = null, int? idContract = null, string contractNumber = null, int? idDevice = null, string deviceName = null)
        {
            Uri uri = new Uri(String.Format("{0}/Contract/GetList?idContractor={1}&contractorName={2}&idContract={3}&contractNumber={4}&idDevice={5}&deviceName={6}", OdataServiceUri, idContractor, contractorName, idContract, contractNumber, idDevice, deviceName));
            string jsonString = GetJson(uri);
            var model = JsonConvert.DeserializeObject<IEnumerable<Contract>>(jsonString);
            return model;
        }
    }
}