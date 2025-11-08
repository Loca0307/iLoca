# BanKuum Tubo

# Technologies

- Frontend: HTML, CSS
- Backend: .NET
- Database: PostgreSQL 
- Testing: xUnit
- Secondary: Docker, Postman
- Security: 
    - BCrypt.Net-Next for password hashing


# Notes

The frontend is in the `"wwwroot"` file as it is the 
default root name for asp.NET to search for `frontend`

### Tecnical notes to solve ###

- For now the DTOs classes contain all the fileds of the models classes, we should decide which field to show and which to hid. 

# .NET

in the "dotnet" folder terminal write: `dotnet run`
and open the localhost written on the terminal


# Docker

1) To build the docker image:
- docker build -t `"image name"` .

2) To run the image and create a container:
- docker run --name `"container name"` -p `"host port"`:`"container port"` `"image name"`
So in this case: docker run -p 5027:5027 "image name"

3) Go to link "http://localhost:5027/" to see the build

# Postgre

1) Install `postGre` on your pc
2) Install `pgAdmin 4` for database GUI

# Test

1) Change directory to the dotnetFiles.Tests
2) Write in terminal: `dotnet test`