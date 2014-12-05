using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace Presentacion.Helpers.Bootstrap
{
    public static class BootstrapHelpers
    {
        public static MvcHtmlString BootstrapTextBoxFor<TModel, TProperty>(
             this HtmlHelper<TModel> helper,
             Expression<Func<TModel, TProperty>> expression)
        {
            return helper.TextBoxFor(expression, new { @class = "form-control" });
        }

        public static MvcHtmlString BootstrapLabelFor<TModel, TProperty>(
             this HtmlHelper<TModel> helper,
             Expression<Func<TModel, TProperty>> expression)
        {
            return helper.LabelFor(expression, new { @class = "control-label" });
        }

        public static MvcHtmlString BootstrapLabelFor<TModel, TProperty>(
             this HtmlHelper<TModel> helper,
             Expression<Func<TModel, TProperty>> expression,
             string text)
        {
            return helper.LabelFor(expression, text, new { @class = "control-label" });
        }

        public static MvcHtmlString BootstrapValidationMessageFor<TModel, TProperty>(
             this HtmlHelper<TModel> helper,
             Expression<Func<TModel, TProperty>> expression)
        {
            return helper.ValidationMessageFor(expression, null, new { @class = "control-label" });
        }

        public static MvcHtmlString BootstrapActionButton(this HtmlHelper helper, string text, string action,
            object htmlAttributes)
        {
            return helper.ActionLink(text, action, null, AddBtnClass(htmlAttributes));
        }

        public static MvcHtmlString BootstrapActionButton(this HtmlHelper helper, string text, string action,
            string controller, object routeValues, object htmlAttributes)
        {
            return helper.ActionLink(text, action, controller, new RouteValueDictionary(routeValues), AddBtnClass(htmlAttributes));
        }

        public static MvcHtmlString BootstrapActionButton(this HtmlHelper helper, string text, string action,
            object routeValues, object htmlAttributes)
        {
            return helper.ActionLink(text, action, new RouteValueDictionary(routeValues), AddBtnClass(htmlAttributes));
        }

        public static RouteValueDictionary AddBtnClass(object htmlAttributes)
        {
            var newAttributes = new RouteValueDictionary(htmlAttributes);

            if (newAttributes.ContainsKey("class"))
                newAttributes["class"] = "btn " + newAttributes["class"];
            else
                newAttributes.Add("class", "btn ");

            return newAttributes;
        }

        public static MvcHtmlString BootstrapDropdownListFor<TModel, TProperty>(
            this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> expression,
            IEnumerable<SelectListItem> model
            )
        {
            return helper.DropDownListFor(expression, model, new { @class = "form-control" });
        }

        public static MvcHtmlString BootstrapDropdownListFor<TModel, TProperty>(
            this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> expression,
            IEnumerable<SelectListItem> model,
            string htmlId
            )
        {
            return helper.DropDownListFor(expression, model, new { id= htmlId, @class = "form-control" });
        }
    }
}