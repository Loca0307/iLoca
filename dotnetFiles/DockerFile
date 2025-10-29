# Step 1: Build
# Step 1.5: Publish
# Step 2: Run

# STEP 1: BUILD
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

# This can change in case of internal additional folder
WORKDIR /app

# Copy .csproj file into working directory and "restoring" the dependencies
COPY dotnetFiles.csproj ./
RUN dotnet restore 


# Copy the rest of the project and build it 
# STEP 1.5: use "RUN dotnet publish" for production
COPY . ./
RUN dotnet build -c Release -o out



# STEP 2: RUN
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

# Copy built files from build stage
COPY --from=build /app/out .

# Start the app
ENTRYPOINT ["dotnet", "dotnetFiles.dll"]

# Expose port (change if your app uses a different one)
EXPOSE 5027
