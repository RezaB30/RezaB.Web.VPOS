using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace RezaB.Web.VPOS.PayTR
{
    public class PayTRVPOS3DHostModel : VPOS3DHostModel
    {
        public PayTRVPOS3DHostModel()
        {
            merchant_oid = Guid.NewGuid().ToString().Replace("-", string.Empty);
        }
        public string merchant_salt { get; set; }
        public string token { get { return GetToken(); } }
        private int merchant_id { get { return Convert.ToInt32(MerchantId); } }
        private string user_ip { get { return GetIp(); } }
        private string merchant_oid { get; set; } // store order no max 64 char.
        public string email { get; set; }
        private int payment_amount { get { return Convert.ToInt32(PurchaseAmount * 100); } } // ex : for 34.56 = 34.56 * 100 = 3456
        private string currency { get { return "TRY"; } } // ex : TL or TRY , EUR , USD ...
        private string user_basket
        {
            get
            {
                object[][] user_basket = { };
                return JsonConvert.SerializeObject(user_basket);
            }
        }
        private int no_installment { get { return base.InstallmentCount == null ? 1 : 0; } } // 1 = no installment , 0 = yes ins..
        private int max_installment { get { return base.InstallmentCount == null ? 0 : base.InstallmentCount.Value; } } // set default 0 
        private string paytr_token { get { return CalculateHash(); } }
        private string user_name { get { return BillingCustomerName; } } // customer fullname max 60 char.
        public string user_address { get; set; } // customer address max 60 char.
        public string user_phone { get; set; } // max 20 char.
        private string merchant_ok_url { get { return base.OkUrl; } } // redirect success page
        private string merchant_fail_url { get { return base.FailUrl; } } // redirect fail page
        private int test_mode { get { return 1; } } // for test set 1
        private int debug_on { get { return 1; } } // set 1 for errors
        //public int timeout_limit { get; set; } // if you don't send , will be 30 min.
        private string lang { get { return Language; } } // if you send empty , will be tr
        public override string ActionLink
        {
            get
            {
                return @"https://www.paytr.com/odeme/guvenli/" + token;
            }
        }

        public override string CalculateHash()
        {
            string Combine = string.Concat(MerchantId, user_ip, merchant_oid, email, payment_amount.ToString(), user_basket, no_installment.ToString(), max_installment.ToString(), currency, test_mode.ToString(), merchant_salt);
            HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(Storekey));
            byte[] b = hmac.ComputeHash(Encoding.UTF8.GetBytes(Combine));
            return Convert.ToBase64String(b);
        }
        private string GetIp()
        {
            string _user_ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (_user_ip == "" || _user_ip == null)
            {
                _user_ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            return "185.188.129.89";
            //return _user_ip;
        }
        private string GetToken()
        {
            NameValueCollection data = new NameValueCollection();
            data.Add("currency", "TRY");
            data.Add("merchant_id", MerchantId);
            data.Add("email", email);
            data.Add("debug_on", debug_on.ToString());
            data.Add("lang", lang);
            data.Add("max_installment", max_installment.ToString());
            data.Add("merchant_oid", merchant_oid);
            data.Add("no_installment", no_installment.ToString());
            data.Add("payment_amount", payment_amount.ToString());
            data.Add("user_basket", user_basket);
            data.Add("merchant_fail_url", merchant_fail_url);
            data.Add("merchant_ok_url", merchant_ok_url);
            data.Add("test_mode", test_mode.ToString());
            //data.Add("timeout_limit", "10");
            data.Add("user_address", user_address);
            data.Add("user_ip", user_ip);
            data.Add("user_name", user_name);
            data.Add("user_phone", user_phone);
            data.Add("paytr_token", paytr_token);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            using (var client = new WebClient())
            {
                client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                byte[] response = client.UploadValues("https://www.paytr.com/odeme/api/get-token", "POST", data);
                string ResultAuthTicket = Encoding.UTF8.GetString(response);
                var result = JsonConvert.DeserializeObject<dynamic>(ResultAuthTicket);
                if (result.status == "success")
                {
                    return result.token;
                }
                return "";
            }
        }
    }
}
