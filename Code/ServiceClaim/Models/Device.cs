﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using ServiceClaim.Helpers;
using ServiceClaim.Objects;

namespace ServiceClaim.Models
{
    public class Device:DbModel
    {
        public string Sid { get; set; }
        public int Id { get; set; }
        public string SerialNum { get; set; }
        public string ModelName { get; set; }
        //public string FullName { get; set; }
        public string Vendor { get; set; }
        public string Address { get; set; }
        public string ObjectName { get; set; }
        public string ContactName { get; set; }
        public string Descr { get; set; }
        public string FullName { get; set; }
        public string ExtendedName { get; set; }
        public int ClassifierCategoryId { get; set; }
        public int? IdCity { get; set; }
        public string CityName { get; set; }

        private int? _age;
        public int? Age
        {
            get { return _age; }
            set
            {
                _age = value;
                HasGuarantee = _age <= 1;
            }
        }
        public int ModelId { get; set; }

        public bool? HasGuarantee { get; set; }

        public bool? FakeSerialNum
        {
            get
            {
                if (String.IsNullOrEmpty(SerialNum))
                {
                    return null;
                }
                else
                {
                    return SerialNum.StartsWith("БН");
                }
            }

        }

        public Device()
        {
        }

        public Device(int id, int? idContract = null)
        {
            SqlParameter pId = new SqlParameter() { ParameterName = "id", SqlValue = id, SqlDbType = SqlDbType.Int };
            SqlParameter pIdContract = new SqlParameter() { ParameterName = "id_contract", SqlValue = idContract, SqlDbType = SqlDbType.Int };
            var dt = Db.UnitProg.ExecuteQueryStoredProcedure("get_device", pId, pIdContract);
            if (dt.Rows.Count > 0)
            {
                FillSelf(dt.Rows[0]);
            }
        }

        public static int Create(int modelId, string serialNum, string creatorSid)
        {
            SqlParameter pModelId = new SqlParameter() { ParameterName = "model_id", SqlValue = modelId, SqlDbType = SqlDbType.Int };
            SqlParameter pSerialNum = new SqlParameter() { ParameterName = "serial_num", SqlValue = serialNum, SqlDbType = SqlDbType.NVarChar };
            SqlParameter pCreatorAdSid = new SqlParameter() { ParameterName = "creator_sid", SqlValue = creatorSid, SqlDbType = SqlDbType.VarChar };

            var dt = Db.Service.ExecuteQueryStoredProcedure("device_create", pModelId, pSerialNum, pCreatorAdSid);
            int id = 0;
            if (dt.Rows.Count > 0)
            {
                id = Db.DbHelper.GetValueIntOrDefault(dt.Rows[0], "id", "id_device");
                //int.TryParse(dt.Rows[0]["id"].ToString(), out id);
            }
            return id;
        }

        public static DeviceInfoResult GetInfo(string serialNum)
        {
            SqlParameter pSerialNum = new SqlParameter() { ParameterName = "serial_num", SqlValue = serialNum, SqlDbType = SqlDbType.NVarChar };
            var dt = Db.UnitProg.ExecuteQueryStoredProcedure("get_device_info", pSerialNum);
            var result = new DeviceInfoResult();
            if (dt.Rows.Count > 0)
            {
                var row = dt.Rows[0];
                result.DeviceId = Db.DbHelper.GetValueIntOrNull(row, "id_device");
                result.DeviceSerialNum = Db.DbHelper.GetValueString(row, "serial_num");
                result.ContractorStr = Db.DbHelper.GetValueString(row, "contractor_name");
                result.ContractStr = Db.DbHelper.GetValueString(row, "contract_number");
                result.AddressStr = Db.DbHelper.GetValueString(row, "device_address");
                result.DeviceStr = Db.DbHelper.GetValueString(row, "device_name");
                result.DescrStr = Db.DbHelper.GetValueString(row, "descr");
            }
            return result;
        }

