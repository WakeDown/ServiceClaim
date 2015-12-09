﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using ServiceClaim.Helpers;
using ServiceClaim.Objects;

namespace ServiceClaim.Models
{
    public class WorkType:DbModel
    {
        //////public int Id { get; set; }
        //////public int IdParent { get; set; }
        //////public string Name { get; set; }
        //////public string SysName { get; set; }
        //////public bool ZipInstall { get; set; }//Установка ЗИП
        //////public bool ZipOrder { get; set; }//Заказ ЗИП

        public string ListName
        {
            get
            {
                //int fixStr= 5 - SysName.Count();
                //string space = new String(' ', fixStr);
                return $"{SysName, 5} - {Name}";
            }
        }

        public int Id { get; set; }
        public int IdParent { get; set; }
        public string Name { get; set; }
        public string SysName { get; set; }
        public bool ZipInstall { get; set; }//Установка ЗИП
        public bool ZipOrder { get; set; }//Заказ ЗИП

        public WorkType() { }

        public WorkType(int id)
        {
            SqlParameter pId = new SqlParameter() { ParameterName = "id", SqlValue = id, SqlDbType = SqlDbType.Int };
            var dt = Db.Service.ExecuteQueryStoredProcedure("get_work_type", pId);

            if (dt.Rows.Count > 0)
            {
                FillSelf(dt.Rows[0]);
            }
        }

        public WorkType(string sysName)
        {
            SqlParameter pSysName = new SqlParameter() { ParameterName = "sys_name", SqlValue = sysName, SqlDbType = SqlDbType.NVarChar };
            var dt = Db.Service.ExecuteQueryStoredProcedure("get_work_type", pSysName);

            if (dt.Rows.Count > 0)
            {
                FillSelf(dt.Rows[0]);
            }
        }

        public WorkType(DataRow row)
        {
            FillSelf(row);
        }

        private void FillSelf(DataRow row)
        {
            Id = Db.DbHelper.GetValueIntOrDefault(row, "id");
            IdParent = Db.DbHelper.GetValueIntOrDefault(row, "id_parent");
            Name = Db.DbHelper.GetValueString(row, "name");
            SysName = Db.DbHelper.GetValueString(row, "sys_name");
            ZipInstall = Db.DbHelper.GetValueBool(row, "zip_install");
            ZipOrder = Db.DbHelper.GetValueBool(row, "zip_order");
        }

        public static IEnumerable<WorkType> GetList()
        {
            //SqlParameter pIdAdmin = new SqlParameter() { ParameterName = "id_admin", SqlValue = idAdmin, SqlDbType = SqlDbType.Int };
            var dt = Db.Service.ExecuteQueryStoredProcedure("get_work_type_list");

            var lst = new List<WorkType>();

            foreach (DataRow row in dt.Rows)
            {
                var model = new WorkType(row);
                lst.Add(model);
            }

            return lst;
        }

        public static IEnumerable<WorkType> GetPlanActionTypeList()
        {
            //SqlParameter pIdAdmin = new SqlParameter() { ParameterName = "id_admin", SqlValue = idAdmin, SqlDbType = SqlDbType.Int };
            var dt = Db.UnitProg.ExecuteQueryStoredProcedure("get_service_action_type_list");

            var lst = new List<WorkType>();

            foreach (DataRow row in dt.Rows)
            {
                var model = new WorkType(row);
                lst.Add(model);
            }

            return lst;
        }

        public static WorkType GetWorkTypeForZipClaim()
        {
            return new WorkType("РТО-ЗИП");
        }


        //////public WorkType()
        //////{

        //////}

        //////private void FillSelf(WorkType model)
        //////{
        //////    Id = model.Id;
        //////    Name = model.Name;
        //////    IdParent = model.IdParent;
        //////    SysName = model.SysName;

        //////}

        //////public static IEnumerable<WorkType> GetList()
        //////{
        //////    Uri uri = new Uri(String.Format("{0}/Classifier/GetWorkTypeList", OdataServiceUri));
        //////    string jsonString = GetJson(uri);
        //////    var model = JsonConvert.DeserializeObject<IEnumerable<WorkType>>(jsonString);
        //////    return model;
        //////}

        public static SelectList GetSelectionList()
        {
            var list = GetList();
            return new SelectList(list, "Id", "ListName");

        }
    }
}