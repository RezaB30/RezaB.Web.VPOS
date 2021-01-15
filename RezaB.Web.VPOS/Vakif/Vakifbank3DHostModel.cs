using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RezaB.Web.VPOS.Vakif
{
    public class Vakifbank3DHostModel : VPOS3DHostModel
    {
        private string TransactionId { get { return Guid.NewGuid().ToString("N"); } }
        public string HostTerminalId { get; set; }
        public string MerchantPassword { get; set; }
        private string TransactionType { get { return "Sale"; } }
        private bool IsSecure { get { return true; } }
        private bool AllowNotEnrolledCard { get { return false; } }
        private string SuccessUrl { get { return OkUrl; } }
        [VPOSParameter]
        public string Ptkn { get; set; }

        public override string ActionLink
        {
            get
            {
                return GetPaymentLink();
                //return @"https://cptest.vakifbank.com.tr/CommonPayment/SecurePayment";
            }
        }
        public override string CalculateHash()
        {
            return null;
        }

        public string GetPaymentLink()
        {
            using (var client = new HttpClient())
            {
                // security issue !! (Shitty Bank) - Confirmed from others
                string parameters = $"HostMerchantId={MerchantId}"
                                + $"&AmountCode={CurrencyCode}"
                                + $"&Amount={PurchaseAmount}"
                                + $"&MerchantPassword={MerchantPassword}"
                                + $"&TransactionType={TransactionType}"
                                + $"&IsSecure={IsSecure}"
                                + $"&AllowNotEnrolledCard={AllowNotEnrolledCard}"
                                + $"&HostTerminalId={HostTerminalId}"
                                + $"&TransactionId={TransactionId}"
                                + $"&SuccessUrl={SuccessUrl}"
                                + $"&FailUrl={FailUrl}";
                var data = new StringContent(parameters, Encoding.UTF8, "application/x-www-form-urlencoded");
                var response = client.PostAsync("https://cpweb.vakifbank.com.tr/CommonPayment/api/RegisterTransaction", data).Result;
                if (response.IsSuccessStatusCode && response.StatusCode == HttpStatusCode.OK)
                {
                    var result = JsonConvert.DeserializeObject<dynamic>(response.Content.ReadAsStringAsync().Result);
                    var postUrl = result.CommonPaymentUrl.Value; /*+ "?Ptkn=" + result.PaymentToken.Value;*/
                    Ptkn = result.PaymentToken.Value;
                    return postUrl;
                }
                return string.Empty;
            }

        }
    }
}
