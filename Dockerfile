# Use the .NET 6 ASP.NET Core runtime as the base
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Bind ASP.NET Core to the dynamic Railway port
ENV ASPNETCORE_URLS=http://+:$PORT

# Build stage

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["TwilioOpenAppointement/TwilioOpenAppointement.csproj", "TwilioOpenAppointement/"]
RUN dotnet restore "TwilioOpenAppointement/TwilioOpenAppointement.csproj"
COPY . .
WORKDIR "/src/TwilioOpenAppointement"
RUN dotnet build "TwilioOpenAppointement.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "TwilioOpenAppointement.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TwilioOpenAppointement.dll"]
