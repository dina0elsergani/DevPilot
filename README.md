# DevPilot - Enterprise .NET 9.0 Application

A comprehensive, enterprise-grade .NET 9.0 web application demonstrating advanced architectural patterns, modern development practices, and production-ready features.

## 🏗️ Architecture

### Clean Architecture with Advanced Patterns
- **Domain-Driven Design (DDD)**: Rich domain models with value objects, aggregates, and domain events
- **CQRS with MediatR**: Command/Query Responsibility Segregation with pipeline behaviors
- **Event Sourcing Foundation**: Domain events for audit trails and system integration
- **Repository & Unit of Work**: Abstracted data access with transaction management
- **Layered Architecture**: Clear separation of concerns across Api, Application, Domain, and Infrastructure

### Enterprise Features
- **Caching Layer**: Memory cache with intelligent cache invalidation
- **Background Jobs**: Hangfire integration for asynchronous processing
- **Rate Limiting**: API protection with configurable policies
- **Advanced Security**: JWT authentication, role-based authorization, and security headers
- **Observability**: Structured logging with Serilog, health checks, and metrics
- **API Versioning**: Future-proof API design with versioning support

## 🚀 Features

### Backend (.NET 9.0)
- **Clean Architecture**: Domain, Application, Infrastructure, and API layers
- **CQRS Pattern**: Separate commands and queries with MediatR
- **Domain Events**: Event-driven architecture for loose coupling
- **Entity Framework Core**: Code-first approach with migrations
- **AutoMapper**: Object-to-object mapping
- **FluentValidation**: Comprehensive input validation
- **JWT Authentication**: Secure token-based authentication
- **Swagger/OpenAPI**: Interactive API documentation
- **Health Checks**: System health monitoring
- **Background Jobs**: Hangfire for async task processing
- **Caching**: Memory cache with intelligent invalidation
- **Rate Limiting**: API protection against abuse
- **Structured Logging**: Serilog with file and console outputs
- **Global Exception Handling**: Centralized error management
- **Unit & Integration Tests**: Comprehensive test coverage

### Frontend (React + TypeScript)
- **Modern React**: Functional components with hooks
- **TypeScript**: Type-safe development
- **Vite**: Fast build tool and dev server
- **React Router**: Client-side routing
- **Context API**: State management
- **Responsive Design**: Mobile-first approach
- **Component Library**: Reusable UI components
- **Form Validation**: Client-side validation
- **Error Handling**: User-friendly error messages

## 🛠️ Technology Stack

### Backend
- **.NET 9.0**: Latest .NET framework
- **ASP.NET Core**: Web framework
- **Entity Framework Core**: ORM
- **MediatR**: CQRS implementation
- **Hangfire**: Background job processing
- **Serilog**: Structured logging
- **FluentValidation**: Validation framework
- **AutoMapper**: Object mapping
- **JWT Bearer**: Authentication
- **Swagger**: API documentation

### Frontend
- **React 18**: UI library
- **TypeScript**: Type safety
- **Vite**: Build tool
- **React Router**: Routing
- **Tailwind CSS**: Styling

### DevOps
- **Docker**: Containerization
- **Docker Compose**: Multi-container orchestration
- **GitHub Actions**: CI/CD pipeline
- **SQL Server**: Database

## 📁 Project Structure

