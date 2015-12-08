using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using ServiceClaim.Objects;

namespace ServiceClaim.Models
{
    public class Address:DbModel
    {
        public int CityId { get; set; }
        public string CityName { get; set; }
        public string AddressName { get; set; }
        public string ObjectName { get; set; }

        public string Id => $"{CityId}[|]{AddressName}[|]{ObjectName}";
        public string Name => $"{CityName} {AddressName} {ObjectName}";

        public Address()
        {
        }

        internal static IEnumerable<Address> GetList(int? idContractor = null, int? idContract = null, int? idDevice = null, string addrName = null)
        {
            Uri uri = new Uri(
                $"{OdataServiceUri}/Address/GetList?idContractor={idContractor}&idContract={idContract}&idDevice={idDevice}&addrName={addrName}");
            string jsonString = GetJson(uri);
            var model = JsonConvert.DeserializeObject<IEnumerable<Address>>(jsonString);
            return model;
        }

        public static IEnumerable<KeyValuePair<string, string>> GetSelectionList(int? idContractor, int? idContract = null, int? idDevice = null, string addrName = null)
        {
            Uri uri = new Uri($"{OdataServiceUri}/Contract/GetSelectionList?idContractor={idContractor}&idContract={idContract}&idDevice={idDevice}&addrName={addrName}");
            string jsonString = GetJson(uri);
            var model = JsonConvert.DeserializeObject<IEnumerable<KeyValuePair<string, string>>>(jsonString);
            return model;
        }
    }
}