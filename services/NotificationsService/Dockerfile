FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /source
ARG service=NotificationsService
COPY ./${service}/*.csproj ./${service}/
COPY ./Common/ ./Common/
RUN dotnet restore ./${service}
COPY ./${service}/ ./${service}/
RUN dotnet publish -c release -o /app --no-restore ./${service}

FROM mianni/ms-services:migrator as migrator
WORKDIR /app
COPY --from=build /app/PostService.dll ./
ENTRYPOINT ["dotnet", "Migrator.dll", "--dll", "NotificationsService.dll"]

FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine
WORKDIR /app
COPY --from=build /app ./
ENV ASPNETCORE_URLS=http://+:5005
ENV ASPNETCORE_BASE=/api/notifications
EXPOSE 5005
HEALTHCHECK CMD curl --fail http://localhost:5005/health || exit
ENTRYPOINT ["dotnet", "NotificationsService.dll"]