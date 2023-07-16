#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["HomeBudgetManagement.Api.Core/HomeBudgetManagement.Api.Core.csproj", "HomeBudgetManagement.Api.Core/"]
COPY ["HomeBudgetManagement.Application/HomeBudgetManagement.Application.csproj", "HomeBudgetManagement.Application/"]
COPY ["HomeBudgetManagement.Core/HomeBudgetManagement.Core.csproj", "HomeBudgetManagement.Core/"]
COPY ["HomeBudgetManagement.DTO/HomeBudgetManagement.DTO.csproj", "HomeBudgetManagement.DTO/"]
COPY ["HomeBudgetManagement.Infrastructure/HomeBudgetManagement.Infrastructure.csproj", "HomeBudgetManagement.Infrastructure/"]
RUN dotnet restore "HomeBudgetManagement.Api.Core/HomeBudgetManagement.Api.Core.csproj"
COPY . .
WORKDIR "/src/HomeBudgetManagement.Api.Core"
RUN dotnet build "HomeBudgetManagement.Api.Core.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HomeBudgetManagement.Api.Core.csproj" -c Release -o /app/publish 

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HomeBudgetManagement.Api.Core.dll"]