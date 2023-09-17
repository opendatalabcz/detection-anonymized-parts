### Analyzer API User Documentation

#### Overview
This document outlines how to interact with the Analyzer API and its associated console application.

#### API Endpoints

---

### 1. Analyze Document

#### Method
- **HTTP Method:** POST
- **Endpoint:** `/analyze`

#### Request
- **Request Object: `AnalyzeDocumentRequest`**
  - `FileLocation`: Path to the file you want to analyze (String)
  - `ReturnImages`: Whether to return images or not (Boolean, default `false`)
  
#### Example Request
```csharp
var request = new HttpRequestMessage(HttpMethod.Post, "/analyze");
request.Content = new StringContent(JsonConvert.SerializeObject(new AnalyzeDocumentRequest("/path/to/file", true)), Encoding.UTF8, "application/json");
```

#### Response
- **Response Object: `AnalyzeDocumentResponse`**
  - `DocumentId`: Unique identifier for the document (GUID)
  - `Url`: URL where the document is stored
  - `ContainsAnonymizedData`: Boolean indicating if the document contains anonymized data
  - `AnonymizedPercentage`: Float representing the percentage of anonymized data
  - `PageCount`: Integer showing the number of pages in the document
  - `AnonymizedPercentagePerPage`: Dictionary mapping page numbers to anonymization percentage
  - `OriginalImages`: Dictionary mapping page numbers to original images (byte arrays)
  - `ResultImages`: Dictionary mapping page numbers to result images (byte arrays)

---

### 2. Get Results

#### Method
- **HTTP Method:** GET
- **Endpoint:** `/results`

#### Request
- **Request Object: `GetDocumentPagesRequest`**
  - `DocumentId`: Unique identifier for the document (String)
  
#### Example Request
```csharp
var request = new HttpRequestMessage(HttpMethod.Get, "/results");
request.Content = new StringContent(JsonConvert.SerializeObject(new GetDocumentPagesRequest("some-document-id")), Encoding.UTF8, "application/json");
```

#### Response
- **Response Object: `GetDocumentPagesResponse`**
  - `DocumentId`: Unique identifier for the document (GUID)
  - `Url`: URL where the document is stored
  - `Pages`: Dictionary containing a mapping of page numbers to a nested dictionary with original and result images


## Console App

- **How to Run**
  ```bash
  dotnet run --file-location path/to/file --return-images true --output-folder path/to/output
  ```
  
- **Options**
  - `--file-location`: Path to the document file. Required.
  - `--return-images`: Boolean to indicate if images should be returned. Default is `false`.
  - `--output-folder`: Folder where images will be saved. If null, no images are saved.

#### Error Handling
- API returns standard HTTP error codes.
- Check the returned JSON for an `IsError` field for additional details.