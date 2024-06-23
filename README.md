# Middleware REST API

## Content
- [Introduction](#introduction)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
- [Running the Application](#running-the-application)
- [Configuration Details](#configuration-details)
  - [Database Configuration](#database-configuration)
  - [Authentication and Authorization](#authentication-and-authorization)
- [How to test the Application](#how-to-test-the-application)
  - [Testing the Endpoints](#testing-the-endpoints)
- [Application Details](#application-details)
  - [Communication inside of the application](#communication-inside-of-the-application)
  - [Caching](#caching)
  - [Unit and Integration tests](#unit-and-integration-tests)
  - [Logging](#logging)
  - [Authentication](#authentication)
  - [Authorization](#authorization)
  - [Exception handling](#exception-handling)
  - [Documentation with Swagger](#documentation-with-swagger)
  - [API Testing with Postman](#api-testing-with-postman)

## Introduction
This project demonstrates various features such as implementing endpoints for retrieving data about products from an external API or a local database, caching, unit and integration tests, logging, authentication, authorization and exception handling. Swagger is used for describing and documenting RESTful APIs. Postman is used as a user interface for sending requests and testing the application.

## Getting Started

### Prerequisites
- .NET 8.0 SDK
- Visual Studio 2022
- ASP.NET Web API
- Entity Framework

## Running the Application
1. Clone the master branch from GitHub and open the solution in Visual Studio.
2. Press F5 to build and run the application.

## Configuration Details
### Database Configuration
The application uses a local database configured through Entity Framework Core migrations. On startup, it checks for the existance of the "ProductDb" database on the "(localdb)\MSSQLLocalDB" server. If the database doesn't exist, the database is automatically created and seeded with initial data.

### Authentication and Authorization
Authentication and authorization are enabled. You can test the application using Postman with the Postman workspace links provided in the "How to test the Application" section.

## How to test the Application
1. Press F5 to build and run the application.
2. When the application is ready to use, you can start testing it with Postman: https://www.postman.com .

### Testing the Endpoints
#### 1. First, to access the sensitive data, we need to generate a JWT token.
  - Here's the prepared Postman request for this action: [click here](https://www.postman.com/material-participant-99475696/workspace/my-workspace/request/36477190-8927a860-1a0d-4628-b7fa-eb83ab0724d7?action=share&creator=36477190&ctx=documentation)
    - This request checks if the username "emilys" with password "emilyspass" exists in the https://dummyjson.com/users API. If the user exists, it generates a JWT token that can be used to access sensitive data.

#### 2. Now we can use the generated JWT token to access the sensitive data.
#### External API data
  - This endpoint returns a list of all products from the external API or Cache if the JWT token is correct: [click here](https://www.postman.com/material-participant-99475696/workspace/my-workspace/request/36477190-f8466e7c-0bd8-4d7b-86bc-d523f8b67c62?action=share&creator=36477190&ctx=documentation)
    - Here we can use the JWT token we generated in the first step. Position yourself on the "Authorization" tab which is directly under the URL of the request and paste your JWT token inside. (If there is not a space where you can paste the JWT token, make sure the Auth Type is set to "Bearer Token").
- Make sure the correct JWT token is being used in every other endpoint in the "Authorization" tab aswell.
  - This endpoint returns a product filtered by ID from the external API or Cache if the JWT token is correct: [click here](https://www.postman.com/material-participant-99475696/workspace/my-workspace/request/36477190-a8a683b0-483b-484a-b4e9-02560610d92d?action=share&creator=36477190&ctx=documentation&tab=auth)
 
  - This endpoint returns a list of products filtered by category and price range from the external API or Cache if the JWT token is correct: [click here](https://www.postman.com/material-participant-99475696/workspace/my-workspace/request/36477190-3e98f3cc-127b-40f5-983b-23fe82b11f34?action=share&creator=36477190&ctx=documentation)
 
  - This endpoint returns a list of products filtered by category from the external API or Cache if the JWT token is correct: [click here](https://www.postman.com/material-participant-99475696/workspace/my-workspace/request/36477190-5910cb22-697e-4c48-886f-fb0a79656d07?action=share&creator=36477190&ctx=documentation)
  - This endpoint returns a list of products filtered by price range from the external API or Cache if the JWT token is correct: [click here](https://www.postman.com/material-participant-99475696/workspace/my-workspace/request/36477190-5910cb22-697e-4c48-886f-fb0a79656d07?action=share&creator=36477190&ctx=documentation)
  - This endpoint returns a list of products filtered by search from the external API or Cache if the JWT token is correct: [click here](https://www.postman.com/material-participant-99475696/workspace/my-workspace/request/36477190-f8d3e363-d28b-401f-96f0-67d4fa910272?action=share&creator=36477190&ctx=documentation)

#### Local Database data
  - This endpoint returns a list of all products from the local database or Cache if the JWT token is correct: [click here](https://www.postman.com/material-participant-99475696/workspace/my-workspace/request/36477190-979c6967-bdeb-4635-8af8-9171ed67e986?action=share&creator=36477190&ctx=documentation)
    - Here we can again use the JWT token we generated in the first step. Position yourself on the "Authorization" tab which is directly under the URL of the request and paste your JWT token inside. (If there is not a space where you can paste the JWT token, make sure the Auth Type is set to "Bearer Token").
- Make sure the correct JWT token is being used in every other endpoint in the "Authorization" tab aswell.
  - This endpoint returns a product filtered by ID from the local database or Cache if the JWT token is correct: [click here](https://www.postman.com/material-participant-99475696/workspace/my-workspace/request/36477190-608fb300-59e5-4a1e-92a4-5416667e9612?action=share&creator=36477190&ctx=documentation)
  - This endpoint returns a list of products filtered by category and price range from the local database or Cache if the JWT token is correct: [click here](https://www.postman.com/material-participant-99475696/workspace/my-workspace/request/36477190-d5fd2412-65bc-4254-93e9-fe3b58b1ad5d?action=share&creator=36477190&ctx=documentation)
  - This endpoint returns a list of products filtered by category from the local database or Cache if the JWT token is correct: [click here](https://www.postman.com/material-participant-99475696/workspace/my-workspace/request/36477190-b5e37fd5-a84c-4d3b-b2d4-2740eb41ce93?action=share&creator=36477190&ctx=documentation)
  - This endpoint returns a list of products filtered by price range from the local database or Cache if the JWT token is correct: [click here](https://www.postman.com/material-participant-99475696/workspace/my-workspace/request/36477190-c2a8271a-89b2-4079-85a1-3a01784e5849?action=share&creator=36477190&ctx=documentation)
  - This endpoint returns a list of products filtered by search from the local database or Cache if the JWT token is correct: [click here](https://www.postman.com/material-participant-99475696/workspace/my-workspace/request/36477190-6c7ad224-de59-46b9-aa4f-0d49fa412cf4?action=share&creator=36477190&ctx=documentation)
 

## Application Details
### Communication inside of the application
- Each layer focuses on a specific aspect of the application, enhancing maintainability and testability.
#### Controller Layer
  - Receives incoming HTTP requests from clients.
  - Handles routing and request validation.
  - Directly interacts with the client parsing request parameters.

#### Service Layer
  - Contains the business logic and rules that define how data should be processed or manipulated.
  - Called by controllers to perform operations that require business logic, such as data validation, data transformation, or coordination of repository calls.

#### Repository Layer
  - Abstracts and encapsulates the data access logic, interacting directly with the external API or local database.
  - Performs CRUD operations on data entities.
  - Responsible for querying data from the data storage.

### Caching
  - Improves application performance and responsiveness by storing frequently accessed data in memory.
  - Implemented inside of the Service layer.
  - If the Cache is "**missed**" the application fetches data from the external API or the local database.
  - If the Cache is "**hit**" the application fectes data from the Cache.
  - Reduces the number of external API or database calls by serving data from the Cache.

### Unit and Integration tests
- The unit tests for this project were implemented using the NUnit framework.
  
#### Unit tests
  - Verify the correctness of individual components or units of code.
  - The components are isolated using mocks for dependencies.
#### Integration tests
  - Test the interaction between multiple components or systems to ensure they work together correctly.
  - Validating end-to-end scenarios that involve multiple layers of the application.
  - Uses real or mock databases and external services to test the complete workflow.

### Logging
  - Captures and records information, warnings and errors within the application.
  - Provides insights into the application's runtime behavior for monitoring and debugging purposes.
  - Categorizes logs by severity (info, warn, error).

### Authentication
  - Verifies the identity of users or systems accessing the application. List of users that can access the application: https://dummyjson.com/users .
  - Requires users to provide credentials (username and password) to access protected resources.
  - Issues JWT tokens upon successful authentication for subsequent requests.
  - Ensures that only authenticated users can access the application and protects sensitive data from unauthorized access.

### Authorization
  - Determines the access rights and permissions of authenticated users.
  - ASP.NET Core provides built-in support for validating JWT tokens through its authentication middleware. The functionality is provided by the **Microsoft.AspNetCore.Authentication.JwtBearer package**.
  - If the token provided by the user is correct, the user is granted permission to the protected resources of the application.

### Exception handling
  - Catches exceptions at various layers of the application.
  - Provides a meaningful error message and status code to clients.
  - Logs exceptions for further analysis.
  - Prevents the application from crashing due to unhandled exceptions.

### Documentation with Swagger
  - Describes and documents RESTful APIs.
  - Generates interactive API documentation based on the application's code and annotations.

### API Testing with Postman
  - Provides a user interface for sending HTTP requests and testing the application.
  - This is our primary way of testing the application which allows us to test the Authentication and Authorization aswell.
