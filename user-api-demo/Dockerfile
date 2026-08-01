# Build stage: compile and publish the API using the full .NET SDK (alpine = smaller image)
FROM mcr.microsoft.com/dotnet/sdk:10.0.302-alpine3.24 AS build
WORKDIR /src

COPY user-api-demo.csproj ./
RUN dotnet restore

COPY . .
RUN dotnet publish -c Release -o /app/publish --no-restore

# Final stage: only the ASP.NET runtime + published output, no SDK/build tools
FROM mcr.microsoft.com/dotnet/aspnet:10.0.10-alpine3.24 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "user-api-demo.dll"]
