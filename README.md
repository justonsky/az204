# az204
Mini-projects to practice for Microsoft's AZ-204 Azure certification.

## Projects
### algodev-arm
An Azure Resource Manager template to create a new Azure VM installed with Docker & Algorand's [sandbox](https://github.com/algorand/sandbox), a fast way to create an Algorand development environment (which is pretty resource-heavy).
#### Getting started
You will need to have an Azure account with appropriate permissions to create new resources and the [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli) installed as a pre-requisite.

1. Clone the repository, go to the `algodev-arm` folder and edit the `parameters.json` file to customize the VM as you like. By default the VM will use SSH key-based authentication, so if you leave the `authenticationType` to `sshPublicKey` you must provide the key as the value for the `adminPasswordOrKey` property.
2. Create the resource group that will contain the VM & its associated resource.
3. Run the following command in the `algodev-arm` folder: `az deployment group create --resource-group <YOUR-RESOURCE-GROUP-NAME> --template-file ./template.json --parameters ./parameters.json`

### fileshare
An anonymous filesharing site in the same vein as Volafile. Backend uses .NET Core, frontend uses React. Azure Blob Storage is used to host files. PostgreSQL stores room & file metadata.
#### Getting started
1. Clone the repository, and go to the `fileshare` folder.
2. Restore any dependencies using `dotnet restore` in the base folder, and `npm install` in the `ClientApp` folder.
3. We use Microsoft's [Secret Manager](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets) tool to store any sensitive data such as database connection strings and app secrets during development. Set up the following secrets using the command for each secret: `dotnet user-secrets set "<SECRET-NAME>" "<SECRET-VALUE>"`:
    - `DbConnectionString`: The connection string to the underlying PostgreSQL database
    - `StorageConnectionString`: The connection string to Azure Blob Storage
4. Run the project with `dotnet run`.
