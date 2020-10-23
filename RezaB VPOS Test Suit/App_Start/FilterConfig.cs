using System.Web;
using System.Web.Mvc;

namespace RezaB_VPOS_Test_Suit
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
