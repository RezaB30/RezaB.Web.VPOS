using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RezaB_VPOS_Test_Suit.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult BasicVPOSInterface()
        {
            return View();
        }

        public ActionResult QNBFinansBasic()
        {
            return View();
        }

        [HttpPost]
        public ActionResult QNBFinansBasic(string merchantId, string userCode, string userPass, decimal amount, string storeKey)
        {
            var POSModel = new RezaB.Web.VPOS.QNBFinans.QNBFinansVPOS3DHostModel()
            {
                OkUrl = Url.Action("VPOSResults", null, null, Request.Url.Scheme),
                FailUrl = Url.Action("VPOSResults", null, null, Request.Url.Scheme),
                CurrencyCode = 949,
                Language = "TR",
                MerchantId = merchantId,
                PurchaseAmount = amount,
                Storekey = storeKey,
                UserCode = userCode,
                UserPass = userPass
            };

            return View(viewName: "QNBFinansPreSend", model: POSModel);
        }

        public ActionResult ZiraatBasic()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ZiraatBasic(string merchantId, decimal amount, string storeKey)
        {
            var POSModel = new RezaB.Web.VPOS.Ziraat.Ziraat3DHostModel()
            {
                OkUrl = Url.Action("VPOSResults", null, null, Request.Url.Scheme),
                FailUrl = Url.Action("VPOSResults", null, null, Request.Url.Scheme),
                CurrencyCode = 949,
                Language = "TR",
                MerchantId = merchantId,
                PurchaseAmount = amount,
                Storekey = storeKey
            };

            return View(viewName: "ZiraatPreSend", model: POSModel);
        }

        public ActionResult VPOSResults()
        {
            return View();
        }

        //[HttpGet]
        //public ActionResult VPOSManagerPOS()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public ActionResult VPOSManagerPOS(decimal amount)
        //{
        //    var VPOSModel = VPOSManager.GetVPOSModel(Url.Action("VPOSResults", null, null, Request.Url.Scheme), Url.Action("VPOSResults", null, null, Request.Url.Scheme), amount, "tr");
        //    return View(viewName: "VPOSManagerPreSend", model: VPOSModel);
        //}
    }
}