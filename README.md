# iLoca

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




# HINTS
 1) In case of the frontend not running, run `npm install`
 2) The frontend is in the `"wwwroot"` file as it is the 
default root name for ASP.NET to search for `frontend`


# Dotnet

 1) In the "dotnet" folder terminal write: `dotnet run`
and open the localhost written on the terminal


# Docker

FOR BACKEND:
1) To build the docker image:
- docker build -t `"image name"` .

2) To run the image and create a container:
- docker run --name `"container name"` -p `"host port"`:`"container port"` `"image name"`
So in this case: docker run -p 5027:5027 "image name"

3) Go to link "http://localhost:5027" to see the build


FOR FRONTEND:
1) To build the docker image:
- docker build -t my-frontend .

2) To run the image and create a container:
- docker run --name frontend -p 3000:3000 my-frontend

3) Go to link "http://localhost:3000" to see the build


FOR BOTH(docker-compose):
1) 'docker-compose up', to run the containers

2) 'docker-compose build --no-cache' to rebuild project without cache
# Postgre

1) Install `PostGre` on your pc to use the database
2) Install `Dbeaver` on your pc to interact with the database

# Test

1) Enter the "Test" folder
2) Write in terminal: `dotnet test`


## Frontend 
 1) Enter the frontend/iLoca folder
 2) Write in terminal `npm run dev`


