namespace Rejigs;

public partial class Rejigs
{
    /// <summary>
    /// Matches a valid email address pattern.
    /// </summary>
    /// <returns>A new Rejigs instance for chaining.</returns>
    public Rejigs Email()
    {
        return AtStart()
              .OneOrMore(r => r.AnyLetterOrDigit().Or().AnyOf("._%+-"))
              .Text("@")
              .OneOrMore(r => r.AnyLetterOrDigit().Or().AnyOf(".-"))
              .Text(".")
              .AnyInRange('a', 'z').Between(2, 6)
              .AtEnd();
    }

    /// <summary>
    /// Matches a URL pattern (http/https).
    /// </summary>
    /// <returns>A new Rejigs instance for chaining.</returns>
    public Rejigs Url()
    {
        return AtStart()
              .Grouping(g => g.Text("http").Optional("s"))
              .Text("://")
              .OneOrMore(g => g.AnyLetterOrDigit().Or().AnyOf("-."))
              .Optional(g => g.Text(":").OneOrMore(gg => gg.AnyDigit()))
              .Optional(g => g.Text("/").ZeroOrMore(gg => gg.AnyLetterOrDigit().Or().AnyOf("/_.-")))
              .Optional(g => g.Text("?").ZeroOrMore(gg => gg.AnyLetterOrDigit().Or().AnyOf("&=%.-")))
              .Optional(g => g.Text("#").ZeroOrMore(gg => gg.AnyLetterOrDigit().Or().AnyOf(".-")))
              .AtEnd();
    }

    /// <summary>
    /// Matches an IPv4 address pattern.
    /// </summary>
    /// <returns>A new Rejigs instance for chaining.</returns>
    public Rejigs IPv4()
    {
        var octet = Fragment()
                   .Group(g => g.Group(g => g.Text("25").AnyInRange('0', '5'))
                                .Or()
                                .Group(g => g.Text("2").AnyInRange('0', '4').AnyDigit())
                                .Or()
                                .Group(g => g.Text("1").AnyDigit().AnyDigit().Optional())
                                .Or()
                                .Group(g => g.AnyDigit()));

        return AtStart()
              .Use(octet).Text(".")
              .Use(octet).Text(".")
              .Use(octet).Text(".")
              .Use(octet)
              .AtEnd();
    }

    /// <summary>
    /// Matches a US phone number pattern (various formats).
    /// </summary>
    /// <returns>A new Rejigs instance for chaining.</returns>
    public Rejigs PhoneNumber()
    {
        return AtStart()
               .Optional(g => g.Text("+1").AnyOf("-.").Or().AnySpace())
               .Optional(g => g.Text("("))
               .AnyDigit().Exactly(3)
               .Optional(g => g.Text(")"))
               .Optional(g => g.AnyOf("-.").Or().AnySpace())
               .AnyDigit().Exactly(3)
               .Optional(g => g.AnyOf("-.").Or().AnySpace())
               .AnyDigit().Exactly(4)
               .AtEnd();
    }

    /// <summary>
    /// Matches a credit card number pattern.
    /// </summary>
    /// <returns>A new Rejigs instance for chaining.</returns>
    public Rejigs CreditCard()
    {
        return AtStart()
               .AnyDigit().Exactly(4)
               .Group(g => g.AnyOf("-").Or().AnySpace()).Optional()
               .AnyDigit().Exactly(4)
               .Group(g => g.AnyOf("-").Or().AnySpace()).Optional()
               .AnyDigit().Exactly(4)
               .Group(g => g.AnyOf("-").Or().AnySpace()).Optional()
               .AnyDigit().Exactly(4)
               .AtEnd();
    }

    /// <summary>
    /// Matches a strong password pattern (at least 8 chars, uppercase, lowercase, digit, special char).
    /// </summary>
    /// <returns>A new Rejigs instance for chaining.</returns>
    public Rejigs StrongPassword()
    {
        return AtStart()
              .Pattern("(?=.*[a-z])")
              .Pattern("(?=.*[A-Z])")
              .Pattern("(?=.*\\d)")
              .Pattern("(?=.*[@$!%*?&])")
              .Group(g => g.AnyLetterOrDigit().Or().AnyOf("@$!%*?&")).AtLeast(8)
              .AtEnd();
    }

