#!/bin/bash

#   I'll add some notes about the requirments for this script here:
#   1. For this to work you need to have the azure cli client installed and be logged in
#      - az --version
#      - az login  
#   2. You'll also need to install and login with the github cli
#      - gh auth status
#      - gh auth login
#   3. And finally this script is utilizing the tool jq (json query) to extract
#      the token from github. On windows with git-bash this might require you to
#      add the following to your .bashrc:
#
#      function jq() {
#          /path/to/Git/usr/bin/jq-win64.exe "$@"
#      }
#      export -f jq
#
#      Verify that it works with: jq --version

runner_cloud_init="cloud-init-selfhosted-runner.sh"
adminuser="azureuser"
rg_name="tipsrundan"
gh_repo=$rg_name
gh_user="CuriosityFanClub"
rg_location="swedencentral"
vm_name="tipsrundan-runner"
vm_size="Standard_B1s"
vm_image="Ubuntu2204"

# Make a temporary cloud init file
cloud_init=$(mktemp)
trap "rm -f $cloud_init" EXIT

# Get the registration token for the github runner
function getRunnerToken () {
    local gh_token_response=$(gh api --method POST \
    -H "Accept: application/vnd.github+json" \
    -H "X-GitHub-Api-Version: 2022-11-28" \
    repos/$gh_user/$gh_repo/actions/runners/registration-token)
    echo $(echo $gh_token_response | jq -r '.token')
}

# Write the temporary cloud init for the runner
function getTempCloudInit () {
    cat $runner_cloud_init > $cloud_init
    sed -i "4i token=$(getRunnerToken)" $cloud_init
    sed -i "4i adminuser=$adminuser" $cloud_init
    sed -i "4i gh_org=$gh_user" $cloud_init
    echo $cloud_init
}

# Provision resources on Azure
function provisionResources () {
    # Provision resource group
    az group create \
        --name $rg_name \
        --location $rg_location
    
    # Provision virtual machine
    az vm create \
        --resource-group $rg_name \
        --name $vm_name \
        --size $vm_size \
        --image $vm_image \
        --admin-username $adminuser \
        --generate-ssh-keys \
        --custom-data @$(getTempCloudInit)

    # Open port 80 for HTTP
    az vm open-port \
        --resource-grou $rg_name \
        --name $vm_name \
        --port 80
}

provisionResources
