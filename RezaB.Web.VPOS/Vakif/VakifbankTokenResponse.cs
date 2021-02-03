using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RezaB.Web.VPOS.Vakif
{
    class VakifbankTokenResponse
    {
        public string PaymentToken { get; set; }

        public string CommonPaymentUrl { get; set; }

        public int? ErrorCode { get; set; }

        public string ResponseMessage { get; set; }
    }
}
