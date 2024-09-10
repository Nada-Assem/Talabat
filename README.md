
# Talabat Project

## Overview

Talabat is an online food delivery service that connects customers with a wide range of restaurants. This project aims to build a robust and scalable API using .NET Core to handle various functionalities such as product management and order processing. The project follows Onion Architecture and employs the Specification Design Pattern, AutoMapper for object-object mapping, and comprehensive error handling.

## Table of Contents

1. [Installation](#installation)
2. [Usage](#usage)
3. [API Endpoints](#api-endpoints)
4. [Architecture and Design Patterns](#architecture-and-design-patterns)
5. [Technologies Used](#technologies-used)
6. [Contributing](#contributing)
7. [License](#license)
8. [Contact](#contact)

## Installation

### Prerequisites

- [.NET Core](https://dotnet.microsoft.com/download) (version 8.0)
- [Git](https://git-scm.com/downloads) for version control

### Steps

1. Clone the repository:

   ```bash
   git clone https://github.com/your-username/talabat-api.git
   cd talabat-api
   ```

2. Run the application:

   ```bash
   dotnet run
   ```

### Database Setup

The database will be automatically created and seeded when you first run the application. This is handled in the `Program.cs` file with the following code:

```csharp
#region Create_Database
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
var _dbContext = services.GetRequiredService<StoreContext>();
var loggerFactory = services.GetRequiredService<ILoggerFactory>();

try
{
    await _dbContext.Database.MigrateAsync();
    await StoreContextSeed.SeedAsync(_dbContext);
}
catch (Exception ex)
{
    var log = loggerFactory.CreateLogger<Program>();
    log.LogError(ex, "An error occurred during migration");
}
#endregion
```

## Usage

### Running the API

To start the API server, navigate to the project directory and run:

```bash
dotnet run
```

The API will be accessible at `https://localhost:5001` or `http://localhost:5000`.

### Testing the API

Use tools like [Postman](https://www.postman.com/) or [Swagger](https://swagger.io/) to test the endpoints.

## API Endpoints

### Products

- **Get All Products**
  - `GET /api/products`
- **Get Product by ID**
  - `GET /api/products/{id}`
- **Add New Product**
  - `POST /api/products`
- **Update Product**
  - `PUT /api/products/{id}`
- **Delete Product**
  - `DELETE /api/products/{id}`

### Orders

- **Place New Order**
  - `POST /api/orders`
- **Get Order by ID**
  - `GET /api/orders/{id}`
- **Get Orders by User**
  - `GET /api/orders/user/{userId}`

_For a complete list of endpoints and their detailed usage, refer to the [API documentation](./API_DOCUMENTATION.md)._

## Architecture and Design Patterns

### Onion Architecture

This project follows the Onion Architecture, which emphasizes the separation of concerns, making the application more modular, maintainable, and testable. The main layers include:

- **Core**: Contains the domain entities and interfaces.
- **Application**: Contains the application services and business logic.
- **Infrastructure**: Contains the data access logic and external service integrations.
- **Presentation**: Contains the API controllers and presentation logic.

### Specification Design Pattern

The Specification Design Pattern is used to encapsulate the criteria for querying the database, making the data access logic more flexible and reusable.

### AutoMapper

AutoMapper is used to map between domain entities and Data Transfer Objects (DTOs), simplifying the data transformation process.

### Error Handling

Comprehensive error handling is implemented to ensure the API returns meaningful error messages and status codes, improving the developer and user experience.

## Technologies Used

- **.NET Core** - for building the API
- **Entity Framework Core** - for database interactions
- **SQL Server** - as the database
- **Swagger** - for API documentation
- **AutoMapper** - for object-object mapping
- **Specification Design Pattern** - for flexible querying
- **Onion Architecture** - for a modular and maintainable structure
- **JWT** - for authentication

## Contributing

We welcome contributions from the community. To contribute:

1. Fork the repository.
2. Create a new branch (`git checkout -b feature-branch`).
3. Make your changes.
4. Commit your changes (`git commit -m 'Add some feature'`).
5. Push to the branch (`git push origin feature-branch`).
6. Open a Pull Request.

## Contact

For any inquiries or issues, please contact:

- **Name**: Nada Assem
- **Email**: nada.assem81@gmail.com
