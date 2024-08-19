
# Build environment image 
FROM mcr.microsoft.com/dotnet/sdk:6.0  AS build-env
WORKDIR /app 
EXPOSE 80
EXPOSE 443



# Copy project files from local machine to the build-env image
# that mean source form local machine to destination source inside build-env image with path /app/Services/Catalog/Catalog.Api/Catalog.Api.csproj
COPY ["Services/Catalog/Catalog.Api/Catalog.Api.csproj","Services/Catalog/Catalog.Api/Catalog.Api.csproj"] 
# ensure to copy all dll => *.csproj that refernced or created 
COPY ["Services/Catalog/Catalog.Application/Catalog.Application.csproj","Services/Catalog/Catalog.Application/Catalog.Application.csproj"] 
COPY ["Services/Catalog/Catalog.Infrastructure/Catalog.Infrastructure.csproj","Services/Catalog/Catalog.Infrastructure/Catalog.Infrastructure.csproj"] 
COPY ["Services/Catalog/Catalog.Core/Catalog.Core.csproj","Services/Catalog/Catalog.Core/Catalog.Core.csproj"] 
COPY ["Infrastructure/Common.Logging/Common.Logging.csproj", "Infrastructure/Common.Logging/"]

# to restore dependencies and Packages from catalog.api.csproj => that includde refernces from catalog.infrastructure.csproj and catalog.application.csproj 
# Restore dependencies and packages
RUN dotnet restore "Services/Catalog/Catalog.Api/Catalog.Api.csproj" 

# to copy all files from local machine to destination build-env image conatiner
# Copy all files from local machine to the build-env image container
COPY . .

# then we need to publish catalog.api  to compile it and ready for deployment 
# that mean catalog.api.dll and catalog.api.exe will publish in path /app/out 
# Publish Catalog.Api
# This will create published files in /app/out
RUN dotnet publish "Services/Catalog/Catalog.Api/Catalog.Api.csproj" -c Release -o out

#here we create a new image final-env to copy published files from build-env image to final-env cause to utilize sizes 
# copu from build-env with path /app/out to /src
FROM mcr.microsoft.com/dotnet/aspnet:6.0  AS final-env
WORKDIR /app
COPY --from=build-env /app/out /app
# here we will execute executable file after copying it inside final-env dll file 
ENTRYPOINT ["dotnet", "Catalog.Api.dll"]




























