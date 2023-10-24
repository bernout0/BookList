# Book Listing Project

Welcome to Book Listing Project! This repository contains the codebase for a RESTful API built using ASP.NET Core WebAPI. This README provides an overview of the project, how to set it up, and important information for developers.

## Table of Contents

- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Technologies](#technologies)
  - [Installation](#installation)
- [Project Structure](#project-structure)
- [Usage](#usage)
  - [Endpoints](#endpoints)
- [Relevant Notes](#relevant-notes)

## Getting Started

# In App Screenshots

![image](https://github.com/bernout0/BookList/assets/6838398/7736ec4b-01fa-4b2c-a505-bad1e9e4f227)

![image](https://github.com/bernout0/BookList/assets/6838398/2320fab1-e1ad-4227-89a4-3f29c4ab8e23)

![image](https://github.com/bernout0/BookList/assets/6838398/f11f6bcb-5ce9-4c64-849b-a08c430d0281)

![image](https://github.com/bernout0/BookList/assets/6838398/4d4ce598-4154-43ad-91c2-7cc17d156519)
![image](https://github.com/bernout0/BookList/assets/6838398/d2e8fcd4-58a1-4f1e-9c87-c09dd53282c7)

### Prerequisites

- [Visual Studio](https://visualstudio.microsoft.com/)

### Technologies

- ASP.NET Core WebAPI - Backend project
- ASP.Net Blazor - Client Solution
- Bootstrap 5 - CSS framework
- Mediatr - For mediatr
- FluentValidations - For the validations
- LocalDB
- NSwag - API Documentation

### Installation

1. Clone this repository to your local machine:

   ```bash
   git clone [https://github.com/bernout0/BookList](https://github.com/bernout0/BookList)](https://github.com/bernout0/BookList.git)https://github.com/bernout0/BookList.git
    ```

2. Restore dependencies and build the project:
     ```bash
    dotnet restore
    dotnet build
    dotnet run
    ```

3. Turn on Multiple Startup projects to have client and server both running

 ![image](https://github.com/bernout0/BookList/assets/6838398/98d3deb1-ba39-4ad8-aa1f-ab7666eb7d8f)


### Credentials

For the admin account (For reporting, usermanagement, Categories api and departments api)

username: admin@localhost

password: Password1!


Guest account: (Basic authentication)

username: bernie@localhost

password: Password1!


### Project Structure
![image](https://github.com/bernout0/api/assets/6838398/88ae0234-bf0a-4979-8b65-a03339768375)

 - BookListing.WebApi: Contains the ASP.NET Core Web API project.
 - BookListing.Client: Blazor application that consumes the webapi project
 - BookListing.Application: Holds application logic, including queries, commands, and business logic.
 - BookListing.Domain: Contains domain entities and business rules.
 - BookListing.Infrastructure: Deals with data persistence and data access.
 - BookListing.Tests: Unit and integration tests

### Usage

#### Endpoints

The API provides the following endpoints:
Auth Endpoints:
    POST /api/Auth/login: Authenticate and get token.
    POST /api/Auth/register: Register a new user.
    
Books Endpoints:

    GET /api/Books: List all books.
    POST /api/Books: Add a new book.
    GET /api/Books/{id}: Retrieve a specific book.
    PUT /api/Books/{id}: Update a specific book.
    DELETE /api/Books/{id}: Delete a specific book.
    
Categories Endpoints:

    GET /api/Categories: List all categories.
    POST /api/Categories: Add a new category.
    GET /api/Categories/{id}: Retrieve a specific category.
    PUT /api/Categories/{id}: Update a specific category.
    DELETE /api/Categories/{id}: Delete a specific category.
    
Departments Endpoints:

    GET /api/Departments: List all departments.
    POST /api/Departments: Add a new department.
    GET /api/Departments/{id}: Retrieve a specific department.
    PUT /api/Departments/{id}: Update a specific department.
    DELETE /api/Departments/{id}: Delete a specific department.
    
Reports Endpoints:

    GET /api/Reports/total-books: Total number of books report.
    GET /api/Reports/books-per-author: Books count per author report.
    GET /api/Reports/books-per-category: Books count per category report.
    GET /api/Reports/books-per-department: Books count per department report.
    
UserAccess Endpoints:

    POST /api/UserAccess: Grant user access (Department/ Category).
    DELETE /api/UserAccess/{id}: Revoke specific user access.
    
Users Endpoints:

    GET /api/Users: List all users.


## Relevant Notes

I wasn't able to create a UI for Category and Departments due to time constraint, but it can be accessible via API


### Design Considerations

I focused on breaking down the application into distinct and reusable modules or components. Each module should encapsulate a specific set of related functionalities. This modular approach ensures that different parts of your application can be developed, tested, and maintained independently. 




