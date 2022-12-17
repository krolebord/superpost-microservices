FROM node:16.18.1-alpine as build
RUN apk update && apk add --no-cache libc6-compat
RUN corepack enable && corepack prepare pnpm@7.16.1 --activate 
WORKDIR /source
COPY package.json pnpm-lock.yaml ./
RUN pnpm install --frozen-lockfile
COPY . .
RUN pnpm build

FROM nginx:stable-alpine
COPY --from=build /source/dist /usr/share/nginx/html
COPY nginx/nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
