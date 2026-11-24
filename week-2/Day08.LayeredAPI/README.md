# Day 08: Layered Architecture and Validation

## Overview
Implements a clean, layered architecture with proper separation of concerns following SOLID principles.

## Architecture Layers

```
┌─────────────────────────────────────────┐
│  API Layer (Controllers, Middleware)    │  ← HTTP concerns
├─────────────────────────────────────────┤
│  Application Layer (Services, DTOs)     │  ← Business logic
├─────────────────────────────────────────┤
│  Infrastructure Layer (Repositories)    │  ← Data access
├─────────────────────────────────────────┤
│  Domain Layer (Entities)                │  ← Core business models
└─────────────────────────────────────────┘
```

## Project Structure
- **API**: Controllers, Middleware, Program.cs
- **Application**: Services, DTOs, Interfaces, Exceptions, Validators
- **Infrastructure**: Repository implementations (In-memory for now, EF Core in Day 09)
- **Domain**: Entities with business logic

## Key Features
- Layered architecture with dependency inversion
- DTOs separate from Entities
- Global exception handling middleware
- Data validation with Data Annotations
- Business validation in services
- Consistent error responses

## Running the API
```bash
cd Day08.LayeredAPI/Day08.LayeredAPI.API
dotnet run
```
Access: https://localhost:7000

Build succeeded with warnings about package versions (non-breaking).
