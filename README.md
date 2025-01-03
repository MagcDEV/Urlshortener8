# URL Shortener Service

This project is a simple URL shortener service built with C#. It provides an API to shorten long URLs and retrieve the original URLs using a short code. The project uses Docker to containerize the API and its database.

## Features

- Shorten long URLs
- Retrieve original URLs using a short code
- Containerized using Docker

## Technologies Used

- C#
- ASP.NET Core
- Entity Framework Core
- SQL Server
- Docker

## Getting Started

### Prerequisites

- Docker
- Docker Compose

### Running the Application
#### Note: Before running the application with docker-compose up, you need to run the database migrations.

1. Clone the repository:
   ```sh
   git clone https://github.com/yourusername/urlshortener.git
   cd urlshortener
   ```
   
2. Start the database container:
   ```sh
   docker-compose up urlshortener-db -d
   ```
   
3. Run the migrations:  
   ```sh
   cd Urlshortener.Api
   dotnet ef database update
   ```
   
4. Set the flag "UseLocalConnection" in appsetting.json to false:  
   ```json
   "UseLocalConnection": false
   ```
   
5. Now you can start the application:  
   ```sh
   cd ..
   docker-compose up --build -d
   ```
   
5. The API will be available at `http://localhost:5001`.

### API Endpoints

- `POST /api/shorten`: Shorten a long URL
- `GET /api/{code}`: Retrieve the original URL using the short code

## Docker Configuration

The project uses Docker Compose to manage the API and database containers. The configuration is defined in the `compose.yaml` file.

### Services

- `urlshortener-db`: SQL Server database container
- `urlshortener.api`: API container

### Volumes

- `./.containers/database:/var/opt/mssql/data`: Maps the database data to a local directory

### Ports

- `1400:1433`: Maps the SQL Server port
- `5001:8080`: Maps the API port

### Environment Variables

- `ACCEPT_EULA`: Accepts the SQL Server EULA
- `SA_PASSWORD`: Sets the SQL Server admin password
