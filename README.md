![NuGet](https://img.shields.io/nuget/v/Rejigs)
[![NuGet](https://img.shields.io/nuget/dt/rejigs.svg)](https://www.nuget.org/packages/rejigs)
![Build Status](https://github.com/omarzawahry/Rejigs/actions/workflows/publish-nuget.yml/badge.svg?branch=main)

# Rejigs

🧩 **A fluent, intuitive, and thread-safe builder for regular expressions in C#**

Rejigs makes creating complex regular expressions simple and readable by providing a fluent API that builds patterns step by step. Built with immutability at its core, Rejigs is inherently thread-safe and follows functional programming principles. No more cryptic regex syntax - write patterns that are easy to understand, maintain, and safely use across multiple threads!

## ✨ Key Features

- **🔧 Fluent API**: Build complex regex patterns with readable, chainable methods
- **🛡️ Thread-Safe**: Immutable design ensures safe usage across multiple threads
- **🔄 Immutable**: Each operation returns a new instance, preventing side effects
- **⚡ Performance**: No locking overhead thanks to immutable architecture
- **🧪 Well-Tested**: 369+ comprehensive tests ensuring reliability

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

## 🛡️ Thread Safety & Immutability

Rejigs is built with **immutability** at its core, making it inherently **thread-safe** without any performance overhead from locking mechanisms.

### How It Works

Every method call returns a **new instance** rather than modifying the existing one:

```csharp
var basePattern = Rejigs.Create().Text("hello");
var pattern1 = basePattern.Text(" world");  // New instance
var pattern2 = basePattern.Text(" there");  // Another new instance

// basePattern remains unchanged: "hello"
// pattern1 contains: "hello world" 
// pattern2 contains: "hello there"
```

### Safe Concurrent Usage

You can safely share Rejigs builders across multiple threads:

```csharp
// Safe to use in multiple threads simultaneously
var sharedBuilder = Rejigs.Create()
                          .AtStart()
                          .OneOrMore(r => r.AnyDigit());

// Thread 1
var phonePattern = sharedBuilder.Text("-").Exactly(3, r => r.AnyDigit()).Build();

// Thread 2 (concurrent with Thread 1)
var idPattern = sharedBuilder.AtEnd().Build();

// No race conditions or shared state issues!
```

### Performance Benefits

- **No locking overhead**: Unlike mutable builders, no synchronization primitives needed
- **Predictable performance**: Operations are always O(1) for state creation
- **Memory efficient**: Only creates new instances when needed
- **Garbage collector friendly**: Short-lived intermediate objects

## 🎯 Tips and Best Practices

1. **Use descriptive variable names**: `var emailRegex = ...` instead of `var regex = ...`
2. **Break complex patterns into smaller parts**: Use variables to store intermediate builders
3. **Add comments**: Explain what each part of your regex does
4. **Test thoroughly**: Use unit tests to verify your patterns work correctly
5. **Use `AtStart()` and `AtEnd()`**: For exact matches, always anchor your patterns
6. **Leverage immutability**: Store base patterns and derive variations safely
7. **Thread-safe by design**: Feel free to share builders across threads without locks
8. **Cache compiled regexes**: Store the final `Build()` result for repeated use
9. **Functional composition**: Chain operations naturally without side effects

### Multi-Threading Example

```csharp
// Create a base pattern (safe to share)
var baseValidation = Rejigs.Create().AtStart().OneOrMore(r => r.AnyLetterOrDigit());

// Multiple threads can safely extend the pattern
Task.Run(() => {
    var emailPattern = baseValidation.Text("@").OneOrMore(r => r.AnyLetterOrDigit()).Build();
    // Use emailPattern...
});

Task.Run(() => {
    var usernamePattern = baseValidation.AtEnd().Build();
    // Use usernamePattern...
});
// No synchronization needed!
```

## 📚 Documentation

For detailed documentation, examples, and API reference, visit the [Rejigs Documentation](https://github.com/omarzawahry/Rejigs/wiki).

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🤝 Contributing

Contributions are welcome! Please feel free to raise an issue or a PR.
