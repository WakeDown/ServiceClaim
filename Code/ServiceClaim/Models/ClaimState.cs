using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Caching;
using Newtonsoft.Json;
using ServiceClaim.Helpers;
using ServiceClaim.Objects;

namespace ServiceClaim.Models
{
    public class ClaimState:DbModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SysName { get; set; }
        public int OrderNum { get; set; }
        public string BackgroundColor { get; set; }
        public string ForegroundColor { get; set; }
        public string BorderColor { get; set; }
        public int ClaimCount { get; set; }

        public string GroupSysName { get; set; }
        public bool IsZipClaim { get; set; }

        public ClaimState() { }

        public ClaimState(int id)
        {
            SqlParameter pId = new SqlParameter() { ParameterName = "id", SqlValue = id, SqlDbType = SqlDbType.Int };
            var dt = Db.Service.ExecuteQueryStoredProcedure("get_claim_state", pId);
            if (dt.Rows.Count > 0)
            {
                var row = dt.Rows[0];
                FillSelf(row);
            }
        }

        public ClaimState(string sysName)
        {
            SqlParameter pSysName = new SqlParameter() { ParameterName = "sys_name", SqlValue = sysName, SqlDbType = SqlDbType.NVarChar };
            var dt = Db.Service.ExecuteQueryStoredProcedure("get_claim_state", pSysName);
            if (dt.Rows.Count > 0)
            {
                var row = dt.Rows[0];
                FillSelf(row);
            }
        }

        public ClaimState(DataRow row)
            : this()
        {
            FillSelf(row);
        }

        private void FillSelf(DataRow row)
        {
            Id = Db.DbHelper.GetValueIntOrDefault(row, "id", "id_claim_state");
            Name = Db.DbHelper.GetValueString(row, "name");
            SysName = Db.DbHelper.GetValueString(row, "sys_name");
            OrderNum = Db.DbHelper.GetValueIntOrDefault(row, "order_num");
            BackgroundColor = Db.DbHelper.GetValueString(row, "background_color");
            ForegroundColor = Db.DbHelper.GetValueString(row, "foreground_color");
            BorderColor = Db.DbHelper.GetValueString(row, "border_color");
            ClaimCount = Db.DbHelper.GetValueIntOrDefault(row, "cnt");
        }

        public static IEnumerable<ClaimState> GetFilterList()
        {
            //SqlParameter pSome = new SqlParameter() { ParameterName = "some", SqlValue = some, SqlDbType = SqlDbType.NVarChar };
            var dt = Db.Service.ExecuteQueryStoredProcedure("get_claim_state_list_filter");

            var lst = new List<ClaimState>();

            foreach (DataRow row in dt.Rows)
            {
                var model = new ClaimState(row);
                lst.Add(model);
            }

            return lst;
        }

        internal static ClaimState GetFirstState()
        {
            return GetNewState();
        }

        internal static ClaimState GetNewState()
        {
            return new ClaimState("NEW");
        }

        internal static ClaimState GetEndState()
        {
            return new ClaimState("END");
        }

        public static ClaimState GetNext(int idClaimState, int claimId)
        {
            var currState = new ClaimState(idClaimState);

            switch (currState.SysName.ToUpper())
            {
                case "NEW":
                    return new ClaimState("NEWADD");
                case "NEWADD":
                    return new ClaimState("SET");
                case "SET":
                    var wtId = new Claim(claimId).IdWorkType;
                    if (!wtId.HasValue) throw new ArgumentException("Невозможно определить следующий статус. Тип работ не указан.");
                    var wtSysName = new WorkType(wtId.Value).SysName;
                    switch (wtSysName)
                    {
                        case "ДНО":
                        case "НПР":
                        case "ТЭО":
                        case "УТЗ":
                            return new ClaimState("TECHWORK");
                        case "РТО":
                        case "МТС":
                        case "УРМ":
                        case "ЗРМ":
                        case "МДО":
                        case "ИПТ":
                        case "РЗРД":
                        case "ЗНЗЧ":
                            return new ClaimState("SRVADMWORK");
                    }

                    break;
                default:
                    return currState;
            }


            //var st = new ClaimState();
            //SqlParameter pIdClaim = new SqlParameter() { ParameterName = "id_claim_state", SqlValue = idClaimState, SqlDbType = SqlDbType.Int };
            //var dt = Db.Service.ExecuteQueryStoredProcedure("get_next_claim_state", pIdClaim);
            //if (dt.Rows.Count > 0)
            //{
            //    st = new ClaimState(dt.Rows[0]);
            //}

            return currState;
        }

