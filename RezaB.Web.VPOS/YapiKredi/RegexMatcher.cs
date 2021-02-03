using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RezaB.Web.VPOS.YapiKredi
{
    public static class RegexMatcher
    {
        public static readonly Regex approved = new Regex(@"(?<=<approved>).*?(?=\</approved>)");
        public static readonly Regex respCode = new Regex(@"(?<=<respCode>).*?(?=\</respCode>)");
        public static readonly Regex respText = new Regex(@"(?<=<respText>).*?(?=\</respText>)");
        public static readonly Regex mac = new Regex(@"(?<=<mac>).*?(?=\</mac>)");
        public static readonly Regex hostlogkey = new Regex(@"(?<=<hostlogkey>).*?(?=\</hostlogkey>)");
        public static readonly Regex authCode = new Regex(@"(?<=<authCode>).*?(?=\</authCode>)");
        public static readonly Regex xid = new Regex(@"(?<=<xid>).*?(?=\</xid>)");
        public static readonly Regex amount = new Regex(@"(?<=<amount>).*?(?=\</amount>)");
        public static readonly Regex mdStatus = new Regex(@"(?<=<mdStatus>).*?(?=\</mdStatus>)");
        public static readonly Regex mdErrorMessage = new Regex(@"(?<=<mdErrorMessage>).*?(?=\</mdErrorMessage>)");
        public static readonly Regex posnetData = new Regex(@"(?<=<data1>).*?(?=\</data1>)");
        public static readonly Regex posnetData2 = new Regex(@"(?<=<data2>).*?(?=\</data2>)");
        public static readonly Regex digest = new Regex(@"(?<=<sign>).*?(?=\</sign>)");
    }
}
