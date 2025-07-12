# Rejigs

🧩 **A fluent, intuitive builder for regular expressions in C#**

Rejigs makes creating complex regular expressions simple and readable by providing a fluent API that builds patterns step by step. No more cryptic regex syntax - write patterns that are easy to understand and maintain!

## 🚀 Getting Started

### Installation

Install Rejigs via NuGet Package Manager:

```bash
dotnet add package Rejigs
```

Or via Package Manager Console:
```
Install-Package Rejigs
```

### Basic Usage

```csharp
using Rejigs;

// Simple text matching
var regex = Rejigs.Create()
                  .AtStart()
                  .Text("hello")
                  .AtEnd()
                  .Build();

Console.WriteLine(regex.IsMatch("hello")); // True
Console.WriteLine(regex.IsMatch("hello world")); // False
```

## 📖 Real-World Examples

### Email Validation
```csharp
var emailRegex = Rejigs.Create()
                       .AtStart()
                       .OneOrMore(r => r.AnyLetterOrDigit().Or().AnyOf(".-_"))  // Local part
                       .Text("@")
                       .OneOrMore(r => r.AnyLetterOrDigit().Or().AnyOf(".-"))   // Domain
                       .Text(".")
                       .Between(2, 6).AnyInRange('a', 'z') // Top-level domain (2-6 letters)
                       .AtEnd()
                       .IgnoreCase() // Case-insensitive matching
                       .Build();

Console.WriteLine(emailRegex.IsMatch("user@example.com")); // True
Console.WriteLine(emailRegex.IsMatch("invalid-email")); // False
```

### Phone Number Formatting
```csharp
var phoneRegex = Rejigs.Create()
                       .AtStart()
                       .Optional("(")
                       .Exactly(3).AnyDigit()  // Area code
                       .Optional(")")
                       .Optional(r => r.AnyOf(" -"))
                       .Exactly(3).AnyDigit()  // Exchange
                       .Optional(r => r.AnyOf(" -"))
                       .Exactly(4).AnyDigit()  // Number
                       .AtEnd()
                       .Build();

Console.WriteLine(phoneRegex.IsMatch("(555) 123-4567")); // True
Console.WriteLine(phoneRegex.IsMatch("555-123-4567")); // True
Console.WriteLine(phoneRegex.IsMatch("5551234567")); // True
```

### URL Validation
```csharp
var urlRegex = Rejigs.Create()
                     .AtStart()
                     .Either(
                         r => r.Text("http"),
                         r => r.Text("https")
                     )
                     .Text("://")
                     .OneOrMore(r => r.AnyLetterOrDigit().Or().AnyOf(".-"))  // Domain
                     .Optional(r => r.Text(":").OneOrMore(c => c.AnyDigit())) // Port
                     .ZeroOrMore(r => r.Text("/").ZeroOrMore(c => c.AnyExcept(" "))) // Path
                     .AtEnd()
                     .Build();

Console.WriteLine(urlRegex.IsMatch("https://example.com/path")); // True
Console.WriteLine(urlRegex.IsMatch("http://localhost:8080")); // True
```

### Credit Card Number
```csharp
var creditCardRegex = Rejigs.Create()
                            .AtStart()
                            .Exactly(4).AnyDigit()
                            .Optional(r => r.AnyOf(" -"))
                            .Exactly(4).AnyDigit()
                            .Optional(r => r.AnyOf(" -"))
                            .Exactly(4).AnyDigit()
                            .Optional(r => r.AnyOf(" -"))
                            .Exactly(4).AnyDigit()
                            .AtEnd()
                            .Build();

Console.WriteLine(creditCardRegex.IsMatch("1234 5678 9012 3456")); // True
Console.WriteLine(creditCardRegex.IsMatch("1234-5678-9012-3456")); // True
```

### Password Validation (Complex)
```csharp
// Password must be 8-20 characters, contain uppercase, lowercase, digit, and special char
var passwordRegex = Rejigs.Create()
                          .AtStart()
                          .Grouping(r => r.ZeroOrMore(c => c.AnyCharacter()).OneOrMore(d => d.AnyInRange('A', 'Z'))) // Has uppercase
                          .Grouping(r => r.ZeroOrMore(c => c.AnyCharacter()).OneOrMore(d => d.AnyInRange('a', 'z'))) // Has lowercase  
                          .Grouping(r => r.ZeroOrMore(c => c.AnyCharacter()).OneOrMore(d => d.AnyDigit())) // Has digit
                          .Grouping(r => r.ZeroOrMore(c => c.AnyCharacter()).OneOrMore(d => d.AnyOf("!@#$%^&*"))) // Has special
                          .Between(8, 20).AnyCharacter() // Length 8-20
                          .AtEnd()
                          .Build();
```

## 📚 Complete API Reference

### Creating a Builder
| Method | Description | Regex Equivalent |
|--------|-------------|------------------|
| `Rejigs.Create()` | Creates a new regex builder instance | - |

