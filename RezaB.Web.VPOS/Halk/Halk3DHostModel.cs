using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RezaB.Web.VPOS.Halk
{
    public class Halk3DHostModel : Netspay3DHostModel
    {
        public override string ActionLink
        { 
            get
            {
                return @"https://sanalpos.halkbank.com.tr/fim/est3dgate";
            }
        }
    }
}
