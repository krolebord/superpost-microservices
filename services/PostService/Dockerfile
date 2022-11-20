FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /source
COPY *.csproj ./
RUN dotnet restore
COPY . .
RUN dotnet publish -c release -o /app --no-restore


FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app ./
ENV ASPNETCORE_URLS=http://+:5000
ENV ASPNETCORE_BASE=/api/posts
EXPOSE 5000
HEALTHCHECK CMD curl --fail http://localhost:5000/health || exit
ENTRYPOINT ["dotnet", "PostService.dll"]
