# Placeholder Dockerfile for apps/web
# This will be replaced when PR #5 (React frontend) is merged
# For now, serves a simple static HTML page

FROM nginx:alpine
WORKDIR /usr/share/nginx/html

# Remove default nginx static assets
RUN rm -rf ./*

# Copy static files
COPY public/ .

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
