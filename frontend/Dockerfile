FROM node:alpine as builder

WORKDIR /app

COPY . .

RUN npm install

RUN npm run build:prod

FROM nginx:latest AS final

# Copy nginx config
COPY ./nginx/nginx.conf /etc/nginx/nginx.conf

# Remove nginx/html files
RUN rm -rf /usr/share/nginx/html/*

# Copy dist files to nginx/html
COPY --from=builder /app/dist /usr/share/nginx/html

ENTRYPOINT [ "nginx", "-g", "daemon off;" ]
