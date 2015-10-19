using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceClaim.Objects;

namespace ServiceClaim.Models
{
    public class ZipClaim : DbModel
    {
        public int Id { get; set; }
        public DateTime DateCreate { get; set; }
    }
}