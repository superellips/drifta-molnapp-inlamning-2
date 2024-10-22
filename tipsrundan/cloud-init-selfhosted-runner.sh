#!/bin/bash

# Variables
app_name="tipsrundan"
app_port=8080

# Install and configure docker
function installDocker () {
    apt-get update -y && apt-get install -y ca-certificates curl
    install -m 0755 -d /etc/apt/keyrings
    curl -fsSL https://download.docker.com/linux/ubuntu/gpg -o /etc/apt/keyrings/docker.asc
    chmod a+r /etc/apt/keyrings/docker.asc
    echo \
    "deb [arch=$(dpkg --print-architecture) signed-by=/etc/apt/keyrings/docker.asc] https://download.docker.com/linux/ubuntu $(lsb_release -cs) stable" | \
    tee /etc/apt/sources.list.d/docker.list > /dev/null
    apt-get update -y
    apt-get install -y docker.io containerd docker-buildx-plugin docker-compose-plugin
    gpasswd -a $adminuser docker
}

# Install github self-hosted runner
function installGithubRunner () {
    # Add self-hosted runner
    mkdir /home/$adminuser/actions-runner; cd /home/$adminuser/actions-runner
    curl -o actions-runner-linux-x64-2.314.1.tar.gz -L https://github.com/actions/runner/releases/download/v2.314.1/actions-runner-linux-x64-2.314.1.tar.gz
    tar xzf ./actions-runner-linux-x64-2.314.1.tar.gz
    chown -R $adminuser:$adminuser ../actions-runner
    sudo -u $adminuser ./config.sh --unattended --url https://github.com/$gh_org/$app_name --token $token
    ./svc.sh install $adminuser
    ./svc.sh start
}

function configureNginx () {
    # Install nginx
    apt-get update -y && apt-get install -y nginx

    # Write configuration file
    cat << EOF > /etc/nginx/sites-available/default
    server {
        listen          80 default_server;
        location / {
            proxy_pass          http://localhost:$app_port/;
            proxy_http_version  1.1;
            proxy_set_header    Upgrade \$http_upgrade;
            proxy_set_header    Connection keep-alive;
            proxy_set_header    Host \$host;
            proxy_cache_bypass  \$http_upgrade;
            proxy_set_header    X-Forwarded-For \$proxy_add_x_forwarded_for;
            proxy_set_header    X-Forwarded-Proto \$scheme;
        }
    }
EOF

    # Ensure config is valid
    nginx -t

    # Reload the nginx service
    systemctl reload nginx
}

# Make sure that dpkg plays nice with being run noninteractively
export DEBIAN_FRONTEND=noninteractive

installDocker

installGithubRunner

configureNginx