        public static IEnumerable<DeviceInfoResult> GetInfoList(DateTime? lastModifyDate = null)
        {
            SqlParameter pLastModifyDate = new SqlParameter() { ParameterName = "last_modify_date", SqlValue = lastModifyDate, SqlDbType = SqlDbType.Date };
            var dt = Db.UnitProg.ExecuteQueryStoredProcedure("get_device_info", pLastModifyDate);
            var list = new List<DeviceInfoResult>();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    var info = new DeviceInfoResult();
                    info.DeviceId = Db.DbHelper.GetValueIntOrNull(row, "id_device");
                    info.DeviceSerialNum = Db.DbHelper.GetValueString(row, "serial_num");
                    info.ContractorStr = Db.DbHelper.GetValueString(row, "contractor_name");
                    info.ContractStr = Db.DbHelper.GetValueString(row, "contract_number");
                    info.AddressStr = Db.DbHelper.GetValueString(row, "device_address");
                    info.DeviceStr = Db.DbHelper.GetValueString(row, "device_name");
                    info.DescrStr = Db.DbHelper.GetValueString(row, "descr");

                    list.Add(info);
                }

            }
            return list;
        }

        public Device(DataRow row)
        {
            FillSelf(row);
        }

        private void FillSelf(DataRow row)
        {
            Sid = Db.DbHelper.GetValueString(row, "sid");
            Id = Db.DbHelper.GetValueIntOrDefault(row, "id");
            ModelName = Db.DbHelper.GetValueString(row, "model_name");
            SerialNum = Db.DbHelper.GetValueString(row, "serial_num");
            Vendor = Db.DbHelper.GetValueString(row, "vendor");
            Address = Db.DbHelper.GetValueString(row, "address");
            ObjectName = Db.DbHelper.GetValueString(row, "object_name");
            ContactName = Db.DbHelper.GetValueString(row, "contact_name");
            Descr = Db.DbHelper.GetValueString(row, "comment");
            FullName = $"{Vendor} {ModelName} №{SerialNum}";
            ExtendedName = $"{FullName} {Address} {ObjectName}";
            ClassifierCategoryId = Db.DbHelper.GetValueIntOrDefault(row, "id_classifier_category");
            Age = Db.DbHelper.GetValueIntOrNull(row, "age");
            IdCity = Db.DbHelper.GetValueIntOrNull(row, "id_city");
            CityName = Db.DbHelper.GetValueString(row, "city_name");
        }

        public static string GetCurServiceAdminSid(int idDevice, int idContract)
        {
            string sid = String.Empty;
            SqlParameter pIdDevice = new SqlParameter() { ParameterName = "id_device", SqlValue = idDevice, SqlDbType = SqlDbType.Int };
            SqlParameter pIdContract = new SqlParameter() { ParameterName = "id_contract", SqlValue = idContract, SqlDbType = SqlDbType.Int };
            var dt = Db.UnitProg.ExecuteQueryStoredProcedure("device_get_current_service_admin", pIdDevice, pIdContract);
            if (dt.Rows.Count > 0)
            {
                sid = dt.Rows[0]["service_admin_sid"].ToString();
            }
            return sid;
        }

        public static IEnumerable<Device> GetList(int? idContractor = null, string contractorName = null, int? idContract = null, string contractNumber = null, int? idDevice = null, string deviceName = null, string serialNum = null)
        {
            //if (idContractor.HasValue) contractorName = null;
            //if (idContract.HasValue) contractNumber = null;
            //if (idDevice.HasValue) deviceName = null;

            SqlParameter pId = new SqlParameter() { ParameterName = "id_contractor", SqlValue = idContractor, SqlDbType = SqlDbType.Int };
            SqlParameter pName = new SqlParameter() { ParameterName = "contractor_name", SqlValue = contractorName, SqlDbType = SqlDbType.NVarChar };
            SqlParameter pIdContract = new SqlParameter() { ParameterName = "id_contract", SqlValue = idContract, SqlDbType = SqlDbType.Int };
            SqlParameter pContractNumber = new SqlParameter() { ParameterName = "contract_number", SqlValue = contractNumber, SqlDbType = SqlDbType.NVarChar };
            SqlParameter pIdDevice = new SqlParameter() { ParameterName = "id_device", SqlValue = idDevice, SqlDbType = SqlDbType.Int };
            SqlParameter pDeviceName = new SqlParameter() { ParameterName = "device_name", SqlValue = deviceName, SqlDbType = SqlDbType.NVarChar };
            SqlParameter pSerialNum = new SqlParameter() { ParameterName = "serial_num", SqlValue = serialNum, SqlDbType = SqlDbType.NVarChar };
            var dt = Db.UnitProg.ExecuteQueryStoredProcedure("get_device_list", pId, pName, pIdContract, pContractNumber, pIdDevice, pDeviceName, pSerialNum);

            var lst = new List<Device>();

            foreach (DataRow row in dt.Rows)
            {
                var model = new Device(row);
                lst.Add(model);
            }

            return lst;
        }

        public static bool SerialNumIsExists(string serialNum, out int? idDevice)
        {
            SqlParameter pSerialNum = new SqlParameter() { ParameterName = "serial_num", SqlValue = serialNum, SqlDbType = SqlDbType.NVarChar };
            var dt = Db.Service.ExecuteQueryStoredProcedure("check_device_serial_num_is_exists", pSerialNum);
            idDevice = null;
            if (dt.Rows.Count > 0)
            {
                idDevice = Db.DbHelper.GetValueIntOrNull(dt.Rows[0], "id_device");
            }

            return idDevice != null;
        }

        public static IEnumerable<KeyValuePair<int, string>> GetModelSelectionList(string model)
        {
            SqlParameter pModel = new SqlParameter() { ParameterName = "model_name", SqlValue = model, SqlDbType = SqlDbType.NVarChar };
            var dt = Db.Service.ExecuteQueryStoredProcedure("device_model_get_list", pModel);
            var list = new List<KeyValuePair<int, string>>();
            if (dt.Rows.Count > 0)
            {
                list.AddRange(from DataRow row in dt.Rows select new KeyValuePair<int, string>(Db.DbHelper.GetValueIntOrDefault(row, "id"), Db.DbHelper.GetValueString(row, "model_name")));
            }

            return list;
        }

        //////    public string Sid { get; set; }
        //////    public int Id { get; set; }
        //////    public string SerialNum { get; set; }
        //////    public string ModelName { get; set; }
        //////    public string Vendor { get; set; }
        //////    public string Address { get; set; }
        //////    public string ObjectName { get; set; }
        //////    public string ContactName { get; set; }
        //////    public string Descr { get; set; }

        //////    public string FullName { get; set; }
        //////    public string ExtendedName { get; set; }
        //////    public int? Age { get; set; }
        //////    public bool? HasGuarantee { get; set; }

        //////    public int ClassifierCategoryId { get; set; }
        //////    public int? IdCity { get; set; }
        //////    public string CityName { get; set; }
        //////    public bool? FakeSerialNum { get; set; }

        //////    private void FillSelf(Device model)
        //////    {
        //////        Sid = model.Sid;
        //////        Id = model.Id;
        //////        SerialNum = model.SerialNum;
        //////        ModelName = model.ModelName;
        //////        Vendor = model.Vendor;
        //////        Address = model.Address;
        //////        ObjectName = model.ObjectName;
        //////        ContactName = model.ContactName;
        //////        Descr = model.Descr;
        //////    }

        //////    public static IEnumerable<Device> GetList(int? idContractor = null, string contractorName = null, int? idContract = null, string contractNumber = null, int? idDevice = null, string deviceName = null)
        //////    {
        //////        Uri uri = new Uri(String.Format("{0}/Device/GetList?idContractor={1}&contractorName={2}&idContract={3}&contractNumber={4}&idDevice={5}&deviceName={6}", OdataServiceUri, idContractor, contractorName, idContract, contractNumber, idDevice, deviceName));
        //////        string jsonString = GetJson(uri);
        //////        var model = JsonConvert.DeserializeObject<IEnumerable<Device>>(jsonString);
        //////        //foreach (var d in model.ToArray())
        //////        //{
        //////        //    d.SetSelListName();
        //////        //}
        //////        return model;
        //////    }

        public static DeviceSearchResult GetSearchList(int? idContractor = null, string contractorName = null, int? idContract = null, string contractNumber = null, int? idDevice = null, string deviceName = null, string serialNum = null)
        {
            var devices = Device.GetList(idContractor, contractorName, idContract, contractNumber, idDevice, deviceName, serialNum);
            var vendors = from device in devices
                          group device by device.Vendor
                into d
                          orderby d.Key
                          select d.Key;

            //devices.GroupBy(d=>d.Vendor).Select();

            return new DeviceSearchResult(devices, vendors);

            //////Uri uri = new Uri(String.Format("{0}/Device/GetSearchList?idContractor={1}&contractorName={2}&idContract={3}&contractNumber={4}&idDevice={5}&deviceName={6}&serialNum={7}", OdataServiceUri, idContractor, contractorName, idContract, contractNumber, idDevice, deviceName, serialNum));
            //////string jsonString = GetJson(uri);
            //////var model = JsonConvert.DeserializeObject<DeviceSearchResult>(jsonString);
            ////////foreach (var d in model.Devices.ToArray())
            ////////{
            ////////    d.SetSelListName();
            ////////}
            //////return model;
        }

        //////    public static IEnumerable<DeviceInfoResult> GetInfoList()
        //////    {
        //////        Uri uri = new Uri(String.Format("{0}/Device/GetInfoList", OdataServiceUri));
        //////        string jsonString = GetJson(uri);
        //////        var model = JsonConvert.DeserializeObject<IEnumerable<DeviceInfoResult>>(jsonString);
        //////        return model;
        //////    }

        //////    //public void SetSelListName()
        //////    //{
        //////    //    SelListName = String.Format("{2} {0} №{1} {4} {3}", ModelName, SerialNum, Vendor, ObjectName, Address);
        //////    //}

        public static bool CheckSerialNumIsExists(string serialNum, int idClaim)
        {
           return Device.CheckSerialNumIsExists(serialNum, idClaim);
            //////Uri uri = new Uri(
            //////    $"{OdataServiceUri}/Device/CheckSerialNumIsExists?serialNum={serialNum}&idClaim={idClaim}");
            //////string jsonString = GetJson(uri);
            //////var model = JsonConvert.DeserializeObject<bool>(jsonString);
            //////return model;
        }

        //////    public static IEnumerable<KeyValuePair<int ,string>> GetModelSelectionList(string modelStr)
        //////    {
        //////        Uri uri = new Uri($"{OdataServiceUri}/Device/GetModelSelectionList?model={modelStr}");
        //////        string jsonString = GetJson(uri);
        //////        var model = JsonConvert.DeserializeObject<IEnumerable<KeyValuePair<int, string>>>(jsonString);
        //////        return model;
        //////    }
    }
}