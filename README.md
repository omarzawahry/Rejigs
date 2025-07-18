![NuGet](https://img.shields.io/nuget/v/Rejigs)
![Build Status](https://github.com/omarzawahry/Rejigs/actions/workflows/publish-nuget.yml/badge.svg?branch=main)

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

## 📖 Real-World Example

### Email Validation
```csharp
var emailRegex = Rejigs.Create()
                       .AtStart()
                       .OneOrMore(r => r.AnyLetterOrDigit().Or().AnyOf(".-_"))  // Local part
                       .Text("@")
                       .OneOrMore(r => r.AnyLetterOrDigit().Or().AnyOf(".-"))   // Domain
                       .Text(".")
                       .AnyInRange('a', 'z') // Top-level domain (2-6 letters)
                       .Between(2, 6)
                       .AtEnd()
                       .IgnoreCase() // Case-insensitive matching
                       .Build();

Console.WriteLine(emailRegex.IsMatch("user@example.com")); // True
Console.WriteLine(emailRegex.IsMatch("invalid-email")); // False
```

## 🎯 Tips and Best Practices

1. **Use descriptive variable names**: `var emailRegex = ...` instead of `var regex = ...`
2. **Break complex patterns into smaller parts**: Use variables to store intermediate builders
3. **Add comments**: Explain what each part of your regex does
4. **Test thoroughly**: Use unit tests to verify your patterns work correctly
5. **Use `AtStart()` and `AtEnd()`**: For exact matches, always anchor your patterns

## 📚 Documentation
For detailed documentation, examples, and API reference, visit the [Rejigs Documentation](https://github.com/omarzawahry/Rejigs/wiki).

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request.
