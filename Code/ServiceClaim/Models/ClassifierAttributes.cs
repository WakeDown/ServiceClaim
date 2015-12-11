using System;
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
    public class ClassifierAttributes:DbModel
    {

        public decimal Wage { get; set; }
        public decimal Overhead { get; set; }

        public ClassifierAttributes() { }

        public ClassifierAttributes(DataRow row)
        {
            FillSelf(row);
        }

        private void FillSelf(DataRow row)
        {
            Wage = Db.DbHelper.GetValueDecimalOrDefault(row, "wage");
            Overhead = Db.DbHelper.GetValueDecimalOrDefault(row, "overhead");
        }

        public static ClassifierAttributes Get()
        {
            var dt = Db.Service.ExecuteQueryStoredProcedure("get_classifier_attributes");
            var model = new ClassifierAttributes(dt.Rows[0]);
            return model;
        }

        public void Save()
        {
            SqlParameter pWage = new SqlParameter() { ParameterName = "wage", SqlValue = Wage, SqlDbType = SqlDbType.Decimal };
            SqlParameter pOverhead = new SqlParameter() { ParameterName = "overhead", SqlValue = Overhead, SqlDbType = SqlDbType.Decimal };

            var dt = Db.Service.ExecuteQueryStoredProcedure("save_classifier_attributes", pWage, pOverhead);
        }


        //////public static ClassifierAttributes Get()
        //////{
        //////    Uri uri = new Uri(String.Format("{0}/Classifier/GetAttributes", OdataServiceUri));
        //////    string jsonString = GetJson(uri);
        //////    var model = JsonConvert.DeserializeObject<ClassifierAttributes>(jsonString);
        //////    return model;
        //////}

        //////public bool Save(out ResponseMessage responseMessage)
        //////{
        //////    Uri uri = new Uri(String.Format("{0}/Classifier/SaveAttributes", OdataServiceUri));
        //////    string json = JsonConvert.SerializeObject(this);
        //////    bool result = PostJson(uri, json, out responseMessage);
        //////    return result;
        //////}
    }
}