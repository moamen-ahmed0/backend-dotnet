# Build stage: install deps and produce the static production build
FROM node:26.5.1-alpine3.24 AS build
WORKDIR /src

COPY package*.json ./
RUN npm ci

COPY . .
RUN npm run build

# Final stage: serve the static files with a lightweight nginx image
FROM nginx:1.31.3-alpine AS final
COPY --from=build /src/dist /usr/share/nginx/html
EXPOSE 80

