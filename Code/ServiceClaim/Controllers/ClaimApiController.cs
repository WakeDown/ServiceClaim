using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ServiceClaim.Models;
using ServiceClaim.Objects;

namespace ServiceClaim.Controllers
{
    public class ClaimApiController : BaseApiController
    {
        [System.Web.Http.AllowAnonymous]
        [System.Web.Http.HttpGet]
        public IHttpActionResult RemoteStateChange(int? idClaim, string stateSysName, string creatorSid, string descr = null, int? idZipClaim = null)
        {
            if (!idClaim.HasValue || String.IsNullOrEmpty(stateSysName)) return NotFound();

            //var goNext = Claim.RemoteStateChange(idClaim.Value, stateSysName, creatorSid, descr, idZipClaim);

            //if (goNext)
            //{
            //    var claim = new Claim(idClaim.Value);
            //    //claim.Descr = descr;
            //    claim.Go(GetCurUser());
            //}
            return Ok();
        }
    }
}
