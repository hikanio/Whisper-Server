![unit tests](https://img.shields.io/badge/unit_tests-passing-dark_green)
![integration tests](https://img.shields.io/badge/integration_tests-passing-dark_green)
# Whisper-Server

A secure and real-time messaging API with end-to-end encryption support.

> [!TIP]
> See also [Whisper-Desktop client](https://github.com/hikanio/Whisper-Desktop).

## Overview

Whisper-Server is a RESTful API built with ASP.NET Core that provides messaging services with a focus on security and privacy. The server manages user authentication, conversation management, and real-time message delivery between users.

## Features

- **Secure Authentication**: JWT-based authentication system
- **End-to-End Encryption Support**: Handles encrypted messages between users
- **Real-time Communication**: SignalR hub for instant message delivery
- **Message Status Tracking**: Support for message read status

## Technology Stack

- **Framework**: ASP.NET Core 9.0
- **Database**: PostgreSQL with Entity Framework Core
- **Authentication**: JWT Bearer tokens
- **Real-time Communication**: SignalR
- **API Documentation**: Scalar (OpenAPI)
- **Testing**: xUnit for unit and integration tests

## Getting Started

### Prerequisites

- .NET 9.0 SDK or later
- PostgreSQL server
- Docker (optional, for containerized database)

### Installation

1. Clone the repository.
   ```bash
   git clone https://github.com/hikanio/Whisper-Server.git
   cd Whisper-Server
   ```

2. Set up the database.

    Using Docker:
    ```bash
    cd src/WhisperServer.Api
    docker compose up -d
    ```
    Or configure your PostgreSQL connection string in `appsettings.json`

3. Run the application.
   ```bash
   cd src/WhisperServer.Api
   dotnet run --launch-profile https-dev
   ```

## API Endpoints
API Documentation: https://localhost:7161/scalar/v1

### Authentication
- `POST /users/signup` - Register a new user
- `POST /users/signin` - Authenticate and get a JWT token

### Messages
- `POST /messages` - Send a new message
- `GET /messages/{id}` - Get a specific message
- `PATCH /messages/{id}` - Update message status (sent/read)

### Conversations
- `GET /conversations` - Get all user conversations
- `GET /conversations/{id}/messages` - Get all messages in a conversation

## Real-time Features

The application uses SignalR to provide real-time communication:
- Message delivery notifications
- Typing indicators
- Online/offline status updates

## Security

- All API endpoints (except authentication) require valid JWT tokens
- Messages are encrypted on the client side
- Public key exchange for end-to-end encryption

## Testing

Run the tests using:

```bash
dotnet test
```

The solution includes:
- Unit tests for core business logic
- Integration tests for API endpoints

## Contributing
Contributions are welcome. Please feel free to submit a Pull Request.
