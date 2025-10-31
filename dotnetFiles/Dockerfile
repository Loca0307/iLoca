# Step 1: Build
# Step 2: Publish
# Step 3: Run

# STEP 1: BUILD
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

# This can change in case of internal additional folder
WORKDIR /app

# Copy .csproj file into working directory and "restoring" the dependencies
COPY dotnetFiles.csproj ./
RUN dotnet restore 


# STEP 2: # Copy the rest of the project and build/publish it 

COPY . ./
RUN dotnet publish -c Release -o out



# STEP 3: RUN
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

# Copy built files from build stage
COPY --from=build /app/out .

# Expose the port you want
EXPOSE 5027

# Tell ASP.NET Core to listen on port 5027 inside the container
ENV ASPNETCORE_URLS=http://+:5027

# Start the app
ENTRYPOINT ["dotnet", "dotnetFiles.dll"]


