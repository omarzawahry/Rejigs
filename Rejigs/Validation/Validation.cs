using System.Text.RegularExpressions;

namespace Rejigs;

public partial class Rejigs
{
    /// <summary>
    /// Validates the input string against the built regex pattern.
    /// </summary>
    /// <param name="input">The string to validate</param>
    /// <param name="errorMessage">Optional custom error message to use when validation fails</param>
    /// <returns>True if the input matches the pattern</returns>
    /// <exception cref="RejigsValidationException">Thrown when the input doesn't match the pattern</exception>
    public bool Validate(string input, string? errorMessage = null)
    {
        if (string.IsNullOrEmpty(input))
        {
            throw new RejigsValidationException(errorMessage ?? "Input cannot be null or empty");
        }

        var regex = new Regex(_pattern, _options);
        bool isValid = regex.IsMatch(input);

        if (!isValid)
        {
            throw new RejigsValidationException(errorMessage ?? $"Input '{input}' does not match the required pattern");
        }

        return true;
    }
}