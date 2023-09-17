# Technical Documentation Template for DAPP REST API

## Overview
- **Project Name**: DAPP
- **Main Functionality**: Analyzer, Document Processing
- **Tech Stack**: .NET Core, C#, Entity Framework, SQLite

---

## Table of Contents
1. Introduction
2. Architecture
3. API Endpoints
4. Database Schema
5. Application Layer
6. Domain Layer
7. Infrastructure Layer
8. PDF Analyzer
9. Error Handling
10. Tests
---

## 1. Introduction
This section serves as a concise overview of what the DAPP project aims to achieve, its key features, and important technical details. It should give anyone unfamiliar with the project a clear understanding of its purpose and scope.
### Objective
The DAPP (Detector of Anonymized Parts in PDFs) is designed to automate the analysis of PDF documents, providing structured insights and facilitating further data processing regarding anonymization.

### Features
- PDF Document Analysis: Extract and detect anonymized areas,such as blackened parts, gray areas or parts that were covered with colored stickers.
- Data Management: Manage analyzed data and associate it with respective documents and pages.
- Error Handling: Capture and report issues during the document processing pipeline.

### Target Audience
Developers and data analysts looking to integrate PDF document analysis into their data workflows.

### Tech Stack
- Back-end: .NET Core, C#, Entity Framework
- Database: SQLite

### Dependencies
- `.NET Core 7.0 or later`
- `SQLite`
- `ImageMagick`
- `GhostScript`
### API Version
- Current API version: `v2`

---

## 2. Architecture

This chapter aims to give a broad understanding of the system's architecture, helping both new and experienced developers understand the high-level design and flow of the application.

### Overview
The DAPP project follows a modular and layered architecture, making it scalable and maintainable. The architecture is broken down into several layers and components, each with a specific responsibility.

### Layers and Components

#### API2
- **Responsibility**: Exposes REST API endpoints.
- **Tech**: ASP.NET Core, Controllers

#### Application
- **Responsibility**: Contains business logic and application services.
- **Tech**: C#, Commands, Queries, Validators

#### Domain
- **Responsibility**: Houses domain entities and value objects.
- **Tech**: C#, Aggregate Roots, Entities, Value Objects

#### Infrastructure
- **Responsibility**: Manages data storage and other infrastructure concerns.
- **Tech**: Entity Framework, SQLite, Repositories

#### PDFAnalyzer
- **Responsibility**: Performs PDF analysis.
- **Tech**: C#

#### ConsoleApp
- **Responsibility**: Standalone application to test or run services.
- **Tech**: C#

#### Contracts
- **Responsibility**: Defines DTOs and contracts for data exchange.
- **Tech**: C#

#### Tests
- **Responsibility**: Unit and integration tests.
- **Tech**: xUnit

### Data Flow
1. **API Request**: User makes an API request.
2. **Application Layer**: The request is processed, validated, and the business logic is executed.
3. **Domain Layer**: Domain rules and validations are checked.
4. **Infrastructure Layer**: Data is fetched or stored in the database.
5. **PDFAnalyzer**: PDFs are analyzed as per the requirement.
6. **Response**: API returns the processed data or feedback to the user.

### Additional Components
- **Migrations**: Keeps track of database changes.
- **Common**: Shared resources like common errors, utilities, and shared logic.

---

## 3. API Endpoints

This section aims to help developers understand how to use API effectively.

#### AnalyzerController

##### Analyze Document

- **Endpoint**: `/analyze`
- **HTTP Method**: POST
- **Description**: Analyzes a document based on the provided request.
- **Request Body**: `AnalyzeDocumentRequest`
- **Response Body**: `AnalyzeDocumentResponse`

###### Example Request
```json
{
  "DocumentId": "123",
  "ReturnImages": true
}
```

###### Example Response
```json
{
  "DocumentId": "123",
  "Url": "some_url",
  "ContainsAnonymizedData": true,
  "AnonymizedPercentage": 0.8,
  // ... other fields
}
```

##### Get Document Results

- **Endpoint**: `/results`
- **HTTP Method**: GET
- **Description**: Retrieves the analysis results for a given document.
- **Request Body**: `GetDocumentPagesRequest`
- **Response Body**: `GetDocumentPagesResponse`

###### Example Request
```json
{
  "DocumentId": "123"
}
```

###### Example Response
```json
{
  "DocumentId": "123",
  "Url": "some_url",
  "Pages": {
    "1": {
      "Original": "image_byte_array",
      "Result": "image_byte_array"
    },
    // ... other pages
  }
}
```
---

## 4. Database Schema
This section is aimed to assist anyone who needs to understand the data structure of the application. Providing the class and namespace where the configurations are defined helps locate them quickly in the codebase.

### Document Table Configuration

- **Table Name**: `Documents`
- **Primary Key**: `Id`
- **Columns**: 
  - `Id` (Guid, Not auto-generated)
  - `Name` (String, Max Length 255)
  - `Url` (String, Max Length 1024)
  - `Hash` (String, Max Length 255)

