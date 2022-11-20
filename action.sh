services=("services/PostService" "services/UserService" "client")
action=${1:-apply}
for service in "${services[@]}"
do
    kubectl $action -f $service/k8s/deployment.yml
    kubectl $action -f $service/k8s/service.yml
    kubectl $action -f $service/k8s/ingress.yml
done

if [ "$action" = "apply" ]; then
    echo "Run minikube tunnel"
    echo "Go to http://$(minikube ip) or http://127.0.0.1"
fi
