using RezaB.Web.VPOS;
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
        public ActionResult YapiKrediBasic()
        {
            return View();
        }
        [HttpPost]
        public ActionResult YapiKrediBasic(string merchantId, decimal amount, string storeKey)
        {
            var POSModel = new RezaB.Web.VPOS.YapiKredi.YapiKrediVPOS3DHostModel()
            {
                OkUrl = Url.Action("YapiKrediResponse", null, null, Request.Url.Scheme),
                FailUrl = Url.Action("VPOSResults", null, null, Request.Url.Scheme),
                Language = "tr",
                MerchantId = "6700972675",
                PurchaseAmount = 1,
                Storekey = "1010052038182556",
                CurrencyCode = (int)CurrencyCodes.TL,
                tid = "67893294",
                InstallmentCount = null,
                EncKey = "10,10,10,10,10,10,10,10",
            };
            return View(viewName: "YapiKrediPreSend", model: POSModel);
        }
        public ActionResult YapiKrediResponse(string MerchantPacket, string BankPacket, string Sign, string Xid)
        {
            var POSModel = new RezaB.Web.VPOS.YapiKredi.YapiKrediVPOS3DHostModel()
            {
                XID = Xid,
                PurchaseAmount = 1,
                CurrencyCode = (int)CurrencyCodes.TL,
                EncKey = "10,10,10,10,10,10,10,10",
                MerchantId = "6700972675",
                tid = "67893294",
            };
            var response = POSModel.GetToken(BankPacket, MerchantPacket, Sign);
            var result = POSModel.GetToken(BankPacket);
            TempData["keys"] = result;
            return Redirect(Url.Action("VPOSResults", null, null, Request.Url.Scheme));
        }
        public ActionResult PayTRBasic()
        {
            return View();
        }
        [HttpPost]
        public ActionResult PayTRBasic(string merchantId, decimal amount, string storeKey, string merchant_salt)
        {
            var guid = Guid.NewGuid().ToString();
            var POSModel = new RezaB.Web.VPOS.PayTR.PayTRVPOS3DHostModel()
            {
                OkUrl = Url.Action("VPOSResults", null, new { id = guid }, Request.Url.Scheme),
                FailUrl = Url.Action("VPOSResults", null, new { id = guid }, Request.Url.Scheme),
                Language = "tr",
                MerchantId = merchantId,
                PurchaseAmount = amount,
                Storekey = storeKey,
                BillingCustomerName = "ONUR",
                merchant_salt = merchant_salt,
                InstallmentCount = null,
                CurrencyCode = (int)CurrencyCodes.TL
            };

            ViewBag.POSForm = POSModel.GetHtmlForm();

            return View(viewName: "PayTRPreSend");
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