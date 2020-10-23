using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RezaB.Web.VPOS
{
    public abstract class VPOS3DHostModel
    {
        public string OkUrl { get; set; }

        public string FailUrl { get; set; }

        public decimal PurchaseAmount { get; set; }

        public string Storekey { get; set; }

        public string MerchantId { get; set; }

        public int CurrencyCode { get; set; }

        public string Language { get; set; }

        public int? InstallmentCount { get; set; }

        public string OrderId { get; set; }

        public string BillingCustomerName { get; set; }

        public abstract string ActionLink { get; }

        public abstract string CalculateHash();
    }
}
