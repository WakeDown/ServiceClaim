using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceClaim.Objects
{
    public class EngeneerNotSeted : Exception
    {
        public EngeneerNotSeted() : base()
        {

        }

        public EngeneerNotSeted(string message) : base(message)
        {

        }
    }
}