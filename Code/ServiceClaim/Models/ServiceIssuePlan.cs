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
    public class ServiceIssuePlan:DbModel
    {

        public int Id { get; set; }
        public int IdServiceIssue { get; set; }
        public int IdServiceIssueType { get; set; }
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public string CreatorSid { get; set; }
        public int ClientId { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
        public string CityShortName { get; set; }
        public int ContractId { get; set; }
        public string Address { get; set; }
        public string ClientName { get; set; }
        public int DeviceId { get; set; }
        public string DeviceName { get; set; }
        public string EngeneerSid { get; set; }
        public string EngeneerName { get; set; }
        public DateTime? DateCame { get; set; }

        public string IdValue => $"{PeriodStart:ddMMyyyy}-{PeriodEnd:ddMMyyyy}";

        public ServiceIssuePlan() { }

        public ServiceIssuePlan(int id)
        {
            SqlParameter pId = new SqlParameter() { ParameterName = "id", SqlValue = id, SqlDbType = SqlDbType.Int };
            var dt = Db.Service.ExecuteQueryStoredProcedure("get_service_issue_plan", pId);
            if (dt.Rows.Count > 0)
            {
                var row = dt.Rows[0];
                FillSelf(row);
            }
        }

        public ServiceIssuePlan(int idServiceIssue, int idServiceIssueType)
        {
            SqlParameter pIdServiceIssue = new SqlParameter() { ParameterName = "id_service_issue", SqlValue = idServiceIssue, SqlDbType = SqlDbType.Int };
            SqlParameter pIdServiceIssueType = new SqlParameter() { ParameterName = "id_service_issue_type", SqlValue = idServiceIssueType, SqlDbType = SqlDbType.Int };
            var dt = Db.Service.ExecuteQueryStoredProcedure("get_service_issue_plan", pIdServiceIssue, pIdServiceIssueType);
            if (dt.Rows.Count > 0)
            {
                var row = dt.Rows[0];
                FillSelf(row);
            }
        }

        public ServiceIssuePlan(DataRow row)
            : this()
        {
            FillSelf(row);
        }

        private void FillSelf(DataRow row)
        {
            Id = Db.DbHelper.GetValueIntOrDefault(row, "id");
            IdServiceIssue = Db.DbHelper.GetValueIntOrDefault(row, "id_service_issue");
            IdServiceIssueType = Db.DbHelper.GetValueIntOrDefault(row, "id_service_issue_type");
            PeriodStart = Db.DbHelper.GetValueDateTimeOrDefault(row, "period_start");
            PeriodEnd = Db.DbHelper.GetValueDateTimeOrDefault(row, "period_end");
            CreatorSid = Db.DbHelper.GetValueString(row, "creator_sid");
            ClientId = Db.DbHelper.GetValueIntOrDefault(row, "id_contractor");
            CityId = Db.DbHelper.GetValueIntOrDefault(row, "id_city");
            ContractId = Db.DbHelper.GetValueIntOrDefault(row, "id_contract");
            CityName = Db.DbHelper.GetValueString(row, "city_name");
            CityShortName = Db.DbHelper.GetValueString(row, "city_shortname");
            Address = Db.DbHelper.GetValueString(row, "address");
            ClientName = Db.DbHelper.GetValueString(row, "client_name");
            DeviceId = Db.DbHelper.GetValueIntOrDefault(row, "id_device");
            DeviceName = Db.DbHelper.GetValueString(row, "device_name");
            EngeneerSid = Db.DbHelper.GetValueString(row, "engeneer_sid");
            EngeneerName = Db.DbHelper.GetValueString(row, "engeneer_name");
            DateCame = Db.DbHelper.GetValueDateTimeOrNull(row, "date_came");
        }

        public void Save(string creatorSid)
        {
            SqlParameter pId = new SqlParameter() { ParameterName = "id", SqlValue = Id, SqlDbType = SqlDbType.Int };
            SqlParameter pIdServiceIssue = new SqlParameter() { ParameterName = "id_service_issue", SqlValue = IdServiceIssue, SqlDbType = SqlDbType.Int };
            SqlParameter pIdServiceIssueType = new SqlParameter() { ParameterName = "id_service_issue_type", SqlValue = IdServiceIssueType, SqlDbType = SqlDbType.Int };
            SqlParameter pPeriodStart = new SqlParameter() { ParameterName = "period_start", SqlValue = PeriodStart, SqlDbType = SqlDbType.Date };
            SqlParameter pPeriodEnd = new SqlParameter() { ParameterName = "period_end", SqlValue = PeriodEnd, SqlDbType = SqlDbType.Date };
            SqlParameter pCreatorAdSid = new SqlParameter() { ParameterName = "creator_sid", SqlValue = creatorSid, SqlDbType = SqlDbType.VarChar };

            var dt = Db.Service.ExecuteQueryStoredProcedure("save_service_issue_plan", pId, pIdServiceIssue, pIdServiceIssueType, pPeriodStart, pPeriodEnd, pCreatorAdSid);
            int id = 0;
            if (dt.Rows.Count > 0)
            {
                Int32.TryParse(dt.Rows[0]["id"].ToString(), out id);
                Id = id;

                if (Db.DbHelper.GetValueBool(dt.Rows[0], "exists"))//Если запись существует
                {
                    FillSelf(dt.Rows[0]);
                    throw new ItemExistsException($"Заявка №{IdServiceIssue} уже влючена в план на период {PeriodStart:dd.MM.yy} - {PeriodEnd:dd.MM.yy} пользователем {AdHelper.GetUserBySid(CreatorSid).DisplayName}");
                }
                if (Db.DbHelper.GetValueBool(dt.Rows[0], "engeneer_not_seted")) //Если не назначен инженер
                {
                    throw new EngeneerNotSeted("Не указан инженер");
                }
            }
        }

        public static string SaveList(string curUserSid, IEnumerable<ServiceIssuePlan> list)
        {
            int existsCount = 0;
            int engNotSet = 0;
            var idList = new List<int>();

            foreach (ServiceIssuePlan item in list)
            {
                //item.CurUserAdSid = curUserSid;
                try
                {
                    item.Save(curUserSid);
                    idList.Add(item.Id);
                }
                catch (ItemExistsException ex)
                {
                    existsCount++;
                }
                catch (EngeneerNotSeted ex)
                {
                    engNotSet++;
                }
            }

            return String.Join(",", idList);

            //if (errCount > 0) throw new ItemExistsException($"Некоторые заявки ({errCount} шт.) не были добавлены, так как уже существуют в плане");

            //SqlParameter pId = new SqlParameter() { ParameterName = "id", SqlValue = Id, SqlDbType = SqlDbType.Int };
            //SqlParameter pIdServiceIssue = new SqlParameter() { ParameterName = "id_service_issue", SqlValue = IdServiceIssue, SqlDbType = SqlDbType.Int };
            //SqlParameter pIdServiceIssueType = new SqlParameter() { ParameterName = "id_service_issue_type", SqlValue = IdServiceIssueType, SqlDbType = SqlDbType.Int };
            //SqlParameter pPeriodStart = new SqlParameter() { ParameterName = "period_start", SqlValue = PeriodStart, SqlDbType = SqlDbType.Date };
            //SqlParameter pPeriodEnd = new SqlParameter() { ParameterName = "period_end", SqlValue = PeriodEnd, SqlDbType = SqlDbType.Date };
            //SqlParameter pCreatorAdSid = new SqlParameter() { ParameterName = "creator_sid", SqlValue = curUserSid, SqlDbType = SqlDbType.VarChar };

            //var dt = Db.Service.ExecuteQueryStoredProcedure("save_service_issue_plan", pId, pIdServiceIssue, pIdServiceIssueType, pPeriodStart, pPeriodEnd, pCreatorAdSid);
            //int id = 0;
            //if (dt.Rows.Count > 0)
            //{
            //    Int32.TryParse(dt.Rows[0]["id"].ToString(), out id);
            //    Id = id;

            //    if (Db.DbHelper.GetValueBool(dt.Rows[0], "exists"))//Если запись существует
            //    {
            //        FillSelf(dt.Rows[0]);
            //        throw new ItemExistsException($"Заявка №{IdServiceIssue} уже влючена в план на период {PeriodStart:dd.MM.yy} - {PeriodEnd:dd.MM.yy} пользователем {AdHelper.GetUserBySid(CreatorSid).DisplayName}");
            //    }
            //}
        }

        public static IEnumerable<ServiceIssuePlan> GetListUnitProg(DateTime periodStart, DateTime periodEnd, int? idCity = null, int? idClient = null, int? idContract = null, string address = null, string engeneerSid = null, bool? done = null)
        {
            SqlParameter pPeriodStart = new SqlParameter() { ParameterName = "period_start", SqlValue = periodStart, SqlDbType = SqlDbType.Date };
            SqlParameter pPeriodEnd = new SqlParameter() { ParameterName = "period_end", SqlValue = periodEnd, SqlDbType = SqlDbType.Date };
            SqlParameter pEngeneerSid = new SqlParameter() { ParameterName = "engeneer_sid", SqlValue = engeneerSid, SqlDbType = SqlDbType.NVarChar };
            SqlParameter pAddress = new SqlParameter() { ParameterName = "address", SqlValue = address, SqlDbType = SqlDbType.NVarChar };
            SqlParameter pIdCity = new SqlParameter() { ParameterName = "id_city", SqlValue = idCity, SqlDbType = SqlDbType.Int };
            SqlParameter pIdClient = new SqlParameter() { ParameterName = "id_client", SqlValue = idClient, SqlDbType = SqlDbType.Int };
            SqlParameter pIdContract = new SqlParameter() { ParameterName = "id_contract", SqlValue = idContract, SqlDbType = SqlDbType.Int };
            SqlParameter pDone = new SqlParameter() { ParameterName = "done", SqlValue = done, SqlDbType = SqlDbType.Bit };
            var dt = Db.Service.ExecuteQueryStoredProcedure("service_issue_plan_get_list_unit_prog", pPeriodStart, pPeriodEnd, pEngeneerSid, pIdCity, pIdClient, pIdContract, pAddress, pDone);

            var lst = new List<ServiceIssuePlan>();

            foreach (DataRow row in dt.Rows)
            {
                var model = new ServiceIssuePlan(row);
                lst.Add(model);
            }

            return lst;
        }

        public static IEnumerable<ServiceIssuePlan> GetList(DateTime periodStart, DateTime periodEnd, string engeneerSid = null)
        {
            SqlParameter pPeriodStart = new SqlParameter() { ParameterName = "period_start", SqlValue = periodStart, SqlDbType = SqlDbType.Date };
            SqlParameter pPeriodEnd = new SqlParameter() { ParameterName = "period_end", SqlValue = periodEnd, SqlDbType = SqlDbType.Date };
            SqlParameter pEngeneerSid = new SqlParameter() { ParameterName = "engeneer_sid", SqlValue = engeneerSid, SqlDbType = SqlDbType.VarChar };
            var dt = Db.Service.ExecuteQueryStoredProcedure("service_issue_plan_get_list", pPeriodStart, pPeriodEnd, pEngeneerSid);

            var lst = new List<ServiceIssuePlan>();

            foreach (DataRow row in dt.Rows)
            {
                var model = new ServiceIssuePlan(row);
                lst.Add(model);
            }

            return lst;
        }
        /// <summary>
        /// Текущий
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public static IEnumerable<ServiceIssuePeriodItem> GetPeriodMonthList(int year, int month)
        {
            var list = new List<ServiceIssuePeriodItem>();

            DateTime lastDayOfMonth = new DateTime(year, month, DateTime.DaysInMonth(year, month));
            //DateTime lastDay = lastDayOfMonth;//DateTimeHelper.GetNextWeekday(lastDayOfMonth, DayOfWeek.Monday);
            DateTime firstDayOfMonth = new DateTime(year, month, 1);
            DateTime fDay = firstDayOfMonth;//DateTimeHelper.GetPrevWeekday(firstDayOfMonth, DayOfWeek.Monday).AddDays(-1);
            bool isFirst = true;
            DateTime lDay = new DateTime();
            while (lDay < lastDayOfMonth)
            {
                if (!isFirst)
                {
                    fDay = DateTimeHelper.GetNextWeekday(lDay, DayOfWeek.Monday);
                }
                else
                {
                    isFirst = false;
                    fDay = firstDayOfMonth;
                }

                lDay = DateTimeHelper.GetNextWeekday(fDay, DayOfWeek.Sunday);
                if (lDay > lastDayOfMonth) lDay = lastDayOfMonth;
                list.Add(new ServiceIssuePeriodItem(fDay, lDay));
            }

            return list;
        }
        /// <summary>
        /// Текущий, прошлый и будущий
        /// </summary>
        /// <param name="curYear"></param>
        /// <param name="curMonth"></param>
        /// <returns></returns>
        public static IEnumerable<ServiceIssuePeriodItem> GetPeriodMonthCurPrevNextList(int curYear, int curMonth)
        {
            var list = new List<ServiceIssuePeriodItem>();
            DateTime curDate = new DateTime(curYear, curMonth, 1);
            DateTime prevDate = curDate.AddMonths(-1);
            DateTime nextDate = curDate.AddMonths(1);
            list.AddRange(GetPeriodMonthList(prevDate.Year, prevDate.Month));
            list.AddRange(GetPeriodMonthList(curDate.Year, curDate.Month));
            list.AddRange(GetPeriodMonthList(nextDate.Year, nextDate.Month));

            return list;
        }

        public static void DeleteIssueItem(int[] planIdList, string curUserSid)
        {
            SqlParameter pIdList = new SqlParameter() { ParameterName = "id_list", SqlValue = String.Join(",", planIdList), SqlDbType = SqlDbType.NVarChar };
            SqlParameter pDeleterSid = new SqlParameter() { ParameterName = "deleter_sid", SqlValue = curUserSid, SqlDbType = SqlDbType.VarChar };
            var dt = Db.Service.ExecuteQueryStoredProcedure("service_issue_plan_close", pDeleterSid, pIdList);
        }

        ////// public int Id { get; set; }
        ////// public int IdServiceIssue { get; set; }
        ////// public int IdServiceIssueType { get; set; }
        ////// public DateTime PeriodStart { get; set; }
        ////// public DateTime PeriodEnd { get; set; }
        ////// public int ClientId { get; set; }
        ////// public int CityId { get; set; }
        ////// public int ContractId { get; set; }

        ////// public string IdValue => $"{PeriodStart:ddMMyyyy}-{PeriodEnd:ddMMyyyy}";

        ////// public ServiceIssuePlan()
        ////// {

        ////// }

        public ServiceIssuePlan(int idServiceIssue, int idServiceIssueType, DateTime periodStart, DateTime periodEnd)
        {
            IdServiceIssue = idServiceIssue;
            IdServiceIssueType = idServiceIssueType;
            PeriodStart = periodStart;
            PeriodEnd = periodEnd;
        }

        ////// public bool Save(out ResponseMessage responseMessage)
        ////// {
        //////     Uri uri = new Uri(String.Format("{0}/ServiceIssuePlan/Save", OdataServiceUri));
        //////     string json = JsonConvert.SerializeObject(this);
        //////     bool result = PostJson(uri, json, out responseMessage);
        //////     return result;
        ////// }

        ////// public static bool SaveList(IEnumerable<ServiceIssuePlan> list,out ResponseMessage responseMessage)
        ////// {
        //////     Uri uri = new Uri(String.Format("{0}/ServiceIssuePlan/SaveList", OdataServiceUri));
        //////     string json = JsonConvert.SerializeObject(list);
        //////     bool result = PostJson(uri, json, out responseMessage);
        //////     return result;
        ////// }

        ////// public static ServiceIssuePlan Get(int id)
        ////// {
        //////     Uri uri = new Uri(String.Format("{0}/ServiceIssuePlan/Get?id={1}", OdataServiceUri, id));
        //////     string jsonString = GetApiClient().DownloadString(uri);
        //////     var model = JsonConvert.DeserializeObject<ServiceIssuePlan>(jsonString);

        //////     return model;
        ////// }

        ////// public static ServiceIssuePlan Get(int idServiceIssue, int idServiceIssueType)
        ////// {
        //////     Uri uri = new Uri(String.Format("{0}/ServiceIssuePlan/Get?idServiceIssue={1}&idServiceIssueType={2}", OdataServiceUri, idServiceIssue, idServiceIssueType));
        //////     string jsonString = GetApiClient().DownloadString(uri);
        //////     var model = JsonConvert.DeserializeObject<ServiceIssuePlan>(jsonString);

        //////     return model;
        ////// }

        ////// public static IEnumerable<ServiceIssuePlan> GetList(DateTime periodStart, DateTime periodEnd, string engeneerSid = null)
        ////// {
        //////     Uri uri = new Uri(
        //////         $"{OdataServiceUri}/ServiceIssuePlan/GetList?periodStart={periodStart:yyyy-MM-dd}&periodEnd={periodEnd:yyyy-MM-dd}&engeneerSid={engeneerSid}");
        //////     string jsonString = GetApiClient().DownloadString(uri);
        //////     var model = JsonConvert.DeserializeObject<IEnumerable<ServiceIssuePlan>>(jsonString);

        //////     return model;
        ////// }

        public static ServiceIssueTotal GetTotal(DateTime periodStart, DateTime periodEnd, string engeneerSid = null)
        {
            var list = ServiceIssuePlan.GetListUnitProg(periodStart, periodEnd, engeneerSid: engeneerSid);
            int totalCount = list.Count();
            int doneCount = list.Count(x => x.DateCame.HasValue);
            int undoneCount = list.Count(x => !x.DateCame.HasValue);

            var total = new ServiceIssueTotal() { TotalCount = totalCount, DoneCount = doneCount, UndoneCount = undoneCount };

            return total;

            //////Uri uri = new Uri(
            //////    $"{OdataServiceUri}/ServiceIssuePlan/GetTotal?periodStart={periodStart:yyyy-MM-dd}&periodEnd={periodEnd:yyyy-MM-dd}&engeneerSid={engeneerSid}");
            //////string jsonString = GetApiClient().DownloadString(uri);
            //////var model = JsonConvert.DeserializeObject<ServiceIssueTotal>(jsonString);

            //////return model;
        }

        public static IEnumerable<ServiceIssuePlaningItem> GetCitiesList(DateTime periodStart, DateTime periodEnd, string engeneerSid = null, int? clientId = null, bool? done = null)
        {
            var planList = ServiceIssuePlan.GetListUnitProg(periodStart, periodEnd, engeneerSid: engeneerSid, idClient: clientId, done: done);
            var citiesList = planList.GroupBy(x => x.CityId)
                .Select(x => new ServiceIssuePlaningItem(x.Key, x.First().CityName, x.Count(), x.First().CityShortName, String.Join(",", x.Select(z => z.IdServiceIssue)), planIdList: String.Join(",", x.Select(z => z.Id)), camesCount: x.Count(z => z.DateCame.HasValue), noCamesCount: x.Count(z => !z.DateCame.HasValue))).OrderBy(x => x.ShortName).ToArray();

            return citiesList;

            //////Uri uri = new Uri(
            //////    $"{OdataServiceUri}/ServiceIssuePlan/GetCitiesList?periodStart={periodStart:yyyy-MM-dd}&periodEnd={periodEnd:yyyy-MM-dd}&engeneerSid={engeneerSid}&clientId={clientId}&done={done}");
            //////string jsonString = GetApiClient().DownloadString(uri);
            //////var model = JsonConvert.DeserializeObject<IEnumerable<ServiceIssuePlaningItem>>(jsonString);

            //////return model;
        }

        public static IEnumerable<ServiceIssuePlaningItem> GetAddressList(DateTime periodStart, DateTime periodEnd, int? idCity = null, string engeneerSid = null, bool? done = null)
        {
            var planList = ServiceIssuePlan.GetListUnitProg(periodStart, periodEnd, idCity: idCity, engeneerSid: engeneerSid, done: done);
            var addressList = planList.GroupBy(x => x.Address)
                .Select(x => new ServiceIssuePlaningItem(0, x.Key, x.Count(), issuesIdList: String.Join(",", x.Select(z => z.IdServiceIssue)), planIdList: String.Join(",", x.Select(z => z.Id)), camesCount: x.Count(z => z.DateCame.HasValue), noCamesCount: x.Count(z => !z.DateCame.HasValue)))
                .OrderBy(x => x.Name)
                .ToArray();

            return addressList;

            //////Uri uri = new Uri(
            //////    $"{OdataServiceUri}/ServiceIssuePlan/GetAddressList?periodStart={periodStart:yyyy-MM-dd}&periodEnd={periodEnd:yyyy-MM-dd}&idCity={idCity}&engeneerSid={engeneerSid}&done={done}");
            //////string jsonString = GetApiClient().DownloadString(uri);
            //////var model = JsonConvert.DeserializeObject<IEnumerable<ServiceIssuePlaningItem>>(jsonString);

            //////return model;
        }

        public static IEnumerable<ServiceIssuePlaningItem> GetClientList(DateTime periodStart, DateTime periodEnd, int? idCity = null, string address = null, string engeneerSid = null, bool? done = null)
        {
            var planList = ServiceIssuePlan.GetListUnitProg(periodStart, periodEnd, idCity: idCity, address: address, engeneerSid: engeneerSid, done: done);
            var clientList = planList.GroupBy(x => x.ClientId)
                .Select(x => new ServiceIssuePlaningItem(x.Key, x.First().ClientName, x.Count(), issuesIdList: String.Join(",", x.Select(z => z.IdServiceIssue)), planIdList: String.Join(",", x.Select(z => z.Id)), camesCount: x.Count(z => z.DateCame.HasValue), noCamesCount: x.Count(z => !z.DateCame.HasValue)))
                .OrderBy(x => x.Name)
                .ToArray();

            return clientList;

            //////Uri uri = new Uri(
            //////    $"{OdataServiceUri}/ServiceIssuePlan/GetClientList?periodStart={periodStart:yyyy-MM-dd}&periodEnd={periodEnd:yyyy-MM-dd}&idCity={idCity}&address={address}&engeneerSid={engeneerSid}&done={done}");
            //////string jsonString = GetApiClient().DownloadString(uri);
            //////var model = JsonConvert.DeserializeObject<IEnumerable<ServiceIssuePlaningItem>>(jsonString);

            //////return model;
        }

        public static IEnumerable<ServiceIssuePlaningItem> GetEngeneerList(DateTime periodStart, DateTime periodEnd, string engeneerSid = null, bool? done = null)
        {
            var planList = ServiceIssuePlan.GetListUnitProg(periodStart, periodEnd, engeneerSid: engeneerSid, done: done);
            var clientList = planList.GroupBy(x => x.EngeneerSid)
                .Select(x => new ServiceIssuePlaningItem(x.Key, x.First().EngeneerName, x.Count(), issuesIdList: String.Join(",", x.Select(z => z.IdServiceIssue)), planIdList: String.Join(",", x.Select(z => z.Id)), camesCount: x.Count(z => z.DateCame.HasValue), noCamesCount: x.Count(z => !z.DateCame.HasValue)))
                .OrderBy(x => x.Name)
                .ToArray();

            return clientList;

            //////Uri uri = new Uri(
            //////    $"{OdataServiceUri}/ServiceIssuePlan/GetEngeneerList?periodStart={periodStart:yyyy-MM-dd}&periodEnd={periodEnd:yyyy-MM-dd}&engeneerSid={engeneerSid}&done={done}");
            //////string jsonString = GetApiClient().DownloadString(uri);
            //////var model = JsonConvert.DeserializeObject<IEnumerable<ServiceIssuePlaningItem>>(jsonString);

            //////return model;
        }

        public static IEnumerable<ServiceIssuePlaningItem> GetDeviceIssueList(DateTime periodStart, DateTime periodEnd, int? idCity = null, string address = null, int? idClient = null, string engeneerSid = null, bool? done = null)
        {
            var planList = ServiceIssuePlan.GetListUnitProg(periodStart, periodEnd, idCity: idCity, address: address, idClient: idClient, engeneerSid: engeneerSid, done: done);
            var deviceIssueList = planList.Where(x => x.CityId == idCity && x.Address == address).GroupBy(x => x.DeviceId)
                .Select(x => new ServiceIssuePlaningItem(x.Key, x.First().DeviceName, x.Count(), issuesIdList: String.Join(",", x.Select(z => z.IdServiceIssue)), planIdList: String.Join(",", x.Select(z => z.Id)), camesCount: x.Count(z => z.DateCame.HasValue), noCamesCount: x.Count(z => !z.DateCame.HasValue)))
                .OrderBy(x => x.Name)
                .ToArray();

            return deviceIssueList;

            //////Uri uri = new Uri(
            //////    $"{OdataServiceUri}/ServiceIssuePlan/GetDeviceIssueList?periodStart={periodStart:yyyy-MM-dd}&periodEnd={periodEnd:yyyy-MM-dd}&idCity={idCity}&address={address}&idClient={idClient}&engeneerSid={engeneerSid}&done={done}");
            //////string jsonString = GetApiClient().DownloadString(uri);
            //////var model = JsonConvert.DeserializeObject<IEnumerable<ServiceIssuePlaningItem>>(jsonString);

            //////return model;
        }

        ////// public static IEnumerable<ServiceIssuePeriodItem> GetPeriodMonthList(int year, int month)
        ////// {
        //////     Uri uri = new Uri(String.Format("{0}/ServiceIssuePlan/GetPeriodMonthList?year={1}&month={2}", OdataServiceUri, year, month));
        //////     string jsonString = GetApiClient().DownloadString(uri);
        //////     var model = JsonConvert.DeserializeObject<IEnumerable<ServiceIssuePeriodItem>>(jsonString);

        //////     return model;
        ////// }
        ////// public static SelectList GetPeriodSelectionList(int year, int month)
        ////// {
        //////     return new SelectList(GetPeriodMonthList(year, month), "ListValue", "ListName");
        ////// }
        ////// public static IEnumerable<ServiceIssuePeriodItem> GetPeriodMonthCurPrevNextList(int year, int month)
        ////// {
        //////     Uri uri = new Uri(String.Format("{0}/ServiceIssuePlan/GetPeriodMonthCurPrevNextList?year={1}&month={2}", OdataServiceUri, year, month));
        //////     string jsonString = GetApiClient().DownloadString(uri);
        //////     var model = JsonConvert.DeserializeObject<IEnumerable<ServiceIssuePeriodItem>>(jsonString);

        //////     return model;
        ////// }


        ////// public static SelectList GetPeriodMonthCurPrevNextSelectionList(int year, int month)
        ////// {
        //////     return new SelectList(GetPeriodMonthCurPrevNextList(year, month), "ListValue", "ListName");
        ////// }

        ////// public static IEnumerable<ServiceIssuePeriodItemMonth> GroupByMonthes(IEnumerable<ServiceIssuePeriodItem> dates)
        ////// {
        //////     var monthes = GetMonthesFromItemArray(dates);
        //////     var list = new List<ServiceIssuePeriodItemMonth>();

        //////     foreach (var mon in monthes)
        //////     {
        //////         var periods = dates.Where(x => x.StartDate.Year == mon.Value.Year && x.StartDate.Month == mon.Value.Month);
        //////         list.Add(new ServiceIssuePeriodItemMonth(mon.Value, periods));
        //////     }

        //////     return list;
        ////// }

        ////// public static IDictionary<string, DateTime> GetMonthesFromItemArray(IEnumerable<ServiceIssuePeriodItem> dates)
        ////// {
        //////     DateTime minDay = dates.Min(x => x.StartDate);
        //////     DateTime maxDay = dates.Max(x => x.EndDate);
        //////     IDictionary<string, DateTime> monthes = new Dictionary<string, DateTime>();
        //////     DateTime curDay = minDay;
        //////     while (curDay <= maxDay)
        //////     {
        //////         if (!monthes.ContainsKey(curDay.ToString("yyyy-MM")))
        //////         {
        //////             monthes.Add(curDay.ToString("yyyy-MM"), new DateTime(curDay.Year, curDay.Month, 1));
        //////         }

        //////         curDay = curDay.AddDays(1);
        //////     }

        //////     return monthes;
        ////// }

        ////// public static bool  DeleteIssueItem(int[] idList, out ResponseMessage responseMessage)
        ////// {
        //////     Uri uri = new Uri(String.Format("{0}/ServiceIssuePlan/DeleteIssueItem", OdataServiceUri));
        //////     string json = JsonConvert.SerializeObject(idList);
        //////     bool result = PostJson(uri, json, out responseMessage);
        //////     return result;
        ////// }

    }
}