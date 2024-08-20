# E-Commerce Microservices Project

This project demonstrates Microservices architecture on .NET platforms, utilizing various technologies and patterns including ASP.NET Web API, Docker, RabbitMQ, MassTransit, gRPC, Ocelot API Gateway, MongoDB, Redis, PostgreSQL, SQL Server, Dapper, Entity Framework Core, CQRS, and Clean Architecture.

## Overview

The project implements e-commerce modules across Product, Basket, and Ordering microservices. It uses both NoSQL (MongoDB, Redis) and Relational databases (PostgreSQL, SQL Server), communicating over RabbitMQ for Event-Driven Communication and utilizing Ocelot API Gateway.

## Final Architecture of the System
![SystemArch](https://github.com/user-attachments/assets/858b768e-e5e1-40ab-8e8d-b8a5b753a7e3)

## Technologies Used
![technologiesUsed](https://github.com/user-attachments/assets/e315cb0d-ae53-4dc3-8680-9bea9a23efce)

## Pub/Sub Pattern 
<img width="830" alt="pubsubpattern" src="https://github.com/user-attachments/assets/d329de49-5b84-4b1d-a0a5-32f7240056d6">

## Container Management via Portainer
![containermanagement](https://github.com/user-attachments/assets/76236f5d-b771-48d9-98c4-a45768dc46b5)

## Elastic Search
<img width="873" alt="ELK" src="https://github.com/user-attachments/assets/7ea7df1a-5e28-4a02-b083-62432478e576">

## Microservices

### Catalog Microservice
- ASP.NET Core Web API application
- REST API principles, CRUD operations
- MongoDB database
- Repository Pattern Implementation
- Swagger Open API implementation

### Basket Microservice
- ASP.NET Web API application
- REST API principles, CRUD operations
- Redis database
- Consumes Discount gRPC Service for inter-service sync communication
- Publishes BasketCheckout Queue using MassTransit and RabbitMQ

### Discount Microservice
- ASP.NET gRPC Server application
- High-performance inter-service gRPC Communication with Basket Microservice
- Protobuf messages for exposing gRPC Services
- Dapper for micro-ORM implementation
- PostgreSQL database

### Ordering Microservice
- Implements DDD, CQRS, and Clean Architecture
- Uses MediatR, FluentValidation, and AutoMapper packages
- Consumes RabbitMQ BasketCheckout event queue using MassTransit-RabbitMQ
- SQL Server database with Entity Framework Core ORM

### Identity Server
- Centralized standalone Authentication Server and Identity Provider
- Implements IdentityServer4 package
- Provides authentication and access control for web applications and Web APIs

### API Gateway (Ocelot)
- Implements API Gateways with Ocelot
- Reroutes microservices/containers through API Gateways
- Implements Gateway aggregation pattern in Shopping.Aggregator
- Secures protected API resources with JWT web tokens

### WebUI ShoppingApp Microservice
- Angular implementation (coming soon)

## Communication
- Sync inter-service gRPC Communication
- Async Microservices Communication with RabbitMQ Message-Broker Service
- RabbitMQ Publish/Subscribe Topic Exchange Model
- MassTransit for abstraction over RabbitMQ
- EventBus.Messages library for shared message types

## Ancillary Containers
- Portainer for lightweight container management UI
- pgAdmin for PostgreSQL administration and development

## Docker
- Docker Compose setup for all microservices
- Containerization of microservices and databases
- Environment variable overrides


## Getting Started
    Clone the repository
    At the root directory which include docker-compose.yml files, run below command:

```
   docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d
```


