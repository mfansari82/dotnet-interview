# Solution Documentation

**Candidate Name:** Mohammedfaisal Ansari  
**Completion Date:** 22/05/2026

---

## Problems Identified

_Describe the issues you found in the original implementation. Consider aspects like:_
- Architecture and design patterns
- Code quality and maintainability
- Security vulnerabilities
- Performance concerns
- Testing gaps

[Your analysis here]

---

## Architectural Decisions

_Explain the architecture you chose and why. Consider:_
- Design patterns applied
- Project structure changes
- Technology choices
- Separation of concerns

[Your decisions here]

---

## Trade-offs

_Discuss compromises you made and the reasoning behind them. Consider:_
- What did you prioritize?
- What did you defer or simplify?
- What alternatives did you consider?

[Your trade-offs here]

---

## How to Run

### Prerequisites
[List required software, versions, etc.]

### Build
```bash
# Add your build commands
```

### Run
```bash
# Add your run commands
```

### Test
```bash
# Add your test commands
```

---

## API Documentation

### Endpoints

#### Create TODO
```
Method: [HTTP method]
URL: [endpoint]
Request Body: [example]
Response: [example]
```

#### Get TODO(s)
```
Method: [HTTP method]
URL: [endpoint]
Request: [example]
Response: [example]
```

#### Update TODO
```
Method: [HTTP method]
URL: [endpoint]
Request Body: [example]
Response: [example]
```

#### Delete TODO
```
Method: [HTTP method]
URL: [endpoint]
Request: [example]
Response: [example]
```

---

## Future Improvements

_What would you do if you had more time? Consider:_
- Additional features
- Performance optimizations
- Enhanced testing
- Better documentation
- Deployment considerations

[Your ideas here]



As we have lack of time so I am adding my and details here at the last of in documents,


Implemented features:
- Create Todo
- Get All Todos
- Get Todo By Id
- Update Todo
- Delete Todo

I used below things, it is lacking in current implementation: 
- ASP.NET Core Web API
- SQLite Database
- Dependency Injection
- Async/Await
- Logging
- DTO Request Models
- Unit Testing with xUnit and Moq

I also changed the project structure as well here to segregate things properly and used proper module like interface and request/Response models(DTOs)

# Project Structure

The project is divided into:
- Controllers
- Services
- Interfaces
- Models
- Request DTOs
- Unit Tests

## Controller Improvements

- Added Dependency Injection
- Used proper HTTP methods
- Added async methods
- Added proper response handling
- Used request DTOs instead of entity models

## Service Improvements

- Added async/await database operations
- Added logging
- Added proper update/delete checks

# Unit Testing

Unit tests were added for:
- Create Todo
- Get Todo
- Get All Todos
- Update Todo
- Delete Todo

Moq was used for:
- IConfiguration
- ILogger

SQLite test database was used for testing.


# What Can Be Improved Further

If more time is available, we can improve the project further by adding:

## Architecture Improvements
- Repository Pattern
- Unit Of Work Pattern
- CQRS
- Clean Architecture

## Security
- JWT Authentication
- Authorization

## Database Improvements
- Entity Framework Core
- Migrations

## Production Improvements
- Global Exception Middleware
- Caching