```
DevPilot/
├── DevPilot.Api/                 # API Layer
│   ├── Controllers/             # API endpoints
│   ├── HealthChecks/            # Custom health checks
│   ├── Middleware/              # Custom middleware
│   ├── Properties/              # Launch settings
│   ├── logs/                    # Application logs
│   ├── appsettings.json         # Configuration
│   ├── appsettings.Development.json
│   ├── DevPilot.Api.csproj      # Project file
│   ├── DevPilot.Api.http        # HTTP requests
│   ├── Dockerfile               # Container configuration
│   └── Program.cs               # Application startup
├── DevPilot.Application/        # Application Layer
│   ├── Commands/               # CQRS commands
│   ├── Queries/                # CQRS queries
│   ├── Handlers/               # Command/Query handlers
│   ├── Services/               # Application services
│   ├── Interfaces/             # Application interfaces
│   ├── Behaviors/              # MediatR pipeline behaviors
│   ├── EventHandlers/          # Domain event handlers
│   ├── Events/                 # Application events
│   ├── Validators/             # FluentValidation validators
│   ├── DTOs/                   # Data transfer objects
│   ├── Mapping/                # AutoMapper profiles
│   ├── DevPilot.Application.csproj
│   ├── DependencyInjection.cs  # Service registration
│   └── Class1.cs               # Placeholder file
├── DevPilot.Domain/            # Domain Layer
│   ├── Entities/               # Domain entities
│   ├── ValueObjects/           # Value objects
│   ├── Events/                 # Domain events
│   ├── Services/               # Domain services
│   ├── IRepository.cs          # Repository interface
│   ├── IUnitOfWork.cs          # Unit of work interface
│   ├── TodoItem.cs             # Domain entities
│   ├── Project.cs
│   ├── Comment.cs
│   ├── RefreshToken.cs
│   ├── DevPilot.Domain.csproj
│   └── Class1.cs               # Placeholder file
├── DevPilot.Infrastructure/    # Infrastructure Layer
│   ├── Repositories/           # Repository implementations
│   ├── Migrations/             # EF Core migrations
│   ├── AppDbContext.cs         # Database context
│   ├── DataSeeder.cs           # Database seeding
│   ├── UnitOfWork.cs           # Unit of work implementation
│   ├── DevPilot.Infrastructure.csproj
│   ├── DependencyInjection.cs  # Infrastructure services
│   └── Class1.cs               # Placeholder file
├── DevPilot.UnitTests/         # Unit tests
├── DevPilot.IntegrationTests/  # Integration tests
├── webapp/                     # Frontend application
│   ├── client/                 # React application
│   │   ├── src/
│   │   │   ├── components/     # React components
│   │   │   ├── pages/          # Page components
│   │   │   ├── services/       # API services
│   │   │   ├── contexts/       # React contexts
│   │   │   ├── hooks/          # Custom hooks
│   │   │   ├── types/          # TypeScript types
│   │   │   ├── lib/            # Utility libraries
│   │   │   ├── App.tsx         # Main app component
│   │   │   ├── main.tsx        # Entry point
│   │   │   └── index.css       # Global styles
│   │   ├── index.html          # HTML entry point
│   │   └── package.json        # Dependencies
│   ├── shared/                 # Shared schemas
│   ├── package.json            # Root dependencies
│   ├── package-lock.json       # Lock file
│   ├── vite.config.ts          # Root Vite config
│   ├── tailwind.config.ts      # Root Tailwind config
│   ├── tsconfig.json           # Root TypeScript config
│   ├── postcss.config.js       # PostCSS configuration
│   ├── drizzle.config.ts       # Database schema
│   ├── components.json         # UI components config
│   ├── .gitignore              # Git ignore rules
│   ├── Dockerfile              # Frontend container
│   └── nginx.conf              # Nginx configuration
├── .github/                    # GitHub workflows
├── docker-compose.yml          # Docker orchestration
├── DevPilot.sln                # Solution file
├── package.json                # Root package.json
├── package-lock.json           # Lock file
├── LICENSE                     # MIT License
├── CONTRIBUTING.md             # Contributing guidelines
└── README.md                   # This file
```

## 🚀 Getting Started

### Prerequisites
- .NET 9.0 SDK
- Node.js 18+
- Docker & Docker Compose
- SQL Server (or use Docker)

### Quick Start

1. **Clone the repository**
   ```bash
   git clone https://github.com/dina0elsergani/DevPilot.git
   cd DevPilot
   ```

2. **Start with Docker Compose**
   ```bash
   docker-compose up -d
   ```

