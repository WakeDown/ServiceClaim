using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using ServiceClaim.Objects;

namespace ServiceClaim.Models
{
    public class WorkType:DbModel
    {
        public int Id { get; set; }
        public int IdParent { get; set; }
        public string Name { get; set; }
        public string SysName { get; set; }

        public string ListName
        {
            get
            {
                //int fixStr= 5 - SysName.Count();
                //string space = new String(' ', fixStr);
                return $"{SysName, 5} - {Name}";
            }
        }

        public WorkType()
        {
            
        }

        private void FillSelf(WorkType model)
        {
            Id = model.Id;
            Name = model.Name;
            IdParent = model.IdParent;
            SysName = model.SysName;
            
        }

        public static IEnumerable<WorkType> GetList()
        {
            Uri uri = new Uri(String.Format("{0}/Classifier/GetWorkTypeList", OdataServiceUri));
            string jsonString = GetJson(uri);
            var model = JsonConvert.DeserializeObject<IEnumerable<WorkType>>(jsonString);
            return model;
        }

        public static SelectList GetSelectionList()
        {
            var list = GetList();
            return new SelectList(list, "Id", "ListName");
            
        }
    }
}