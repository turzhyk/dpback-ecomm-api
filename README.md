# DPBack - Print Shop API

Backend for a print shop / e-commerce platform built with ASP.NET Core 8

## Features

- **Order management:** full state workflow with status transitions.
- **Authentication and Security:** JWT Bearer with role-based access (Admin, Worker, Customer), refresh token rotation with
  automatic expired refresh tokens cleanup.
- **Payments:** PayU integration with webhook handling, HMAC signature verification for notification requests.
- **Pricing engine:** dynamic product price calculation based on product options (via Strategy+Factory)
- **Async document generation:** receipt generation with QuestPdf, asynchronous receipt generation via system channels (in-memory queue) with PeriodicTimer fallback
- **Pagination and Filtering**: paginated order queries.

## Tech Stack

- **Framework:** ASP.NET Core 8
- **Database:**  PostgreSQL + Entity Framework Core
- **Auth:** JWT Bearer
- **Integrations:** PayU REST API, QuestPDF, MailKit, MimeKit
- **Testing:** Moq, xUnit


## Architecture

Clean Architecture with 4 layers:

- **Domain** - domain models, enums.
- **Application** - interfaces, business logic, DTOs, services, exceptions.
- **Infrastructure** - EF Core, repositories, PayU, QuestPdf, MailKit/MimeKit, entities, background services.
- **API** - controllers, middleware, extensions, background services.
- **Tests** - unit-tests.

## Design Decisions

- Clean Architecture for isolated and sustainable API layers that allows horizontal expansion.
- Interfase-based dependency inversion.
- Strategy pattern for price calculators. Adding new product type requires only a new class implementing
  IPriceCalculator
- PayU webhook ("notify") signature verification via HMAC
- Global exception handler middleware for forming precise request responses / hiding stack traces

## Getting started

- Install and launch Docker  
  `https://www.docker.com/`
- Clone the repo  
  `git clone https://github.com/turzhyk/dpback-ecomm-api`
- Create your .env file (`cd dpback-ecomm-api`)   
  `cp .env.example .env`  
- edit `.env` with your own credentials and API keys
- Compose and run the containers  
  `docker compose up -d --build`
- The API is running!

__Swagger will be available at http://localhost:8080/swagger/index.html__

#

__API documentation available via Swagger__