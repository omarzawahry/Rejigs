# Rejigs

🧩 A fluent builder for regular expressions in C#.


## Example

```csharp
var regex = Rejigs.Create()
                  .AtStart()
                  .Text("cat")
                  .Optional("s")
                  .AtEnd()
                  .Build();

Console.WriteLine(regex.IsMatch("cats")); // True
Console.WriteLine(regex.IsMatch("cat")); // True


var emailRegex = Rejigs.Create()
                       .AtStart()
                       .OneOrMore(r => r.Word().Or().CharSet(".-_"))      // local part
                       .Text("@")
                       .OneOrMore(r => r.Word())                          // domain
                       .Text(".")
                       .OneOrMore(r => r.Word())                          // top level domain
                       .AtEnd()
                       .Build();

Console.WriteLine(emailRegex.IsMatch("user.name-1@example-domain.com")); // True
Console.WriteLine(emailRegex.IsMatch("not-an-email@")); // False

