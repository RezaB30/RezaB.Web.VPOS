using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RezaB.Web.VPOS.QNBFinans
{
    public class QNBFinansVPOS3DHostModel : VPOS3DHostModel
    {
        [VPOSParameter]
        public string SecureType { get { return "3DHost"; } }

        [VPOSParameter]
        public string TxnType { get { return "Auth"; } }

        [VPOSParameter]
        public string MbrId { get { return "5"; } }

        [VPOSParameter]
        public new string OrderId { get { return base.OrderId; } set { base.OrderId = value; } }

        [VPOSParameter]
        public int? OrgOrderId { get; set; }

        [VPOSParameter]
        public long Rnd { get { return randomNumber; } }

        private readonly long randomNumber = DateTime.Now.Ticks;

        [VPOSParameter]
        public int Currency { get { return CurrencyCode; } }

        [VPOSParameter]
        public string Lang { get { return Language; } }

        [VPOSParameter]
        public string PurchAmount { get { return PurchaseAmount.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture); } }

        [VPOSParameter]
        public string MerchantID { get { return MerchantId; } }

        [VPOSParameter]
        public string UserCode { get; set; }

        public string UserPass { get; set; }

        [VPOSParameter]
        public new int? InstallmentCount { get { return base.InstallmentCount ?? 0; } set { base.InstallmentCount = value; } }

        [VPOSParameter]
        public new string OkUrl { get { return base.OkUrl; } set { base.OkUrl = value; } }

        [VPOSParameter]
        public new string FailUrl { get { return base.FailUrl; } set { base.FailUrl = value; } }

        [VPOSParameter]
        public string Hash { get { return CalculateHash(); } }

        [VPOSParameter]
        public string BillingNameSurname { get { return BillingCustomerName; } }

        public override string ActionLink
        {
            get
            {
                return @"https://vpos.qnbfinansbank.com/Gateway/3DHost.aspx";
            }
        }

        public override string CalculateHash()
        {
            var hasher = SHA1.Create();
            var hashString = Convert.ToString(MbrId) + Convert.ToString(OrderId) + PurchAmount + OkUrl + FailUrl + TxnType + Convert.ToString(InstallmentCount) + Convert.ToString(Rnd) + Storekey;
            var bytes = Encoding.UTF8.GetBytes(hashString);
            var hashedBytes = hasher.ComputeHash(bytes);
            return Convert.ToBase64String(hashedBytes);
        }
    }
}
