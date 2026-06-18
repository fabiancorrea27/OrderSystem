FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY OrderSystem.slnx .
COPY OrderSystem.Domain/OrderSystem.Domain.csproj OrderSystem.Domain/
COPY OrderSystem.Application/OrderSystem.Application.csproj OrderSystem.Application/
COPY OrderSystem.Infrastructure/OrderSystem.Infrastructure.csproj OrderSystem.Infrastructure/
COPY OrderSystem.API/OrderSystem.API.csproj OrderSystem.API/

RUN dotnet restore OrderSystem.API/OrderSystem.API.csproj

COPY . .

WORKDIR /src/OrderSystem.API
RUN dotnet publish -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
EXPOSE 5108
EXPOSE 7024

ENV ASPNETCORE_URLS=http://+:5108
ENV ASPNETCORE_ENVIRONMENT=Development

COPY --from=build /app .

ENTRYPOINT ["dotnet", "OrderSystem.API.dll"]
