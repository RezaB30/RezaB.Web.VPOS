using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

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

        public MvcHtmlString GetHtmlForm(string method = "post")
        {
            var properties = GetType().GetProperties();
            var validProps = new List<PropertyInfo>();
            foreach (var property in properties)
            {
                var attrib = property.GetCustomAttribute<VPOSParameterAttribute>();
                if (attrib != null)
                    validProps.Add(property);
            }

            TagBuilder form = new TagBuilder("form");
            form.MergeAttributes(new Dictionary<string, string>() {
                    { "action", ActionLink },
                    { "method", method },
                    { "name", "payment" }
                });
            form.AddCssClass("vpos-redirect-container");

            form.InnerHtml = Messages.Redirecting;

            foreach (var property in validProps)
            {
                var currentValue = property.GetValue(this);
                TagBuilder hidden = new TagBuilder("input");
                hidden.MergeAttribute("type", "hidden");
                hidden.MergeAttribute("name", property.Name);
                hidden.MergeAttribute("value", currentValue != null ? currentValue.ToString() : string.Empty);
                form.InnerHtml += hidden.ToString(TagRenderMode.SelfClosing);
            }

            TagBuilder script = new TagBuilder("script");
            script.MergeAttribute("type", "text/javascript");
            script.InnerHtml += "document.payment.submit();";

            return new MvcHtmlString(form.ToString(TagRenderMode.Normal) + Environment.NewLine + script.ToString(TagRenderMode.Normal));
        }
    }
}
