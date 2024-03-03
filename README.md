# Speechify

The Speechify project consists of two main components: Azure Functions and an API. These components work together to process images, extract text from them, generate audio files, and provide an interface for uploading files and checking processing status.

## Azure Functions

The Azure Functions project is responsible for processing images stored in Azure Blob Storage. It utilizes Azure Cognitive Services for text extraction and Azure Speech Services for audio generation. Here's an overview of its features:

### Description

The Azure Functions project processes images stored in Azure Blob Storage, extracts text from them using Azure Cognitive Services, and generates audio files from the extracted text. It tracks the processing status of each operation using Azure Table Storage.

## API

The API project serves as an interface for uploading files to Azure Blob Storage and checking the status of processing tasks initiated by the Azure Functions project. Here's an overview of its features:

### Description

The API project provides endpoints for uploading files to Azure Blob Storage and querying the status of processing tasks. It interacts with Azure Blob Storage and other services to facilitate these operations.

### Prerequisites

- Azure Account: Required to set up Azure Blob Storage.
- Blob Storage Account: Storage account for storing uploaded files.
- Configuration: Ensure proper configuration of application settings.

## Installation Process Overview:

1. **Clone the Repository**: Start by cloning the Speechify repository from the version control system (e.g., GitHub) to your local machine.

2. **Navigate to Azure Functions Directory**: In your terminal or command prompt, navigate to the directory containing the Azure Functions project within the Speechify repository.

3. **Create App Settings**:
   - Create an `appsettings.json` file in the Azure Functions project directory.
   - Populate the `appsettings.json` file with the necessary configuration values for Azure services and other settings as outlined earlier.

#### Azure Functions `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "SpeechifyStorageConnectionString": "YOUR_STORAGE_CONNECTION_STRING"
  },
  "TextEndpoint": "YOUR_AZURE_COGNITIVE_SERVICES_TEXT_ENDPOINT",
  "TextKey": "YOUR_AZURE_COGNITIVE_SERVICES_TEXT_KEY",
  "SpeechSubscriptionKey": "YOUR_AZURE_SPEECH_SERVICES_SUBSCRIPTION_KEY",
  "SpeechRegion": "YOUR_AZURE_SPEECH_SERVICES_REGION"
}
```

4. **Build and Run the Project**:
   - Use the appropriate .NET build commands (e.g., `dotnet build`) to build the Azure Functions project.
   - Run the project locally using the .NET CLI or your preferred development environment to test its functionality.

5. **Deploy to Azure (Optional)**:
   - Once tested locally, deploy the Azure Functions project to Azure Functions using Azure CLI, Visual Studio, or Azure DevOps.

6. **Test Functionality**:
   - After deployment, test the functionality of the Azure Functions project to ensure it is working as expected in a production environment.

7. **Troubleshooting and Debugging**:
   - If any issues arise during installation or testing, troubleshoot and debug the code as necessary.


## Installation Process for API:

1. **Navigate to API Directory**: In your terminal or command prompt, navigate to the directory containing the API project within the Speechify repository.

2. **Create App Settings**:
   - Create an `appsettings.json` file in the API project directory.
   - Populate the `appsettings.json` file with the necessary configuration values for Azure Blob Storage and API URL as outlined earlier.

#### API `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "BlobStorageConnectionString": "YOUR_BLOB_STORAGE_CONNECTION_STRING"
  },
  "ApiUrl": "YOUR_API_URL"
}
```

3. **Build and Run the Project**:
   - Use the appropriate .NET build commands (e.g., `dotnet build`) to build the API project.
   - Run the project locally using the .NET CLI or your preferred development environment to test its functionality.

4. **Deploy to Azure (Optional)**:
   - Once tested locally, deploy the API project to Azure App Service or any other hosting service of your choice.

5. **Test Functionality**:
   - After deployment, test the functionality of the API project to ensure it is working as expected in a production environment.

6. **Troubleshooting and Debugging**:
   - If any issues arise during installation or testing, troubleshoot and debug the code as necessary.


## License


[MIT](https://choosealicense.com/licenses/mit/)
[![MIT License](https://img.shields.io/badge/License-MIT-green.svg)](https://choosealicense.com/licenses/mit/)


