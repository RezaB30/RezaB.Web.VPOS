using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RezaB.Web.VPOS
{
    public interface ITokenBasedVPOS3DHostModel
    {
        abstract Dictionary<string, string> GetToken(string BankPacket, string MerchantPacket, string Sign);
        abstract Dictionary<string, string> GetToken(string BankPacket);
        abstract Dictionary<string, string> GetToken(); // first xml data
        abstract bool Finalize();
    }
}
