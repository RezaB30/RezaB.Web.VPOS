using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
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
                var parameters = new KeyValuePair<string, string>[]
                {
                    new KeyValuePair<string, string>( "HostMerchantId", MerchantId ),
                    new KeyValuePair<string, string>( "AmountCode", Convert.ToString(CurrencyCode) ),
                    new KeyValuePair<string, string>( "Amount", PurchaseAmount.ToString("#0.00",CultureInfo.InvariantCulture) ),
                    new KeyValuePair<string, string>( "MerchantPassword", MerchantPassword ),
                    new KeyValuePair<string, string>( "TransactionType", TransactionType ),
                    new KeyValuePair<string, string>( "IsSecure", Convert.ToString(IsSecure) ),
                    new KeyValuePair<string, string>( "AllowNotEnrolledCard", Convert.ToString(AllowNotEnrolledCard) ),
                    new KeyValuePair<string, string>( "HostTerminalId", HostTerminalId ),
                    new KeyValuePair<string, string>( "TransactionId", TransactionId  ),
                    new KeyValuePair<string, string>( "SuccessUrl", SuccessUrl ),
                    new KeyValuePair<string, string>( "FailUrl", FailUrl )
                };

                try
                {
                    var response = client.PostAsync("https://cpweb.vakifbank.com.tr/CommonPayment/api/RegisterTransaction", new FormUrlEncodedContent(parameters)).Result;
                    if (response.IsSuccessStatusCode && response.StatusCode == HttpStatusCode.OK)
                    {
                        var result = JsonConvert.DeserializeObject<VakifbankTokenResponse>(response.Content.ReadAsStringAsync().Result);
                        var postUrl = result.CommonPaymentUrl ?? FailUrl;
                        Ptkn = result.PaymentToken;
                        return postUrl;
                    }
                }
                catch
                {
                }

                return FailUrl;
            }

        }
    }
}
