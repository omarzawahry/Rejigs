using NUnit.Framework;

namespace Rejigs.Tests;

public class CommonPatternsTests
{
    // Email tests
    [TestCase("user@example.com")]
    [TestCase("test.email@domain.org")]
    [TestCase("user+tag@example.co.uk")]
    [TestCase("firstname.lastname@company.travel")]
    [TestCase("user_name@sub-domain.example.museum")]
    [TestCase("test123@example.io")]
    [TestCase("a@b.co")]
    public void Email_MatchesValidEmailAddresses(string email)
    {
        var regex = Rejigs.Create().Email().IgnoreCase().Build();
        Assert.That(email, Does.Match(regex));
    }

    [TestCase("invalid.email")]
    [TestCase("@example.com")]
    [TestCase("user@")]
    [TestCase("user@domain")]
    [TestCase("user@domain.")]
    [TestCase("user@domain.c")]
    [TestCase("user name@example.com")]
    [TestCase("user@domain.toolongextension")]
    public void Email_RejectsInvalidEmailAddresses(string email)
    {
        var regex = Rejigs.Create().Email().IgnoreCase().Build();
        Assert.That(email, Does.Not.Match(regex));
    }

    // URL tests
    [TestCase("http://example.com")]
    [TestCase("https://www.example.com")]
    [TestCase("https://sub.domain.example.org")]
    [TestCase("http://example.com:8080")]
    [TestCase("https://example.com/path/to/resource")]
    [TestCase("https://example.com/search?q=test&lang=en")]
    [TestCase("https://example.com/page#section")]
    [TestCase("https://api.example.com/v1/users?id=123&format=json#results")]
    public void Url_MatchesValidUrls(string url)
    {
        var regex = Rejigs.Create().Url().IgnoreCase().Build();
        Assert.That(url, Does.Match(regex));
    }

    [TestCase("ftp://example.com")]
    [TestCase("example.com")]
    [TestCase("://example.com")]
    [TestCase("http://")]
    [TestCase("https://")]
    public void Url_RejectsInvalidUrls(string url)
    {
        var regex = Rejigs.Create().Url().IgnoreCase().Build();
        Assert.That(url, Does.Not.Match(regex));
    }

    // IPv4 tests
    [TestCase("192.168.1.1")]
    [TestCase("10.0.0.1")]
    [TestCase("172.16.0.1")]
    [TestCase("127.0.0.1")]
    [TestCase("255.255.255.255")]
    [TestCase("0.0.0.0")]
    [TestCase("8.8.8.8")]
    [TestCase("203.0.113.1")]
    public void IPv4_MatchesValidIPAddresses(string ip)
    {
        var regex = Rejigs.Create().IPv4().Build();
        Assert.That(ip, Does.Match(regex));
    }

    [TestCase("256.1.1.1")]
    [TestCase("192.168.1")]
    [TestCase("192.168.1.1.1")]
    [TestCase("192.168.1.256")]
    [TestCase("192.168.-1.1")]
    [TestCase("192.168.1.")]
    [TestCase(".192.168.1.1")]
    [TestCase("192.168.1.01")]
    public void IPv4_RejectsInvalidIPAddresses(string ip)
    {
        var regex = Rejigs.Create().IPv4().Build();
        Assert.That(ip, Does.Not.Match(regex));
    }

    // Phone Number tests
    [TestCase("5551234567")]
    [TestCase("555-123-4567")]
    [TestCase("555.123.4567")]
    [TestCase("555 123 4567")]
    [TestCase("(555) 123-4567")]
    [TestCase("(555)123-4567")]
    [TestCase("+1-555-123-4567")]
    [TestCase("+1.555.123.4567")]
    public void PhoneNumber_MatchesValidPhoneNumbers(string phone)
    {
        var regex = Rejigs.Create().PhoneNumber().Build();
        Assert.That(phone, Does.Match(regex));
    }

    [TestCase("555123456")]
    [TestCase("55512345678")]
    [TestCase("555-123-456")]
    [TestCase("555-123-45678")]
    [TestCase("abc-123-4567")]
    [TestCase("+2 555-123-4567")]
    public void PhoneNumber_RejectsInvalidPhoneNumbers(string phone)
    {
        var regex = Rejigs.Create().PhoneNumber().Build();
        Assert.That(phone, Does.Not.Match(regex));
    }

    // Credit Card tests
    [TestCase("1234567890123456")]
    [TestCase("1234 5678 9012 3456")]
    [TestCase("1234-5678-9012-3456")]
    [TestCase("1234567812345678")]
    [TestCase("1234 5678 1234 5678")]
    public void CreditCard_MatchesValidCreditCardNumbers(string card)
    {
        var regex = Rejigs.Create().CreditCard().Build();
        Assert.That(card, Does.Match(regex));
    }

    [TestCase("123456789012345")]
    [TestCase("12345678901234567")]
    [TestCase("1234-5678-9012-345")]
    [TestCase("1234.5678.9012.3456")]
    [TestCase("abcd-efgh-ijkl-mnop")]
    public void CreditCard_RejectsInvalidCreditCardNumbers(string card)
    {
        var regex = Rejigs.Create().CreditCard().Build();
        Assert.That(card, Does.Not.Match(regex));
    }

