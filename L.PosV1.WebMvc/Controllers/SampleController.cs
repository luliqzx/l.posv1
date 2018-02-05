using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace L.PosV1.WebMvc.Controllers
{
    public class SampleController : Controller
    {
        // GET: Sample
        public ActionResult Index()
        {
            return Redirect("~/SamplejqGridBS.html");
        }
    }
}