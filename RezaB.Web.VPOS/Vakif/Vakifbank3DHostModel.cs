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
        public string TransactionId { get { return Guid.NewGuid().ToString("N"); } }
        //[VPOSParameter]
        public string HostMerchantId { get; set; }
        //[VPOSParameter]
        public string HostTerminalId { get; set; }
        //[VPOSParameter]
        public int AmountCode { get; set; }
        //[VPOSParameter]
        public string MerchantPassword { get; set; }
        //[VPOSParameter]
        public string TransactionType { get { return "Sale"; } }
        //[VPOSParameter]
        public bool IsSecure { get { return true; } }
        //[VPOSParameter]
        public bool AllowNotEnrolledCard { get { return false; } }
        //[VPOSParameter]
        public string SuccessUrl { get; set; }
        public decimal Amount { get; set; }
        [VPOSParameter]
        public string Ptkn { get; set; }
        //[VPOSParameter]
        //public new string FailUrl { get { return failUrl; } }


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
            //var data = string.Format("{0}{1}{2}{3}{4}{5}", HostMerchantId, AmountCode, Amount.ToString("##################"), MerchantPassword, TransactionId, "VBank3DPay2014");
            //SHA1 sha1 = new SHA1CryptoServiceProvider();
            //byte[] notHashedBytes = System.Text.Encoding.ASCII.GetBytes(data);
            //byte[] hashedByte = sha1.ComputeHash(notHashedBytes);
            //string hashedData = System.Convert.ToBase64String(hashedByte);
            //return hashedData;
            return null;
        }

        public string GetPaymentLink()
        {
            using (var client = new HttpClient())
            {
                // security issue !! (Shitty Bank) - Confirmed from others
                string parameters = $"HostMerchantId={HostMerchantId}"
                                + $"&AmountCode={AmountCode}"
                                + $"&Amount={Amount}"
                                + $"&MerchantPassword={MerchantPassword}"
                                + $"&TransactionType={TransactionType}"
                                + $"&IsSecure={IsSecure}"
                                + $"&AllowNotEnrolledCard={AllowNotEnrolledCard}"
                                + $"&HostTerminalId={HostTerminalId}"
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