    // Strong Password tests
    [TestCase("Password123!")]
    [TestCase("MyStr0ng&P@ssw0rd")]
    [TestCase("Test123$")]
    [TestCase("Abcd1234!")]
    [TestCase("ComplexP@ss1")]
    [TestCase("Str0ng*Password!")]
    public void StrongPassword_MatchesValidStrongPasswords(string password)
    {
        var regex = Rejigs.Create().StrongPassword().Build();
        Assert.That(password, Does.Match(regex));
    }

    [TestCase("password")]
    [TestCase("PASSWORD")]
    [TestCase("12345678")]
    [TestCase("Password")]
    [TestCase("password123")]
    [TestCase("PASSWORD123")]
    [TestCase("Password!")]
    [TestCase("Pass1!")] // Too short
    [TestCase("NoDigits!")]
    [TestCase("nouppercasehere123!")]
    [TestCase("NOLOWERCASE123!")]
    [TestCase("NoSpecialChar123")]
    public void StrongPassword_RejectsWeakPasswords(string password)
    {
        var regex = Rejigs.Create().StrongPassword().Build();
        Assert.That(password, Does.Not.Match(regex));
    }

    // Hex Color tests
    [TestCase("#fff")]
    [TestCase("#000")]
    [TestCase("#123")]
    [TestCase("#ffffff")]
    [TestCase("#000000")]
    [TestCase("#123456")]
    [TestCase("#abcdef")]
    [TestCase("#ABCDEF")]
    [TestCase("#123ABC")]
    public void HexColor_MatchesValidHexColors(string color)
    {
        var regex = Rejigs.Create().HexColor().IgnoreCase().Build();
        Assert.That(color, Does.Match(regex));
    }

    [TestCase("fff")]
    [TestCase("#ff")]
    [TestCase("#fffffff")]
    [TestCase("#gggggg")]
    [TestCase("#123xyz")]
    public void HexColor_RejectsInvalidHexColors(string color)
    {
        var regex = Rejigs.Create().HexColor().IgnoreCase().Build();
        Assert.That(color, Does.Not.Match(regex));
    }

    // MM/DD/YYYY Date tests
    [TestCase("01/01/2024")]
    [TestCase("12/31/2024")]
    [TestCase("06/15/2023")]
    [TestCase("02/29/2024")] // Leap year
    [TestCase("11/30/1999")]
    public void DateMMDDYYYY_MatchesValidDates(string date)
    {
        var regex = Rejigs.Create().DateMMDDYYYY().Build();
        Assert.That(date, Does.Match(regex));
    }

    [TestCase("13/01/2024")]
    [TestCase("00/01/2024")]
    [TestCase("01/32/2024")]
    [TestCase("01/00/2024")]
    [TestCase("01/01/24")]
    [TestCase("2024/01/01")]
    public void DateMMDDYYYY_RejectsInvalidDates(string date)
    {
        var regex = Rejigs.Create().DateMMDDYYYY().Build();
        Assert.That(date, Does.Not.Match(regex));
    }

    // DD/MM/YYYY Date tests
    [TestCase("01/01/2024")]
    [TestCase("31/12/2024")]
    [TestCase("15/06/2023")]
    [TestCase("29/02/2024")] // Leap year
    [TestCase("30/11/1999")]
    public void DateDDMMYYYY_MatchesValidDates(string date)
    {
        var regex = Rejigs.Create().DateDDMMYYYY().Build();
        Assert.That(date, Does.Match(regex));
    }

    // ISO Date tests
    [TestCase("2024-01-01")]
    [TestCase("2024-12-31")]
    [TestCase("2023-06-15")]
    [TestCase("2024-02-29")] // Leap year
    [TestCase("1999-11-30")]
    public void DateISO_MatchesValidISODates(string date)
    {
        var regex = Rejigs.Create().DateISO().Build();
        Assert.That(date, Does.Match(regex));
    }

    // UTC Date tests
    [TestCase("2024-01-01T14:30:45Z")]
    [TestCase("2023-12-25T00:00:00Z")]
    [TestCase("2024-06-15T23:59:59Z")]
    [TestCase("2024-01-01T14:30:45.123Z")]
    [TestCase("2024-01-01T14:30:45.1Z")]
    [TestCase("2024-01-01T14:30:45.12Z")]
    [TestCase("2024-01-01T14:30:45+05:30")]
    [TestCase("2024-01-01T14:30:45-08:00")]
    [TestCase("2024-01-01T14:30:45.123+02:00")]
    public void DateUTC_MatchesValidUTCDates(string date)
    {
        var regex = Rejigs.Create().DateUTC().Build();
        Assert.That(date, Does.Match(regex));
    }

