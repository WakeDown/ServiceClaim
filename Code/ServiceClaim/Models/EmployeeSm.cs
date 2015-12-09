using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using ServiceClaim.Helpers;
using ServiceClaim.Objects;

namespace ServiceClaim.Models
{
    public class EmployeeSm
    {
        public string AdSid { get; set; }
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string DepartmentName { get; set; }
        public string PositionName { get; set; }

        public EmployeeSm()
        {
        }

        public EmployeeSm(int id)
        {
            SqlParameter pId = new SqlParameter() { ParameterName = "id", SqlValue = id, SqlDbType = SqlDbType.Int };
            var dt = Db.Stuff.ExecuteQueryStoredProcedure("get_employee_sm", pId);
            if (dt.Rows.Count > 0)
            {
                var row = dt.Rows[0];
                FillSelf(row);
            }
        }

        public EmployeeSm(string sid)
        {
            if (String.IsNullOrEmpty(sid)) return;
            SqlParameter pSid = new SqlParameter() { ParameterName = "ad_sid", SqlValue = sid, SqlDbType = SqlDbType.VarChar };
            var dt = Db.Stuff.ExecuteQueryStoredProcedure("get_employee_sm", pSid);
            if (dt.Rows.Count > 0)
            {
                var row = dt.Rows[0];
                FillSelf(row);
            }
            else
            {
                var adUser = AdHelper.GetUserBySid(sid);
                FillSelf(adUser);
            }
        }

        private void FillSelf(DataRow row)
        {
            Id = Db.DbHelper.GetValueIntOrDefault(row, "id");
            AdSid = Db.DbHelper.GetValueString(row, "ad_sid");
            DisplayName = Db.DbHelper.GetValueString(row, "display_name");
            FullName = Db.DbHelper.GetValueString(row, "full_name");
            Email = Db.DbHelper.GetValueString(row, "email");
            DepartmentName = Db.DbHelper.GetValueString(row, "dep_name");
            PositionName = Db.DbHelper.GetValueString(row, "pos_name");
        }

        private void FillSelf(EmployeeSm user)
        {
            Id = user.Id;
            AdSid = user.AdSid;
            DisplayName = user.DisplayName;
            FullName = user.FullName;
            Email = user.Email;
            DepartmentName = user.DepartmentName;
        }

        public static string ShortName(string fullName)
        {
            string result = String.Empty;
            string[] nameArr = fullName.Split(' ');
            for (int i = 0; i < nameArr.Count(); i++)
            {
                //if (i > 2) break;
                string name = nameArr[i];
                if (String.IsNullOrEmpty(name)) continue;
                if (i > 0) name = name[0] + ".";
                if (i == 1) name = " " + name;
                result += name;
            }
            return result;
        }

        public static string GetEmailBySid(string sid)
        {
            if (String.IsNullOrEmpty(sid)) return String.Empty;
            SqlParameter pSid = new SqlParameter() { ParameterName = "sid", SqlValue = sid, SqlDbType = SqlDbType.VarChar };
            var dt = Db.Stuff.ExecuteQueryStoredProcedure("get_email", pSid);
            string email = String.Empty;
            if (dt.Rows.Count > 0)
            {
                email = Db.DbHelper.GetValueString(dt.Rows[0], "email");
            }
            return email;
        }
    }
}