**Relations**: 
- One-to-Many with `Pages` table via `DocumentId`

#### Configuration Code

Refer to `DocumentConfiguration` class under `Infrastructure.Configurations` namespace.

---

### Page Table Configuration

- **Table Name**: `Pages`
- **Primary Key**: `Id`
- **Columns**: 
  - `Id` (Guid, Not auto-generated)
  - `OriginalImageUrl` (String, Max Length 1024)
  - `ResultImageUrl` (String, Max Length 1024)
  - `AnonymizationResult` (Type TBD)

**Relations**: 
- Many-to-One with `Documents` table via `DocumentId`

#### Configuration Code

Refer to `PageConfiguration` class under `Infrastructure.Persistance.Configurations` namespace.

---
## 5. Application Layer
This section should provide a clear overview of what the Application Layer is responsible for and its key components. Code structure, class responsibilities, and dependencies are explained here.
### Responsibilities

- Coordinates application behavior through Command and Query objects
- Mediation between the domain and data layers using MediatR

### Components

#### Commands

- `AnalyzeDocumentCommand`: Handles the logic for document analysis
- `ParseDocumentCommand`: Responsible for parsing a document
- `RegisterDocumentCommand`: Manages document registration

#### Queries

- `GetAnalyzedDocumentDataQuery`: Retrieves analyzed document data

#### Handlers

Handlers for the above commands and queries handle the business logic and database interactions. Implementation is located in a separate file in the folder together with the command/query. 

#### DTOs (Data Transfer Objects)

- `AnalyzeDocumentRequest`: DTO for analyzing documents
- `AnalyzeDocumentResponse`: DTO for returning analysis results
- `GetDocumentPagesRequest`: DTO for fetching document pages
- `GetDocumentPagesResponse`: DTO for returning fetched pages

#### Mapper

- Uses Mapster for object mapping between layers.

### Key Classes and Files

- `AnalyzerController`: API controller for handling document analysis
- `Application.Analyzer.Commands`: Namespace for all Command classes related to document analysis
- `Application.Analyzer.Queries`: Namespace for all Query classes related to document analysis

### Dependencies

- MediatR for CQRS pattern
- Mapster for object-to-object mapping

---

## 6. Domain Layer

This section should elaborate on the essential aspects of the Domain Layer, giving clarity on its design, responsibilities, and how it enforces business rules and constraints.
### Introduction

In a DDD (Domain-Driven Design) context, the Domain Layer is structured around the concepts of Entities, Value Objects, and Aggregates. 

- **Entities**: Objects that have a distinct identity and run through the system, with a lifecycle. They can change state but maintain their identity.
  
- **Value Objects**: Immutable objects that describe some characteristic or attribute but lack a distinct identity. They are fully defined by their attributes.

- **Aggregates**: A cluster of domain objects that can be treated as a single unit for data changes. The Aggregate Root is the main entry point to the Aggregate; it ensures all invariants are satisfied before and after any operation that changes the state of the Aggregate.

### Responsibilities

- Encapsulates core business logic and rules
- Defines entities, value objects, aggregates, and domain events

### Components

#### Entities

- `Document`: Represents a document in the system
- `Page`: Represents a page in a document

#### Value Objects

- `DocumentId`: Uniquely identifies a document
- `PageId`: Uniquely identifies a page in a document

#### Aggregates

- `DocumentAggregate`: Handles operations that affect the Document entity and its child entities or value objects.

#### Domain Events

- None in current implementation

### Relationships

- One `Document` can have multiple `Pages`
  
### Business Rules

- Document must have a unique ID and URL
- Pages belong to a single Document and must have unique IDs within that Document

### Key Classes and Files

- `Domain.DocumentAggregate`: Namespace for all classes related to the Document entity
- `Domain.PageAggregate`: Namespace for all classes related to the Page entity
- `Domain.DocumentAggregate.ValueObjects`: Namespace for all value objects related to the Document entity
- `Domain.PageAggregate.ValueObjects`: Namespace for all value objects related to the Page entity

### Dependencies

- No external dependencies. Relies solely on core .NET libraries.

---

### 7. Infrastructure Layer

#### Migrations
Responsible for evolving the database schema over time without data loss. Generated and managed through Entity Framework Core commands like `Add-Migration`, `Update-Database`.

#### Configurations
Defines how domain entities are mapped to database tables. In your code, `DocumentConfiguration` and `PageConfiguration` classes serve this purpose. They specify table names, primary keys, field lengths, and relationships.

#### Repositories
Abstraction layer between domain logic and data source. Implement interfaces defined in the application layer to handle CRUD operations. Typically use Entity Framework Core's `DbContext` for database interactions.

---

### 8. PDF Analyzer

`PDFAnalyzer` class is a service layer class responsible for performing PDF analysis. It's fairly straightforward; the main method, `AnalyzeAsync`, processes a `DappPDF` object and returns an `AnalyzedResult`. The workflow can be broken down as follows:

