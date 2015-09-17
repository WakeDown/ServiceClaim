using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using ServiceClaim.Objects;

namespace ServiceClaim.Models
{
    public class Device:DbModel
    {
        public string Sid { get; set; }
        public int Id { get; set; }
        public string SerialNum { get; set; }
        public string ModelName { get; set; }
        public string Vendor { get; set; }
        public string Address { get; set; }
        public string ObjectName { get; set; }
        public string ContactName { get; set; }
        public string Descr { get; set; }

        public string FullName { get; set; }
        public string ExtendedName { get; set; }
       

        private void FillSelf(Device model)
        {
            Sid = model.Sid;
            Id = model.Id;
            SerialNum = model.SerialNum;
            ModelName = model.ModelName;
            Vendor = model.Vendor;
            Address = model.Address;
            ObjectName = model.ObjectName;
            ContactName = model.ContactName;
            Descr = model.Descr;
        }

        public static IEnumerable<Device> GetList(int? idContractor = null, string contractorName = null, int? idContract = null, string contractNumber = null, int? idDevice = null, string deviceName = null)
        {
            Uri uri = new Uri(String.Format("{0}/Device/GetList?idContractor={1}&contractorName={2}&idContract={3}&contractNumber={4}&idDevice={5}&deviceName={6}", OdataServiceUri, idContractor, contractorName, idContract, contractNumber, idDevice, deviceName));
            string jsonString = GetJson(uri);
            var model = JsonConvert.DeserializeObject<IEnumerable<Device>>(jsonString);
            //foreach (var d in model.ToArray())
            //{
            //    d.SetSelListName();
            //}
            return model;
        }

        public static DeviceSearchResult GetSearchList(int? idContractor = null, string contractorName = null, int? idContract = null, string contractNumber = null, int? idDevice = null, string deviceName = null, string serialNum = null)
        {
            Uri uri = new Uri(String.Format("{0}/Device/GetSearchList?idContractor={1}&contractorName={2}&idContract={3}&contractNumber={4}&idDevice={5}&deviceName={6}&serialNum={7}", OdataServiceUri, idContractor, contractorName, idContract, contractNumber, idDevice, deviceName, serialNum));
            string jsonString = GetJson(uri);
            var model = JsonConvert.DeserializeObject<DeviceSearchResult>(jsonString);
            //foreach (var d in model.Devices.ToArray())
            //{
            //    d.SetSelListName();
            //}
            return model;
        }

        public static IEnumerable<DeviceInfoResult> GetInfoList()
        {
            Uri uri = new Uri(String.Format("{0}/Device/GetInfoList", OdataServiceUri));
            string jsonString = GetJson(uri);
            var model = JsonConvert.DeserializeObject<IEnumerable<DeviceInfoResult>>(jsonString);
            return model;
        }

        //public void SetSelListName()
        //{
        //    SelListName = String.Format("{2} {0} №{1} {4} {3}", ModelName, SerialNum, Vendor, ObjectName, Address);
        //}
    }
}