3. **Or run locally**
   ```bash
   # Backend
   dotnet run --project DevPilot.Api
   
   # Frontend
   cd webapp/client
   npm install
   npm run dev
   ```

4. **Access the application**
   - API: http://localhost:5024
   - Swagger: http://localhost:5024
   - Frontend: http://localhost:5173
   - Hangfire Dashboard: http://localhost:5024/hangfire
   - Health Checks: http://localhost:5024/health

### Database Setup
```bash
# Run migrations
dotnet ef database update --project DevPilot.Infrastructure --startup-project DevPilot.Api
```

### Database Seeding
The application automatically seeds the database with initial data on startup:
- **Test User**: `dshaban696@gmail.com` / `Dina13@`
- **Sample Project**: "Personal Tasks" project for the test user

The seeding is handled by `DevPilot.Infrastructure.DataSeeder` and runs automatically when the application starts.

## 🔧 Configuration

### Environment Variables
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=DevPilotDb;User Id=sa;Password=Your_password123;TrustServerCertificate=True;"
  },
  "Jwt": {
    "Key": "dev_secret_key_12345"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

## 🧪 Testing

### Run Tests
```bash
# All tests
dotnet test

# Specific test project
dotnet test DevPilot.UnitTests
dotnet test DevPilot.IntegrationTests
```

### Test Coverage
- Unit tests for services, handlers, and validators
- Integration tests for API endpoints
- Mock-based testing with Moq
- FluentAssertions for readable assertions

## 🚀 Deployment

### Docker Deployment
```bash
# Build and run with Docker Compose
docker-compose up --build

# Production build
docker build -t devpilot-api ./DevPilot.Api
docker build -t devpilot-frontend ./webapp/client
```

### CI/CD Pipeline
The project includes GitHub Actions workflows for:
- Automated testing
- Code quality checks
- Docker image building
- Deployment to staging/production

## 🔒 Security Features

- **JWT Authentication**: Secure token-based authentication
- **Role-Based Authorization**: Fine-grained access control
- **Rate Limiting**: API protection against abuse
- **Input Validation**: Comprehensive validation with FluentValidation
- **Global Exception Handling**: Secure error responses
- **CORS Configuration**: Cross-origin resource sharing setup

## 📊 Monitoring & Observability

- **Health Checks**: System health monitoring
- **Structured Logging**: Serilog with multiple sinks
- **Hangfire Dashboard**: Background job monitoring
- **API Metrics**: Request/response monitoring
- **Error Tracking**: Centralized error handling

## 🎯 Enterprise Features

### Advanced Caching
- Memory cache with intelligent invalidation
- Cache-aside pattern implementation
- Configurable cache expiration
- Cache key management

### Background Job Processing
- Hangfire integration for async tasks
- Email notifications
- Scheduled jobs with cron expressions
- Job retry and failure handling
- Real-time job monitoring

### Domain Events
- Event-driven architecture
- Loose coupling between components
- Audit trail capabilities
- Integration event support

### Rate Limiting
- Global rate limiting policies
- User-specific rate limiting
- Configurable limits and windows
- Rate limit headers

## 🤝 Contributing

We welcome contributions! Please see our [Contributing Guidelines](CONTRIBUTING.md) for details.

### Development Setup
1. Fork the repository
2. Create a feature branch: `git checkout -b feature/amazing-feature`
3. Commit your changes: `git commit -m 'Add amazing feature'`
4. Push to the branch: `git push origin feature/amazing-feature`
5. Open a Pull Request

### Code Style
- Follow C# coding conventions
- Use meaningful variable and method names
- Add XML documentation for public APIs
- Write unit tests for new features

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🙏 Acknowledgments

- Built with .NET 9.0 and React 18
- Uses Clean Architecture principles
- Inspired by modern enterprise development practices

---

**DevPilot** - Enterprise-grade .NET application demonstrating modern development practices and architectural excellence.

> **Note for Open Source**: The `Class1.cs` files in each project are placeholder files that should be removed before open source publication. They are typically created by Visual Studio when creating new projects.
