# Contributing to DevPilot

Thank you for your interest in contributing to DevPilot! This document provides guidelines for contributing to this project.

## 🚀 Getting Started

### Prerequisites
- .NET 9.0 SDK
- Node.js 18+
- Git
- SQL Server or Docker

### Development Setup
1. Fork the repository
2. Clone your fork: `git clone https://github.com/YOUR_USERNAME/DevPilot.git`
3. Add upstream: `git remote add upstream https://github.com/dina0elsergani/DevPilot.git`
4. Create a feature branch: `git checkout -b feature/your-feature-name`

## 📝 Code Style Guidelines

### C# (.NET)
- Follow [Microsoft C# Coding Conventions](https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions)
- Use meaningful variable and method names
- Add XML documentation for public APIs
- Keep methods under 20 lines when possible
- Use async/await for asynchronous operations

### TypeScript/React
- Follow [TypeScript Style Guide](https://github.com/microsoft/TypeScript/wiki/Coding-guidelines)
- Use functional components with hooks
- Prefer const over let when possible
- Use meaningful component and prop names
- Add JSDoc comments for complex functions

### General
- Write self-documenting code
- Keep functions small and focused
- Use consistent naming conventions
- Add comments for complex business logic

## 🧪 Testing

### Backend Tests
- Write unit tests for all business logic
- Write integration tests for API endpoints
- Aim for >80% code coverage
- Use descriptive test names

```csharp
[Fact]
public async Task CreateTodo_WithValidData_ShouldReturnCreatedTodo()
{
    // Arrange
    // Act
    // Assert
}
```

### Frontend Tests
- Write unit tests for components
- Test user interactions
- Mock API calls in tests
- Test error scenarios

## 🔄 Pull Request Process

1. **Update Documentation**: Update README.md if needed
2. **Add Tests**: Include tests for new functionality
3. **Run Tests**: Ensure all tests pass locally
4. **Check Style**: Follow code style guidelines
5. **Update Dependencies**: Update package versions if needed
6. **Create PR**: Use descriptive title and description

### PR Template
```markdown
## Description
Brief description of changes

## Type of Change
- [ ] Bug fix
- [ ] New feature
- [ ] Breaking change
- [ ] Documentation update

## Testing
- [ ] Unit tests pass
- [ ] Integration tests pass
- [ ] Manual testing completed

## Checklist
- [ ] Code follows style guidelines
- [ ] Self-review completed
- [ ] Documentation updated
- [ ] No breaking changes
```

## 🐛 Bug Reports

When reporting bugs, please include:
- Clear description of the issue
- Steps to reproduce
- Expected vs actual behavior
- Environment details (OS, .NET version, etc.)
- Screenshots if applicable

## 💡 Feature Requests

When requesting features, please include:
- Clear description of the feature
- Use cases and benefits
- Implementation suggestions if possible
- Mockups or examples if applicable

## 📞 Questions & Discussion

- Use GitHub Issues for questions
- Join discussions in existing issues
- Be respectful and constructive
- Help other contributors

## 🏷️ Issue Labels

- `bug`: Something isn't working
- `enhancement`: New feature or request
- `documentation`: Improvements to documentation
- `good first issue`: Good for newcomers
- `help wanted`: Extra attention is needed

## 📄 License

By contributing, you agree that your contributions will be licensed under the MIT License.

Thank you for contributing to DevPilot! 🚀 