using NUnit.Framework;

namespace Rejigs.Tests;

public class ValidationTests
{
    [Test]
    public void Validate_ValidInput_ReturnsTrue()
    {
        var rejigs = Rejigs.Create().Text("hello");
        
        var result = rejigs.Validate("hello");
        
        Assert.That(result, Is.True);
    }

    [Test]
    public void Validate_InvalidInput_ThrowsRejigsValidationException()
    {
        var rejigs = Rejigs.Create().Text("hello");
        
        var exception = Assert.Throws<RejigsValidationException>(() => rejigs.Validate("world"));
        
        Assert.That(exception.Message, Is.EqualTo("Input 'world' does not match the required pattern"));
    }

    [Test]
    public void Validate_NullInput_ThrowsRejigsValidationException()
    {
        var rejigs = Rejigs.Create().Text("hello");
        
        var exception = Assert.Throws<RejigsValidationException>(() => rejigs.Validate(null!));
        
        Assert.That(exception.Message, Is.EqualTo("Input cannot be null or empty"));
    }

    [Test]
    public void Validate_EmptyInput_ThrowsRejigsValidationException()
    {
        var rejigs = Rejigs.Create().Text("hello");
        
        var exception = Assert.Throws<RejigsValidationException>(() => rejigs.Validate(""));
        
        Assert.That(exception.Message, Is.EqualTo("Input cannot be null or empty"));
    }

    [Test]
    public void Validate_WhitespaceInput_ThrowsRejigsValidationException()
    {
        var rejigs = Rejigs.Create().Text("hello");
        
        var exception = Assert.Throws<RejigsValidationException>(() => rejigs.Validate("   "));
        
        Assert.That(exception.Message, Is.EqualTo("Input '   ' does not match the required pattern"));
    }

    [Test]
    public void Validate_CustomErrorMessage_ThrowsWithCustomMessage()
    {
        var rejigs = Rejigs.Create().Text("hello");
        const string customMessage = "Custom validation error";
        
        var exception = Assert.Throws<RejigsValidationException>(() => 
            rejigs.Validate("world", customMessage));
        
        Assert.That(exception.Message, Is.EqualTo(customMessage));
    }

    [Test]
    public void Validate_CustomErrorMessageForNullInput_ThrowsWithCustomMessage()
    {
        var rejigs = Rejigs.Create().Text("hello");
        const string customMessage = "Input is required";
        
        var exception = Assert.Throws<RejigsValidationException>(() => 
            rejigs.Validate(null!, customMessage));
        
        Assert.That(exception.Message, Is.EqualTo(customMessage));
    }

    [Test]
    public void Validate_CustomErrorMessageForEmptyInput_ThrowsWithCustomMessage()
    {
        var rejigs = Rejigs.Create().Text("hello");
        const string customMessage = "Input cannot be empty";
        
        var exception = Assert.Throws<RejigsValidationException>(() => 
            rejigs.Validate("", customMessage));
        
        Assert.That(exception.Message, Is.EqualTo(customMessage));
    }

    [Test]
    public void Validate_ComplexPattern_ValidInput_ReturnsTrue()
    {
        var rejigs = Rejigs.Create()
            .AtStart()
            .OneOrMore(r => r.AnyDigit())
            .Text("@")
            .OneOrMore(r => r.AnyInRange('a', 'z'))
            .Text(".com")
            .AtEnd();
        
        var result = rejigs.Validate("123@example.com");
        
        Assert.That(result, Is.True);
    }

    [Test]
    public void Validate_ComplexPattern_InvalidInput_ThrowsException()
    {
        var rejigs = Rejigs.Create()
            .AtStart()
            .OneOrMore(r => r.AnyDigit())
            .Text("@")
            .OneOrMore(r => r.AnyInRange('a', 'z'))
            .Text(".com")
            .AtEnd();
        
        var exception = Assert.Throws<RejigsValidationException>(() => 
            rejigs.Validate("invalid-email"));
        
        Assert.That(exception.Message, Contains.Substring("does not match the required pattern"));
    }

    [Test]
    public void Validate_EmailPattern_ValidEmails_ReturnsTrue()
    {
        var rejigs = Rejigs.Create()
            .AtStart()
            .OneOrMore(r => r.AnyLetterOrDigit())
            .Text("@")
            .OneOrMore(r => r.AnyLetterOrDigit())
            .Text(".")
            .AnyInRange('a', 'z')
            .Between(2, 4)
            .AtEnd();

        Assert.That(rejigs.Validate("user@domain.com"), Is.True);
        Assert.That(rejigs.Validate("test123@example.org"), Is.True);
        Assert.That(rejigs.Validate("abc@xyz.co"), Is.True);
    }

