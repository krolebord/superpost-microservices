## Initial setup

### Services development
1. install dotnet 6

### Client development
1. install node 16
2. [install pnpm](https://pnpm.io/installation)

### Kubernetes
1. install `minukube` and `kubectl`
2. `minikube start --driver=docker`
3. `minikube addons enable ingress`


## Build images
`sh build.sh`

or
1. make sure buildkit is enabled
2. `eval $(minikube docker-env)`
3. `docker build -t ms-post-service -f services/PostService/Dockerfile services/PostService/`
4. `docker build -t ms-user-service -f services/UserService/Dockerfile services/UserService/`
5. `docker build -t ms-client -f client/Dockerfile client/`


## Start services
1. `sh start.sh`
2. run `minikube tunnel` in another terminal to expose services
3. visit either `http://127.0.0.1` or `http://$(minikube ip)`

## Delete services and built services
1. `sh start.sh delete`
2. `sh delete.sh`
