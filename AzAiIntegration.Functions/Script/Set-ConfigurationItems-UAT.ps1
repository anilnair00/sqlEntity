# Define variables
$subscriptionId = "0ebd558a-06d8-4285-8f6b-42faecee4ebe"
$resourceGroupName = "RG-CAC-CX-APPX-INTEG-UAT-01"
$functionAppName = "FUNC-CAC-CX-APPX-INTEG-UAT-01"
$keyVaultName = "KV-CAC-CX-INTEG-UAT-01"
$secretValue = "[SecretValue]"
$secrets = @(
    @{ Name = "APPX-AzureServiceBus-ConnectionString"; Value = $secretValue; ContentType = "Connection String" },
    @{ Name = "APPX-BlobStorage-ConnectionString"; Value = $secretValue; ContentType = "Connection String" },
    @{ Name = "APPX-Claim-ConnectionString"; Value = $secretValue; ContentType = "Connection String" },
    @{ Name = "APPX-ConnectionString"; Value = $secretValue; ContentType = "Connection String" },
    @{ Name = "APPX-INTEG-WebJobsStorage-ConnectionString"; Value = $secretValue; ContentType = "Connection String" }
)
$tagKey = "Application"
$tagValue = "AzAiIntegration"

# Define environment variables
$envVariables = @(
    @{ Name = "ASPNETCORE_ENVIRONMENT"; Value = "UAT" },
    @{ Name = "CalibrationType"; Value = "absolute" },
    @{ Name = "CalibrationValue"; Value = "0.95" }
)

# Login to Azure
az login

# Set the subscription
az account set --subscription $subscriptionId

# Add secrets to Key Vault and set corresponding Function App environment variables
foreach ($secret in $secrets) {
    $secretName = $secret.Name
    $secretValue = $secret.Value
    $contentType = $secret.ContentType

    # Check if the secret already exists
    $existingSecret = az keyvault secret show --vault-name $keyVaultName --name $secretName --query "id" --output tsv

    if (-not $existingSecret) {
        # Add the secret to the Key Vault with content type and tag if it does not exist
        $secretIdentifier = az keyvault secret set --vault-name $keyVaultName --name $secretName --value $secretValue --content-type $contentType --tags $tagKey=$tagValue --query "id" --output tsv
        Write-Output "Secret '$secretName' added to Key Vault '$keyVaultName' with content type '$contentType' and tag '$tagKey=$tagValue'."
    } else {
        $secretIdentifier = $existingSecret
        Write-Output "Secret '$secretName' already exists in Key Vault '$keyVaultName'."
    }

    # Set the Function App environment variable with the Secret Identifier
    az functionapp config appsettings set --name $functionAppName --resource-group $resourceGroupName --settings "$secretName=@Microsoft.KeyVault(SecretUri=$secretIdentifier)"
    Write-Output "Environment variable '$secretName' set to '@Microsoft.KeyVault(SecretUri=$secretIdentifier)' for Function App '$functionAppName'."

    # Output the secret metadata to verify
    $retrievedSecretMetadata = az keyvault secret show --vault-name $keyVaultName --name $secretName --query "{name:name, version:version, created:attributes.created, contentType:contentType, tags:tags}"
    Write-Output "Secret Metadata: $retrievedSecretMetadata"
}

# Set additional environment variables for the Function App
foreach ($envVar in $envVariables) {
    $name = $envVar.Name
    $value = $envVar.Value

    az functionapp config appsettings set --name $functionAppName --resource-group $resourceGroupName --settings "$name=$value"
    Write-Output "Environment variable '$name' set to '$value' for Function App '$functionAppName'."
}

# Output the set environment variables to verify
$settings = az functionapp config appsettings list --name $functionAppName --resource-group $resourceGroupName
Write-Output $settings