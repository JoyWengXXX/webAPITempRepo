#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
ENV ASPNETCORE_ENVIRONMENT=Test

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["WebAPITemp/WebAPITemp.csproj", "WebAPITemp/"]
COPY ["CommomLibrary/CommomLibrary.csproj", "CommomLibrary/"]
RUN dotnet restore "WebAPITemp/WebAPITemp.csproj"
COPY . .
WORKDIR "/src/WebAPITemp"
RUN dotnet build "WebAPITemp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebAPITemp.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
ENV TZ=Asia/Taipei
COPY --from=publish /app/publish .
COPY WebAPITemp/Temp/MonthlyRecordsExport_Temp.xlsx /app/Temp/MonthlyRecordsExport_Temp.xlsx
COPY WebAPITemp/Temp/ShiftAndPunchInfosExport_Temp.xlsx /app/Temp/ShiftAndPunchInfosExport_Temp.xlsx
ENTRYPOINT ["dotnet", "WebAPITemp.dll"]
