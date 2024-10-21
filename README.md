# AirCanada.Appx.Integration

Air Passenger Protection (APPX) repository provides capability to integrate Azure based Artificial Intelligence (AZAI) system with PCAT application. It uses Azure function responsible for handling request and response between the PCAT storage account and Azure AI sub-system. 

## Build Requirements

Visual Studio 2022 - Version 17.10.3 or later

Updated Azure Function:
```
Visual Studio -> Tools -> Options -> Projects & Solutions -> Azure Functions and click on the "Check for updates" button.
```
## Build Steps

### Using Command Line
```csharp
- Clone this repository 
  git clone https://github.com/AC-IT-Development/AirCanada.Appx.Integration.git

- Navigate to repository folder
  cd AirCanada.Appx.Integration

- Build solution
  dotnet build
```
### Using Visual Studio

```csharp
- Navigate to repository using file explorer.

- Open solution file in Visual Studio 2022.

- Select [AzAiIntegration.Fuctions] as startup project.

- Build solution.
```

## Trigger/Test Azure Function Steps

#### Install Microsoft Microsoft Storage Explorer
- Add 'Azure Storage Emulator.xml' file in task scheduler and run the task added. 

  > **_Note:_** Please connect with developer to use updated .xml file.

- Download 'Microsoft Azure Storage Explorer' using [Microsoft link](https://azure.microsoft.com/en-us/products/storage/storage-explorer)
- Add local.settings.json file in the AzAiIntegration.Functions Project. 
  > **_Note:_** Please connect with developer to use updated .json file.

- Run AzAiIntegration.Function using Visual Studio.
- Launch 'Microsoft Azure Storage Explorer'.
- Add New Blob Container in the Storage Accounts tab.
- Add 'Sample Receipt' file in the Microsoft Azure Storage Explorer.
- Running Azure function will be triggered after the sample receipt is added. 

#### Install Service Bus Explorer locally 

- Download Chocolatey using [https://chocolatey.org/install#individual](https://chocolatey.org/install#individual)
- Install [Service Bus Explorer](https://github.com/paolosalvatori/ServiceBusExplorer) using following command in PowerShell
  ```ps
   choco install ServiceBusExplorer
  ```
- Add new connection string navigating using following
  ```
   File -> New Connection -> Enter connection string -> Connection string
  ```
  > **_Note:_** Please connect with developer to use updated connection string.


- ReceiptReader and ReceiptResponse queues will be available to use and test project on local development environment.
- For verifying the content messages in the queues please use the message 'Peek' option rather 'Receive and Delete'.

## Dependabot

- To learn how to manage the Dependabot PRs, please visit https://docs.github.com/en/code-security/dependabot/working-with-dependabot/managing-pull-requests-for-dependency-updates
- For Dependabot configuration options, please visit https://docs.github.com/en/code-security/dependabot/dependabot-version-updates/configuration-options-for-the-dependabot.yml-file 

## Contributing

Pull requests are welcome. Please submit pull requests with appropriate description of changes to facilitate review process. 