    [Test]
    public void Validate_EmailPattern_InvalidEmails_ThrowsException()
    {
        var rejigs = Rejigs.Create()
            .AtStart()
            .OneOrMore(r => r.AnyLetterOrDigit())
            .Text("@")
            .OneOrMore(r => r.AnyLetterOrDigit())
            .Text(".")
            .AnyInRange('a', 'z')
            .Between(2, 4)
            .AtEnd();

        Assert.Throws<RejigsValidationException>(() => rejigs.Validate("invalid.email"));
        Assert.Throws<RejigsValidationException>(() => rejigs.Validate("@domain.com"));
        Assert.Throws<RejigsValidationException>(() => rejigs.Validate("user@.com"));
        Assert.Throws<RejigsValidationException>(() => rejigs.Validate("user@domain"));
    }

    [Test]
    public void Validate_PhonePattern_ValidPhones_ReturnsTrue()
    {
        var rejigs = Rejigs.Create()
            .AtStart()
            .Text("(")
            .AnyDigit()
            .Exactly(3)
            .Text(") ")
            .AnyDigit()
            .Exactly(3)
            .Text("-")
            .AnyDigit()
            .Exactly(4)
            .AtEnd();

        Assert.That(rejigs.Validate("(123) 456-7890"), Is.True);
        Assert.That(rejigs.Validate("(999) 888-7777"), Is.True);
    }

    [Test]
    public void Validate_PhonePattern_InvalidPhones_ThrowsException()
    {
        var rejigs = Rejigs.Create()
            .AtStart()
            .Text("(")
            .AnyDigit()
            .Exactly(3)
            .Text(") ")
            .AnyDigit()
            .Exactly(3)
            .Text("-")
            .AnyDigit()
            .Exactly(4)
            .AtEnd();

        Assert.Throws<RejigsValidationException>(() => rejigs.Validate("123-456-7890"));
        Assert.Throws<RejigsValidationException>(() => rejigs.Validate("(12) 456-7890"));
        Assert.Throws<RejigsValidationException>(() => rejigs.Validate("(123) 45-7890"));
        Assert.Throws<RejigsValidationException>(() => rejigs.Validate("(123) 456-789"));
    }

    [Test]
    public void Validate_OptionalPattern_ValidInputs_ReturnsTrue()
    {
        var rejigs = Rejigs.Create()
            .Text("http")
            .Optional(r => r.Text("s"))
            .Text("://")
            .OneOrMore(r => r.AnyLetterOrDigit());

        Assert.That(rejigs.Validate("http://example"), Is.True);
        Assert.That(rejigs.Validate("https://example"), Is.True);
    }

    [Test]
    public void Validate_GroupPattern_ValidInput_ReturnsTrue()
    {
        var rejigs = Rejigs.Create()
            .AtStart()
            .Text("cat")
            .OneOrMore(r => r.AnySpace())
            .Text("loves")
            .AtEnd();

        Assert.That(rejigs.Validate("cat loves"), Is.True);
        Assert.That(rejigs.Validate("cat  loves"), Is.True);
    }

    [Test]
    public void Validate_GroupPattern_InvalidInput_ThrowsException()
    {
        var rejigs = Rejigs.Create()
            .AtStart()
            .Text("cat")
            .OneOrMore(r => r.AnySpace())
            .Text("loves")
            .AtEnd();

        Assert.Throws<RejigsValidationException>(() => rejigs.Validate("bird loves"));
        Assert.Throws<RejigsValidationException>(() => rejigs.Validate("catloves"));
    }

    [Test]
    public void Validate_CaseInsensitivePattern_ValidInput_ReturnsTrue()
    {
        var rejigs = Rejigs.Create()
            .IgnoreCase()
            .Text("HELLO");

        Assert.That(rejigs.Validate("hello"), Is.True);
        Assert.That(rejigs.Validate("Hello"), Is.True);
        Assert.That(rejigs.Validate("HELLO"), Is.True);
        Assert.That(rejigs.Validate("HeLLo"), Is.True);
    }

