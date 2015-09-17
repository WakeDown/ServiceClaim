using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ServiceClaim.Objects;

namespace ServiceClaim.Controllers
{
    public class ErrorController : BaseController
    {
        public ViewResult Index()
        {
            return View("Error");
        }
        public ViewResult NotFound()
        {
            Response.StatusCode = 404;
            return View();
        }
        public ViewResult AccessDenied()
        {
            Response.StatusCode = 403;
            return View();
        }

        public ViewResult HandledError(string message)
        {
            ViewData["message"] = message;
            return View();
        }
    }
}