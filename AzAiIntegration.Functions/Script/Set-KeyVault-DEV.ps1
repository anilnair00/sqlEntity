# Define variables
$subscriptionId = "5bb26e59-5cf4-415f-a2d3-ce76a78d1b42"
$resourceGroupName = "RG-CAC-CX-APPX-INTEG-DEV-01"
$keyVaultName = "KV-CAC-CX-INTEG-DEV-01"
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

# Login to Azure
az login

# Set the subscription
az account set --subscription $subscriptionId

foreach ($secret in $secrets) {
    $secretName = $secret.Name
    $secretValue = $secret.Value
    $contentType = $secret.ContentType

    # Check if the secret already exists
    $existingSecret = az keyvault secret show --vault-name $keyVaultName --name $secretName --query "id" --output tsv

    if (-not $existingSecret) {
        # Add the secret to the Key Vault with content type and tag if it does not exist
        az keyvault secret set --vault-name $keyVaultName --name $secretName --value $secretValue --content-type $contentType --tags $tagKey=$tagValue
        Write-Output "Secret '$secretName' added to Key Vault '$keyVaultName' with content type '$contentType' and tag '$tagKey=$tagValue'."
    } else {
        Write-Output "Secret '$secretName' already exists in Key Vault '$keyVaultName'."
    }

    # Output the secret metadata to verify
    $retrievedSecretMetadata = az keyvault secret show --vault-name $keyVaultName --name $secretName --query "{name:name, version:version, created:attributes.created, contentType:contentType, tags:tags}"
    Write-Output "Secret Metadata: $retrievedSecretMetadata"
}