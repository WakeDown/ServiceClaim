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
        public bool? ClientSdNumRequired { get; set; }
        public string ContractZipTypeSysName { get; set; }

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

        internal static IEnumerable<Contract> GetList(int? idContractor = null, string contractorName = null, int? idContract = null, string contractNumber = null, int? idDevice = null, string deviceName = null, string addrStrId = null)
        {
            Uri uri = new Uri(
                $"{OdataServiceUri}/Contract/GetList?idContractor={idContractor}&contractorName={contractorName}&idContract={idContract}&contractNumber={contractNumber}&idDevice={idDevice}&deviceName={deviceName}&addrStrId={addrStrId}");
            string jsonString = GetJson(uri);
            var model = JsonConvert.DeserializeObject<IEnumerable<Contract>>(jsonString);
            return model;
        }

        public static IEnumerable<KeyValuePair<int, string>> GetSelectionList(int? idContractor)
        {
            Uri uri = new Uri($"{OdataServiceUri}/Contract/GetSelectionList?idContractor={idContractor}");
            string jsonString = GetJson(uri);
            var model = JsonConvert.DeserializeObject<IEnumerable<KeyValuePair<int, string>>>(jsonString);
            return model;
        }
    }
}