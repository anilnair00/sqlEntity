# Define variables
$subscriptionId = "5bb26e59-5cf4-415f-a2d3-ce76a78d1b42"
$resourceGroupName = "RG-CAC-CX-APPX-INTEG-DEV-01"
$functionAppName = "FUNC-CAC-CX-APPX-INTEG-DEV-01"

# Define environment variables
$envVariables = @(
    @{ Name = "ASPNETCORE_ENVIRONMENT"; Value = "DEV" },
    @{ Name = "CalibrationType"; Value = "absolute" },
    @{ Name = "CalibrationValue"; Value = "0.95" },
    @{ Name = "APPX-AzureServiceBus-ConnectionString"; Value = "@Microsoft.KeyVault(SecretUri=$keyVaultSecretUri)" }
)

# Login to Azure
az login

# Set the subscription
az account set --subscription $subscriptionId

# Set environment variables for the Function App
foreach ($envVar in $envVariables) {
    $name = $envVar.Name
    $value = $envVar.Value

    az functionapp config appsettings set --name $functionAppName --resource-group $resourceGroupName --settings "$name=$value"
    Write-Output "Environment variable '$name' set to '$value' for Function App '$functionAppName'."
}

# Output the set environment variables to verify
$settings = az functionapp config appsettings list --name $functionAppName --resource-group $resourceGroupName
Write-Output $settings