# BanKuum Tubo

# Technologies

- Frontend: React
- Backend: ASP.NET
- Database: PostgreSQL, Dbeaver
- Testing: xUnit
- Secondary: Docker, Postman
- Security: 
    - BCrypt.Net-Next for password hashing
    - HTTPS
    - (JSON Web Token) and more


# Notes

The frontend is in the `"wwwroot"` file as it is the 
default root name for ASP.NET to search for `frontend`

### Tecnical notes to solve ###

- For now the DTOs classes contain all the fileds of the models classes, we should decide which field to show and which to hid. 

# .NET

in the "dotnet" folder terminal write: `dotnet run`
and open the localhost written on the terminal


# Docker

FOR BACKEND:
1) To build the docker image:
- docker build -t `"image name"` .

2) To run the image and create a container:
- docker run --name `"container name"` -p `"host port"`:`"container port"` `"image name"`
So in this case: docker run -p 5027:5027 "image name"

3) Go to link "http://localhost:5027/" to see the build


FOR FRONTEND:
1) To build the docker image:
- docker build -t my-frontend .

2) To run the image and create a container:
- docker run --name frontend -p 3000:80 my-frontend

# Postgre

1) Install `PostGre` on your pc
2) Install `Dbeaver` on your pc

# Test

1) Change directory to the dotnetFiles.Tests
2) Write in terminal: `dotnet test`


## Frontend 


dotnet run --launch-profile https
```


cp -r build/* ../dotnetFiles/wwwroot/
```