        public static ClaimState GetPrev(int idClaimState, int claimId)
        {
            //TODO: Написать функцию выбора предыдущего статуса
            var currState = new ClaimState(idClaimState);

            //switch (currState.SysName.ToUpper())
            //{
            //    case "NEW":
            //        return new ClaimState("NEWADD");
            //    case "NEWADD":
            //        return new ClaimState("SET");
            //    case "SET":
            //        var wtSysName = new WorkType(new Claim(claimId).IdWorkType).SysName;
            //        switch (wtSysName)
            //        {
            //            case "ДНО":
            //            case "НПР":
            //            case "ТЭО":
            //            case "УТЗ":
            //                return new ClaimState("TECHWORK");
            //            case "РТО":
            //            case "МТС":
            //            case "УРМ":
            //            case "ЗРМ":
            //            case "МДО":
            //            case "ИПТ":
            //            case "РЗРД":
            //            case "ЗНЗЧ":
            //                return new ClaimState("TECHWORK");
            //        }

            //        break;
            //}


            //var st = new ClaimState();
            //SqlParameter pIdClaim = new SqlParameter() { ParameterName = "id_claim_state", SqlValue = idClaimState, SqlDbType = SqlDbType.Int };
            //var dt = Db.Service.ExecuteQueryStoredProcedure("get_next_claim_state", pIdClaim);
            //if (dt.Rows.Count > 0)
            //{
            //    st = new ClaimState(dt.Rows[0]);
            //}

            return currState;
        }

        //////public int Id { get; set; }
        //////public string Name { get; set; }
        //////public string SysName { get; set; }
        //////public DateTime DateCreate { get; set; }
        //////public EmployeeSm Creator { get; set; }
        //////public string Descr { get; set; }
        //////public string BackgroundColor { get; set; }
        //////public string ForegroundColor { get; set; }
        //////public string BorderColor { get; set; }
        //////public int ClaimCount { get; set; }

        //////public static IEnumerable<ClaimState> GetHistory(int idClaim)
        //////{
        //////    var list = new List<ClaimState>();
        //////    for (int i = 1; i < 6; i++)
        //////    {
        //////        //list.Add(GetFake(i));
        //////    }
        //////    //list = list.OrderByDescending(s => s.DateCreate).ToList();
        //////    return list;
        //////}

        //////public static IEnumerable<ClaimState> GetFilterList()
        //////{
        //////    Uri uri = new Uri(String.Format("{0}/ClaimState/GetFilterList", OdataServiceUri));
        //////    string jsonString = GetJson(uri);
        //////    var model = JsonConvert.DeserializeObject<IEnumerable<ClaimState>>(jsonString);
        //////    return model;
        //////}

        ////////public static ClaimState GetFake(int i)
        ////////{
        ////////    var cl = new ClaimState() { Id = i, Name = "Статус " + i, DateCreate = DateTime.Now.AddHours(i), Creator = new EmployeeSm() { DisplayName = "Рехов А.И." }, Color = "FFD905" };

        ////////    switch (i)
        ////////    {
        ////////        case 1:
        ////////            cl.Color = "fff";//FFD905
        ////////            cl.Descr = "Не печатает и пищит.";
        ////////            cl.SysName = "NEW";
        ////////            break;
        ////////        case 2:
        ////////            cl.Color = "FFD491";
        ////////            cl.Descr = "Надо выезжать там что-то серьезное. Перезагрузка не помогла.";
        ////////            cl.SysName = "TECH";
        ////////            break;
        ////////        case 3:
        ////////            cl.Color = "FFAAA5";
        ////////            cl.Descr = "Там барабан поломался, надо заказывать большой такой барабан..";
        ////////            cl.SysName = "ENG";
        ////////            break;
        ////////        case 4:
        ////////            cl.Color = "BAC6FF";
        ////////            cl.Descr = "Привезли барабан.";
        ////////            cl.SysName = "SUP";
        ////////            break;
        ////////        case 5:
        ////////            cl.Color = "B3FFA5";
        ////////            cl.Descr = "Все норм заказчик доволен.";
        ////////            cl.SysName = "ENG";
        ////////            break;
        ////////        default:
        ////////            cl.Color = "fff";
        ////////            cl.SysName = "END";
        ////////            break;
        ////////    }

        ////////    return cl;
        ////////}
    }
}