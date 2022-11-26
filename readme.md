## Initial setup

### Services development
 - dotnet 7

### Client development
 - node 16
 - [pnpm](https://pnpm.io/installation)

### Kubernetes
 - `minukube`
 - `kubectl`
 - `helm`

#### Start minikube
 - `sh init.sh`

#### Install istio
 - `helm repo add istio https://istio-release.storage.googleapis.com/charts`
 - `helm repo update`
 - `kubectl create namespace istio-system`
 - `helm install istio-base istio/base -n istio-system`
 - `helm install istiod istio/istiod -n istio-system --wait`
 - `kubectl label namespace default istio-injection=enabled`

## Build images
 - `sh build.sh`


## Start with helm
  - `helm install local helm`
  - `minikube tunnel`
  - `http://localhost`

## Start services
1. `sh start.sh`
2. visit `http://localhost`

## Delete services and built images
 - `sh delete.sh`

---

## Dev
```
docker compose -f dev/docker-compose.yml up -d
```

Run migrations `sh migrations.sh`

Or
```
(cd AuthService && dotnet ef database update) &&
(cd UserService && dotnet ef database update) &&
(cd PostService && dotnet ef database update)
```
