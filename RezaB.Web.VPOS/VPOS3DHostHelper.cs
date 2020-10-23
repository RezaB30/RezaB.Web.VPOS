using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace RezaB.Web.VPOS
{
    public static class VPOS3DHostHelper
    {
        public static MvcHtmlString VPOS3DHostFormFor<TModel,TResult>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TResult>> expression) where TResult : VPOS3DHostModel
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            var fieldName = ExpressionHelper.GetExpressionText(expression);
            var fullName = helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(fieldName);
            var Model = metadata.Model;

            var properties = Model.GetType().GetProperties();
            var validProps = new List<PropertyInfo>();
            foreach (var property in properties)
            {
                var attrib = property.GetCustomAttribute<VPOSParameterAttribute>();
                if (attrib != null)
                    validProps.Add(property);
            }

            var actionLink = ((VPOS3DHostModel)Model).ActionLink;

            TagBuilder form = new TagBuilder("form");
            form.MergeAttributes(new Dictionary<string, string>() {
                    { "action", actionLink },
                    { "method", "post" },
                    { "name", "payment" }
                });
            form.AddCssClass("vpos-redirect-container");

            form.InnerHtml = Messages.Redirecting;

            foreach (var property in validProps)
            {
                var currentValue = property.GetValue(Model);
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