    /// <summary>
    /// Matches a hexadecimal color code (#RGB or #RRGGBB).
    /// </summary>
    /// <returns>The current Rejigs instance for chaining.</returns>
    public Rejigs HexColor()
    {
        return AtStart()
              .Text("#")
              .Group(g => g.AnyInRange('0', '9').Or().AnyInRange('a', 'f').Or().AnyInRange('A', 'F')).Between(3, 6)
              .AtEnd();
    }

    /// <summary>
    /// Matches a date in MM/DD/YYYY format.
    /// </summary>
    /// <returns>The current Rejigs instance for chaining.</returns>
    public Rejigs DateMMDDYYYY()
    {
        var month = Fragment()
                   .Group(m => 
                       m.Group(g => g.Optional(d => d.Text("0")).AnyInRange('1', '9'))
                        .Or()
                        .Group(g => g.Text("1").AnyInRange('0', '2')));
            
        var day = Fragment()
                 .Group(d => d.Group(g => g.Optional(d => d.Text("0")).AnyInRange('1', '9'))
                              .Or()
                              .Group(g => g.Text("1").AnyInRange('0', '9'))
                              .Or()
                              .Group(g => g.Text("2").AnyInRange('0', '9'))
                              .Or()
                              .Group(g => g.Text("3").AnyInRange('0', '1')));

        return AtStart()
              .Use(month).Text("/")
              .Use(day).Text("/")
              .Group(y => y.AnyDigit().Exactly(4))
              .AtEnd();
    }

    /// <summary>
    /// Matches a date in DD/MM/YYYY format.
    /// </summary>
    /// <returns>The current Rejigs instance for chaining.</returns>
    public Rejigs DateDDMMYYYY()
    {
        var day = Fragment()
            .Group(g => g.Text("0").AnyInRange('1', '9'))
            .Or()
            .Group(g => g.AnyInRange('1', '2').AnyDigit())
            .Or()
            .Group(g => g.Text("3").AnyInRange('0', '1'));
            
        var month = Fragment()
            .Group(g => g.Text("0").AnyInRange('1', '9'))
            .Or()
            .Group(g => g.Text("1").AnyInRange('0', '2'));

        return AtStart()
               .Use(day).Text("/")
               .Use(month).Text("/")
               .AnyDigit().Exactly(4)
               .AtEnd();
    }

    /// <summary>
    /// Matches a date in YYYY-MM-DD format (ISO 8601).
    /// </summary>
    /// <returns>The current Rejigs instance for chaining.</returns>
    public Rejigs DateISO()
    {
        var month = Fragment()
            .Group(g => g.Text("0").AnyInRange('1', '9'))
            .Or()
            .Group(g => g.Text("1").AnyInRange('0', '2'));
            
        var day = Fragment()
            .Group(g => g.Text("0").AnyInRange('1', '9'))
            .Or()
            .Group(g => g.AnyInRange('1', '2').AnyDigit())
            .Or()
            .Group(g => g.Text("3").AnyInRange('0', '1'));

        return AtStart()
               .AnyDigit().Exactly(4).Text("-")
               .Use(month).Text("-")
               .Use(day)
               .AtEnd();
    }

    /// <summary>
    /// Matches time in 24-hour format (HH:MM).
    /// </summary>
    /// <returns>The current Rejigs instance for chaining.</returns>
    public Rejigs Time24Hour()
    {
        var hour = Fragment()
                  .Group(g => g.Group(g => g.AnyOf("01").AnyDigit())
                  .Or()
                  .Group(g => g.Text("2").AnyInRange('0', '3')));

        return AtStart()
               .Use(hour).Text(":")
               .AnyInRange('0', '5').AnyDigit()
               .AtEnd();
    }

