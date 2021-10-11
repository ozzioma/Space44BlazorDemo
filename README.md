# Space44 Student CRUD Blazor Demo
Blazor server app to demonstrate CRUD functionality for a domain model.

This solution is built using CLEAN architecture.

# Solution overview
This solution enables CRUD functionality for a Student data model.
The UI is a Blazor Server app talking to a REST API.

# Stack Details
  - UI: Blazor Server/ASP.NET Core 5
  - Data Access: EF Core 5
  - Database: Embedded SQLite
  - API: ASP.Net Core 5.0 REST API with OData endpoints for easy search and filtering
  - Middleware: Mediatr for CQRS implementation, Automapper for DTO mapping, FluentValidation for entity/DTO validation
  - Security: JWT bearer token authentication using ASP.Net Identity and EF Core
  

# Modules
There are a total of 5 modules, listed below
- Common
  > This module contains common utilities, classes and domain objects

- Persistence
  > This module contains the Persistence model/entities. The data access layer is built using EF Core 5.0 and SQLite.
  
- Application
  - This module contains business logic for the application. This is split into two use cases
      - Student CRUD handlers: Logic for creating, updating and deleting student records along with DTO validations.
      - Authentication handlers: Logic for registering new users and handling user logins along with DTO validations
  
  
- API
  - This module contains REST API endpoints exposing CRUD functionality.
  - The port is set to 7070 and the Swagger UI can be accessed when started here: http://localhost:7070/swagger/index.html
  - An OData endpoint for filtering/sorting Student records is exposed at /odata/$metadata
  - Authentication and Authorization is implemented using JWT tokens.
  - The embedded SQLite database is located within this project and is named sample3.db, this database is used for all the modules.
  
  
- BlazorServerApp
  - This module contains Web UI for managing Student records.
  - The port is set to 7072 and the Swagger UI can be accessed when started here: http://localhost:7072
  - Two UI libraries are used namely AntDesign Blazor and Syncfusion Blazor Components.
  - The web app authenticates agains the API to retrieve access tokens which are subsequently used to access the endpoints.
  - All data access happens on the API side and the API and Web UI can be hosted and scaled separately
  
