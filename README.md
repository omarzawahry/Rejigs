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
    .StartOfLine()
    .Text("cat")
    .Optional("s")
    .EndOfLine()
    .Build();

Console.WriteLine(regex.IsMatch("cats")); // True
