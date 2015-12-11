using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceClaim.Objects
{
    public class AccessDenyException : Exception
    {
        public AccessDenyException()
        {

        }

        public AccessDenyException(string message) : base(message)
        {

        }
    }
}