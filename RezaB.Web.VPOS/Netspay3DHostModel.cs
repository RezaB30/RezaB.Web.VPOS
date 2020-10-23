using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RezaB.Web.VPOS
{
    public abstract class Netspay3DHostModel : VPOS3DHostModel
    {
        [VPOSParameter]
        public string clientid { get { return MerchantId; } }

        [VPOSParameter]
        public string stotetype { get { return "3d_pay_hosting"; } }

        [VPOSParameter]
        public string islemtipi { get { return "Auth"; } }

        [VPOSParameter]
        public string amount { get { return PurchaseAmount.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture); } }

        [VPOSParameter]
        public int currency { get { return CurrencyCode; } }

        [VPOSParameter]
        public string oid { get { return OrderId; } }

        [VPOSParameter]
        public string okUrl { get { return OkUrl; } }

        [VPOSParameter]
        public string failUrl { get { return FailUrl; } }

        [VPOSParameter]
        public string callbackUrl { get { return FailUrl; } }

        [VPOSParameter]
        public string lang { get { return Language; } }

        [VPOSParameter]
        public long rnd { get { return randomNumber; } }

        private readonly long randomNumber = DateTime.Now.Ticks;

        [VPOSParameter]
        public int? Taksit { get { return InstallmentCount; } }

        [VPOSParameter]
        public string hash { get { return CalculateHash(); } }

        [VPOSParameter]
        public int refreshtime { get { return 1; } }

        [VPOSParameter]
        public string Faturafirma { get { return BillingCustomerName; } }

        #region private empty
        [VPOSParameter]
        public string BillToCompany { get { return " "; } }

        [VPOSParameter]
        public string BillToName { get { return " "; } }

        //[VPOSParameter]
        //public string Fismi { get { return " "; } }

        //[VPOSParameter]
        //public string faturaFirma { get { return " "; } }

        //[VPOSParameter]
        //public string Fadres { get { return " "; } }

        //[VPOSParameter]
        //public string Fadres2 { get { return " "; } }

        //[VPOSParameter]
        //public string Fil { get { return " "; } }

        //[VPOSParameter]
        //public string Filce { get { return " "; } }

        //[VPOSParameter]
        //public string Fpostakodu { get { return " "; } }

        //[VPOSParameter]
        //public string tel { get { return " "; } }

        //[VPOSParameter]
        //public string fulkekod { get { return " "; } }

        //[VPOSParameter]
        //public string nakliyeFirma { get { return " "; } }

        //[VPOSParameter]
        //public string tismi { get { return " "; } }

        //[VPOSParameter]
        //public string tadres { get { return " "; } }

        //[VPOSParameter]
        //public string tadres2 { get { return " "; } }

        //[VPOSParameter]
        //public string til { get { return " "; } }

        //[VPOSParameter]
        //public string tilce { get { return " "; } }

        //[VPOSParameter]
        //public string tpostakodu { get { return " "; } }

        //[VPOSParameter]
        //public string tulkekod { get { return " "; } }

        //[VPOSParameter]
        //public string itemnumber1 { get { return " "; } }

        //[VPOSParameter]
        //public string productcode1 { get { return " "; } }

        //[VPOSParameter]
        //public string qty1 { get { return " "; } }

        //[VPOSParameter]
        //public string desc1 { get { return " "; } }

        //[VPOSParameter]
        //public string id1 { get { return " "; } }

        //[VPOSParameter]
        //public string price1 { get { return " "; } }

        //[VPOSParameter]
        //public string total1 { get { return " "; } }
        #endregion

        public override string CalculateHash()
        {
            var hasher = SHA1.Create();
            var hashString = clientid + Convert.ToString(oid) + amount + OkUrl + FailUrl + islemtipi + Convert.ToString(Taksit) + Convert.ToString(rnd) + callbackUrl + Storekey;
            var bytes = Encoding.UTF8.GetBytes(hashString);
            var hashedBytes = hasher.ComputeHash(bytes);
            return Convert.ToBase64String(hashedBytes);
        }
    }
}
