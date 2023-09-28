using System.Globalization;
using System.Windows.Controls;

namespace WebFormAction.Validations
{
    public class NotEmptyValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string text = (value ?? "").ToString();
            return string.IsNullOrWhiteSpace(text)
                ? new ValidationResult(false, "输入不能为空。")
                : ValidationResult.ValidResult;
        }
    }
}
