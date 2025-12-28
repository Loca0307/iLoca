# Step 1: Build
# Step 2: Publish
# Step 3: Run

# STEP 1: BUILD
# Import in the image the tools to compile and build dotnet
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

# Folder that works as working directory where all commands are run
WORKDIR /builds

# Copy .csproj file into working directory and "restoring" the dependencies defined there
COPY VidaLoca.csproj ./
RUN dotnet restore 

# STEP 2: # Copy the rest of the project in the working directory
COPY . ./

# Publish: compiles the project into a deployable version in release mode 
# and put the output files in the /out folder inside the working directory
RUN dotnet publish -c Release -o out

# STEP 3: RUN
# Imports the tools to run the app
FROM mcr.microsoft.com/dotnet/aspnet:9.0


WORKDIR /run

# Copy built files from build stage to /run directory to actually run the container
COPY --from=build /builds/out .

# Expose the port you want
EXPOSE 5112

# Tell .NET Core to listen on port 5112 inside the container
ENV ASPNETCORE_URLS=http://+:5112

# Start the app
ENTRYPOINT ["dotnet", "VidaLoca.dll"]