    [TestCase("2024-01-01 14:30:45Z")] // Space instead of T
    [TestCase("2024-01-01T14:30:45")] // Missing timezone
    [TestCase("2024-01-01T25:30:45Z")] // Invalid hour
    [TestCase("2024-01-01T14:60:45Z")] // Invalid minute
    [TestCase("2024-01-01T14:30:60Z")] // Invalid second
    [TestCase("2024-01-01T14:30:45.1234Z")] // Too many millisecond digits
    [TestCase("2024-01-01T14:30:45+25:00")] // Invalid timezone offset
    public void DateUTC_RejectsInvalidUTCDates(string date)
    {
        var regex = Rejigs.Create().DateUTC().Build();
        Assert.That(date, Does.Not.Match(regex));
    }

    // 24-Hour Time tests
    [TestCase("00:00")]
    [TestCase("12:30")]
    [TestCase("23:59")]
    [TestCase("01:15")]
    [TestCase("18:45")]
    public void Time24Hour_MatchesValidTimes(string time)
    {
        var regex = Rejigs.Create().Time24Hour().Build();
        Assert.That(time, Does.Match(regex));
    }

    [TestCase("24:00")]
    [TestCase("12:60")]
    [TestCase("1:30")]
    [TestCase("12:3")]
    [TestCase("12:30:45")]
    public void Time24Hour_RejectsInvalidTimes(string time)
    {
        var regex = Rejigs.Create().Time24Hour().Build();
        Assert.That(time, Does.Not.Match(regex));
    }

    // 12-Hour Time tests
    [TestCase("12:30AM")]
    [TestCase("12:30PM")]
    [TestCase("1:15am")]
    [TestCase("11:45pm")]
    [TestCase("12:00 AM")]
    [TestCase("6:30 PM")]
    public void Time12Hour_MatchesValidTimes(string time)
    {
        var regex = Rejigs.Create().Time12Hour().Build();
        Assert.That(time, Does.Match(regex));
    }

    [TestCase("13:30PM")]
    [TestCase("12:60AM")]
    [TestCase("12:30")] // Missing AM/PM
    [TestCase("12:30XM")]
    public void Time12Hour_RejectsInvalidTimes(string time)
    {
        var regex = Rejigs.Create().Time12Hour().Build();
        Assert.That(time, Does.Not.Match(regex));
    }

    // ZIP Code tests
    [TestCase("12345")]
    [TestCase("90210")]
    [TestCase("12345-6789")]
    [TestCase("00501")]
    [TestCase("99950-0077")]
    public void ZipCode_MatchesValidZipCodes(string zipCode)
    {
        var regex = Rejigs.Create().ZipCode().Build();
        Assert.That(zipCode, Does.Match(regex));
    }

    [TestCase("1234")]
    [TestCase("123456")]
    [TestCase("12345-678")]
    [TestCase("12345-67890")]
    [TestCase("abcde")]
    [TestCase("12345_6789")]
    public void ZipCode_RejectsInvalidZipCodes(string zipCode)
    {
        var regex = Rejigs.Create().ZipCode().Build();
        Assert.That(zipCode, Does.Not.Match(regex));
    }

    // SSN tests
    [TestCase("123-45-6789")]
    [TestCase("000-12-3456")]
    [TestCase("999-99-9999")]
    public void SSN_MatchesValidSSNs(string ssn)
    {
        var regex = Rejigs.Create().SSN().Build();
        Assert.That(ssn, Does.Match(regex));
    }

    [TestCase("123456789")]
    [TestCase("123-456-789")]
    [TestCase("12-45-6789")]
    [TestCase("123-4-6789")]
    [TestCase("123-45-678")]
    [TestCase("abc-de-fghi")]
    [TestCase("123_45_6789")]
    public void SSN_RejectsInvalidSSNs(string ssn)
    {
        var regex = Rejigs.Create().SSN().Build();
        Assert.That(ssn, Does.Not.Match(regex));
    }

    // MAC Address tests
    [TestCase("00:1B:44:11:3A:B7")]
    [TestCase("aa:bb:cc:dd:ee:ff")]
    [TestCase("AA:BB:CC:DD:EE:FF")]
    [TestCase("12-34-56-78-9A-BC")]
    [TestCase("00-00-00-00-00-00")]
    [TestCase("FF-FF-FF-FF-FF-FF")]
    public void MacAddress_MatchesValidMACAddresses(string macAddress)
    {
        var regex = Rejigs.Create().MacAddress().IgnoreCase().Build();
        Assert.That(macAddress, Does.Match(regex));
    }

    [TestCase("00:1B:44:11:3A")] // Too short
    [TestCase("00:1B:44:11:3A:B7:CC")] // Too long
    [TestCase("00_1B_44_11_3A_B7")] // Wrong separator
    [TestCase("GG:1B:44:11:3A:B7")] // Invalid hex char
    [TestCase("00:1B:44:11:3A:B")] // Incomplete last octet
    public void MacAddress_RejectsInvalidMACAddresses(string macAddress)
    {
        var regex = Rejigs.Create().MacAddress().IgnoreCase().Build();
        Assert.That(macAddress, Does.Not.Match(regex));
    }
}
