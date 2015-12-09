using System;
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
    public class ServiceIssue : DbModel
    {
        //////public int Id { get; set; }
        //////public int IdClaim { get; set; }
        //////public string SpecialistSid { get; set; }
        //////public string Descr { get; set; }
        //////public DateTime DatePlan { get; set; }

        public ServiceIssue()
        {
            DatePlan = DateTime.Now;
        }

        public int Id { get; set; }
        public int IdClaim { get; set; }
        public string SpecialistSid { get; set; }
        public string Descr { get; set; }
        public DateTime DatePlan { get; set; }


        public ServiceIssue(int id)
        {
            //SqlParameter pId = new SqlParameter() { ParameterName = "id", SqlValue = id, SqlDbType = SqlDbType.Int };
            //var dt = Db.Stuff.ExecuteQueryStoredProcedure("get_model", pId);
            //if (dt.Rows.Count > 0)
            //{
            //    var row = dt.Rows[0];
            //    FillSelf(row);
            //}
        }

        public ServiceIssue(DataRow row)
            : this()
        {
            FillSelf(row);
        }

        private void FillSelf(DataRow row)
        {
            Id = Db.DbHelper.GetValueIntOrDefault(row, "id");
            IdClaim = Db.DbHelper.GetValueIntOrDefault(row, "id_claim");
            SpecialistSid = Db.DbHelper.GetValueString(row, "specialist_sid");
            Descr = Db.DbHelper.GetValueString(row, "descr");
            DatePlan = Db.DbHelper.GetValueDateTimeOrDefault(row, "date_plan");
        }

        public int Save(string creatorSid)
        {
            SqlParameter pId = new SqlParameter() { ParameterName = "id", SqlValue = Id, SqlDbType = SqlDbType.Int };
            SqlParameter pIdClaim = new SqlParameter() { ParameterName = "id_claim", SqlValue = IdClaim, SqlDbType = SqlDbType.Int };
            SqlParameter pSpecialistSid = new SqlParameter() { ParameterName = "specialist_sid", SqlValue = SpecialistSid, SqlDbType = SqlDbType.VarChar };
            SqlParameter pDescr = new SqlParameter() { ParameterName = "descr", SqlValue = Descr, SqlDbType = SqlDbType.NVarChar };
            SqlParameter pDatePlan = new SqlParameter() { ParameterName = "date_plan", SqlValue = DatePlan, SqlDbType = SqlDbType.DateTime };
            SqlParameter pCreatorAdSid = new SqlParameter() { ParameterName = "creator_sid", SqlValue = creatorSid, SqlDbType = SqlDbType.VarChar };

            var dt = Db.Service.ExecuteQueryStoredProcedure("save_service_issue", pId, pIdClaim, pSpecialistSid, pDescr, pDatePlan, pCreatorAdSid);
            int id = 0;
            if (dt.Rows.Count > 0)
            {
                int.TryParse(dt.Rows[0]["id"].ToString(), out id);
                Id = id;
            }
            return Id;
        }

        public static IEnumerable<ServiceIssue> GetList()
        {
            //SqlParameter pSome = new SqlParameter() { ParameterName = "some", SqlValue = some, SqlDbType = SqlDbType.NVarChar };
            //var dt = Db.Stuff.ExecuteQueryStoredProcedure("get_model_list", pSome);

            var lst = new List<ServiceIssue>();

            //foreach (DataRow row in dt.Rows)
            //{
            //    var model = new ServiceIssue(row);
            //    lst.Add(model);
            //}

            return lst;
        }

        //////private void FillSelf(ServiceIssue model)
        //////{
        //////    Id = model.Id;
        //////    IdClaim = model.IdClaim;
        //////    SpecialistSid = model.SpecialistSid;
        //////    Descr = model.Descr;
        //////    DatePlan = model.DatePlan;
        //////}

        public static ServiceIssueTotal GetTotal(DateTime month, AdUser curUser, string serviceEngeneerSid = null, bool? planed = null, bool? seted = null)
        {
            string serviceAdminSid = null;
            if (curUser.Is(AdGroup.ServiceAdmin)) serviceAdminSid = curUser.Sid;

            var planList = PlanServiceIssue.GetClaimList(month, serviceAdminSid: serviceAdminSid, serviceEngeneerSid: serviceEngeneerSid, planed: planed, seted: seted);


            int totalCount = planList.Count();
            int setedCount = planList.Count(x => x.Seted);
            int noSetedCount = planList.Count(x => !x.Seted);
            int planedCount = planList.Count(x => x.Planed);
            int noPlanedCount = planList.Count(x => !x.Planed);

            var total = new ServiceIssueTotal() { TotalCount = totalCount, SetedCount = setedCount, NoSetedCount = noSetedCount, PlanedCount = planedCount, NoPlanedCount = noPlanedCount };
            return total;
            //////Uri uri = new Uri($"{OdataServiceUri}/ServiceIssue/GetTotal?month={month:yyyy-MM-dd}&serviceEngeneerSid={serviceEngeneerSid}&planed={planed}&seted={seted}");
            //////string jsonString = GetJson(uri);
            //////var model = JsonConvert.DeserializeObject<ServiceIssueTotal>(jsonString);
            //////return model;
        }

        public static IEnumerable<ServiceIssuePlaningItem> GetPlaningCityList(DateTime month, AdUser curUser, string serviceEngeneerSid = null, bool? planed = null, int? clientId = null, bool? seted = null)
        {
            string serviceAdminSid = null;
            if (curUser.Is(AdGroup.ServiceAdmin)) serviceAdminSid = curUser.Sid;

            var planList = PlanServiceIssue.GetClaimList(month, serviceAdminSid: serviceAdminSid, serviceEngeneerSid: serviceEngeneerSid, planed: planed, idClient: clientId, seted: seted);
            var citiesList = planList.GroupBy(x => x.IdCity)
                .Select(x => new ServiceIssuePlaningItem(x.Key, x.First().CityName, x.Count(), x.First().CityShortName, String.Join(",", x.Select(z => z.IdServiceClaim)))).OrderBy(x => x.ShortName).ToArray();

            return citiesList;
            //////Uri uri = new Uri($"{OdataServiceUri}/ServiceIssue/GetPlaningCityList?month={month:yyyy-MM-dd}&serviceEngeneerSid={serviceEngeneerSid}&planed={planed}&clientId={clientId}&seted={seted}");
            //////string jsonString = GetJson(uri);
            //////var model = JsonConvert.DeserializeObject<IEnumerable<ServiceIssuePlaningItem>>(jsonString);
            //////return model;
        }

        public static IEnumerable<ServiceIssuePlaningItem> GetPlaningAddressList(DateTime month, int idCity, AdUser curUser, string serviceEngeneerSid = null, bool? planed = null, int? clientId = null, bool? seted = null)
        {
            string serviceAdminSid = null;
            if (curUser.Is(AdGroup.ServiceAdmin)) serviceAdminSid = curUser.Sid;

            var planList = PlanServiceIssue.GetClaimList(month, idCity, serviceAdminSid: serviceAdminSid, serviceEngeneerSid: serviceEngeneerSid, idClient: clientId, planed: planed, seted: seted);
            var addressList = planList.GroupBy(x => x.Address)
                .Select(x => new ServiceIssuePlaningItem(0, x.Key, x.Count(), issuesIdList: String.Join(",", x.Select(z => z.IdServiceClaim))))
                .OrderBy(x => x.Name)
                .ToArray();

            return addressList;

            //////Uri uri = new Uri(
            //////    $"{OdataServiceUri}/ServiceIssue/GetPlaningAddressList?month={month:yyyy-MM-dd}&idCity={idCity}&serviceEngeneerSid={serviceEngeneerSid}&planed={planed}&clientId={clientId}&seted={seted}");

            //////string jsonString = GetApiClient().DownloadString(uri);
            //////var model = JsonConvert.DeserializeObject<IEnumerable<ServiceIssuePlaningItem>>(jsonString);
            //////return model;

            ////////string jsonString = GetJson(uri);
            ////////var model = JsonConvert.DeserializeObject<IEnumerable<ServiceIssuePlaningItem>>(jsonString);
            ////////return model;
        }

        public static IEnumerable<ServiceIssuePlaningItem> GetPlaningClientList(DateTime month, AdUser curUser, int? idCity = null, string address = null, string serviceEngeneerSid = null, bool? planed = null, bool? seted = null)
        {
            string serviceAdminSid = null;
            if (curUser.Is(AdGroup.ServiceAdmin)) serviceAdminSid = curUser.Sid;

            var planList = PlanServiceIssue.GetClaimList(month, idCity, address, serviceAdminSid: serviceAdminSid, serviceEngeneerSid: serviceEngeneerSid, planed: planed, seted: seted);
            var clientList = planList.GroupBy(x => x.IdClient)
                .Select(x => new ServiceIssuePlaningItem(x.Key, x.First().ClientName, x.Count(), issuesIdList: String.Join(",", x.Select(z => z.IdServiceClaim))))
                .OrderBy(x => x.Name)
                .ToArray();

            return clientList;

            //////Uri uri = new Uri(
            //////    $"{OdataServiceUri}/ServiceIssue/GetPlaningClientList?month={month:yyyy-MM-dd}&idCity={idCity}&address={address}&serviceEngeneerSid={serviceEngeneerSid}&planed={planed}&seted={seted}");
            //////string jsonString = GetJson(uri);
            //////var model = JsonConvert.DeserializeObject<IEnumerable<ServiceIssuePlaningItem>>(jsonString);
            //////return model;
        }
        public static IEnumerable<ServiceIssuePlaningItem> GetPlaningEngeneerList(DateTime month, AdUser curUser, string serviceEngeneerSid = null, bool? planed = null, bool? seted = null)
        {
            string serviceAdminSid = null;
            if (curUser.Is(AdGroup.ServiceAdmin)) serviceAdminSid = curUser.Sid;

            var planList = PlanServiceIssue.GetClaimList(month, serviceAdminSid: serviceAdminSid, serviceEngeneerSid: serviceEngeneerSid, planed: planed, seted: seted);
            var clientList = planList.GroupBy(x => x.SpecialistSid)
                .Select(x => new ServiceIssuePlaningItem(x.Key, x.First().SpecialistName, x.Count(), issuesIdList: String.Join(",", x.Select(z => z.IdServiceClaim))))
                .OrderBy(x => x.Name)
                .ToArray();

            return clientList;
            //////Uri uri = new Uri(
            //////    $"{OdataServiceUri}/ServiceIssue/GetPlaningEngeneerList?month={month:yyyy-MM-dd}&serviceEngeneerSid={serviceEngeneerSid}&planed={planed}&seted={seted}");
            //////string jsonString = GetJson(uri);
            //////var model = JsonConvert.DeserializeObject<IEnumerable<ServiceIssuePlaningItem>>(jsonString);
            //////return model;
        }

        public static IEnumerable<ServiceIssuePlaningItem> GetPlaningDeviceIssueList(DateTime month, int idCity, string address, AdUser curUser, int? idClient = null, string serviceEngeneerSid = null, bool? planed = null, bool? seted = null)
        {
            string serviceAdminSid = null;
            if (curUser.Is(AdGroup.ServiceAdmin)) serviceAdminSid = curUser.Sid;
            var planList = PlanServiceIssue.GetClaimList(month, idCity, address, idClient, serviceAdminSid: serviceAdminSid, serviceEngeneerSid: serviceEngeneerSid, planed: planed, seted: seted);
            var deviceIssueList = planList.Where(x => x.IdCity == idCity && x.Address == address).GroupBy(x => x.IdDevice)
                .Select(x => new ServiceIssuePlaningItem(x.First().IdServiceClaim, x.First().DeviceName, x.Count()))
                .OrderBy(x => x.Name)
                .ToArray();

            return deviceIssueList;
            ////////Uri uri = new Uri(
            ////////    $"{OdataServiceUri}/ServiceIssue/GetPlaningDeviceIssueList?month={month:yyyy-MM-dd}&idCity={idCity}&address={address}&idClient={idClient}&serviceEngeneerSid={serviceEngeneerSid}&planed={planed}&seted={seted}");
            ////////string jsonString = GetJson(uri);
            ////////var model = JsonConvert.DeserializeObject<IEnumerable<ServiceIssuePlaningItem>>(jsonString);
            ////////return model;
        }

        //////public static ServiceIssuePlaningResult GetPlaningList(DateTime month)
        //////{
        //////    Uri uri = new Uri(String.Format("{0}/ServiceIssue/GetPlaningList?month={1:yyyy-MM-dd}", OdataServiceUri, month));
        //////    string jsonString = GetJson(uri);
        //////    var model = JsonConvert.DeserializeObject<ServiceIssuePlaningResult>(jsonString);
        //////    return model;
        //////}

        //////public static SelectList GetEngeneerSelectionList()
        //////{
        //////    return new SelectList(GetEngeneerList(), "Key", "Value");
        //////}

        public static IEnumerable<KeyValuePair<string, string>> GetEngeneerList(string orgSid = null, bool appendAllItem = false)
        {
            var model = new List<KeyValuePair<string, string>>();
            if (orgSid == null)
            {
                model = AdHelper.GetUserListByAdGroup(AdGroup.ServiceEngeneer).ToList();
            }
            else
            {
                model = AdHelper.GetUserListByAdGroup(orgSid).ToList();
            }
            //////if (orgSid == "all") orgSid = null;
            //////Uri uri = new Uri($"{OdataServiceUri}/ServiceIssue/GetEngeneerList?orgSid={orgSid}");
            //////string jsonString = GetJson(uri);
            var list = new List<KeyValuePair<string, string>>();
            //////var model = JsonConvert.DeserializeObject<IEnumerable<KeyValuePair<string, string>>>(jsonString);
            if (appendAllItem && list.Count() > 1) list.Add(new KeyValuePair<string, string>("all", "все инженеры"));
            list.AddRange(model);
            return list;
        }
    }
}