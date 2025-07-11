# Rejigs Library Documentation

## Overview

Rejigs is a fluent regex builder library for .NET that provides a human-readable way to construct regular expressions. Instead of writing complex regex patterns directly, you can use Rejigs' intuitive method chaining to build patterns step by step.

## Installation

```xml
<PackageReference Include="Rejigs" Version="1.0.1" />
```

## Basic Usage

```csharp
using Rejigs;

// Create a regex builder
var pattern = Rejigs.Create()
    .AtStart()
    .Text("Hello")
    .AnySpace()
    .AnyLetterOrDigit().OneOrMore()
    .AtEnd();

// Build the regex
var regex = pattern.Build();

// Or get the pattern string
string regexPattern = pattern.Expression; // "^Hello\s\w+"
```

## API Reference

### Factory Method

| Method | Regex Equivalent | Description |
|--------|------------------|-------------|
| `Rejigs.Create()` | N/A | Creates a new Rejigs instance |

### Text Matching

| Method | Regex Equivalent | Description | Example |
|--------|------------------|-------------|---------|
| `Text(string text)` | `Regex.Escape(text)` | Matches literal text exactly | `Text("hello")` → `hello` |
| `Pattern(string rawPattern)` | `rawPattern` | Appends raw regex pattern as-is | `Pattern(@"\d+")` → `\d+` |

### Anchors

| Method | Regex Equivalent | Description | Example |
|--------|------------------|-------------|---------|
| `AtStart()` | `^` | Matches start of string | `AtStart().Text("hello")` → `^hello` |
| `AtEnd()` | `$` | Matches end of string | `Text("hello").AtEnd()` → `hello$` |
| `AtWordBoundary()` | `\b` | Matches word boundary | `AtWordBoundary().Text("word")` → `\bword` |
| `NotAtWordBoundary()` | `\B` | Matches non-word boundary | `NotAtWordBoundary().Text("word")` → `\Bword` |

### Character Classes

| Method | Regex Equivalent | Description | Example |
|--------|------------------|-------------|---------|
| `AnyDigit()` | `\d` | Matches any digit (0-9) | `AnyDigit()` → `\d` |
| `AnyNonDigit()` | `\D` | Matches any non-digit | `AnyNonDigit()` → `\D` |
| `AnyLetterOrDigit()` | `\w` | Matches word characters (a-z, A-Z, 0-9, _) | `AnyLetterOrDigit()` → `\w` |
| `AnyNonLetterOrDigit()` | `\W` | Matches non-word characters | `AnyNonLetterOrDigit()` → `\W` |
| `AnySpace()` | `\s` | Matches whitespace characters | `AnySpace()` → `\s` |
| `AnyNonSpace()` | `\S` | Matches non-whitespace characters | `AnyNonSpace()` → `\S` |
| `AnyCharacter()` | `.` | Matches any character except newline | `AnyCharacter()` → `.` |
| `AnyOf(string characters)` | `[characters]` | Matches any of the specified characters | `AnyOf("abc")` → `[abc]` |
| `AnyInRange(char from, char to)` | `[from-to]` | Matches any character in range | `AnyInRange('a', 'z')` → `[a-z]` |
| `AnyExcept(string characters)` | `[^characters]` | Matches any character except specified | `AnyExcept("abc")` → `[^abc]` |

### Quantifiers

| Method | Regex Equivalent | Description | Example |
|--------|------------------|-------------|---------|
| `Exactly(int count)` | `{count}` | Matches exactly n times | `AnyDigit().Exactly(3)` → `\d{3}` |
| `AtLeast(int count)` | `{count,}` | Matches n or more times | `AnyDigit().AtLeast(2)` → `\d{2,}` |
| `Between(int min, int max)` | `{min,max}` | Matches between min and max times | `AnyDigit().Between(2, 4)` → `\d{2,4}` |
| `ZeroOrMore(Func<Rejigs, Rejigs> pattern)` | `(...)*` | Matches zero or more occurrences | `ZeroOrMore(r => r.AnyDigit())` → `(\d)*` |
| `OneOrMore(Func<Rejigs, Rejigs> pattern)` | `(...)+` | Matches one or more occurrences | `OneOrMore(r => r.AnyDigit())` → `(\d)+` |
| `Optional(string text)` | `text?` | Makes text optional | `Optional("s")` → `s?` |
| `Optional(Func<Rejigs, Rejigs> pattern)` | `(...)?` | Makes pattern optional | `Optional(r => r.AnyDigit())` → `(\d)?` |