### Anchors
| Method | Description | Regex Equivalent |
|--------|-------------|------------------|
| `AtStart()` | Match at the beginning of string | `^` |
| `AtEnd()` | Match at the end of string | `$` |
| `AtWordBoundary()` | Match at word boundary | `\b` |
| `NotAtWordBoundary()` | Match at non-word boundary | `\B` |

### Character Classes
| Method | Description | Regex Equivalent |
|--------|-------------|------------------|
| `AnyDigit()` | Match any digit (0-9) | `\d` |
| `AnyNonDigit()` | Match any non-digit | `\D` |
| `AnyLetterOrDigit()` | Match any word character (letters, digits, underscore) | `\w` |
| `AnyNonLetterOrDigit()` | Match any non-word character | `\W` |
| `AnySpace()` | Match any whitespace character | `\s` |
| `AnyNonSpace()` | Match any non-whitespace character | `\S` |
| `AnyCharacter()` | Match any character except newline | `.` |
| `AnyOf(string chars)` | Match any character from the specified set | `[chars]` |
| `AnyInRange(char from, char to)` | Match any character in the specified range | `[from-to]` |
| `AnyExcept(string chars)` | Match any character except those specified | `[^chars]` |

### Text Matching
| Method | Description | Regex Equivalent |
|--------|-------------|------------------|
| `Text(string text)` | Match exact text (automatically escaped) | `text` (escaped) |
| `Pattern(string pattern)` | Insert raw regex pattern | `pattern` (as-is) |

### Quantifiers
| Method | Description | Regex Equivalent |
|--------|-------------|------------------|
| `Optional(string text)` | Match text zero or one time | `text?` |
| `Optional(Func<Rejigs, Rejigs> pattern)` | Match pattern zero or one time | `(pattern)?` |
| `ZeroOrMore(Func<Rejigs, Rejigs> pattern)` | Match pattern zero or more times | `(pattern)*` |
| `OneOrMore(Func<Rejigs, Rejigs> pattern)` | Match pattern one or more times | `(pattern)+` |
| `Exactly(int count)` | Match exactly n times | `{count}` |
| `AtLeast(int count)` | Match at least n times | `{count,}` |
| `Between(int min, int max)` | Match between min and max times | `{min,max}` |

### Grouping and Alternation
| Method | Description | Regex Equivalent |
|--------|-------------|------------------|
| `Grouping(Func<Rejigs, Rejigs> pattern)` | Create non-capturing group | `(?:pattern)` |
| `Or()` | Alternation operator | `\|` |
| `Either(params Func<Rejigs, Rejigs>[] patterns)` | Match any of the provided patterns | `(?:pattern1\|pattern2\|...)` |

### Options
| Method | Description | Regex Equivalent |
|--------|-------------|------------------|
| `IgnoreCase()` | Enable case-insensitive matching | `RegexOptions.IgnoreCase` |
| `Compiled()` | Compile regex for better performance | `RegexOptions.Compiled` |

### Building
| Method | Description | Returns |
|--------|-------------|---------|
| `Expression` | Get the regex pattern as string | `string` |
| `Build()` | Build a Regex object with current options | `Regex` |
| `Build(RegexOptions options)` | Build a Regex object with custom options | `Regex` |

## 🔧 Performance & Options

### Case-Insensitive Matching
Use `IgnoreCase()` for case-insensitive matching:

```csharp
var regex = Rejigs.Create()
                  .Text("hello")
                  .IgnoreCase()
                  .Build();

Console.WriteLine(regex.IsMatch("HELLO")); // True
Console.WriteLine(regex.IsMatch("Hello")); // True
```

### Performance Optimization
Use `Compiled()` for better performance when the regex will be used many times:

```csharp
var regex = Rejigs.Create()
                  .AtStart()
                  .OneOrMore(r => r.AnyDigit())
                  .Compiled()
                  .IgnoreCase()  // Can combine multiple options
                  .Build();
```

### Recommended Usage Patterns

**For simple, one-time use:**
```csharp
var regex = Rejigs.Create().Text("pattern").Build();
```

**For case-insensitive matching:**
```csharp
var regex = Rejigs.Create().Text("pattern").IgnoreCase().Build();
```

**For high-performance scenarios (repeated use):**
```csharp
var regex = Rejigs.Create().Pattern().Compiled().Build();
```

**For maximum flexibility:**
```csharp
var regex = Rejigs.Create()
                  .Pattern()
                  .IgnoreCase()
                  .Compiled()
                  .Build();
```

## 🎯 Tips and Best Practices

1. **Use descriptive variable names**: `var emailRegex = ...` instead of `var regex = ...`
2. **Break complex patterns into smaller parts**: Use variables to store intermediate builders
3. **Add comments**: Explain what each part of your regex does
4. **Test thoroughly**: Use unit tests to verify your patterns work correctly
5. **Use `AtStart()` and `AtEnd()`**: For exact matches, always anchor your patterns

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request.
