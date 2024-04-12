FROM node:20-alpine as build
WORKDIR /app/src
COPY package*.json ./
RUN npm ci
COPY . ./
RUN npm run build

FROM node:20-alpine
WORKDIR /usr/app
COPY --from=build /app/src/dist/financial-planing ./
CMD node server/server.mjs
EXPOSE 4000