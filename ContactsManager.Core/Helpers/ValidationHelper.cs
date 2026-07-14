using System.ComponentModel.DataAnnotations;

namespace ContactsManager.Core.Helpers
{
    public class ValidationHelper
    {
        /// <summary>
        /// Validates the specified object using data annotation attributes and throws an exception if validation fails.
        /// </summary>
        /// <remarks>All validation attributes applied to the object's public properties are evaluated. If
        /// any validation errors are found, only the first error message is included in the exception. This method
        /// performs a deep validation, including properties marked with validation attributes.</remarks>
        /// <param name="obj">The object to validate. All public properties are checked against their data annotation attributes.</param>
        /// <exception cref="ArgumentException">Thrown when the object fails validation. The exception message contains the first validation error message.</exception>
        internal static void ModelValidation(object obj)
        {
            //validation
            ValidationContext validationContext = new ValidationContext(obj);
            List<ValidationResult> validationResults = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResults, true);

            if (!isValid)
            {
                throw new ArgumentException(validationResults.FirstOrDefault()?.ErrorMessage);
            }
        }
    }
}