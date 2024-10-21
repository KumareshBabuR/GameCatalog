# Game Catalog API

## Overview
============
This is a RESTful API for managing a game catalog, supporting CRUD operations 

## Setup Instructions
1. Clone the repository.
2. Navigate to the project directory.
3. Run `dotnet restore`.
4. Run `dotnet run`.

## The Schema Information
=============================
The following is the table schema for Game entity

Datatype	Field name
===========     ==============
ID		int		
Title		nvarchar(MAX)	
Genre		nvarchar(MAX)	
Description	nvarchar(MAX)	
Price		float		
ReleaseDate	datetime2(7)	
StockQuantity	int		
		

## API Endpoints
=================

- **GET /api/GamesHub/GetGames**: List all games with optional pagination (`?pageNumber=1&pageSize=10`).
- **GET /api/GamesHub/GetGame/{id}**: Retrieve a game by ID.
- **POST /api/GamesHub/PostGame**: Create a new game.
- **PUT /api/GamesHub/PutGame/{id}**: Update an existing game.
- **DELETE /api/GamesHub/DeleteGame/{id}**: Delete a game.

## Usage guidelines
====================

Launch the application by using the URL http://localhost:5041/index.html

The page lists all the endpoints as mentioned above.

The user can select an endpoint and click on Try it out option and can view the results.

Sample URLs
============
GET Game Endpoint URL `		- http://localhost:5041/api/GamesHub/GetGames?pageNumber=1&pageSize=10
GET Game by ID Endpoint URL 	- http://localhost:5041/api/GamesHub/GetGame/1
POST Game Endpoint URL `	- http://localhost:5041/api/GamesHub/PostGame
PUT Game Endpoint URL `		- http://localhost:5041/api/GamesHub/PutGame/6
DELETE Game Endpoint URL `	- http://localhost:5041/api/GamesHub/DeleteGame/6

## Testing
Unit tests are available in the Tests directory.

## Technologies Used
- .NET Core
- Entity Framework Core