    [Test]
    public void Validate_MultilinePattern_ValidInput_ReturnsTrue()
    {
        var rejigs = Rejigs.Create()
            .AtStart()
            .Text("Line1")
            .AtEnd();

        Assert.That(rejigs.Validate("Line1"), Is.True);
    }

    [Test]
    public void Validate_SpecialCharactersPattern_ValidInput_ReturnsTrue()
    {
        var rejigs = Rejigs.Create()
            .Text("Price: $")
            .OneOrMore(r => r.AnyDigit())
            .Text(".")
            .AnyDigit()
            .Exactly(2);

        Assert.That(rejigs.Validate("Price: $19.99"), Is.True);
        Assert.That(rejigs.Validate("Price: $5.00"), Is.True);
    }

    [Test]
    public void Validate_UnicodePattern_ValidInput_ReturnsTrue()
    {
        var rejigs = Rejigs.Create()
            .Text("Hello ")
            .OneOrMore(r => r.AnyCharacter());

        Assert.That(rejigs.Validate("Hello 世界"), Is.True);
        Assert.That(rejigs.Validate("Hello émoji 😀"), Is.True);
    }

    [Test]
    public void Validate_LongInput_ValidInput_ReturnsTrue()
    {
        var rejigs = Rejigs.Create()
            .OneOrMore(r => r.AnyInRange('a', 'z'));

        var longInput = new string('a', 10000);
        Assert.That(rejigs.Validate(longInput), Is.True);
    }

    [Test]
    public void Validate_LongInput_InvalidInput_ThrowsException()
    {
        var rejigs = Rejigs.Create()
            .OneOrMore(r => r.AnyDigit());

        var longInput = new string('a', 10000);
        var exception = Assert.Throws<RejigsValidationException>(() => rejigs.Validate(longInput));
        Assert.That(exception.Message, Contains.Substring("does not match the required pattern"));
    }

    // TryValidate Tests

    [TestCase(null)]
    [TestCase("")]
    public void TryValidate_NullOrEmptyInput_ReturnsFailureResult(string? input)
    {
        var rejigs = Rejigs.Create().Text("hello");

        var result = rejigs.TryValidate(input!);

        Assert.That(result.IsValid, Is.False);
        Assert.That(result.ErrorMessage, Is.EqualTo("Input cannot be null or empty"));
    }

    [Test]
    public void TryValidate_ValidInput_ReturnsSuccessResult()
    {
        var rejigs = Rejigs.Create().Text("hello");

        var result = rejigs.TryValidate("hello");

        Assert.That(result.IsValid, Is.True);
        Assert.That(result.ErrorMessage, Is.Null);
    }

    [Test]
    public void TryValidate_InvalidInput_ReturnsFailureResult()
    {
        var rejigs = Rejigs.Create().Text("hello");

        var result = rejigs.TryValidate("world");

        Assert.That(result.IsValid, Is.False);
        Assert.That(result.ErrorMessage, Is.EqualTo("Input 'world' does not match the required pattern"));
    }

    [Test]
    public void TryValidate_ComplexPattern_ValidInput_ReturnsSuccess()
    {
        var rejigs = Rejigs.Create()
                           .AtStart()
                           .OneOrMore(r => r.AnyLetterOrDigit())
                           .Text("@")
                           .OneOrMore(r => r.AnyLetterOrDigit())
                           .AtEnd();

        var result = rejigs.TryValidate("user@example");

        Assert.That(result.IsValid, Is.True);
        Assert.That(result.ErrorMessage, Is.Null);
    }

    [Test]
    public void TryValidate_ComplexPattern_InvalidInput_ReturnsFailure()
    {
        var rejigs = Rejigs.Create()
            .AtStart()
            .OneOrMore(r => r.AnyLetterOrDigit())
            .Text("@")
            .OneOrMore(r => r.AnyLetterOrDigit())
            .AtEnd();

        var result = rejigs.TryValidate("invalid-email");

        Assert.That(result.IsValid, Is.False);
        Assert.That(result.ErrorMessage, Does.Contain("does not match"));
    }

    [Test]
    public void TryValidate_DoesNotThrowException()
    {
        var rejigs = Rejigs.Create().Text("hello");

        Assert.DoesNotThrow(() => rejigs.TryValidate("world"));
        Assert.DoesNotThrow(() => rejigs.TryValidate(null!));
        Assert.DoesNotThrow(() => rejigs.TryValidate(""));
    }
}