1. **AnalyzeAsync Method**: Takes a `DappPDF` object, processes it page by page, and returns an `AnalyzedResult`. Uses `Task.Run()` to perform the operations asynchronously.

2. **AnalyzePage Method**: Takes a single `Mat` (a single PDF page) and returns relevant metrics like whether the page contains anonymized data and the percentage of anonymized content.

3. **GetAnonymizedParts Method**: Processes the given `Mat` image and returns another `Mat` containing the parts of the image that are considered "anonymized".

4. **ColoredPixels, Dilate, Erode, Threshold, IncreaseSaturation, MaskOriginal**: These are utility methods used within the main `AnalyzePage` and `GetAnonymizedParts` methods to manipulate and process the image data.

5. **InternalsVisibleTo Attribute**: Making internals of this assembly visible to a "Unit.Tests" assembly for testing.

6. **Return Types**: Methods like `AnalyzePage` and `AnalyzeAsync` return tuples and task-wrapped custom objects, making it easier to use the results asynchronously.

---

## 9. Error Handling

### `API2/Common/Errors`

The `API2/Common/Errors` directory contains components that are responsible for handling errors across the API. The primary class here is `DappProblemDetailsFactory`, which extends ASP.NET Core's default `ProblemDetailsFactory`.

#### DappProblemDetailsFactory

##### Description

The `DappProblemDetailsFactory` class provides a centralized way of creating `ProblemDetails` instances, which are used for error responses throughout the API. By extending `ProblemDetailsFactory`, this custom implementation allows you to standardize the API's error format and include additional information.

##### Location

- Project: API2
- Namespace: API2.Common.Errors
- File path: `API2/Common/Errors/DappProblemDetailsFactory.cs`

### `Domain/Common/Errors`

The `Domain/Common/Errors` directory is responsible for defining domain-specific errors that could occur during the application's operation. Each class in this directory represents a domain or context within which a set of errors is defined.

#### ErrorOr

Before diving into specific classes, it's important to mention that the `ErrorOr` namespace is used for creating domain errors. [`ErrorOr`](https://github.com/amantinband/error-or) is a third-party library that is used to create standardized error objects.

#### Analyzer

##### Description

The `Analyzer` class contains errors specifically associated with document analysis.

##### Location

- Project: Domain
- Namespace: Domain.Common.Errors
- File path: `Domain/Common/Errors/Analyzer.cs`

##### Error Definitions

- `DocumentNotYetAnalyzed`: Triggered when the document has not been analyzed yet.
  - Code: 701
  - Description: "The document has not been analyzed yet."
  
#### FileHandle

##### Description

The `FileHandle` class contains errors that could occur during file operations.

##### Location

- Project: Domain
- Namespace: Domain.Common.Errors
- File path: `Domain/Common/Errors/FileHandle.cs`

##### Error Definitions

- `LoadingPdfError`: Triggered when there is a failure in loading a PDF file.
  - Code: 901
  - Description: "Failed to load a pdf."

#### Repository

##### Description

The `Repository` class defines errors that could occur at the repository layer.

##### Location

- Project: Domain
- Namespace: Domain.Common.Errors
- File path: `Domain/Common/Errors/Repository.cs`

##### Error Definitions

- `EntityDoesNotExist`: Triggered when an entity does not exist in the database.
  - Code: 801
  - Description: "Entity with given Id is not in the database."

### Sample Code

```csharp
// Analyzer.cs
public static readonly Error DocumentNotYetAnalyzed = Error.NotFound(code: "701", description: "The document has not been analyzed yet.");

// FileHandle.cs
public static readonly Error LoadingPdfError = Error.Validation(code: "901", description: "Failed to load a pdf.");

// Repository.cs
public static readonly Error EntityDoesNotExist = Error.Failure(code: "801", description: "Entity with given Id is not in the database.");
```

These error definitions can then be used in the application logic to throw errors or return responses in a standardized manner.

---

## 10. Tests
### `Tests`
- Unit Tests
  - Analyzer.Tests
  - Models.Tests
  - Services.Tests
- Integration Tests
  - Api.Tests

### High-Level Overview of Unit Tests

#### File Handling

- **DappFileHandleServiceTests**: Verifies file reading functionality, supporting both internet and local file paths.

#### Date and Time

- **DateTimeProviderServiceTests**: Validates the retrieval of system date and time.

#### PDF Operations

- **DappPDFTests**: Ensures proper object creation for PDFs, including error-handling for invalid data.

#### Page Object Tests

- **PageTests**: Validates object creation and property assignments for page-related operations.

#### Document Object Tests

- **DocumentTests**: Validates object creation and property assignments for document-related operations.

#### Entity Common Tests

- **CommonTests**: Tests equality and hashing operations on Document objects.

#### Image and PDF Analysis

- **DappAnalyzerTests**: Covers image manipulation and PDF analysis, including color adjustments and error handling.
---