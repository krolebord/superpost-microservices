## Initial setup

### Services development
 - dotnet 6

### Client development
 - node 16
 - [pnpm](https://pnpm.io/installation)

### Kubernetes
 - `minukube`
 - `kubectl`

#### Start minikube
  
 - `sh init.sh`

or

1. `minikube start --driver=docker`
2. `minikube addons enable ingress`


## Build images
 - `sh build.sh`


## Start services
1. `sh start.sh`
2. visit either `http://localhost`

## Delete services and built images
 - `sh delete.sh`

---

## Dev nginx
```
docker compose -f dev/docker-compose.nginx.yml up -d
```
