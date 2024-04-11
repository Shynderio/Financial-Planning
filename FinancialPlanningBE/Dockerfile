FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5085


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["FinancialPlanning.WebAPI/FinancialPlanning.WebAPI.csproj", "FinancialPlanning.WebAPI/"]
COPY ["FinancialPlanning.Service/FinancialPlanning.Service.csproj", "FinancialPlanning.Service/"]
COPY ["FinancialPlanning.Data/FinancialPlanning.Data.csproj", "FinancialPlanning.Data/"]
COPY ["FinancialPlanning.Common/FinancialPlanning.Common.csproj", "FinancialPlanning.Common/"]
RUN dotnet restore "FinancialPlanning.WebAPI\FinancialPlanning.WebAPI.csproj"
COPY . .
WORKDIR "/src/FinancialPlanning.WebAPI"
RUN dotnet build "FinancialPlanning.WebAPI.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "FinancialPlanning.WebAPI.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://*:5085
ENTRYPOINT ["dotnet", "FinancialPlanning.WebAPI.dll"]
