using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using ServiceClaim.Objects;

namespace ServiceClaim.Models
{
    public class ClassifierItem : DbModel
    {
        public int Id { get; set; }
        public int IdCategory { get; set; }
        public int IdWorkType { get; set; }
        public int Time { get; set; }
        public decimal Price { get; set; }
        public decimal CostPeople { get; set; }//Стоимость для инженера
        public decimal CostCompany { get; set; }//Стоимость для компании

        private void FillSelf(ClassifierItem model)
        {
            Id = model.Id;
            IdCategory = model.IdCategory;
            IdWorkType = model.IdWorkType;
            Time = model.Time;
            Price = model.Price;
            CostPeople = model.CostPeople;
            CostCompany = model.CostCompany;
        }
    }
}