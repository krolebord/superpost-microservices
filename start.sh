kubectl apply -f k8s/postgres
sleep 5s
kubectl apply -f services/PostService/k8s
kubectl apply -f services/UserService/k8s
kubectl apply -f client/k8s

echo "Run minikube tunnel"
echo "Go to http://$(minikube ip) or http://127.0.0.1"

minikube tunnel
