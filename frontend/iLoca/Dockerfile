# STEP 1: BUILD THE APP
FROM node:20-alpine AS build

WORKDIR /app

# Install dependencies
COPY package*.json ./
RUN npm install

# Copy source code
COPY . .

# Build production bundle
RUN npm run build

# STEP 2: PUBLISH WITH NGINX
FROM nginx:alpine

# Remove default nginx config
RUN rm /etc/nginx/conf.d/default.conf

# Copy custom SPA nginx config
COPY nginx.conf /etc/nginx/conf.d/default.conf

# Copy built frontend files 
COPY --from=build /app/dist /usr/share/nginx/html

# Expose HTTP port
EXPOSE 80

# Start nginx
CMD ["nginx", "-g", "daemon off;"]
