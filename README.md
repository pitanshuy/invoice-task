# invoice task

# Invoice Management API

This is a RESTful API for managing invoices. The API provides functionality for creating, retrieving, updating, and deleting invoices. It also includes user authentication and authorization through JWT tokens.

## Table of Contents

1. [Introduction](#introduction)
2. [Prerequisites](#prerequisites)
3. [API Endpoints](#api-endpoints)
4. [Code Implementation Details](#code-implementation-details)
5. [Authentication](#authentication)
6. [Database Structure](#database-structure)
7. [Running the API Locally](#running-the-api-locally)
8. [Testing the API](#testing-the-api)

---

## Introduction

This Invoice Management API allows users to interact with the **Invoices** data stored in a database. It includes features such as:

- CRUD operations for invoices
- Authentication using JWT
- Pagination support for invoice retrieval
- Role-based access control

---

## Prerequisites

Before you run the API, ensure you have the following installed:

- .NET 5 or above
- SQL Server or SQL Server Express
- Postman or any other tool to test API requests

---

## API Endpoints

The following are the primary API endpoints provided by the Invoice Management System:

### 1. **User Authentication**

#### POST /api/auth/login
- **Description**: Authenticates a user and returns a JWT token.
- **Request**:  
  Body (JSON):
  ```json
  {
    "username": "user123",
    "password": "password123"
  }
  ```
- **Response**:  
  200 OK  
  ```json
  {
    "Token": "your-jwt-token-here"
  }
  ```
- **Error**:  
  401 Unauthorized (if username or password is incorrect)

---

### 2. **Invoices**

#### GET /api/invoices
- **Description**: Retrieves a list of invoices with pagination.
- **Request**:  
  Query Parameters:  
  `page`: The page number (default: 1)  
  `pageSize`: Number of invoices per page (default: 10)  
- **Response**:  
  200 OK  
  ```json
  {
    "page": 1,
    "pageSize": 10,
    "totalCount": 100,
    "data": [
      {
        "id": 1,
        "customerName": "John Doe",
        "customerEmail": "johndoe@example.com",
        "amount": 150.00,
        "status": "Pending",
        "createdAt": "2025-01-09T15:45:44.950Z",
        "updatedAt": "2025-01-09T15:45:44.950Z"
      },
      ...
    ]
  }
  ```
- **Error**:  
  400 Bad Request (if `page` or `pageSize` are invalid)

#### POST /api/invoices
- **Description**: Creates a new invoice.
- **Request**:  
  Body (JSON):
  ```json
  {
    "customerName": "Jane Doe",
    "customerEmail": "janedoe@example.com",
    "amount": 200.00,
    "status": "Paid"
  }
  ```
- **Response**:  
  201 Created  
  ```json
  {
    "id": 3,
    "customerName": "Jane Doe",
    "customerEmail": "janedoe@example.com",
    "amount": 200.00,
    "status": "Paid",
    "createdAt": "2025-01-10T16:00:00.000Z",
    "updatedAt": "2025-01-10T16:00:00.000Z"
  }
  ```
- **Error**:  
  400 Bad Request (if data is invalid)

#### PUT /api/invoices/{id}
- **Description**: Updates an existing invoice by ID.
- **Request**:  
  Body (JSON):
  ```json
  {
    "customerName": "Jane Smith",
    "customerEmail": "janesmith@example.com",
    "amount": 220.00,
    "status": "Paid"
  }
  ```
- **Response**:  
  200 OK  
  ```json
  {
    "id": 3,
    "customerName": "Jane Smith",
    "customerEmail": "janesmith@example.com",
    "amount": 220.00,
    "status": "Paid",
    "createdAt": "2025-01-10T16:00:00.000Z",
    "updatedAt": "2025-01-10T17:00:00.000Z"
  }
  ```
- **Error**:  
  404 Not Found (if invoice with specified ID does not exist)

#### DELETE /api/invoices/{id}
- **Description**: Deletes an invoice by ID.
- **Response**:  
  204 No Content
- **Error**:  
  404 Not Found (if invoice with specified ID does not exist)

---

## Code Implementation Details

### 1. **User Authentication Logic**
   The user login endpoint verifies the username and password, and if correct, generates a JWT token for the user. 

```csharp
public IActionResult Login([FromBody] LoginRequestDto request)
{
    var user = _userRepository.GetUserByUsername(request.Username);
    if (user == null || !VerifyPassword(request.Password, user.PasswordHash))
        return Unauthorized("Invalid username or password.");

    var token = _authService.GenerateJwtToken(user.Username, user.Role);
    return Ok(new { Token = token });
}
```

### 2. **Invoice CRUD Operations**
   Invoice-related operations are performed using SQL queries and Entity Framework Core for CRUD functionality.

```csharp
public async Task<Invoice> CreateInvoice(Invoice invoice)
{
    _dbContext.Invoices.Add(invoice);
    await _dbContext.SaveChangesAsync();
    return invoice;
}
```

For GET operations, pagination is supported through `Skip` and `Take` methods.

```csharp
public async Task<PagedResult<Invoice>> GetInvoices(int page, int pageSize)
{
    var invoices = await _dbContext.Invoices
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

    var totalCount = await _dbContext.Invoices.CountAsync();

    return new PagedResult<Invoice>
    {
        Page = page,
        PageSize = pageSize,
        TotalCount = totalCount,
        Data = invoices
    };
}
```

---

## Authentication

To access protected endpoints, you need to pass a JWT token in the `Authorization` header of the request. Here's how to do it in Postman:

1. Send a `POST` request to `/api/auth/login` with your username and password.
2. Copy the token from the response.
3. For other protected API endpoints like `/api/invoices`, include the token in the `Authorization` header:

```
Authorization: Bearer <your-jwt-token-here>
```

---

## Database Structure

The primary table for this application is `Invoices`, which has the following columns:

| Column        | Type         | Description                  |
|---------------|--------------|------------------------------|
| Id            | INT          | Primary key, auto-incremented |
| CustomerName  | VARCHAR(255)  | Customer's name              |
| CustomerEmail | VARCHAR(255)  | Customer's email address     |
| Amount        | DECIMAL(18,2) | Invoice amount               |
| Status        | VARCHAR(50)   | Invoice status (e.g., Pending, Paid) |
| CreatedAt     | DATETIME      | Date and time of creation    |
| UpdatedAt     | DATETIME      | Date and time of last update |

---

## Running the API Locally

To run the API locally, follow these steps:

1. Clone this repository:
   ```bash
   git clone https://github.com/your-username/invoice-management-api.git
   ```

2. Navigate to the project folder:
   ```bash
   cd invoice-management-api
   ```

3. Restore the dependencies:
   ```bash
   dotnet restore
   ```

4. Set up the database connection string in `appsettings.json`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=YOUR_SERVER;Database=INVOICE_MANAGEMENT;TrustServerCertificate=True"
   }
   ```

5. Run the application:
   ```bash
   dotnet run
   ```

6. The API should now be running at `http://localhost:5000` or `https://localhost:5001`.

---

## Testing the API

To test the API, you can use **Postman** or **Swagger** (if Swagger is configured in the project). Swagger will be available at `http://localhost:5000/swagger` (or `https://localhost:5001/swagger`).

For Postman:

1. First, send a POST request to `/api/auth/login` with valid credentials to get a JWT token.
2. Use the token to authenticate requests to `/api/invoices`.

---

## Conclusion

This API provides basic functionalities for managing invoices with authentication and pagination. You can extend it further by adding more features like filtering invoices based on date or status, and implementing more advanced authorization.
