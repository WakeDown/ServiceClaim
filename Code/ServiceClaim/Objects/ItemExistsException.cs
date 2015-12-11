using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceClaim.Objects
{
    public class ItemExistsException : Exception
    {
        public ItemExistsException() : base()
        {

        }

        public ItemExistsException(string message) : base(message)
        {

        }
    }
}