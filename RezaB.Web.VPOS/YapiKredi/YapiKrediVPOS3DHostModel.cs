using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace RezaB.Web.VPOS.YapiKredi
{
    public class YapiKrediVPOS3DHostModel : VPOS3DHostModel, ITokenBasedVPOS3DHostModel
    {
        public YapiKrediVPOS3DHostModel()
        {
            XID = "YKB_" + Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 16) + "";
        }
        public string XID { get; set; } // like orderId
        [VPOSParameter]
        public string mid { get { return MerchantId; } }
        public string tid { get; set; } // terminal id
        [VPOSParameter]
        public string posnetID { get { return base.Storekey; } }
        public string tranType { get { return "Sale"; } } //  I get default Sale
        private string installment { get { return base.InstallmentCount == null ? "00" : base.InstallmentCount.Value.ToString(); } }
        private string Currency_Code
        {
            get
            {
                return CurrencyCode == (int)CurrencyCodes.TRY ? "TL" : CurrencyCode == (int)CurrencyCodes.USD ? "US" : "EU";
            }
        }
        public int amount { get { return Convert.ToInt32(base.PurchaseAmount * 100); } }
        [VPOSParameter]
        public string lang { get { return Language; } }
        [VPOSParameter]
        public string merchantReturnURL { get { return base.OkUrl; } }
        [VPOSParameter]
        public bool openANewWindow { get { return true; } }
        [VPOSParameter]
        public string posnetData { get { return DataResponse["posnetData"]; } }
        [VPOSParameter]
        public string posnetData2 { get { return DataResponse["posnetData2"]; } }
        [VPOSParameter]
        public string digest { get { return DataResponse["digest"]; } }
        [VPOSParameter]
        public string url { get; set; }
        public string MAC { get { return GetHash(XID + ';' + amount + ';' + Currency_Code + ';' + mid + ';' + GetHash(EncKey + ';' + tid)); } }
        public string EncKey { get; set; }
        public override string ActionLink
        {
            get
            {
                return @"https://setmpos.ykb.com/3DSWebService/YKBPaymentService";
            }
        }
        public override string CalculateHash()
        {
            return "";
        }
        private Dictionary<string, string> dataResponse { get; set; }
        public Dictionary<string, string> DataResponse
        {
            get
            {
                if (dataResponse == null)
                {
                    dataResponse = GetToken();
                }
                return dataResponse;
            }
        }
        private string GetHash(string plainText)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(plainText));
                return Convert.ToBase64String(bytes);
            }
        }

        public Dictionary<string, string> GetToken(string BankPacket, string MerchantPacket, string Sign) // for validate 
        {
            var content = "<posnetRequest>" +
                "<mid>" + mid + "</mid>" +
                "<tid>" + tid + "</tid>" +
                "<oosResolveMerchantData>" +
                "<bankData>" + BankPacket + "</bankData>" +
                "<merchantData>" + MerchantPacket + "</merchantData>" +
                "<sign>" + Sign + "</sign>" +
                "<mac>" + MAC + "</mac>" +
                "</oosResolveMerchantData>" +
                "</posnetRequest>";
            var EncodeContent = HttpUtility.UrlEncode(content, Encoding.UTF8);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://setmpos.ykb.com/PosnetWebService/XML?xmldata=" + EncodeContent + "");
            request.ContentType = "application/xwww-form-urlencoded; charset=utf-8";
            request.Method = "POST";
            Stream requestStream = request.GetRequestStream();
            requestStream.Close();
            HttpWebResponse response;
            response = (HttpWebResponse)request.GetResponse();
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream responseStream = response.GetResponseStream();
                string responseStr = new StreamReader(responseStream).ReadToEnd();
                keyValuePairs.Add("approved", RegexMatcher.approved.Match(responseStr).Value);
                keyValuePairs.Add("respCode", RegexMatcher.respCode.Match(responseStr).Value);
                keyValuePairs.Add("respText", RegexMatcher.respText.Match(responseStr).Value);
                keyValuePairs.Add("mdErrorMessage", RegexMatcher.mdErrorMessage.Match(responseStr).Value);
                keyValuePairs.Add("mdStatus", RegexMatcher.mdStatus.Match(responseStr).Value);
                return keyValuePairs;
            }
            return keyValuePairs;
        }
        public bool Finalize()
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, string> GetToken(string BankPacket)   // for finance
        {
            var content = "<posnetRequest>" +
                "<mid>" + mid + "</mid>" +
                "<tid>" + tid + "</tid>" +
                "<oosTranData>" +
                "<bankData>" + BankPacket + "</bankData>" +
                "<wpAmount>0</wpAmount>" +
                "<mac>" + MAC + "</mac>" +
                "</oosTranData>" +
                "</posnetRequest>";
            var EncodeContent = HttpUtility.UrlEncode(content, Encoding.UTF8);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://setmpos.ykb.com/PosnetWebService/XML?xmldata=" + EncodeContent + "");
            request.ContentType = "application/xwww-form-urlencoded; charset=utf-8";
            request.Method = "POST";
            Stream requestStream = request.GetRequestStream();
            requestStream.Close();
            HttpWebResponse response;
            response = (HttpWebResponse)request.GetResponse();
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream responseStream = response.GetResponseStream();
                string responseStr = new StreamReader(responseStream).ReadToEnd();
                keyValuePairs.Add("approved", RegexMatcher.approved.Match(responseStr).Value);
                keyValuePairs.Add("authCode", RegexMatcher.authCode.Match(responseStr).Value);
                keyValuePairs.Add("hostlogkey", RegexMatcher.hostlogkey.Match(responseStr).Value);
                keyValuePairs.Add("mac", RegexMatcher.mac.Match(responseStr).Value);
                keyValuePairs.Add("respCode", RegexMatcher.respCode.Match(responseStr).Value);
                keyValuePairs.Add("respText", RegexMatcher.respText.Match(responseStr).Value);
                return keyValuePairs;
            }
            return keyValuePairs;
        }

        public Dictionary<string, string> GetToken()
        {
            var content = "<posnetRequest>" +
                    "<mid>" + mid + "</mid>" +
                    "<tid>" + tid + "</tid>" +
                    "<oosRequestData>" +
                    "<posnetid>" + posnetID + "</posnetid>" +
                    "<XID>" + XID + "</XID>" +
                    "<amount>" + amount + "</amount>" +
                    "<currencyCode>" + Currency_Code + "</currencyCode>" +
                    "<installment>" + installment + "</installment>" +
                    "<tranType>" + tranType + "</tranType>" +
                    "<cardHolderName></cardHolderName>" +
                    "<ccno></ccno>" +
                    "<expDate></expDate>" +
                    "<cvc></cvc>" +
                    "</oosRequestData>" +
                    "</posnetRequest > ";
            var EncodeContent = HttpUtility.UrlEncode(content, Encoding.UTF8);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://setmpos.ykb.com/PosnetWebService/XML?xmldata=" + EncodeContent + "");
            request.Headers.Add("X-MERCHANT-ID", mid);
            request.Headers.Add("X-TERMINAL-ID", tid);
            request.Headers.Add("X-POSNET-ID", posnetID);
            request.Headers.Add("X-CORRELATION-ID", XID);
            request.ContentType = "application/xwww-form-urlencoded; charset=utf-8";
            request.Method = "POST";
            Stream requestStream = request.GetRequestStream();
            requestStream.Close();
            HttpWebResponse response;
            response = (HttpWebResponse)request.GetResponse();
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream responseStream = response.GetResponseStream();
                string responseStr = new StreamReader(responseStream).ReadToEnd();
                keyValuePairs.Add("posnetData", RegexMatcher.posnetData.Match(responseStr).Value);
                keyValuePairs.Add("posnetData2", RegexMatcher.posnetData2.Match(responseStr).Value);
                keyValuePairs.Add("digest", RegexMatcher.digest.Match(responseStr).Value);
                return keyValuePairs;
            }
            return keyValuePairs;
        }
    }
}