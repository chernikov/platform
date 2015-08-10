using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace platformAthletic.Attributes.Validation
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ValidEmailAttribute : ValidationAttribute, IClientValidatable
    {
        // old regular expretion: @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
        //protected string pattern = "^\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*$";
        protected string pattern = @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }
            if (!(value is string))
            {
                return true;
            }

            var source = value as string;

            var regex = new Regex(this.pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var match = regex.Match(source);
            return (match.Success && match.Length == source.Length);
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            //string errorMessage = this.FormatErrorMessage(metadata.DisplayName);
            string errorMessage = ErrorMessageString;

            // The value we set here are needed by the jQuery adapter
            ModelClientValidationRule validEmailRule = new ModelClientValidationRule();
            validEmailRule.ErrorMessage = errorMessage;
            validEmailRule.ValidationType = "validemail"; // This is the name the jQuery validator will use
            validEmailRule.ValidationParameters.Add("regex", this.pattern.Replace("\\","\\\\"));
            yield return validEmailRule;
        }
    }
}