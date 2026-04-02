# HowlCore

A .NET Standard 2.1 class library providing common utilities, extension methods, and patterns for the Howl framework.

## Features

### Result Pattern
A robust operation result pattern for API responses and service operations:
- `Result` / `Result<T>` - Encapsulates operation status, message, error codes, and data
- `ResultBuilder` - Static factory methods for creating success/failure results

### Pagination Support
Built-in support for paginated queries:
- `Query<T>` - Pagination parameters (page, take, skip, sorting, includes)
- `Paged<T>` - Paginated result container with total count and items

### Extension Methods
Rich set of extension methods for common operations:

**Type Conversion**
- `To<T>()` - Safe type conversion with default value
- `Nullable<T>()` - Convert to nullable types
- `As<T>()` - Try cast to target type

**String Operations**
- `EqualsIgnoreCase()` / `EqualsFully()` - String comparison helpers
- `SplitString()` / `SplitTo<T>()` - String splitting with type conversion
- `ToQueryString()` / `AppendQueries()` - URL query string helpers
- `Coalesce()` - Return first non-empty string

**Collection Operations**
- `WhereIf()` - Conditional LINQ filtering
- `IsAny()` / `IsEmpty()` - Collection null-safe checks
- `StringJoin()` - Join collection elements to string
- `AsList()` - Convert to List efficiently

**Number Operations**
- `Between()` - Range checking for comparable values
- `Max()` / `Min()` - Value comparison
- `ToFixed()` / `ToFixedAsString()` / `ToFixedFormat()` - Decimal precision and formatting

**Object Mapping**
- `Map<T>()` - Auto object mapping using AutoMapper
- `Inherit<T1, T2>()` - Copy properties from source to target

**JSON Serialization**
- `TrySerialize<T>()` / `TryDeserialize<T>()` - Safe JSON operations with Newtonsoft.Json

### Factory Pattern
Simple factory abstractions:
- `IFactory<T>` - Factory interface
- `DelegateFactory<T>` - Delegate-based factory implementation

### Exceptions
Custom exception types:
- `PlatformException` - Generic platform exception with error code and data
- `HttpStatusCodeException` - HTTP status code based exception
- `BadRequestException` - 400 Bad Request exception

### Reflection Helpers
Efficient reflection utilities using expression trees:
- `Get()` / `Set()` - Dynamic property access
- `ToDictionary()` - Convert object to dictionary
- Expression tree parsing for member access paths

## Dependencies

| Package | Version | Purpose |
|---------|---------|---------|
| AutoMapper | 9.0.0 | Object mapping |
| Newtonsoft.Json | 13.0.4 | JSON serialization |
| System.ComponentModel.Annotations | 5.0.0 | Data annotations |

## Installation

Install via NuGet:

```bash
dotnet add package Howl.Core
```

## Usage Examples

### Result Pattern

```csharp
// Success result
var result = ResultBuilder.Succeed(data, "Operation completed");

// Failure results
var failResult = ResultBuilder.Fail("Invalid input");
var notFoundResult = ResultBuilder.NotFound("User");

// In async methods
public async Task<Result<User>> GetUser(int id)
{
    var user = await _repository.FindById(id);
    if (user == null)
        return ResultBuilder.NotFound("User");
    return user; // Implicit conversion to Result<User>
}
```

### Pagination

```csharp
public async Task<Paged<User>> GetUsers(Query<User> query)
{
    var total = await _context.Users.CountAsync();
    var items = await _context.Users
        .Skip(query.Skip ?? 0)
        .Take(query.Take ?? 10)
        .ToListAsync();
    return Paged<User>.Create(items, total);
}
```

### Extension Methods

```csharp
// Type conversion
var age = "25".To<int>(); // 25
var enabled = "true".To<bool>(); // true

// Conditional filtering
var filtered = users.WhereIf(u => u.Active, isActive);

// String operations
var query = parameters.ToQueryString();
var url = "https://api.example.com".AppendQueries(new { page = 1, size = 10 });

// Object mapping
var dto = entity.Map<UserDto>();
```

## License

MIT License - Copyright 2020 hflhhb

## Repository

[GitHub](https://github.com/hflhhb/HowlCore)
