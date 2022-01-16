#!/bin/bash
# Post-deployment install script

# Install Docker, SSH server and other useful tools
export DEBIAN_FRONTEND=noninteractive 
export TZ=Etc/UTC
apt-get update && apt-get install -y ca-certificates \
    curl \
    gnupg \
    lsb-release \
    tzdata
curl -fsSL https://download.docker.com/linux/ubuntu/gpg \
    | gpg --dearmor -o /usr/share/keyrings/docker-archive-keyring.gpg
echo \
      "deb [arch=$(dpkg --print-architecture) \
      signed-by=/usr/share/keyrings/docker-archive-keyring.gpg] \
      https://download.docker.com/linux/ubuntu \
      $(lsb_release -cs) stable" \
      | tee /etc/apt/sources.list.d/docker.list > /dev/null
apt-get update && apt-get install docker-ce \
    docker-ce-cli \
    containerd.io -y
service docker start

# Install Algorand sandbox
curl -L \
    "https://github.com/docker/compose/releases/download/1.29.2/\
docker-compose-$(uname -s)-$(uname -m)" -o /usr/local/bin/docker-compose
chmod +x /usr/local/bin/docker-compose
git clone https://github.com/algorand/sandbox.git
cd sandbox
./sandbox $1