### Grouping

| Method | Regex Equivalent | Description | Example |
|--------|------------------|-------------|---------|
| `Grouping(Func<Rejigs, Rejigs> pattern)` | `(?:...)` | Non-capturing group | `Grouping(r => r.Text("abc"))` → `(?:abc)` |

### Alternation

| Method | Regex Equivalent | Description | Example |
|--------|------------------|-------------|---------|
| `Or()` | `\|` | Alternation operator | `Text("cat").Or().Text("dog")` → `cat\|dog` |
| `Either(params Func<Rejigs, Rejigs>[] patterns)` | `(?:pattern1\|pattern2\|...)` | Matches any of the patterns | `Either(r => r.Text("cat"), r => r.Text("dog"))` → `(?:cat\|dog)` |

### Options

| Method | Regex Equivalent | Description | Example |
|--------|------------------|-------------|---------|
| `IgnoreCase()` | `RegexOptions.IgnoreCase` | Enables case-insensitive matching | `Text("hello").IgnoreCase()` → matches "HELLO", "Hello", etc. |
| `Compiled()` | `RegexOptions.Compiled` | Compiles regex for better performance | `AnyDigit().Compiled()` → compiled for faster execution |

### Building

| Method | Return Type | Description | Example |
|--------|-------------|-------------|---------|
| `Expression` | `string` | Gets the regex pattern as string | `pattern.Expression` |
| `Build()` | `Regex` | Builds Regex with current options | `pattern.Build()` |
| `Build(RegexOptions options)` | `Regex` | Builds Regex with custom options | `pattern.Build(RegexOptions.IgnoreCase)` |

## Performance and Options Guide

### When to Use Compiled()

The `Compiled()` method should be used when:
- The regex will be used many times (hundreds or thousands of matches)
- Performance is critical
- The application has sufficient memory for compiled regex cache

```csharp
// Good for repeated use
var compiledRegex = Rejigs.Create()
    .AtStart()
    .OneOrMore(r => r.AnyDigit())
    .Compiled()
    .Build();

// Use in a loop or repeatedly
for (int i = 0; i < 10000; i++)
{
    compiledRegex.IsMatch(input[i]);
}
```

### When to Use IgnoreCase()

Use `IgnoreCase()` when you need case-insensitive matching:

```csharp
// Email validation (case-insensitive)
var emailRegex = Rejigs.Create()
    .AtStart()
    .OneOrMore(r => r.AnyLetterOrDigit().Or().AnyOf(".-_"))
    .Text("@")
    .OneOrMore(r => r.AnyLetterOrDigit().Or().AnyOf(".-"))
    .Text(".")
    .AtLeast(2).AnyInRange('a', 'z')
    .AtEnd()
    .IgnoreCase()  // Allows mixed case emails
    .Build();
```

### Combining Options

You can chain multiple options together:

```csharp
var optimizedRegex = Rejigs.Create()
    .Text("important pattern")
    .IgnoreCase()  // Case-insensitive
    .Compiled()    // High performance
    .Build();
```

### Recommended Usage Patterns

**1. Simple one-time patterns:**
```csharp
var regex = Rejigs.Create().Text("pattern").Build();
```

**2. Case-insensitive matching:**
```csharp
var regex = Rejigs.Create()
    .Text("pattern")
    .IgnoreCase()
    .Build();
```

**3. High-performance scenarios:**
```csharp
var regex = Rejigs.Create()
    .ComplexPattern()
    .Compiled()
    .Build();
```

**4. Production-ready patterns:**
```csharp
var regex = Rejigs.Create()
    .ComplexPattern()
    .IgnoreCase()
    .Compiled()
    .Build();
```

**5. Custom options override:**
```csharp
// Builder options are ignored when using Build(options)
var regex = Rejigs.Create()
    .Text("pattern")
    .IgnoreCase()  // This is ignored
    .Build(RegexOptions.Multiline | RegexOptions.IgnoreCase);
```

