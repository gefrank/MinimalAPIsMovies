using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace MinimalAPIsMovies.Validations
{
    public class ValidationUtilities
    {
        public static string NonEmptyMessage = "The field {PropertyName} is required";
        public static string MaximumLengthMessage = "The field {PropertyName} should be less than {MaxLength} characters";
        public static string FirstLetterIsUppercaseMessage = "The field {PropertyName} should start with an uppercase letter";
        public static string GreaterThanDate(DateTime value) => "The field { PropertyName } should be greater than or equal to " + value.ToString("yyyy-MM-dd");

        public static bool FirstLetterIsUppercase(string value)
        {

            // THis isn't the objective of this validation rule
            if (string.IsNullOrWhiteSpace(value))
            {
                return true;
            }

            var firstLetter = value[0].ToString();
            return firstLetter == firstLetter.ToUpper();

        }

    }
}
