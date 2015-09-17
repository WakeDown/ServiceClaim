using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceClaim.Models
{
    public class ClaimState
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SysName { get; set; }
        public DateTime DateCreate { get; set; }
        public EmployeeSm Creator { get; set; }
        public string Descr { get; set; }
        public string BackgroundColor { get; set; }
        public string ForegroundColor { get; set; }

        public static IEnumerable<ClaimState> GetHistory(int idClaim)
        {
            var list = new List<ClaimState>();
            for (int i = 1; i < 6; i++)
            {
                //list.Add(GetFake(i));
            }
            //list = list.OrderByDescending(s => s.DateCreate).ToList();
            return list;
        }

        //public static ClaimState GetFake(int i)
        //{
        //    var cl = new ClaimState() { Id = i, Name = "Статус " + i, DateCreate = DateTime.Now.AddHours(i), Creator = new EmployeeSm() { DisplayName = "Рехов А.И." }, Color = "FFD905" };

        //    switch (i)
        //    {
        //        case 1:
        //            cl.Color = "fff";//FFD905
        //            cl.Descr = "Не печатает и пищит.";
        //            cl.SysName = "NEW";
        //            break;
        //        case 2:
        //            cl.Color = "FFD491";
        //            cl.Descr = "Надо выезжать там что-то серьезное. Перезагрузка не помогла.";
        //            cl.SysName = "TECH";
        //            break;
        //        case 3:
        //            cl.Color = "FFAAA5";
        //            cl.Descr = "Там барабан поломался, надо заказывать большой такой барабан..";
        //            cl.SysName = "ENG";
        //            break;
        //        case 4:
        //            cl.Color = "BAC6FF";
        //            cl.Descr = "Привезли барабан.";
        //            cl.SysName = "SUP";
        //            break;
        //        case 5:
        //            cl.Color = "B3FFA5";
        //            cl.Descr = "Все норм заказчик доволен.";
        //            cl.SysName = "ENG";
        //            break;
        //        default:
        //            cl.Color = "fff";
        //            cl.SysName = "END";
        //            break;
        //    }

        //    return cl;
        //}
    }
}