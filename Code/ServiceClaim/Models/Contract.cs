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
    public class Contract:DbModel
    {

        public string Sid { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }
        public bool? ClientSdNumRequired { get; set; }
        public string ContractZipTypeSysName { get; set; }
        public string ManagerSid { get; set; }
        public int? ManagerIdUnitProg { get; set; }
        public string TypeName { get; set; }

        public Contract()
        {
        }

        public Contract(int id)
        {
            SqlParameter pId = new SqlParameter() { ParameterName = "id", SqlValue = id, SqlDbType = SqlDbType.Int };
            var dt = Db.UnitProg.ExecuteQueryStoredProcedure("get_contract", pId);
            if (dt.Rows.Count > 0)
            {
                FillSelf(dt.Rows[0]);
            }
        }

        public Contract(DataRow row)
        {
            FillSelf(row);
        }

        private void FillSelf(DataRow row)
        {
            Sid = Db.DbHelper.GetValueString(row, "sid");
            Id = Db.DbHelper.GetValueIntOrDefault(row, "id");
            Name = Db.DbHelper.GetValueString(row, "name");
            Number = Db.DbHelper.GetValueString(row, "number");
            ClientSdNumRequired = Db.DbHelper.GetValueBoolOrNull(row, "client_sd_num_required");
            ContractZipTypeSysName = Db.DbHelper.GetValueString(row, "zip_state_sys_name");
            ManagerSid = Db.DbHelper.GetValueString(row, "manager_sid");
            ManagerIdUnitProg = Db.DbHelper.GetValueIntOrNull(row, "id_manager");
            TypeName = Db.DbHelper.GetValueString(row, "contract_type_name");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="idContractor"></param>
        /// <param name="contractorName"></param>
        /// <param name="idContract"></param>
        /// <param name="contractNumber"></param>
        /// <param name="idDevice"></param>
        /// <param name="deviceName"></param>
        /// <param name="addrStrId">Состоит из $"{CityId}[|]{AddressName}"</param>
        /// <returns></returns>
        public static IEnumerable<Contract> GetList(int? idContractor = null, string contractorName = null, int? idContract = null, string contractNumber = null, int? idDevice = null, string deviceName = null, string addrStrId = null)
        {
            //if (idContractor.HasValue) contractorName = null;
            //if (idContract.HasValue) contractNumber = null;
            //if (idDevice.HasValue) deviceName = null;
            int? cityId = null;
            string address = null;
            if (!String.IsNullOrEmpty(addrStrId))
            {
                try
                {
                    var arr = addrStrId.Split(new[] { "[|]" }, StringSplitOptions.RemoveEmptyEntries);
                    cityId = Convert.ToInt32(arr[0]);
                    if (arr.Length > 1)
                    {
                        address = arr[1];
                    }
                }
                catch (Exception)
                {
                    cityId = null;
                    address = null;
                }
            }

            SqlParameter pIdContractor = new SqlParameter() { ParameterName = "id_contractor", SqlValue = idContractor, SqlDbType = SqlDbType.Int };
            SqlParameter pName = new SqlParameter() { ParameterName = "contractor_name", SqlValue = contractorName, SqlDbType = SqlDbType.NVarChar };
            SqlParameter pIdContract = new SqlParameter() { ParameterName = "id_contract", SqlValue = idContract, SqlDbType = SqlDbType.Int };
            SqlParameter pContractNumber = new SqlParameter() { ParameterName = "contract_number", SqlValue = contractNumber, SqlDbType = SqlDbType.NVarChar };
            SqlParameter pIdDevice = new SqlParameter() { ParameterName = "id_device", SqlValue = idDevice, SqlDbType = SqlDbType.Int };
            SqlParameter pDeviceName = new SqlParameter() { ParameterName = "device_name", SqlValue = deviceName, SqlDbType = SqlDbType.NVarChar };
            SqlParameter pCityId = new SqlParameter() { ParameterName = "id_city", SqlValue = cityId, SqlDbType = SqlDbType.Int };
            SqlParameter pAddress = new SqlParameter() { ParameterName = "address", SqlValue = address, SqlDbType = SqlDbType.NVarChar };
            var dt = Db.UnitProg.ExecuteQueryStoredProcedure("get_contract_list", pIdContractor, pName, pIdContract, pContractNumber, pIdDevice, pDeviceName, pCityId, pAddress);

            var lst = new List<Contract>();

            foreach (DataRow row in dt.Rows)
            {
                var model = new Contract(row);
                lst.Add(model);
            }

            return lst;
        }

        public static IEnumerable<KeyValuePair<int, string>> GetSelectionList(int? idContractor = null)
        {
            SqlParameter pIdContractor = new SqlParameter() { ParameterName = "id_contractor", SqlValue = idContractor, SqlDbType = SqlDbType.Int };
            var dt = Db.UnitProg.ExecuteQueryStoredProcedure("get_contract_list", pIdContractor);

            var lst = new List<KeyValuePair<int, string>>();

            foreach (DataRow row in dt.Rows)
            {
                var model = new KeyValuePair<int, string>(Db.DbHelper.GetValueIntOrDefault(row, "id"), Db.DbHelper.GetValueString(row, "number"));
                lst.Add(model);
            }

            return lst;
        }
        //////public int Id { get; set; }
        //////public string Number { get; set; }
        //////public Contractor Contractor { get; set; }
        //////public bool? ClientSdNumRequired { get; set; }
        //////public string ContractZipTypeSysName { get; set; }
        //////public string ManagerSid { get; set; }
        //////public int? ManagerIdUnitProg { get; set; }
        //////public string TypeName { get; set; }

        //////public Contract()
        //////{
        //////    Contractor=new Contractor();
        //////}

        //////public Contract(int id):this()
        //////{
        //////    //var con = GetFake(id);
        //////    //Id = con.Id;
        //////    //Number = con.Number;
        //////    //Contractor = con.Contractor;
        //////}

        //////internal static IEnumerable<Contract> GetList(int? idContractor = null, string contractorName = null, int? idContract = null, string contractNumber = null, int? idDevice = null, string deviceName = null, string addrStrId = null)
        //////{
        //////    Uri uri = new Uri(
        //////        $"{OdataServiceUri}/Contract/GetList?idContractor={idContractor}&contractorName={contractorName}&idContract={idContract}&contractNumber={contractNumber}&idDevice={idDevice}&deviceName={deviceName}&addrStrId={addrStrId}");
        //////    string jsonString = GetJson(uri);
        //////    var model = JsonConvert.DeserializeObject<IEnumerable<Contract>>(jsonString);
        //////    return model;
        //////}

        //////public static IEnumerable<KeyValuePair<int, string>> GetSelectionList(int? idContractor)
        //////{
        //////    Uri uri = new Uri($"{OdataServiceUri}/Contract/GetSelectionList?idContractor={idContractor}");
        //////    string jsonString = GetJson(uri);
        //////    var model = JsonConvert.DeserializeObject<IEnumerable<KeyValuePair<int, string>>>(jsonString);
        //////    return model;
        //////}
    }
}