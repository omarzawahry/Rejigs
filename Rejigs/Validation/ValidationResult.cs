/// <summary>
/// Represents the result of a Rejigs validation operation.
/// </summary>
public class RejigsValidationResult
{
    /// <summary>
    /// Gets a value indicating whether the validation was successful.
    /// </summary>
    public bool IsValid { get; }

    /// <summary>
    /// Gets the error message if validation failed, or null if validation succeeded.
    /// </summary>
    public string? ErrorMessage { get; }

    /// <summary>
    /// Initializes a new instance of the RejigsValidationResult class.
    /// </summary>
    /// <param name="isValid">Whether the validation was successful</param>
    /// <param name="errorMessage">The error message if validation failed</param>
    public RejigsValidationResult(bool isValid, string? errorMessage)
    {
        IsValid = isValid;
        ErrorMessage = errorMessage;
    }
}