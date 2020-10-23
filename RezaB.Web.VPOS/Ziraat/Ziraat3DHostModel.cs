using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RezaB.Web.VPOS.Ziraat
{
    public class Ziraat3DHostModel : Netspay3DHostModel
    {
        //[VPOSParameter]
        //public string clientid { get { return MerchantId; } }

        //[VPOSParameter]
        //public string stotetype { get { return "3d_pay_hosting"; } }

        //[VPOSParameter]
        //public string islemtipi { get { return "Auth"; } }

        //[VPOSParameter]
        //public string amount { get { return PurchaseAmount.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture); } }

        //[VPOSParameter]
        //public int currency { get { return CurrencyCode; } }

        //[VPOSParameter]
        //public string oid { get { return OrderId; } }

        //[VPOSParameter]
        //public string okUrl { get { return OkUrl; } }

        //[VPOSParameter]
        //public string failUrl { get { return FailUrl; } }

        //[VPOSParameter]
        //public string callbackUrl { get { return FailUrl; } }

        //[VPOSParameter]
        //public string lang { get { return Language; } }

        //[VPOSParameter]
        //public long rnd { get { return randomNumber; } }

        //private readonly long randomNumber = DateTime.Now.Ticks;

        //[VPOSParameter]
        //public int? Taksit { get { return InstallmentCount; } }

        //[VPOSParameter]
        //public string hash { get { return CalculateHash(); } }

        //[VPOSParameter]
        //public int refreshtime { get { return 1; } }

        //[VPOSParameter]
        //public string Faturafirma { get { return BillingCustomerName; } }

        public override string ActionLink
        {
            get
            {
                return @"https://sanalpos2.ziraatbank.com.tr/fim/est3dgate";
            }
        }

        //public override string CalculateHash()
        //{
        //    var hasher = SHA1.Create();
        //    var hashString = clientid + Convert.ToString(oid) + amount + OkUrl + FailUrl + islemtipi + Convert.ToString(Taksit) + Convert.ToString(rnd) + callbackUrl + Storekey;
        //    var bytes = Encoding.UTF8.GetBytes(hashString);
        //    var hashedBytes = hasher.ComputeHash(bytes);
        //    return Convert.ToBase64String(hashedBytes);
        //}
    }
}
