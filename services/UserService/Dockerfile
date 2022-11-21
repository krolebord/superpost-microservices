FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /source
COPY *.csproj ./
RUN dotnet restore
COPY . .
RUN dotnet publish -c release -o /app --no-restore

FROM ms-migrator as migrator
WORKDIR /
COPY --from=build /app/UserService.dll ./
ARG CONNECTION_STRING
ENV CONNECTION_STRING=${CONNECTION_STRING}
CMD "dotnet Migrator.dll -- --str $CONNECTION_STRING --dll UserService.dll"

FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine
WORKDIR /app
COPY --from=build /app ./
EXPOSE 5000
ENV ASPNETCORE_URLS=http://+:5000
ENV ASPNETCORE_BASE=/api/users
HEALTHCHECK CMD curl --fail http://localhost:5000/health || exit
ENTRYPOINT ["dotnet", "UserService.dll"]
