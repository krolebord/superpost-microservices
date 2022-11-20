eval $(minikube docker-env)
DOCKER_BUILDKIT=1 docker build -t ms-post-service -f services/PostService/Dockerfile services/PostService/
DOCKER_BUILDKIT=1 docker build -t ms-user-service -f services/UserService/Dockerfile services/UserService/
DOCKER_BUILDKIT=1 docker build -t ms-client -f client/Dockerfile client/
