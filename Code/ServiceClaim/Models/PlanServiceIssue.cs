using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using ServiceClaim.Objects;

namespace ServiceClaim.Models
{
    public class PlanServiceIssue:DbModel
    {
        public int IdServiceClaim { get; set; }
        public int IdContract { get; set; }
        public int IdDevice { get; set; }
        public string DeviceName { get; set; }
        public string ContractNumber { get; set; }
        public string DeviceModel { get; set; }
        public DateTime PlaningDate { get; set; }
        public string Descr { get; set; }
        public string CityName { get; set; }
        public string Address { get; set; }
        public string ObjectName { get; set; }
        public string ClientName { get; set; }

        private void FillSelf(PlanServiceIssue model)
        {
            IdServiceClaim = model.IdServiceClaim;
            IdContract = model.IdContract;
            IdDevice = model.IdDevice;
            DeviceName = model.DeviceName;
            ContractNumber = model.ContractNumber;
            DeviceModel = model.DeviceModel;
            PlaningDate = model.PlaningDate;
            Descr = model.Descr;
            CityName = model.CityName;
            Address = model.Address;
            ObjectName = model.ObjectName;
            ClientName = model.ClientName;
        }
    }
}