eval $(minikube docker-env)
DOCKER_BUILDKIT=1 docker build -t ms-migrator -f services/Migrator/Dockerfile services/Migrator

DOCKER_BUILDKIT=1 docker build -t ms-post-service -f services/PostService/Dockerfile services/PostService/
DOCKER_BUILDKIT=1 docker build -t ms-post-service-migrator -f services/PostService/Dockerfile --target=migrator services/PostService/

DOCKER_BUILDKIT=1 docker build -t ms-user-service -f services/UserService/Dockerfile services/UserService/
DOCKER_BUILDKIT=1 docker build -t ms-user-service-migrator -f services/UserService/Dockerfile --target=migrator services/UserService/

DOCKER_BUILDKIT=1 docker build -t ms-client -f client/Dockerfile client/