    /// <summary>
    /// Matches time in 12-hour format with AM/PM.
    /// </summary>
    /// <returns>The current Rejigs instance for chaining.</returns>
    public Rejigs Time12Hour()
    {
        var hour = Fragment()
                  .Group(h => h.Group(g => g.Text("0").AnyDigit())
                               .Or()
                               .Group(g => g.Text("1").AnyInRange('0', '2'))
                               .Or()
                               .Group(g => g.AnyDigit()));
        
        var minute = Fragment()
                    .Group(g => 
                            g.Group(m => m.AnyInRange('0', '5').AnyDigit()
                             .Or()
                             .Group(mm => mm.Text("0").AnyDigit())));

        return AtStart()
              .Use(hour)
              .Text(":")
              .Use(minute)
              .Group(s => s.AnySpace()).Optional()
              .Group(g => g.Text("AM").Or().Text("PM").Or().Text("am").Or().Text("pm"))
              .AtEnd();
    }

    /// <summary>
    /// Matches a US ZIP code (5 digits or 5+4 format).
    /// </summary>
    /// <returns>The current Rejigs instance for chaining.</returns>
    public Rejigs ZipCode()
    {
        return AtStart()
               .AnyDigit().Exactly(5)
               .Group(g => g.Text("-").AnyDigit().Exactly(4)).Optional()
               .AtEnd();
    }

    /// <summary>
    /// Matches a US Social Security Number (XXX-XX-XXXX format).
    /// </summary>
    /// <returns>The current Rejigs instance for chaining.</returns>
    public Rejigs SSN()
    {
        return AtStart()
               .AnyDigit().Exactly(3).Text("-")
               .AnyDigit().Exactly(2).Text("-")
               .AnyDigit().Exactly(4)
               .AtEnd();
    }

    /// <summary>
    /// Matches a UTC date pattern in ISO 8601 format with timezone.
    /// </summary>
    /// <returns>The current Rejigs instance for chaining.</returns>
    public Rejigs DateUTC()
    {
        var month = Fragment()
                   .Group(m => m.Group(g => g.Text("0").AnyInRange('1', '9'))
                                .Or()
                                .Group(g => g.Text("1").AnyInRange('0', '2')));
            
        var day = Fragment()
                 .Group(d => d.Group(g => g.Text("0").AnyInRange('1', '9'))
                              .Or()
                              .Group(g => g.AnyInRange('1', '2').AnyDigit())
                              .Or()
                              .Group(g => g.Text("3").AnyInRange('0', '1')));

        var hour = Fragment()
                  .Group(h => h.Group(g => g.AnyInRange('0', '1').AnyDigit())
                               .Or()
                               .Group(g => g.Text("2").AnyInRange('0', '3')));

        var timezone = Fragment()
                      .Group(t => t.Text("Z")
                                   .Or()
                                   .Group(g => g.AnyOf("+-")
                                       .Use(hour)
                                       .Text(":")
                                       .AnyInRange('0', '5').AnyDigit()));

        return AtStart()
              .AnyDigit().Exactly(4).Text("-")
              .Use(month).Text("-")
              .Use(day)
              .Text("T")
              .Use(hour).Text(":")
              .AnyInRange('0', '5').AnyDigit().Text(":")
              .AnyInRange('0', '5').AnyDigit()
              .Group(g => g.Text(".").AnyDigit().Between(1, 3)).Optional()
              .Use(timezone)
              .AtEnd();
    }

    /// <summary>
    /// Matches a MAC address pattern.
    /// </summary>
    /// <returns>The current Rejigs instance for chaining.</returns>
    public Rejigs MacAddress()
    {
        var hexPair = Fragment()
            .Group(g => g.AnyInRange('0', '9').Or().AnyInRange('A', 'F').Or().AnyInRange('a', 'f')).Exactly(2);
        
        return AtStart()
               .Use(hexPair).AnyOf(":-")
               .Use(hexPair).AnyOf(":-")
               .Use(hexPair).AnyOf(":-")
               .Use(hexPair).AnyOf(":-")
               .Use(hexPair).AnyOf(":-")
               .Use(hexPair)
               .AtEnd();
    }
}
