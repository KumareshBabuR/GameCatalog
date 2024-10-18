# Game Catalog API

## Overview
This is a RESTful API for managing a game catalog with CRUD operations 

## Setup Instructions
1. Clone the repository.
2. Navigate to the project directory.
3. Run `dotnet restore`.
4. Run `dotnet run`.

## API Endpoints
- **GET /api/GetGames**: List all games with optional pagination (`?pageNumber=1&pageSize=10`).
- **GET /api/GetGame/{id}**: Retrieve a game by ID.
- **POST /api/PostGame**: Create a new game.
- **PUT /api/PutGame/{id, Game}**: Update an existing game.
- **DELETE /api/DeleteGame/{id}**: Delete a game.

## Testing
Unit tests are available in the Tests directory.

## Technologies Used
- .NET Core
- Entity Framework Core
