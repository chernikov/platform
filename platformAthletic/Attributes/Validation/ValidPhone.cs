using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace platformAthletic.Attributes.Validation
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ValidPhoneAttribute : ValidationAttribute, IClientValidatable
    {
        protected string pattern = @"[0-9,\+,\(,\),\-,\x20]+";

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

            var regex = new Regex(this.pattern, RegexOptions.Compiled);
            var match = regex.Match(source);
            return (match.Success && match.Length == source.Length);
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            //string errorMessage = this.FormatErrorMessage(metadata.DisplayName);
            string errorMessage = ErrorMessageString;

            // The value we set here are needed by the jQuery adapter
            ModelClientValidationRule validPhoneRule = new ModelClientValidationRule();
            validPhoneRule.ErrorMessage = errorMessage;
            validPhoneRule.ValidationType = "validphone"; // This is the name the jQuery validator will use
            validPhoneRule.ValidationParameters.Add("regex", this.pattern);
            yield return validPhoneRule;
        }
    }
}