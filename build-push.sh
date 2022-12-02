DOCKER_BUILDKIT=1 docker build --platform linux/amd64 -t ms-migrator -f services/Migrator/Dockerfile services/Migrator

DOCKER_BUILDKIT=1 docker build --platform linux/amd64 -t mianni/ms-services:post -f services/PostService/Dockerfile services/
DOCKER_BUILDKIT=1 docker build --platform linux/amd64 -t mianni/ms-services:post-migrator -f services/PostService/Dockerfile --target=migrator services/

DOCKER_BUILDKIT=1 docker build --platform linux/amd64 -t mianni/ms-services:user -f services/UserService/Dockerfile services/
DOCKER_BUILDKIT=1 docker build --platform linux/amd64 -t mianni/ms-services:user-migrator -f services/UserService/Dockerfile --target=migrator services/

DOCKER_BUILDKIT=1 docker build --platform linux/amd64 -t mianni/ms-services:auth -f services/AuthService/Dockerfile services/
DOCKER_BUILDKIT=1 docker build --platform linux/amd64 -t mianni/ms-services:auth-migrator -f services/AuthService/Dockerfile --target=migrator services/

DOCKER_BUILDKIT=1 docker build --platform linux/amd64 -t mianni/ms-services:timeline -f services/TimelineService/Dockerfile services/

DOCKER_BUILDKIT=1 docker build --platform linux/amd64 -t mianni/ms-services:client -f client/Dockerfile client/

docker push mianni/ms-services:post
docker push mianni/ms-services:post-migrator
docker push mianni/ms-services:user
docker push mianni/ms-services:user-migrator
docker push mianni/ms-services:auth
docker push mianni/ms-services:auth-migrator
docker push mianni/ms-services:timeline
docker push mianni/ms-services:client
