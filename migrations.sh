services=("AuthService" "UserService" "PostService")
for service in "${services[@]}"
do
    (cd services/$service && dotnet ef database update)
done
