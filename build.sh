eval $(minikube docker-env)
DOCKER_BUILDKIT=1 docker build -t ms-migrator -f services/Migrator/Dockerfile services/Migrator

DOCKER_BUILDKIT=1 docker build -t ms-post-service -f services/PostService/Dockerfile services/
DOCKER_BUILDKIT=1 docker build -t ms-post-service-migrator -f services/PostService/Dockerfile --target=migrator services/

DOCKER_BUILDKIT=1 docker build -t ms-user-service -f services/UserService/Dockerfile services/
DOCKER_BUILDKIT=1 docker build -t ms-user-service-migrator -f services/UserService/Dockerfile --target=migrator services/

DOCKER_BUILDKIT=1 docker build -t ms-auth-service -f services/AuthService/Dockerfile services/
DOCKER_BUILDKIT=1 docker build -t ms-auth-service-migrator -f services/AuthService/Dockerfile --target=migrator services/

DOCKER_BUILDKIT=1 docker build -t ms-timeline-service -f services/TimelineService/Dockerfile services/

DOCKER_BUILDKIT=1 docker build -t ms-client -f client/Dockerfile client/
