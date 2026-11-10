using System.ComponentModel.DataAnnotations;

namespace School.Application.Services;

internal static class ServiceValidation
{
    internal static void Validate(object dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        Validator.ValidateObject(dto, new ValidationContext(dto), validateAllProperties: true);
    }

    internal static void Require(bool condition, string message)
    {
        if (!condition) throw new ValidationException(message);
    }
}
