FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /source
COPY *.csproj ./
RUN dotnet restore
COPY . .
RUN dotnet publish -c release -o /out --no-restore

FROM mcr.microsoft.com/dotnet/runtime:7.0-alpine
WORKDIR /
COPY --from=build /out ./
