services=("k8s/postgres" "services/PostService/k8s" "services/UserService/k8s" "client/k8s")
action=${1}
for service in "${services[@]}"
do
    kubectl $action -f $service
done
