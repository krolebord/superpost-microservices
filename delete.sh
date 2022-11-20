sh action.sh delete
sleep 2s
minikube image rm ms-post-service
minikube image rm ms-user-service
minikube image rm ms-client
