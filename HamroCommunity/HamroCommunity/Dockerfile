#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["HamroCommunity/HamroCommunity.csproj", "HamroCommunity/"]
COPY ["Project.BLL/Project.BLL.csproj", "Project.BLL/"]
COPY ["Project.DLL/Project.DLL.csproj", "Project.DLL/"]
COPY ["Project.TEST/Project.TEST.csproj", "Project.TEST/"]
RUN dotnet restore "HamroCommunity/HamroCommunity.csproj"
COPY . .
WORKDIR "/src/HamroCommunity"
RUN dotnet build "HamroCommunity.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HamroCommunity.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HamroCommunity.dll"]