# Base image
FROM node:slim as dependency

# Create app directory
WORKDIR /app

COPY . .

# Install app dependencies

RUN npm install


FROM node:slim as build

WORKDIR /app

# Bundle app source
COPY --from=dependency /app .

# Creates a "dist" folder with the production build
RUN npm run build

RUN npm prune --production


FROM node:slim as runtime

WORKDIR /app

# Copy the "dist" folder from the build stage to the runtime stage
COPY --from=build /app .

EXPOSE 3000

# Start the server using the production build
CMD [ "node", "dist/src/main.js" ]