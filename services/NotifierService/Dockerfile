FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /source
ARG service=NotifierService
COPY ./${service}/*.csproj ./${service}/
COPY ./Common/ ./Common/
RUN dotnet restore ./${service}
COPY ./${service}/ ./${service}/
RUN dotnet publish -c release -o /app --no-restore ./${service}

FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine
WORKDIR /app
COPY --from=build /app ./
ENV ASPNETCORE_URLS=http://+:5004
ENV ASPNETCORE_BASE=/api/notifier
EXPOSE 5004
HEALTHCHECK CMD curl --fail http://localhost:5004/health || exit
ENTRYPOINT ["dotnet", "NotifierService.dll"]
