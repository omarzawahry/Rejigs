# Rejigs

🧩 A LINQ-style fluent builder for regular expressions in C#.

## Features

- Build complex regex with readable method chains
- Non-capturing and capturing group support
- Literal and raw pattern input
- Optional, repeated, and ranged patterns
- Word boundaries, character sets, anchors, and more

## Example

```csharp
var regex = Rejigs.Create()
                  .AtStart()
                  .Text("cat")
                  .Optional("s")
                  .AtEnd()
                  .Build();

Console.WriteLine(regex.IsMatch("cats")); // True


var emailRegex = Rejigs.Create()
                       .AtStart()
                       .OneOrMore(r => r.Word().Or().CharSet(".-_"))      // local part
                       .Text("@")
                       .OneOrMore(r => r.Word())                          // domain
                       .Text(".")
                       .OneOrMore(r => r.Word())                          // TLD
                       .AtEnd()
                       .Build();

Console.WriteLine(emailRegex.IsMatch("user.name-1@example-domain.com")); // True
Console.WriteLine(emailRegex.IsMatch("not-an-email@")); // False

