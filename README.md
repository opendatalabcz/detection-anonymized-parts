[![codecov](https://codecov.io/gh/Oranged9922/detection-anonymized-parts-in-pdfs-bachelor-thesis/branch/master/graph/badge.svg?token=C7H0CZLJZU)](https://codecov.io/gh/Oranged9922/detection-anonymized-parts-in-pdfs-bachelor-thesis)
![Tests](https://github.com/Oranged9922/detection-anonymized-parts-in-pdfs-bachelor-thesis/actions/workflows/test_api.yml/badge.svg)
![Build .NET](https://github.com/Oranged9922/detection-anonymized-parts-in-pdfs-bachelor-thesis/actions/workflows/build_api.yml/badge.svg)
![Build and Verify PDF](https://github.com/Oranged9922/detection-anonymized-parts-in-pdfs-bachelor-thesis/actions/workflows/build_thesis.yml/badge.svg)
[![Release](https://github.com/Oranged9922/detection-anonymized-parts-in-pdfs-bachelor-thesis/actions/workflows/release.yml/badge.svg)](https://github.com/Oranged9922/detection-anonymized-parts-in-pdfs-bachelor-thesis/releases)
# Project Specification: Detection of Anonymized Sections in PDF Documents

## Purpose and Description

- Software Title: DAPP (**D**etector of **A**nonymized **P**arts in **P**DFs)
- Description: DAPP is a tool for analyzing PDF documents to detect anonymized sections. It is designed as a web service that receives input data as an HTTP request with local paths or URLs to PDF files and returns analysis results in JSON format.

## Table of Contents
- [Purpose and Description](#purpose-and-description)
- [Usage](#usage)
    - [Prerequisites](#prerequisites)
    - [Installation](#installation)

        -[Command-line Options](#command-line-options)

        -[Example Commands](#example-commands)

        -[Output](#output)
- [Objectives and Requirements](#objectives-and-requirements)
    - [Functional Requirements](#functional-requirements)
    - [Technical Requirements](#technical-requirements)
- [Architecture and Design](#architecture-and-design)
    - [Technologies and Tools](#technologies-and-tools)
    - [Data Model](#data-model)
    - [User Interface](#user-interface)
    - [Algorithms and Procedures](#algorithms-and-procedures)
- [Testing and Validation](#testing-and-validation)
    - [Testing Plan](#testing-plan)
    - [Validation of Results](#validation-of-results)
- [Timeline](#timeline)
    - [Estimated Milestone Deadlines](#estimated-milestone-deadlines)
- [Configuration (Versioning)](#configuration-versioning)
- [Assumptions](#assumptions)
- [Limitations](#limitations)

## Usage
### Prerequisites

- Install .NET 7.0 or higher
- Install ImageMagick (https://imagemagick.org/)
- Install Ghostscript (https://www.ghostscript.com/)

## Installation

1. Clone the repository.
   ```bash
   git clone github.com/Oranged9922/detection-anonymized-parts-in-pdfs-bachelor-thesis.git
   ```
2. Navigate to the project folder and build the solution.
   ```bash
   cd implementation/Dapp
   dotnet build
   ```
Run the console application (in the ConsoleApp folder) with the following command-line options:

### Command-line Options

- `--file-location` *(mandatory)*: Path to the file to analyze.
- `--return-images` *(optional, default=false)*: Enter `true` if you want to return images.
- `--output-folder` *(optional)*: Directory where the images will be saved. If not specified, images won't be saved.

### Example Commands

Analyzing a document without returning images:
```bash
dotnet run -- --file-location /path/to/file
```

Analyzing a document and returning images, but not saving them:
```bash
dotnet run -- --file-location /path/to/file --return-images true
```

Analyzing a document, returning and saving images:
```bash
dotnet run -- --file-location /path/to/file --return-images true --output-folder /path/to/output/folder
```

## Output

The console will print JSON data received from the API, including the document ID. If an output folder is specified, images will be saved in the format `original_{i}.jpg` and `result_{i}.jpg`.

That's it. Follow the steps to effectively use the console application.

## Objectives and Requirements
The aim of this software is to develop and implement a tool capable of detecting anonymized sections in PDF documents. The software will be written in C# using the minimal API interface. The output will be in JSON format containing data on the analyzed PDF, such as the number of pages, the percentage of anonymization on each page, and the overall average anonymization.

### Functional Requirements

- The software must be able to accept an HTTP request containing a URL link to a PDF file or a local path to the file.
- The software must be able to read and process PDF files.
- The software must be able to detect anonymized sections in PDF documents.
- The software must be able to analyze and calculate the percentage of anonymization on each page of the PDF file and the overall average anonymization.
- The software must be able to return the results in a specified JSON format containing the number of pages, the percentage of anonymization on each page, and the overall average anonymization.
- The software must be accompanied by appropriate documentation, both user and developer.

### Technical Requirements

- The software must be written in C#.
- The software will use the minimal API interface for receiving requests and returning results.
- The software must be compatible with the latest version of the .NET platform (.NET 7).
- The software must support the processing of PDF files.
- The software must support the JSON format for output data.

## Architecture and Design
### System Architecture:
- Client-Server: The client sends requests to the server, which processes PDF files and returns results in JSON format.

### Technologies and Tools:
- Language: C#
- Framework: .NET 7
- API: Minimal API
- PDF Processing: Custom implementation of analyzer for working with PDFs in C#

### Data Models:
- Input:

 HTTP Request (URL link to a PDF file or a local path)
- Output: JSON Response (Number of pages, percentage of anonymization on each page, overall average anonymization)

### Algorithms and Procedures:
- PDF File Reader: Custom implementation using third-party libraries to read and analyze PDF files.
- Anonymization Detection: Custom implementation using image processing and data analysis.

## Testing and Validation
### Testing Plan:
- Unit Testing: For each implemented method and function.
- Integration Testing: For testing the interaction between different components.
- Acceptance Testing: For verifying that the application meets the requirements specified.

### Validation of Results:
- The output results will be validated by analyzing a set of sample PDFs that contain known amounts of anonymized sections.

## Timeline
### Estimated Milestone Deadlines:
- 1. Define Objectives and Requirements: [Date]
- 2. Develop Preliminary Design: [Date]
- 3. Implement Core Features: [Date]
- 4. Testing: [Date]
- 5. Finalize and Deploy: [Date]

## Configuration (Versioning)
The project will be maintained on GitHub, and versioning will be managed through Git. Periodic backups and branches for new features will be created.

## Assumptions
- The user will provide valid paths or URLs to PDF files.
- The PDF files will be properly formatted.

## Limitations
- The accuracy of anonymization detection may vary depending on the quality and content of the PDFs.
- May not be able to handle very large PDF files due to hardware limitations.

## Conclusion

The DAPP system will be a valuable tool for detecting anonymized sections in PDF documents, providing quick and accurate results. By following the specified objectives and requirements, the project is expected to meet the needs of the users effectively.
