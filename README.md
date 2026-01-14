# Shipment Manager – Console Application

This project is a console application that manages shipments, cargo, and pirate ships. The goal was to create a well-layered and maintainable application using .NET 8 and Entity Framework Core.

## 🎯 Features

- Managing pirate ships, shipments, and cargo records
- Adding, updating, and deleting shipments (CRUD)
- Automatic delay handling (e.g., if a ship hasn’t departed on time)
- Generating reports in XML format
- LINQ-based queries for report generation
- Unit tests using NUnit to verify business logic

## 🧱 Architecture

The project is organized into multiple layers:

- `Domain`: business logic and entities (e.g., PirateShip, Shipment, Cargo)
- `Infrastructure`: database access and EF Core context (`AppDbContext`)
- `Application`: services, interfaces, and application logic
- `Presentation`: console user interface (CLI)
- `Tests`: unit tests using NUnit framework

## 💡 Challenges

- Designing a proper layered architecture to keep components testable and extensible
- Configuring and using EF Core in a console environment
- Implementing XML reading and writing
- Dynamic report generation (including Reflection usage)
- Writing unit tests for business logic

## 🔧 Technologies

- .NET 8.0
- C#
- Entity Framework Core
- NUnit
- XML handling
- LINQ

## 📦 Requirements

- .NET 8 SDK
- Visual Studio 2022 / VS Code
