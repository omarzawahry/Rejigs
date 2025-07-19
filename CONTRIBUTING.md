# Contributing to Rejigs

Thank you for your interest in contributing to Rejigs! 🎉 We welcome contributions from the community and are grateful for any help you can provide.

## 📋 Table of Contents

- [Code of Conduct](#-code-of-conduct)
- [Getting Started](#-getting-started)
- [Development Setup](#-development-setup)
- [Making Changes](#-making-changes)
- [Testing](#-testing)
- [Submitting Changes](#-submitting-changes)
- [Code Style Guidelines](#-code-style-guidelines)
- [Reporting Issues](#-reporting-issues)
- [Feature Requests](#-feature-requests)

## 🤝 Code of Conduct

By participating in this project, you agree to abide by our Code of Conduct. Please be respectful, inclusive, and constructive in all interactions.

## 🚀 Getting Started

### Prerequisites

- [.NET SDK 6.0 or later](https://dotnet.microsoft.com/download) (the project targets .NET 6, 7, 8, and 9)
- A code editor (Visual Studio, VS Code, JetBrains Rider, etc.)
- Git

### Fork and Clone

1. Fork the repository on GitHub
2. Clone your fork locally:
   ```bash
   git clone https://github.com/YOUR_USERNAME/Rejigs.git
   cd Rejigs
   ```
3. Add the upstream repository:
   ```bash
   git remote add upstream https://github.com/omarzawahry/Rejigs.git
   ```

## 🔧 Development Setup

1. **Restore dependencies:**

   ```bash
   dotnet restore
   ```

2. **Build the project:**

   ```bash
   dotnet build
   ```

3. **Run tests:**

   ```bash
   dotnet test
   ```

4. **Verify everything works:**
   ```bash
   dotnet run --project Rejigs.Tests
   ```

## 🛠️ Making Changes

### Branch Strategy

- Create a new branch for each feature or bug fix:

  ```bash
  git checkout -b feature/your-feature-name
  # or
  git checkout -b fix/issue-description
  ```

- Use descriptive branch names:
  - `feature/add-lookahead-support`
  - `fix/quantifier-validation-bug`
  - `docs/improve-readme-examples`

### Development Workflow

1. **Keep your fork updated:**

   ```bash
   git fetch upstream
   git checkout main
   git merge upstream/main
   ```

2. **Create your feature branch:**

   ```bash
   git checkout -b feature/your-feature
   ```

3. **Make your changes** following the [Code Style Guidelines](#code-style-guidelines)

4. **Write tests** for your changes (see [Testing](#testing))

5. **Commit your changes:**
   ```bash
   git add .
   git commit -m "Add feature: your feature description"
   ```

## 🧪 Testing

### Running Tests

- **Run all tests:**

  ```bash
  dotnet test
  ```

- **Run tests with coverage:**

  ```bash
  dotnet test --collect:"XPlat Code Coverage"
  ```

- **Run tests with detailed coverage report:**

  ```bash
  dotnet test --collect:"XPlat Code Coverage" --results-directory ./TestResults
  ```

- **Generate HTML coverage report (requires reportgenerator):**

  ```bash
  dotnet test --collect:"XPlat Code Coverage"
  dotnet tool install -g dotnet-reportgenerator-globaltool
  reportgenerator -reports:"Rejigs.Tests/TestResults/*/coverage.cobertura.xml" -targetdir:"Rejigs.Tests/TestResults/html" -reporttypes:Html
  ```

- **Run specific test class:**
  ```bash
  dotnet test --filter "ClassName=LiteralTests"
  ```

### Writing Tests

- All new features **must** include comprehensive tests
- Tests are written using **NUnit**
- Place tests in the appropriate test file in `Rejigs.Tests/`
- Follow the existing test naming convention: `MethodName_Scenario_ExpectedResult`

**Example test:**

```csharp
[Test]
public void Text_WithSpecialCharacters_EscapesCorrectly()
{
    var regex = Rejigs.Create().Text("a.b*c+").Build();

    Assert.That("a.b*c+", Does.Match(regex));
    Assert.That("abc", Does.Not.Match(regex));
}
```

### Test Categories

- **Unit Tests**: Test individual methods and components
- **Integration Tests**: Test complete regex patterns
- **Edge Cases**: Test boundary conditions and error scenarios

## 📤 Submitting Changes

### Pull Request Process

1. **Push your branch:**

   ```bash
   git push origin feature/your-feature
   ```

2. **Create a Pull Request** on GitHub with:

   - Clear title describing the change
   - Detailed description of what was changed and why
   - Reference any related issues (`Fixes #123`)
   - Screenshots or examples if applicable

3. **PR Checklist:**
   - [ ] Code follows the style guidelines
   - [ ] Self-review completed
   - [ ] Tests added/updated and all pass
   - [ ] Documentation updated if needed
   - [ ] No breaking changes (or properly documented)

### PR Title Format

Use conventional commit format:

- `feat: add support for lookahead assertions`
- `fix: resolve quantifier validation issue`
- `docs: update README with new examples`
- `test: add tests for character classes`
- `refactor: improve pattern building performance`

## 📏 Code Style Guidelines

### General Guidelines

- **Write XML documentation** for public APIs
- **Keep methods focused** - single responsibility principle

### Specific Patterns

1. **Fluent Interface:**

   ```csharp
   public Rejigs MethodName(parameters)
   {
       // Implementation
       return this;
   }
   ```

2. **Method Documentation:**
   ```csharp
   /// <summary>
   /// Matches one or more of the specified characters.
   /// </summary>
   /// <param name="characters">The characters to match</param>
   /// <returns>The current Rejigs instance for method chaining</returns>
   public Rejigs OneOrMore(string characters)
   ```

### File Organization

- Place related functionality in separate files (e.g., `Quantifiers.cs`, `Anchors.cs`)
- Use partial classes to organize the main `Rejigs` class
- Keep validation logic in the `Validation/` folder

## 🐛 Reporting Issues

### Before Reporting

1. **Search existing issues** to avoid duplicates
2. **Test with the latest version**
3. **Provide a minimal reproduction case**

### Issue Template

````markdown
**Describe the bug**
A clear description of the issue.

**To Reproduce**

```csharp
var regex = Rejigs.Create()
                  .YourCode()
                  .Build();
```
````

**Expected behavior**
What you expected to happen.

**Actual behavior**
What actually happened.

**Environment**

- Rejigs version: [e.g., 1.1.0]
- .NET version: [e.g., .NET 8.0]
- OS: [e.g., Windows 11, macOS 14, Ubuntu 22.04]

````

## 💡 Feature Requests

We welcome feature requests! Please:

1. **Check existing issues** for similar requests
2. **Describe the use case** - why is this feature needed?
3. **Provide examples** of how the API should look
4. **Consider backwards compatibility**

### Feature Request Template

```markdown
**Feature Description**
A clear description of the proposed feature.

**Use Case**
Why is this feature needed? What problem does it solve?

**Proposed API**
```csharp
// Example of how the feature would be used
var regex = Rejigs.Create()
                  .NewFeature()
                  .Build();
````

**Alternatives Considered**
What alternatives have you considered?

```

## 🎯 Areas for Contribution

We especially welcome contributions in these areas:

- **New regex features** (lookahead/lookbehind, backreferences, etc.)
- **Performance improvements**
- **Better error messages and validation**
- **Documentation and examples**
- **Additional test cases**
- **Cross-platform testing**

## 📞 Getting Help

- **GitHub Discussions**: For questions and general discussion
- **Issues**: For bugs and feature requests
- **Pull Requests**: For code contributions

## 🙏 Recognition

Contributors will be recognized in:
- Release notes
- Contributors section (if we add one)
- Special thanks for significant contributions

---

Thank you for contributing to Rejigs! Your efforts help make regex more accessible and enjoyable for C# developers everywhere. 🚀
```
