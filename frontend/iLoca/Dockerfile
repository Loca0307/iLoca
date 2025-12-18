# STEP 1: BUILD
FROM node:20-alpine AS build

WORKDIR /app

# Copy package files and install dependencies
COPY package*.json ./
RUN npm install

# Copy the rest of the frontend code
COPY . .

# Build the production app
RUN npm run build

# STEP 2: RUN / SERVE
FROM nginx:alpine

# Remove default nginx files
RUN rm -rf /usr/share/nginx/html/*


COPY --from=build /app/dist /usr/share/nginx/html

# Expose port 80
EXPOSE 80

# Start nginx
CMD ["nginx", "-g", "daemon off;"]
