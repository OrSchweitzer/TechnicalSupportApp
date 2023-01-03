FROM mcr.microsoft.com/dotnet/sdk:7.0 as build-env
WORKDIR /app

COPY *.csproj ./
RUN dotnet restore
RUN dotnet dev-certs https
COPY . ./

RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build-env /root/.dotnet/corefx/cryptography/x509stores/my/* /root/.dotnet/corefx/cryptography/x509stores/my/
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "CommandsService.dll"]