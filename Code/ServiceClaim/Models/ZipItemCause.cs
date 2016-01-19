using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using ServiceClaim.Helpers;

namespace ServiceClaim.Models
{
    public class ZipItemCause
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ZipItemCause()
        {
            
        }

        public ZipItemCause(DataRow row)
            : this()
        {
            FillSelf(row);
        }

        private void FillSelf(DataRow row)
        {
            Id = Db.DbHelper.GetValueIntOrDefault(row, "id");
            Name = Db.DbHelper.GetValueString(row, "name");
        }

        public static IEnumerable<ZipItemCause> GetList()
        {
            var dt = Db.Service.ExecuteQueryStoredProcedure("zip_item_cause_get_list");

            var list = new List<ZipItemCause>();

            foreach (DataRow row in dt.Rows)
            {
                list.Add(new ZipItemCause(row));
            }
            return list;
        }
    }
}