## Complete Examples

### Email Validation

```csharp
var emailPattern = Rejigs.Create()
    .AtStart()
    .OneOrMore(r => r.AnyLetterOrDigit().Or().AnyOf(".-_"))
    .Text("@")
    .OneOrMore(r => r.AnyLetterOrDigit().Or().AnyOf(".-"))
    .Text(".")
    .AtLeast(2, r => r.AnyInRange('a', 'z'))
    .AtEnd();

var emailRegex = emailPattern.Build(RegexOptions.IgnoreCase);
```

### Phone Number (US Format)

```csharp
var phonePattern = Rejigs.Create()
    .AtStart()
    .Optional("(")
    .Exactly(3, r => r.AnyDigit())
    .Optional(")")
    .Optional(r => r.AnyOf(" -"))
    .Exactly(3, r => r.AnyDigit())
    .AnyOf(" -")
    .Exactly(4, r => r.AnyDigit())
    .AtEnd();
```

### URL Validation

```csharp
var urlPattern = Rejigs.Create()
    .AtStart()
    .Either(
        r => r.Text("http"),
        r => r.Text("https")
    )
    .Text("://")
    .OneOrMore(r => r.AnyLetterOrDigit().Or().AnyOf(".-"))
    .Optional(r => r.Text(":").OneOrMore(p => p.AnyDigit()))
    .ZeroOrMore(r => r.Text("/").ZeroOrMore(p => p.AnyExcept(" ")))
    .AtEnd();
```

### Date Matching (MM/DD/YYYY)

```csharp
var datePattern = Rejigs.Create()
    .AtStart()
    .Between(1, 2, r => r.AnyDigit())  // Month
    .Text("/")
    .Between(1, 2, r => r.AnyDigit())  // Day
    .Text("/")
    .Exactly(4, r => r.AnyDigit())     // Year
    .AtEnd();
```

### Password Validation

```csharp
// At least 8 characters, contains letter and digit
var passwordPattern = Rejigs.Create()
    .AtStart()
    .Grouping(r => r
        .ZeroOrMore(p => p.AnyCharacter())
        .AnyInRange('a', 'z')
        .ZeroOrMore(p => p.AnyCharacter())
    )
    .Grouping(r => r
        .ZeroOrMore(p => p.AnyCharacter())
        .AnyDigit()
        .ZeroOrMore(p => p.AnyCharacter())
    )
    .AtLeast(8, r => r.AnyCharacter())
    .AtEnd();
```

## Method Chaining

All Rejigs methods return the Rejigs instance, enabling fluent method chaining:

```csharp
var pattern = Rejigs.Create()
    .AtStart()
    .Text("Hello")
    .AnySpace()
    .AnyLetterOrDigit().OneOrMore()
    .Optional("!")
    .AtEnd();
```

## Error Handling

Rejigs includes validation for method parameters:

- `Exactly()`, `AtLeast()`, and `Between()` throw `ArgumentOutOfRangeException` for negative counts
- `Between()` throws `ArgumentException` when min > max
- `AnyInRange()` throws `ArgumentException` when from > to

## Regex Options

Use the `Build(RegexOptions)` method to specify regex options:

```csharp
var regex = pattern.Build(RegexOptions.IgnoreCase | RegexOptions.Multiline);
```

## Unsupported Regex Features

The following regex features are not directly supported by Rejigs methods but can be added using the `Pattern()` method:

- Lookahead/Lookbehind assertions (`(?=...)`, `(?!...)`, `(?<=...)`, `(?<!...)`)
- Named capture groups (`(?<name>...)`)
- Backreferences (`\1`, `\2`, etc.)
- Atomic groups (`(?>...)`)
- Possessive quantifiers (`*+`, `++`, etc.)
- Conditional expressions (`(?(condition)yes|no)`)
- Unicode categories (`\p{...}`, `\P{...}`)
- Lazy quantifiers (`*?`, `+?`, etc.)
- Inline modifiers (`(?i)`, `(?m)`, etc.)

To use these features, incorporate them with the `Pattern()` method:

```csharp
var pattern = Rejigs.Create()
    .Text("word")
    .Pattern(@"(?=\d)")  // Positive lookahead
    .AnyDigit();
```
