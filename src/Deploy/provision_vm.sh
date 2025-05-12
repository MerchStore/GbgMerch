#!/bin/bash
set -e  # Avbryt scriptet om något går fel

# === Variabler ===
resource_group="GbgMerchRG"
location="northeurope"
env_name="GbgMerchEnv"
app_name="gbgmerch"
app_port=8080
workspace_name="GbgMerchLogs"
image="ghcr.io/merchstore/gbgmerch:latest"

echo "🔐 Registrerar Log Analytics-provider om det behövs..."
az provider register -n Microsoft.OperationalInsights --wait

echo "📦 Skapar Resource Group: $resource_group"
az group create --name "$resource_group" --location "$location" --output none

echo "📊 Skapar Log Analytics workspace: $workspace_name"
az monitor log-analytics workspace create \
  --resource-group "$resource_group" \
  --workspace-name "$workspace_name" \
  --location "$location" \
  --output none

echo "📥 Hämtar Workspace ID och Key"
workspace_id=$(az monitor log-analytics workspace show \
  --resource-group "$resource_group" \
  --workspace-name "$workspace_name" \
  --query customerId -o tsv)

workspace_key=$(az monitor log-analytics workspace get-shared-keys \
  --resource-group "$resource_group" \
  --workspace-name "$workspace_name" \
  --query primarySharedKey -o tsv)

echo "🏗️ Skapar Container App Environment: $env_name"
az containerapp env create \
  --name "$env_name" \
  --resource-group "$resource_group" \
  --location "$location" \
  --logs-workspace-id "$workspace_id" \
  --logs-workspace-key "$workspace_key" \
  --output none

# Väntar tills miljön är redo
echo "⏳ Väntar tills miljön är redo..."
while true; do
    state=$(az containerapp env show \
      --name "$env_name" \
      --resource-group "$resource_group" \
      --query properties.provisioningState \
      --output tsv)
    if [ "$state" == "Succeeded" ]; then
        echo "✅ Miljön är nu aktiv!"
        break
    fi
    echo "⌛ Status: $state... väntar 5 sek"
    sleep 5
done

echo "🚀 Skapar Container App: $app_name"
az containerapp create \
  --name "$app_name" \
  --resource-group "$resource_group" \
  --image "$image" \
  --environment "$env_name" \
  --target-port "$app_port" \
  --ingress external \
  --query properties.configuration.ingress.fqdn
