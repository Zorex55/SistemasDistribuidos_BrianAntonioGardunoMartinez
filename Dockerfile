FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base 
WORKDIR /app
EXPOSE 8080
# Puerto que se va a usar 

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build 
ARG configuration=Release
WORKDIR /src
COPY ["FileDownload.csproj", "./"]
RUN dotnet restore "FileDownload.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "FileDownload.csproj" -c $configuration -o /app/build 

FROM build AS publish 
ARG configuration=Release
RUN dotnet publish "FileDownload.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN echo "Brian Antonio Garduño Martínez" > /tmp/numeroAlumno.txt
ENTRYPOINT ["dotnet", "FileDownload.dll"]
