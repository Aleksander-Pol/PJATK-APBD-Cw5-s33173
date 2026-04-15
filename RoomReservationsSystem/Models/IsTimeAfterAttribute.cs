using System.ComponentModel.DataAnnotations;

namespace RoomReservationsSystem.Models;

public class IsTimeAfterAttribute : ValidationAttribute
{
    private readonly string _startTime;
    private readonly string? _errorMessage;

    public IsTimeAfterAttribute(string startTime, string? errorMessage = null)
    {
        _startTime = startTime;
        _errorMessage = errorMessage;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is null)
            return ValidationResult.Success;

        if (value.GetType() != typeof(TimeOnly))
            return ValidationResult.Success;
        
        var endTime = (TimeOnly)value;

        var startTimeProperty = validationContext.ObjectType.GetProperty(_startTime);

        if (startTimeProperty is null) return new ValidationResult("Nie znaleziono właściwości o takiej nazwie");

        var startTimeValue = startTimeProperty.GetValue(validationContext.ObjectInstance);

        if (startTimeValue is null) return ValidationResult.Success;


        if (startTimeValue.GetType() != typeof(TimeOnly)) return ValidationResult.Success;

        TimeOnly startTime = (TimeOnly)startTimeValue;
        
        if (endTime <= startTime)
            return new ValidationResult(_errorMessage ?? "Czas zakończenia musi być późniejszy");

        return ValidationResult.Success;
    }
}